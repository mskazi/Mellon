import { DatePipe } from '@angular/common';
import { AfterViewInit, Component } from '@angular/core';
import { PageEvent } from '@angular/material/paginator';
import { PaginatedListResults, defaultPageIndex, defaultPageSize } from '@core/table-model';
import { MtxGridColumn } from '@ng-matero/extensions/grid';
import { EMPTY, catchError, expand, finalize, map, reduce } from 'rxjs';
import * as XLSX from 'xlsx';
import { VoucherCommandService } from '../../core/services/voucher-commands.service';
import { Utilities } from '@shared/utils/utilities';
import { ExportFormatTypes } from '@core';
import { SpinnerService, SpinnerType } from '@shared/components/spinner/spinner.service';
import { DialogDynamicService } from '@shared/components/dialog/dialog.service';
import { VoucherDetailsCommonComponent } from '../common/vouchers/voucher-details.component';
import { VoucherServiceItem } from '@core/models/voucher-search-item';
@Component({
  selector: 'app-service-voucher-list',
  templateUrl: './voucher-list.component.html',
  styleUrls: ['./voucher-list.component.scss'],
})
export class ServiceVoucherListComponent implements AfterViewInit {
  constructor(
    private serviceCommandsService: VoucherCommandService,
    private datePipe: DatePipe,
    private spinnerService: SpinnerService,
    private dialogDynamicService: DialogDynamicService
  ) {}

  defaultPageIndex = defaultPageIndex;
  defaultPageSize = defaultPageSize;
  exportChannel = Utilities.GenGUI();
  exportChannelSpinnerType = SpinnerType.FIXED_CIRCLE;
  exportLoading = false;
  ExportFormatTypes = ExportFormatTypes;

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

  data: any[] = [];
  total = 0;
  isLoading = true;
  sortActive = '';

  query = {
    term: '',
    pageSize: 10,
    start: 0,
    index: 0,
    orderBy: [] as any[],
  };

  ngAfterViewInit(): void {
    this.getList();
  }

  get params() {
    const p = Object.assign({}, this.query);
    return p;
  }

  private getList() {
    this.isLoading = true;
    const params = this.params;
    params.start = params.index * params.pageSize;
    this.serviceCommandsService
      .getVoucherList(this.query.term, params)
      .pipe(
        finalize(() => {
          this.isLoading = false;
        })
      )
      .subscribe(res => {
        this.data = res.data;
        this.total = res.total;
        this.isLoading = false;
      });
  }

  getNextPage(e: PageEvent) {
    this.query.index = e.pageIndex;
    this.query.pageSize = e.pageSize;
    this.getList();
  }

  getSortPage(e: any) {
    this.query.index = 0;
    this.sortActive = e.active;
    this.query.orderBy = [];
    const order = { name: e.active, dir: e.direction };
    this.query.orderBy.push(order);
    this.getList();
  }

  search() {
    this.query.index = 0;
    this.getList();
  }

  reset() {
    this.query.term = '';
    this.query.index = 0;
    this.query.orderBy = [];
    this.sortActive = '';
    this.getList();
  }

  export(type: ExportFormatTypes) {
    if (!this.total) {
      return;
    }
    if (this.exportLoading) {
      return;
    }
    this.exportLoading = true;
    this.spinnerService.activate(this.exportChannel);

    const length = 999;
    const totalPages = Math.ceil(this.total / length);
    const params = this.params;
    params.start = 0;
    params.pageSize = 999;
    params.index = 0;
    this.serviceCommandsService
      .getVoucherList(this.query.term, params)
      .pipe(
        map((r: PaginatedListResults<any>) => {
          return r.data;
        }),
        // get the fist page
        expand((value, index) => {
          if (index <= totalPages) {
            params.index++;
            params.start = params.index * params.pageSize;
            return this.serviceCommandsService.getVoucherList(this.query.term, params).pipe(
              map((r: PaginatedListResults<any>) => {
                return r.data;
              })
            );
          }
          return EMPTY;
        }),
        reduce((a: any, v: any) => [...a, ...v], []),
        catchError((error: any) => {
          this.exportLoading = false;
          this.spinnerService.deactivate(this.exportChannel);
          throw error;
        })
      )
      .subscribe((dataToSave: any[]) => {
        const headers = [
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
        Utilities.exportFile(dataToSave, headers, 'Voucher_List', this.datePipe, type);
        this.spinnerService.deactivate(this.exportChannel);
        this.exportLoading = false;
      });
  }

  showVoucherDetails(item: VoucherServiceItem) {
    this.dialogDynamicService.open<VoucherDetailsCommonComponent, number, any>(
      VoucherDetailsCommonComponent,
      item.id,
      {
        titleKey: 'Voucher Details',
        acceptLabelKey: 'Ok',
        declineLabelKey: 'Cancel',
      }
    );
  }
}
