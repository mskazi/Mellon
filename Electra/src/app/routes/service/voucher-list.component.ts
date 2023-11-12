import { DatePipe } from '@angular/common';
import { Component } from '@angular/core';
import { VoucherServiceItem } from '@core/models/voucher-search-item';
import { MtxGridColumn } from '@ng-matero/extensions/grid';
import { DialogDynamicService } from '@shared/components/dialog/dialog.service';
import { VoucherCommandService } from '../../core/services/voucher-commands.service';
import { BaseVoucherListComponent } from '../common/vouchers/voucher-base-list.component';
@Component({
  selector: 'app-service-voucher-list',
  templateUrl: './voucher-list.component.html',
  styleUrls: ['./voucher-list.component.scss'],
})
export class ServiceVoucherListComponent extends BaseVoucherListComponent<VoucherServiceItem> {
  constructor(
    protected serviceCommandsService: VoucherCommandService,
    private datePipe: DatePipe,
    protected dialogDynamicService: DialogDynamicService
  ) {
    super(serviceCommandsService.serviceCommands, dialogDynamicService);
  }

  /* case '0':
                    $trflag_stat = 'text-info';
                    break;
                case '10':
                    $trflag_stat = 'text-primary';
                    break;
                case '20':
                    $trflag_stat = 'text-danger';
                    break;
                case '90':
                    $trflag_stat = 'text-success';
                    break;
                default:
                    $trflag_stat = 'text-muted';
                    break; */
  exportName = 'Service_Voucher_List';
  columns: MtxGridColumn[] = [
    {
      header: 'Status',
      field: 'systemStatus',
      formatter: (data: any) => {
        switch (data.systemStatus) {
          case '0':
            return `<span class="bg-teal-100"> &nbsp</span>`;
          case '10':
            return `<span class="bg-blue-100">&nbsp</span>`;
          case '20':
            return `<span class="bg-red-100">&nbsp</span>`;
          case '90':
            return `<span class="bg-green-100">&nbsp</span>`;
          default:
            return `<span class="bg-gray-100 p-x-16 p-y-8">&nbsp</span>`;
        }
      },
    },
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
      formatter: (data: any) => `${data.voucherName} ${data.voucherContact ?? ''}`,
    },
    {
      header: 'Delivery Address',
      sortable: true,
      field: 'voucherAddress',
      formatter: (data: any) =>
        `${data.voucherAddress} | ${data.voucherCity} | ${data.voucherPostCode ?? ''}`,
    },
    { header: 'Delivery PhoneNo', sortable: true, field: 'voucherPhoneNo' },
    { header: 'Delivery MobileNo', sortable: true, field: 'voucherMobileNo' },
    { header: 'Delivery Description', sortable: true, field: 'voucherDescription' },
    { header: 'Serial No', sortable: true, field: 'serialNo' },
    { header: 'Carrier Name', sortable: true, field: 'carrierName' },
    { header: 'Voucher Status', sortable: true, field: 'statusDescription' },
    { header: 'Voucher No', sortable: true, field: 'carrierVoucherNo' },
    {
      header: 'Function',
      field: 'function',
      pinned: 'right',
      right: '0px',
    },
  ];

  getExportHeaders() {
    return [
      'Id',
      'System Status',
      'Voucher Name',
      'Voucher Contact',
      'Voucher Address',
      'Voucher City',
      'Voucher PostCode',
      'Voucher Phone No',
      'Voucher Mobile No',
      'Voucher Description',
      'Serial No',
      'System Company',
      'Navision Service OrderNo',
      'Navision Sales OrderNo',
      'Action Type Description',
      'Carrier Voucher No',
      'Status Description',
      'Ordered By',
      'Mellon Project',
      'Carrier Name',
      'Created By',
      'Created At',
    ];
  }
}
