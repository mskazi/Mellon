import { Component } from '@angular/core';
import { ContactMode } from '@core/models/contac';
import { VoucherCreateRoleType } from '@core/models/voucher-create';
import { DialogDynamicService } from '@shared/components/dialog/dialog.service';
import { VoucherCreateNewContactComponent } from 'app/routes/common/vouchers/create/voucher-create-contact.component';
import { VoucherCreateNewContactComponentData } from 'app/routes/service/scanSend/voucher-scan-send.component';
@Component({
  selector: 'app-office-contact-list',
  templateUrl: './contact-list.component.html',
  styleUrls: [],
})
export class OfficeContactListComponent {
  contactMode = ContactMode.Office;
  constructor(private dialogDynamicService: DialogDynamicService) {}

  createNewVoucher(contactId: number) {
    this.dialogDynamicService.open<
      VoucherCreateNewContactComponent,
      VoucherCreateNewContactComponentData,
      any
    >(
      VoucherCreateNewContactComponent,
      { contactId: contactId, roleType: VoucherCreateRoleType.OFFICE },
      {
        titleKey: 'Create Voucher',
        acceptLabelKey: 'Ok',
        declineLabelKey: 'Cancel',
      }
    );
  }
}
