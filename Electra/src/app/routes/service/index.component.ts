import {
  Component,
  OnInit,
  ChangeDetectionStrategy,
  ChangeDetectorRef,
  TemplateRef,
  ViewChild,
} from '@angular/core';

import { MtxDialog } from '@ng-matero/extensions/dialog';
import { MtxGridColumn } from '@ng-matero/extensions/grid';
import { TranslateService } from '@ngx-translate/core';
import { VoucherService } from '@core/voucher/voucher.service';
import { finalize, Observable, of } from 'rxjs';

@Component({
  selector: 'app-service-index',
  templateUrl: './index.component.html',
  providers: [],
  changeDetection: ChangeDetectionStrategy.OnPush,
})
export class ServiceIndexComponent implements OnInit {
  @ViewChild('operationTpl', { static: true }) operationTpl!: TemplateRef<any>;

  columns: MtxGridColumn[] = [
    {
      header: this.translate.stream('table_kitchen_sink.position'),
      field: 'position',
      sortable: true,
      minWidth: 100,
      width: '100px',
    },
    {
      header: this.translate.stream('table_kitchen_sink.name'),
      field: 'name',
      sortable: true,
      disabled: true,
      minWidth: 100,
      width: '100px',
    },
    {
      header: this.translate.stream('table_kitchen_sink.weight'),
      field: 'weight',
      minWidth: 100,
    },
    {
      header: this.translate.stream('table_kitchen_sink.symbol'),
      field: 'symbol',
      minWidth: 100,
    },
    {
      header: this.translate.stream('table_kitchen_sink.gender'),
      field: 'gender',
      minWidth: 100,
    },
    {
      header: this.translate.stream('table_kitchen_sink.mobile'),
      field: 'mobile',
      hide: true,
      minWidth: 120,
    },
    {
      header: this.translate.stream('table_kitchen_sink.tele'),
      field: 'tele',
      minWidth: 120,
      width: '120px',
    },
    {
      header: this.translate.stream('table_kitchen_sink.birthday'),
      field: 'birthday',
      minWidth: 180,
    },
    {
      header: this.translate.stream('table_kitchen_sink.city'),
      field: 'city',
      minWidth: 120,
    },
    {
      header: this.translate.stream('table_kitchen_sink.address'),
      field: 'address',
      minWidth: 180,
      width: '200px',
    },
    {
      header: this.translate.stream('table_kitchen_sink.company'),
      field: 'company',
      minWidth: 120,
    },
    {
      header: this.translate.stream('table_kitchen_sink.website'),
      field: 'website',
      minWidth: 180,
    },
    {
      header: this.translate.stream('table_kitchen_sink.email'),
      field: 'email',
      minWidth: 180,
    },
    {
      header: this.translate.stream('table_kitchen_sink.operation'),
      field: 'operation',
      minWidth: 160,
      width: '160px',
      pinned: 'right',
      type: 'button',
      cellTemplate: this.operationTpl,
      /*    buttons: [
        {
          type: 'icon',
          icon: 'edit',
          tooltip: this.translate.stream('table_kitchen_sink.edit'),
           click: record => this.edit(record),
        },
        {
          color: 'warn',
          icon: 'delete',
          text: this.translate.stream('table_kitchen_sink.delete'),
          tooltip: this.translate.stream('table_kitchen_sink.delete'),
          pop: {
            title: this.translate.stream('table_kitchen_sink.confirm_delete'),
            closeText: this.translate.stream('table_kitchen_sink.close'),
            okText: this.translate.stream('table_kitchen_sink.ok'),
          },
          click: record => this.delete(record),
        },
      ], */
    },
  ];
  list$: Observable<any[]> = of([]);
  isLoading = true;

  multiSelectable = true;
  rowSelectable = true;
  hideRowSelectionCheckbox = false;
  showToolbar = true;
  columnHideable = true;
  columnSortable = true;
  columnPinnable = true;
  rowHover = false;
  rowStriped = false;
  showPaginator = true;
  expandable = false;
  columnResizable = false;

  constructor(
    private translate: TranslateService,
    private voucher: VoucherService,
    public dialog: MtxDialog,
    private cdr: ChangeDetectorRef
  ) {}

  ngOnInit() {
    this.isLoading = true;
    this.voucher.loadServiceVoucher().pipe(
      finalize(() => {
        this.isLoading = false;
      })
    );
    this.list$ = this.voucher.loadServiceVoucher();
  }

  /*   edit(value: any) {
    const dialogRef = this.dialog.originalOpen(TablesKitchenSinkEditComponent, {
      width: '600px',
      data: { record: value },
    });

    dialogRef.afterClosed().subscribe(() => console.log('The dialog was closed'));
  } */

  delete(value: any) {
    this.dialog.alert(`You have deleted ${value.position}!`);
  }

  changeSelect(e: any) {
    console.log(e);
  }

  changeSort(e: any) {
    console.log(e);
  }

  enableRowExpandable() {
    this.columns[0].showExpand = this.expandable;
  }

  updateCell() {
    /*  this.list = this.list.map(item => {
      item.weight = Math.round(Math.random() * 1000) / 100;
      return item;
    }); */
  }

  updateList() {
    /*  this.list = this.list.splice(-1).concat(this.list); */
  }
}