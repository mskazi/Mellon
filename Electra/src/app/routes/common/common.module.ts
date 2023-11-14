import { NgModule } from '@angular/core';
import { SharedModule } from '@shared/shared.module';
import { VoucherDetailsCommonComponent } from './vouchers/voucher-details.component';
import { ContactListComponent } from './contact/contact-list.component';
import { ContactEditComponent } from './contact/contact-edit.component';
import { VoucherCommandsComponent } from './vouchers/voucher-commands.component';
import { VoucherTrackCommonComponent } from './vouchers/voucher-track.component';
import { VoucherCreateNewContactComponent } from './vouchers/create/voucher-create-contact.component';
import { VoucherSendToDetailsComponent } from './vouchers/common/voucher-send-to-details.component';
import { VoucherDetailsComponent } from './vouchers/common/voucher-details.component';
import { VoucherSpecialsDetailsComponent } from './vouchers/common/voucher-special-details.component';

const COMPONENTS: any[] = [
  VoucherDetailsCommonComponent,
  ContactListComponent,
  ContactEditComponent,
  VoucherCommandsComponent,
  VoucherTrackCommonComponent,
  VoucherCreateNewContactComponent,
  VoucherSendToDetailsComponent,
  VoucherDetailsComponent,
  VoucherSpecialsDetailsComponent,
];
const COMPONENTS_DYNAMIC: any[] = [];

@NgModule({
  imports: [SharedModule],
  declarations: [...COMPONENTS, ...COMPONENTS_DYNAMIC],
  exports: [...COMPONENTS, ...COMPONENTS_DYNAMIC],
})
export class RoutesCommonModule {}
