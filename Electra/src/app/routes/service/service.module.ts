import { NgModule } from '@angular/core';
import { ServiceOrderComponent } from './srv-order.component';
import { SharedModule } from '@shared';
import { ServiceRoutingModule } from './service-routing.module';
import { ServiceVoucherListComponent } from './voucher-list.component';
import { RoutesCommonModule } from '../common/common.module';

const COMPONENTS: any[] = [ServiceOrderComponent, ServiceVoucherListComponent];
const COMPONENTS_DYNAMIC: any[] = [];

@NgModule({
  imports: [SharedModule, RoutesCommonModule, ServiceRoutingModule],
  declarations: [...COMPONENTS, ...COMPONENTS_DYNAMIC],
})
export class ServiceModule {}
