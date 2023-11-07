import { NgModule } from '@angular/core';
import { SharedModule } from '@shared/shared.module';
import { VoucherDetailsCommonComponent } from './vouchers/voucher-details.component';
import { ContactListComponent } from './contact/contact-list.component';
import { ContactEditComponent } from './contact/contact-edit.component';

const COMPONENTS: any[] = [
  VoucherDetailsCommonComponent,
  ContactListComponent,
  ContactEditComponent,
];
const COMPONENTS_DYNAMIC: any[] = [];

@NgModule({
  imports: [SharedModule],
  declarations: [...COMPONENTS, ...COMPONENTS_DYNAMIC],
  exports: [...COMPONENTS, ...COMPONENTS_DYNAMIC],
})
export class RoutesCommonModule {}
