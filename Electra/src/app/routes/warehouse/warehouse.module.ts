import { NgModule } from '@angular/core';
import { SharedModule } from '@shared';
import { RoutesCommonModule } from '../common/common.module';
import { WarehouseContactListComponent } from './contact/contact-list.component';
import { WarehouseRoutingModule } from './warehouse-routing.module';
import { WarehouseVoucherListComponent } from './voucher/voucher-list.component';

const COMPONENTS: any[] = [WarehouseContactListComponent, WarehouseVoucherListComponent];
const COMPONENTS_DYNAMIC: any[] = [];

@NgModule({
  imports: [SharedModule, RoutesCommonModule, WarehouseRoutingModule],
  declarations: [...COMPONENTS, ...COMPONENTS_DYNAMIC],
})
export class WarehouseModule {}
