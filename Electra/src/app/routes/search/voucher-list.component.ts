import { DatePipe } from '@angular/common';
import { AfterViewInit, Component, OnDestroy } from '@angular/core';
import { ActivatedRoute } from '@angular/router';
import { VoucherSearchItem } from '@core/models/voucher-search-item';
import { VoucherCommandService } from '@core/services/voucher-commands.service';
import { MtxGridColumn } from '@ng-matero/extensions/grid';
import { DialogDynamicService } from '@shared/components/dialog/dialog.service';
import { BaseVoucherListComponent } from 'app/routes/common/vouchers/voucher-base-list.component';
import { Subscription } from 'rxjs';
@Component({
  selector: 'app-service-voucher-list',
  templateUrl: './voucher-list.component.html',
  styleUrls: ['./voucher-list.component.scss'],
})
export class SearchVoucherListComponent
  extends BaseVoucherListComponent<VoucherSearchItem>
  implements AfterViewInit, OnDestroy
{
  private routeSubscription: Subscription;

  constructor(
    protected serviceCommandsService: VoucherCommandService,
    protected datePipe: DatePipe,
    protected dialogDynamicService: DialogDynamicService,
    protected route: ActivatedRoute
  ) {
    super(serviceCommandsService.searchCommands, dialogDynamicService, false);
  }

  ngAfterViewInit(): void {
    this.routeSubscription = this.route.queryParams.subscribe((queryParam: any) => {
      this.query.term = queryParam['searchTerm'];
      this.search();
    });
  }

  ngOnDestroy() {
    if (this.routeSubscription) {
      this.routeSubscription.unsubscribe();
    }
  }

  exportName = 'Search_Results_List';
  columns: MtxGridColumn[] = [
    { header: 'Electra_ID', field: 'id', sortable: true },
    {
      header: 'Created',
      field: 'createdAt',
      sortable: true,
      formatter: (data: any) =>
        `${this.datePipe.transform(data.createdAt, 'dd/MM/YYYY', 'UTC', 'en')}`,
    },
    { header: 'Ordered By', sortable: true, field: 'orderedBy' },
    { header: 'Company', sortable: true, field: 'systemCompany' },
    {
      header: 'Delivery Name',
      sortable: true,
      field: 'voucherName',
      formatter: (data: any) => `${data.voucherName}`,
    },
    {
      header: 'Delivery Contact',
      sortable: true,
      field: 'voucherContact',
      formatter: (data: any) => `${data.voucherContact}`,
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
    { header: 'Voucher Status', sortable: true, field: 'carrierDeliveryStatus' },
    { header: 'Voucher No', sortable: true, field: 'carrierVoucherNo' },
    { header: 'Carrier Name', sortable: true, field: 'carrierName' },
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
      'System Company',
      'Navision Service OrderNo',
      'Navision Sales OrderNo',
      'Action Type Description',
      'Carrier Voucher No',
      'Ordered By',
      'Mellon Project',
      'Carrier Name',
      'Created By',
      'Created At',
    ];
  }
}
