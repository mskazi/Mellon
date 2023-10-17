import { Component, OnInit, ViewEncapsulation } from '@angular/core';
import { Router } from '@angular/router';
import { LoginService, User, UserInfo } from '@core/authentication';

@Component({
  selector: 'app-user-panel',
  template: `
    <ng-container *ngIf="userInfo">
      <div class="matero-user-panel">
        <h4 class="matero-user-panel-name">{{ userInfo.user.member }}</h4>
      </div>
    </ng-container>
  `,
  styleUrls: ['./user-panel.component.scss'],
  encapsulation: ViewEncapsulation.None,
})
export class UserPanelComponent implements OnInit {
  userInfo!: UserInfo;

  constructor(
    private router: Router,
    private loginService: LoginService
  ) {}

  ngOnInit(): void {
    this.loginService.userinfo$.subscribe(userInfo => (this.userInfo = userInfo));
  }

  logout() {
    this.loginService.logout();
  }
}
