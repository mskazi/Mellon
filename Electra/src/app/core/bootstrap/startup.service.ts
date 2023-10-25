import { Inject, Injectable } from '@angular/core';
import {
  MSAL_GUARD_CONFIG,
  MsalBroadcastService,
  MsalGuardConfiguration,
  MsalService,
} from '@azure/msal-angular';
import { AccountInfo, EventMessage, EventType } from '@azure/msal-browser';
import { LoginService, UserInfo } from '@core/authentication';
import { NgxPermissionsService, NgxRolesService } from 'ngx-permissions';
import { Subject } from 'rxjs';
import { filter, mergeMap, tap } from 'rxjs/operators';
import { MenuService } from './menu.service';

@Injectable({
  providedIn: 'root',
})
export class StartupService {
  constructor(
    @Inject(MSAL_GUARD_CONFIG) private msalGuardConfig: MsalGuardConfiguration,
    private msalService: MsalService,
    private msalBroadcastService: MsalBroadcastService,
    private menuService: MenuService,
    private permissonsService: NgxPermissionsService,
    private rolesService: NgxRolesService,
    private loginService: LoginService
  ) {}

  private readonly _destroying$ = new Subject<void>();

  /**
   * Load the application only after get the menu or other essential information
   * such as permissions and roles.
   */
  load() {
    this.msalBroadcastService.msalSubject$
      .pipe(
        filter(
          (msg: EventMessage) =>
            msg.eventType === EventType.ACCOUNT_ADDED || msg.eventType === EventType.ACCOUNT_REMOVED
        )
      )
      .subscribe((result: EventMessage) => {
        if (this.msalService.instance.getAllAccounts().length === 0) {
          window.location.pathname = '/';
        }
      });

    return new Promise<void>((resolve, reject) => {
      this.msalService
        .handleRedirectObservable()
        .pipe(
          tap(response => console.log('response from handle: ', response)),
          mergeMap(response => {
            if (!response && this.msalService.instance.getAllAccounts().length === 0) {
              console.log('No account found -> login');
              return this.msalService.loginRedirect();
            } else {
              this.checkAndSetActiveAccount();
              console.log('Logged in! Set account etc.');
              return this.loginService.info(
                this.msalService.instance.getActiveAccount() ?? ({} as AccountInfo)
              );
            }
          }),
          mergeMap(() => this.loginService.userinfo$),
          tap((userInfo: UserInfo) => this.setPermissions(userInfo)),
          tap((userInfo: UserInfo) => this.setMenu(userInfo))
        )
        .subscribe({
          next: () => resolve(),
          error: () => resolve(),
        });
    });
  }

  checkAndSetActiveAccount() {
    let activeAccount = this.msalService.instance.getActiveAccount();

    if (!activeAccount && this.msalService.instance.getAllAccounts().length > 0) {
      let accounts = this.msalService.instance.getAllAccounts();
      this.msalService.instance.setActiveAccount(accounts[0]);
    }
  }

  private setMenu(userInfo: UserInfo) {
    const menu = this.loginService.menu();
    this.menuService.addNamespace(menu, 'menu');
    this.menuService.set(menu);
  }

  private setPermissions(userInfo: UserInfo) {
    // In a real app, you should get permissions and roles from the user information.
    /* const permissions = ['canAdd', 'canDelete', 'canEdit', 'canRead'];
    this.permissonsService.loadPermissions(permissions); */
    this.rolesService.flushRoles();
    this.rolesService.addRoles({ [userInfo.user.roles]: [] });
    // Tips: Alternatively you can add permissions with role at the same time.
    // this.rolesService.addRolesWithPermissions({ ADMIN: permissions });
  }
}
