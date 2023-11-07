import { NgModule } from '@angular/core';
import { SharedModule } from '@shared/shared.module';
import { RoutesRoutingModule } from './routes-routing.module';

import { DashboardComponent } from './dashboard/dashboard.component';
import { RegisterComponent } from './sessions/register/register.component';
import { Error403Component } from './sessions/403.component';
import { Error404Component } from './sessions/404.component';
import { Error500Component } from './sessions/500.component';
import { UnauthorizedComponent } from './sessions/unauthorized.component';
import { RoutesCommonModule } from './common/common.module';
import { SearchVoucherListComponent } from './search/voucher-list.component';

const COMPONENTS: any[] = [
  DashboardComponent,
  RegisterComponent,
  Error403Component,
  Error404Component,
  Error500Component,
  UnauthorizedComponent,
  SearchVoucherListComponent,
];
const COMPONENTS_DYNAMIC: any[] = [];

@NgModule({
  imports: [SharedModule, RoutesCommonModule, RoutesRoutingModule],
  declarations: [...COMPONENTS, ...COMPONENTS_DYNAMIC],
})
export class RoutesModule {}
