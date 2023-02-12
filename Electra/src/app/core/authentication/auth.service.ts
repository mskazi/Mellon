import { Inject, Injectable } from '@angular/core';
import { BehaviorSubject, iif, merge, of, Subject } from 'rxjs';
import {
  catchError,
  filter,
  map,
  mergeMap,
  share,
  switchMap,
  takeUntil,
  tap,
} from 'rxjs/operators';
import { LoginService } from './login.service';
import { filterObject, isEmptyObject } from './helpers';
import { User } from './interface';
import {
  MSAL_GUARD_CONFIG,
  MsalGuardConfiguration,
  MsalService,
  MsalBroadcastService,
} from '@azure/msal-angular';
import {
  AuthenticationResult,
  EventMessage,
  EventType,
  InteractionStatus,
  PopupRequest,
} from '@azure/msal-browser';
import { Menu, MenuService } from '@core/bootstrap/menu.service';
import { NgxPermissionsService, NgxRolesService } from 'ngx-permissions';

@Injectable({
  providedIn: 'root',
})
export class AuthService {
  private loginDisplay = false;
  private readonly _destroying$ = new Subject<void>();

  private user$ = new BehaviorSubject<User>({});

  private initMSALSubscriptions() {
    this.msalBroadcastService.msalSubject$
      .pipe(
        filter(
          (msg: EventMessage) =>
            msg.eventType === EventType.ACCOUNT_ADDED || msg.eventType === EventType.ACCOUNT_REMOVED
        )
      )
      .subscribe((result: EventMessage) => {
        if (this.authService.instance.getAllAccounts().length === 0) {
          window.location.pathname = '/';
        } else {
          this.setLoginDisplay();
        }
      });
    this.msalBroadcastService.inProgress$
      .pipe(
        filter((status: InteractionStatus) => status === InteractionStatus.None),
        takeUntil(this._destroying$),
        tap(() => {
          this.setLoginDisplay();
          this.checkAndSetActiveAccount();
        }),
        mergeMap(() => {
          return this.assignUser().pipe(
            tap(user => this.setPermissions(user)),
            switchMap(() => this.menu()),
            tap(menu => this.setMenu(menu))
          );
        })
      )
      .subscribe();
  }

  private setMenu(menu: Menu[]) {
    this.menuService.addNamespace(menu, 'menu');
    this.menuService.set(menu);
  }

  private setPermissions(user: User) {
    // In a real app, you should get permissions and roles from the user information.
    const permissions = ['canAdd', 'canDelete', 'canEdit', 'canRead'];
    this.permissonsService.loadPermissions(permissions);
    this.rolesService.flushRoles();
    this.rolesService.addRoles({ ADMIN: permissions });
    // Tips: Alternatively you can add permissions with role at the same time.
    // this.rolesService.addRolesWithPermissions({ ADMIN: permissions });
  }

  checkAndSetActiveAccount() {
    let activeAccount = this.authService.instance.getActiveAccount();
    if (!activeAccount && this.authService.instance.getAllAccounts().length > 0) {
      let accounts = this.authService.instance.getAllAccounts();
      this.authService.instance.setActiveAccount(accounts[0]);
    }
  }

  setLoginDisplay() {
    this.loginDisplay = this.authService.instance.getAllAccounts().length > 0;
  }

  constructor(
    private loginService: LoginService,
    @Inject(MSAL_GUARD_CONFIG) private msalGuardConfig: MsalGuardConfiguration,
    private authService: MsalService,
    private msalBroadcastService: MsalBroadcastService,
    private menuService: MenuService,
    private permissonsService: NgxPermissionsService,
    private rolesService: NgxRolesService
  ) {
    this.initMSALSubscriptions();
  }

  check() {
    return this.authService.instance.getAllAccounts().length > 0;
    //return this.tokenService.valid();
  }

  logout() {
    this.authService.logoutPopup({
      mainWindowRedirectUri: '/',
    });

    /*  return this.loginService.logout().pipe(
      tap(() => this.tokenService.clear()),
      map(() => !this.check())
    ); */
  }

  user() {
    return this.user$.pipe(share());
  }

  menu() {
    return iif(() => this.check(), this.loginService.menu(), of([]));
  }

  private assignUser() {
    return this.loginService.me().pipe(tap(user => this.user$.next(user)));
  }
}
