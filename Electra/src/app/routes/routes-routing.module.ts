import { NgModule } from '@angular/core';
import { RouterModule, Routes } from '@angular/router';
import { environment } from '@env/environment';

import { MsalGuard } from '@azure/msal-angular';
import { BrowserUtils } from '@azure/msal-browser';
import { AdminLayoutComponent } from '../theme/admin-layout/admin-layout.component';
import { AuthLayoutComponent } from '../theme/auth-layout/auth-layout.component';
import { DashboardComponent } from './dashboard/dashboard.component';
import { Error403Component } from './sessions/403.component';
import { Error404Component } from './sessions/404.component';
import { Error500Component } from './sessions/500.component';
import { RegisterComponent } from './sessions/register/register.component';
import { UnauthorizedComponent } from './sessions/unauthorized.component';
import { NgxPermissionsGuard } from 'ngx-permissions';
import { Roles } from '@core';

const routes: Routes = [
  {
    path: '',
    component: AdminLayoutComponent,
    canActivate: [MsalGuard],
    canActivateChild: [MsalGuard],
    children: [
      { path: '', redirectTo: 'dashboard', pathMatch: 'full' },
      { path: 'dashboard', component: DashboardComponent },
      { path: '403', component: Error403Component },
      { path: '404', component: Error404Component },
      { path: '500', component: Error500Component },
      {
        path: 'service',
        loadChildren: () => import('./service/service.module').then(m => m.ServiceModule),
        canActivate: [NgxPermissionsGuard],
        data: {
          permissions: {
            // only: Roles.SERVICE,
            redirectTo: '/dashboard',
          },
        },
      },
      {
        path: 'administration',
        loadChildren: () =>
          import('./administration/administration.module').then(m => m.AdministrationModule),
        canActivate: [NgxPermissionsGuard],
        data: {
          permissions: {
            // only: Roles.SERVICE,
            redirectTo: '/dashboard',
          },
        },
      },
    ],
  },
  {
    path: 'auth',
    component: AuthLayoutComponent,
    children: [
      { path: 'register', component: RegisterComponent },
      {
        path: 'unauthorized',
        component: UnauthorizedComponent,
        canActivate: [],
        canDeactivate: [],
      },
    ],
  },
  { path: '**', redirectTo: 'dashboard' },
];

@NgModule({
  imports: [
    RouterModule.forRoot(routes, {
      useHash: environment.useHash,
      initialNavigation:
        !BrowserUtils.isInIframe() && !BrowserUtils.isInPopup() ? 'enabledNonBlocking' : 'disabled',
    }),
  ],
  exports: [RouterModule],
})
export class RoutesRoutingModule {}
