import { Injectable } from '@angular/core';
import { HttpClient, HttpErrorResponse } from '@angular/common/http';
import { Menu, Roles, STATUS, User, UserInfo } from '@core';
import { catchError, map, tap } from 'rxjs/operators';
import { ReplaySubject, of } from 'rxjs';
import { AccountInfo } from '@azure/msal-browser';
import { MsalService } from '@azure/msal-angular';
import { environment } from '@env/environment';
import { Router } from '@angular/router';

@Injectable({
  providedIn: 'root',
})
export class LoginService {
  private userinfoSubject: ReplaySubject<UserInfo> = new ReplaySubject<UserInfo>();
  userinfo$ = this.userinfoSubject.asObservable();
  private userInfo: UserInfo;
  private unauthorized: boolean = false;

  constructor(
    protected http: HttpClient,
    protected authService: MsalService,
    private router: Router
  ) {}

  login(username: string, password: string, rememberMe = false) {
    return this.http.post<any>('/auth/login', { username, password, rememberMe });
  }

  refresh(params: Record<string, any>) {
    return this.http.post<any>('/auth/refresh', params);
  }

  logout() {
    this.authService.logoutPopup({
      mainWindowRedirectUri: '/',
    });
  }

  info(accountInfo: AccountInfo) {
    if (!accountInfo.username) {
      return of({ user: {}, accountInfo });
    }
    return this.http.get<any>(`${environment.servicesMembersUrl}/me`).pipe(
      tap((user: User) => {
        if (user.department === 'service') {
          user.roles = Roles.SERVICE;
        }
        this.userInfo = { user, accountInfo };
        this.userinfoSubject.next(this.userInfo);
      }),
      catchError((error: HttpErrorResponse) => {
        if (error.status === STATUS.UNAUTHORIZED) {
          this.router.navigateByUrl('/auth/unauthorized');
        }
        throw error;
      })
    );
  }

  me(accountInfo: AccountInfo) {
    return this.http.get<any>('/me');
  }

  menu(): Menu[] {
    return [
      ...this.getServiceMenu(),
      ...this.getOfficeMenu(),
      ...this.getWarehouseMenu(),
      ...this.getReturnsMenu(),
      {
        route: '/',
        name: 'openBI',
        type: 'extTabLink',
        icon: 'extension',
        permissions: {
          only: '',
        },
      },
    ];
  }

  check() {
    return this.unauthorized;
  }

  getServiceMenu() {
    if (this.userInfo?.user.roles !== Roles.SERVICE) {
      return [];
    }
    return [
      {
        route: 'service',
        name: 'service',
        type: 'sub',
        icon: 'brightness_low',
        expanded: true,
        children: [
          {
            route: 'voucher-list',
            name: 'VoucherList',
            type: 'link',
          },
          {
            route: 'scan-send',
            name: 'ScanSend',
            type: 'link',
          },
          {
            route: 'srv-orders',
            name: 'SRVOrderManual',
            type: 'link',
          },
        ],
        permissions: {
          only: ['SERVICE'],
        },
      },
    ] as Menu[];
  }

  getWarehouseMenu() {
    return [
      {
        route: 'warehouse',
        name: 'warehouse',
        type: 'sub',
        icon: 'domain',
        expanded: true,

        children: [
          {
            route: 'vouchers',
            name: 'voucherList',
            type: 'link',
          },
          {
            route: 'contacts',
            name: 'contactList',
            type: 'link',
          },
          {
            route: 'search-send',
            name: 'searchAndSend',
            type: 'link',
          },
        ],
        permissions: {
          only: ['SERVICE'],
        },
      },
    ] as Menu[];
  }

  getOfficeMenu() {
    return [
      {
        route: 'office',
        name: 'office',
        type: 'sub',
        icon: 'meeting_room',
        expanded: true,
        children: [
          {
            route: 'vouchers',
            name: 'voucherList',
            type: 'link',
          },
          {
            route: 'contacts',
            name: 'contactList',
            type: 'link',
          },
          {
            route: 'members',
            name: 'memberList',
            type: 'link',
          },
          {
            route: 'print-voucher',
            name: 'printVoucherList',
            type: 'link',
          },
        ],
        permissions: {
          only: ['SERVICE'],
        },
      },
    ] as Menu[];
  }

  getReturnsMenu() {
    return [
      {
        route: 'returns',
        name: 'returns',
        type: 'sub',
        icon: 'settings_backup_restore',
        expanded: true,
        children: [
          {
            route: 'import',
            name: 'import',
            type: 'link',
          },
          {
            route: 'returns',
            name: 'returns',
            type: 'link',
          },
        ],
        permissions: {
          only: ['SERVICE'],
        },
      },
    ] as Menu[];
  }
}
