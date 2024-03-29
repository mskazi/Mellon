import { ChangeDetectorRef, Component, OnInit } from '@angular/core';
import { Router } from '@angular/router';
import { LoginService, SettingsService, User, UserInfo } from '@core';
import { debounceTime, tap } from 'rxjs/operators';

@Component({
  selector: 'app-user',
  template: `
    <ng-container *ngIf="user">
      <button class="r-full" mat-button [matMenuTriggerFor]="menu">
        <mat-icon width="24">account_circle</mat-icon>
      </button>
      <mat-menu #menu="matMenu">
        <button mat-menu-item (click)="logout()">
          <mat-icon>exit_to_app</mat-icon>
          <span>{{ 'logout' | translate }}</span>
        </button>
      </mat-menu>
    </ng-container>
  `,
  styles: [
    `
      .avatar {
        width: 24px;
        height: 24px;
      }
    `,
  ],
})
export class UserComponent implements OnInit {
  user!: User;

  constructor(
    private router: Router,
    private cdr: ChangeDetectorRef,
    private settings: SettingsService,
    private loginService: LoginService
  ) {}

  ngOnInit(): void {
    this.loginService.userinfo$
      .pipe(
        tap((userInfo: UserInfo) => (this.user = userInfo.user)),
        debounceTime(10)
      )
      .subscribe(() => this.cdr.detectChanges());
  }

  logout() {
    this.loginService.logout();
  }

  restore() {
    this.settings.reset();
    window.location.reload();
  }
}
