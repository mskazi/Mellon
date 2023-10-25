import { AfterViewInit, Component } from '@angular/core';
import { FormBuilder } from '@angular/forms';
import { MtxGridColumn } from '@ng-matero/extensions/grid';
import { ServiceCommandsService } from './service-commands.service';
import { finalize } from 'rxjs';
import { PageEvent } from '@angular/material/paginator';
import { defaultPageIndex, defaultPageSize } from '@core/table-model';
import { DatePipe } from '@angular/common';
import * as _ from 'lodash';

@Component({
  selector: 'app-service-voucher-list',
  templateUrl: './voucher-list.component.html',
  styleUrls: ['./voucher-list.component.scss'],
})
export class ServiceVoucherListComponent implements AfterViewInit {
  constructor(
    private serviceCommandsService: ServiceCommandsService,
    private datePipe: DatePipe
  ) {}

  defaultPageIndex = defaultPageIndex;
  defaultPageSize = defaultPageSize;

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
}
