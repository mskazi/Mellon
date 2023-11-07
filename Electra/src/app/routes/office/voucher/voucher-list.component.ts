import { DatePipe } from '@angular/common';
import { Component } from '@angular/core';
import { VoucherOfficeItem } from '@core/models/voucher-search-item';
import { VoucherCommandService } from '@core/services/voucher-commands.service';
import { MtxGridColumn } from '@ng-matero/extensions/grid';
import { DialogDynamicService } from '@shared/components/dialog/dialog.service';
import { BaseVoucherListComponent } from 'app/routes/common/vouchers/voucher-base-list.component';
@Component({
  selector: 'app-office-voucher-list',
  templateUrl: './voucher-list.component.html',
  styleUrls: ['./voucher-list.component.scss'],
})
export class OfficeVoucherListComponent extends BaseVoucherListComponent<VoucherOfficeItem> {
  constructor(
    protected serviceCommandsService: VoucherCommandService,
    protected datePipe: DatePipe,
    protected dialogDynamicService: DialogDynamicService
  ) {
    super(serviceCommandsService.officeCommands, dialogDynamicService);
  }
  exportName = 'Warehouse_Voucher_List';
  columns: MtxGridColumn[] = [
    { header: 'Electra_ID', field: 'id', sortable: true },
    { header: 'Company', field: 'systemCompany', sortable: true },
    { header: 'Action', sortable: true, field: 'actionTypeDescription' },
    {
      header: 'Created',
      field: 'createdAt',
      sortable: true,
      formatter: (data: any) =>
        `${this.datePipe.transform(data.createdAt, 'dd/MM/YYYY', 'UTC', 'en')}`,
    },
    {
      header: 'Delivery Name',
      sortable: true,
      field: 'voucherName',
    },
    {
      header: 'Delivery Contact',
      sortable: true,
      field: 'voucherContact',
    },
    {
      header: 'Delivery Address',
      sortable: true,
      field: 'voucherAddress',
      formatter: (data: any) =>
        `${data.voucherAddress} | ${data.voucherCity} | ${data.voucherPostCode}`,
    },
    { header: 'Delivery PhoneNo', sortable: true, field: 'voucherPhoneNo' },
    { header: 'Delivery MobileNo', sortable: true, field: 'voucherMobileNo' },
    { header: 'Delivery Description', sortable: true, field: 'voucherDescription' },
    { header: 'Deliver Time', sortable: true, field: 'deliveryTimeDescription' },
    { header: 'Voucher Status', sortable: true, field: 'statusDescription' },
    { header: 'Voucher No', sortable: true, field: 'carrierVoucherNo' },
    { header: 'Carrier', sortable: true, field: 'carrierName' },
    { header: 'Created By', sortable: true, field: 'createdBy' },
    { header: 'Ordered By', sortable: true, field: 'orderedBy' },
    {
      header: 'Function',
      field: 'function',
      pinned: 'right',
      right: '0px',
    },
  ];
}
