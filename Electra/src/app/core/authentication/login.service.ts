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
    if (this.userInfo?.user.roles === Roles.SERVICE) {
      return [
        {
          route: 'service',
          name: 'service',
          type: 'sub',
          icon: 'color_lens',
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

    return [];
  }

  check() {
    return this.unauthorized;
  }
}
