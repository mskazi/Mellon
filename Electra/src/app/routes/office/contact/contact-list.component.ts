import { Component } from '@angular/core';
import { ContactMode } from '@core/models/contac';
import { DialogDynamicService } from '@shared/components/dialog/dialog.service';
import { OfficeVoucherNewComponent } from '../voucher/voucher-new.component';
@Component({
  selector: 'app-office-contact-list',
  templateUrl: './contact-list.component.html',
  styleUrls: [],
})
export class OfficeContactListComponent {
  contactMode = ContactMode.Office;
  constructor(private dialogDynamicService: DialogDynamicService) {}

  createNewVoucher(contactId: number) {
    const dialogRef = this.dialogDynamicService.open<OfficeVoucherNewComponent, number, any>(
      OfficeVoucherNewComponent,
      contactId,
      {
        titleKey: 'Create Voucher',
        acceptLabelKey: 'Ok',
        declineLabelKey: 'Cancel',
      }
    );
  }
}
