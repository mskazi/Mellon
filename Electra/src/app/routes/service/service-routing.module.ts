import { NgModule } from '@angular/core';
import { ServiceVoucherListComponent } from './voucher-list.component';
import { Routes, RouterModule } from '@angular/router';
import { ServiceScanSendComponent } from './scanSend/voucher-scan-send.component';
import { ServiceOrderComponent } from './srvOrder/voucher-order.component';

const routes: Routes = [
  { path: 'voucher-list', component: ServiceVoucherListComponent },
  { path: 'scan-send', component: ServiceScanSendComponent },
  { path: 'srv-orders', component: ServiceOrderComponent },
];

@NgModule({
  imports: [RouterModule.forChild(routes)],
  exports: [RouterModule],
})
export class ServiceRoutingModule {}
