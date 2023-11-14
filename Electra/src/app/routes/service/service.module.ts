import { NgModule } from '@angular/core';
import { SharedModule } from '@shared';
import { ServiceRoutingModule } from './service-routing.module';
import { ServiceVoucherListComponent } from './voucher-list.component';
import { RoutesCommonModule } from '../common/common.module';
import { ServiceScanSendComponent } from './scanSend/voucher-scan-send.component';
import { ServiceOrderComponent } from './srvOrder/voucher-order.component';
import { VoucherCreateNewFromServiceComponent as ServiceVoucherCreateNewFromOrderComponent } from './srvOrder/voucher-order-create.component';

const COMPONENTS: any[] = [
  ServiceVoucherListComponent,
  ServiceScanSendComponent,
  ServiceOrderComponent,
  ServiceVoucherCreateNewFromOrderComponent,
];
const COMPONENTS_DYNAMIC: any[] = [];

@NgModule({
  imports: [SharedModule, RoutesCommonModule, ServiceRoutingModule],
  declarations: [...COMPONENTS, ...COMPONENTS_DYNAMIC],
})
export class ServiceModule {}
