import { NgModule } from '@angular/core';
import { Routes, RouterModule } from '@angular/router';
import { ServiceOrderComponent } from './srv-order.component';
import { ServiceVoucherListComponent } from './voucher-list.component';
import { VoucherCommandService } from '../../core/services/voucher-commands.service';

const routes: Routes = [
  { path: 'voucher-list', component: ServiceVoucherListComponent },
  { path: 'scan-send', component: ServiceOrderComponent },
  { path: 'srv-orders', component: ServiceOrderComponent },
];

@NgModule({
  imports: [RouterModule.forChild(routes)],
  exports: [RouterModule],
})
export class ServiceRoutingModule {}
