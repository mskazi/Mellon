import { DatePipe } from '@angular/common';
import { AfterViewInit, Component } from '@angular/core';
import { PageEvent } from '@angular/material/paginator';
import { ExportFormatTypes } from '@core';
import { defaultPageIndex, defaultPageSize } from '@core/core-model';
import { MemberItem } from '@core/models/member';
import { MembersCommandService } from '@core/services/member-commands.service';
import { MtxGridColumn } from '@ng-matero/extensions/grid';
import { DialogDynamicService } from '@shared/components/dialog/dialog.service';
import { SpinnerService, SpinnerType } from '@shared/components/spinner/spinner.service';
import { Utilities } from '@shared/utils/utilities';
import { finalize } from 'rxjs';
import { AdministrationMemberEditComponent } from './member-edit.component';
@Component({
  selector: 'app-administration-member-list',
  templateUrl: './member-list.component.html',
  styleUrls: ['./member-list.component.scss'],
})
export class AdministrationMemberListComponent implements AfterViewInit {
  constructor(
    private administrationCommandService: MembersCommandService,
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

  columns: MtxGridColumn[] = [
    { header: 'Id', field: 'id', sortable: true },
    { header: 'Member', field: 'memberName', sortable: true },
    { header: 'Company', field: 'company', sortable: true },
    { header: 'Department', field: 'department', sortable: true },
    { header: 'Active', field: 'isActive', sortable: true },
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
    this.administrationCommandService
      .getMemberList(this.query.term, params)
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

  editMember(item?: MemberItem) {
    const dialogRef = this.dialogDynamicService.open<
      AdministrationMemberEditComponent,
      MemberItem,
      MemberItem
    >(AdministrationMemberEditComponent, item ?? ({} as MemberItem), {
      titleKey: 'Voucher Details',
      acceptLabelKey: 'Ok',
      declineLabelKey: 'Cancel',
    });
    dialogRef.afterClosed().subscribe((response: any) => {
      if (response) {
        this.getList();
      }
    });
  }
}
