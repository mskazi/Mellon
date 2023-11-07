import { DatePipe } from '@angular/common';
import { Component } from '@angular/core';
import { VoucherWarehouseItem } from '@core/models/voucher-search-item';
import { VoucherCommandService } from '@core/services/voucher-commands.service';
import { MtxGridColumn } from '@ng-matero/extensions/grid';
import { DialogDynamicService } from '@shared/components/dialog/dialog.service';
import { BaseVoucherListComponent } from 'app/routes/common/vouchers/voucher-base-list.component';
@Component({
  selector: 'app-service-voucher-list',
  templateUrl: './voucher-list.component.html',
  styleUrls: ['./voucher-list.component.scss'],
})
export class WarehouseVoucherListComponent extends BaseVoucherListComponent<VoucherWarehouseItem> {
  constructor(
    protected serviceCommandsService: VoucherCommandService,
    protected datePipe: DatePipe,
    protected dialogDynamicService: DialogDynamicService
  ) {
    super(serviceCommandsService.warehouseCommands, dialogDynamicService);
  }
  exportName = 'Warehouse_Voucher_List';
  columns: MtxGridColumn[] = [
    { header: 'Electra_ID', field: 'id', sortable: true },
    {
      header: 'Created',
      field: 'createdAt',
      sortable: true,
      formatter: (data: any) =>
        `${this.datePipe.transform(data.createdAt, 'dd/MM/YYYY', 'UTC', 'en')}`,
    },
    { header: 'Project', field: 'mellonProject', sortable: true },
    { header: 'Action', field: 'actionTypeDescription', sortable: true },
    {
      header: 'Delivery Name',
      sortable: true,
      field: 'voucherName',
      formatter: (data: any) => `${data.voucherName}${data.voucherContact}`,
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
    { header: 'Carrier Name', sortable: true, field: 'carrierName' },

    { header: 'Created By', sortable: true, field: 'createdBy' },
    { header: 'Ordered By', sortable: true, field: 'orderedBy' },
    { header: 'Voucher Status', sortable: true, field: 'statusDescription' },
    { header: 'Voucher No', sortable: true, field: 'carrierVoucherNo' },
    {
      header: 'Function',
      field: 'function',
      pinned: 'right',
      right: '0px',
    },
  ];
}
