import { NgModule } from '@angular/core';
import { ServiceOrderComponent } from './srv-order.component';
import { SharedModule } from '@shared';
import { ServiceRoutingModule } from './service-routing.module';
import { ServiceVoucherListComponent } from './voucher-listcomponent';

const COMPONENTS: any[] = [ServiceOrderComponent, ServiceVoucherListComponent];
const COMPONENTS_DYNAMIC: any[] = [];

@NgModule({
  imports: [SharedModule, ServiceRoutingModule],
  declarations: [...COMPONENTS, ...COMPONENTS_DYNAMIC],
})
export class ServiceModule {}