import { NgModule } from '@angular/core';
import { RouterModule, Routes } from '@angular/router';
import { WarehouseContactListComponent } from './contact/contact-list.component';
import { WarehouseVoucherListComponent } from './voucher/voucher-list.component';

const routes: Routes = [
  { path: 'contacts', component: WarehouseContactListComponent },
  { path: 'vouchers', component: WarehouseVoucherListComponent },
];

@NgModule({
  imports: [RouterModule.forChild(routes)],
  exports: [RouterModule],
})
export class WarehouseRoutingModule {}
