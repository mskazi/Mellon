import { AfterViewInit, Component, EventEmitter, Input, Output } from '@angular/core';
import { BaseSearchFormComponent } from '@core/forms/base-form-search.component';
import { ContactMode } from '@core/models/contac';
import { VoucherServiceItem } from '@core/models/voucher-search-item';
import { ContactsCommandService } from '@core/services/contact-commands.service';
import { MtxGridColumn } from '@ng-matero/extensions/grid';
import { DialogDynamicService } from '@shared/components/dialog/dialog.service';
import { ContactEditComponent, ContactEditModalData } from './contact-edit.component';
import { ContactSearchItem } from '@core/models/contact-search-item';
import { ContactEditItem } from '@core/models/contact-edit-item';
@Component({
  selector: 'app-contact-list',
  templateUrl: './contact-list.component.html',
  styleUrls: ['./contact-list.component.scss'],
})
export class ContactListComponent
  extends BaseSearchFormComponent<ContactSearchItem>
  implements AfterViewInit
{
  constructor(
    private contactsCommandService: ContactsCommandService,
    private dialogDynamicService: DialogDynamicService
  ) {
    super(contactsCommandService, false);
  }

  @Input() mode: ContactMode;
  @Output() createNewVoucher = new EventEmitter<number>();

  override getExportHeaders() {
    return [
      'Id',
      'Company Name',
      'From',
      'Name',
      'Contact',
      'Address',
      'City',
      'Post Code',
      'Country',
      'Phone',
      'Mobile',
      'Note',
      'Active',
    ];
  }

  override getExtraParams() {
    return {
      mode: this.mode,
    };
  }
  exportName = 'Contact_List';
  columns: MtxGridColumn[] = [
    { header: 'Company Name', field: 'voucherName', sortable: true },
    { header: 'Contact', field: 'voucherContact', sortable: true },
    { header: 'Address', sortable: true, field: 'voucherAddress' },
    { header: 'City', sortable: true, field: 'voucherCity' },
    { header: 'PostCode', sortable: true, field: 'voucherPostCode' },
    { header: 'PhoneNo', sortable: true, field: 'voucherPhoneNo' },
    { header: 'MobileNo', sortable: true, field: 'voucherMobileNo' },
    { header: 'Notes', sortable: true, field: 'contactNotes' },
    {
      header: 'Function',
      field: 'function',
      pinned: 'right',
      right: '0px',
    },
  ];

  ngAfterViewInit(): void {
    this.search();
  }

  editContactDetails(item?: ContactSearchItem) {
    const dialogRef = this.dialogDynamicService.open<
      ContactEditComponent,
      ContactEditModalData,
      ContactEditItem
    >(
      ContactEditComponent,
      {
        item: item ?? ({} as ContactSearchItem),
        mode: this.mode,
      },
      {
        titleKey: 'Contact Details',
        acceptLabelKey: 'Ok',
        declineLabelKey: 'Cancel',
      }
    );
    dialogRef.afterClosed().subscribe((response: any) => {
      if (response) {
        this.search();
      }
    });
  }

  createVoucher(contactId: number) {
    this.createNewVoucher.emit(contactId);
  }
}
