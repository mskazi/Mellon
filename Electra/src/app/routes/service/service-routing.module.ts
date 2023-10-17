import { NgModule } from '@angular/core';
import { Routes, RouterModule } from '@angular/router';
import { ServiceOrderComponent } from './srv-order.component';

const routes: Routes = [
  { path: 'voucher-list', component: ServiceOrderComponent },
  { path: 'scan-send', component: ServiceOrderComponent },
  { path: 'srv-orders', component: ServiceOrderComponent },
];

@NgModule({
  imports: [RouterModule.forChild(routes)],
  exports: [RouterModule],
})
export class ServiceRoutingModule {}
