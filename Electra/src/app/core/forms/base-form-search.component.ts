import { DatePipe } from '@angular/common';
import { PageEvent } from '@angular/material/paginator';
import { PaginatedListResults, defaultPageIndex, defaultPageSize } from '@core/core-model';
import { InjectorService } from '@core/injector.service';
import { ExportFormatTypes } from '@core/settings';
import { MtxGridColumn } from '@ng-matero/extensions/grid';
import { SpinnerService, SpinnerType } from '@shared/components/spinner/spinner.service';
import { Utilities } from '@shared/utils/utilities';
import { EMPTY, Observable, catchError, expand, finalize, map, reduce } from 'rxjs';

export abstract class BaseSearchFormComponent<T> {
  protected service: ISearchService<T>;

  abstract columns: MtxGridColumn[];
  abstract exportName: string;
  defaultPageIndex = defaultPageIndex;
  defaultPageSize = defaultPageSize;
  exportChannel = Utilities.GenGUI();
  exportChannelSpinnerType = SpinnerType.FIXED_CIRCLE;
  exportLoading = false;
  ExportFormatTypes = ExportFormatTypes;

  constructor(service: ISearchService<T>, forceLoad = true) {
    this.service = service;
    if (forceLoad) {
      this.getList();
    }
  }

  getExportHeaders(): string[] {
    return [];
  }

  getExtraParams(): any {
    return {};
  }

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

  get params() {
    const p = Object.assign({}, this.query);
    return p;
  }

  private getList() {
    this.isLoading = true;
    const params = this.params;
    params.start = params.index * params.pageSize;
    this.service
      .search(this.query.term, params, this.getExtraParams())
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
    const spinnerService = InjectorService.injector.get(SpinnerService);
    const datePipe = InjectorService.injector.get(DatePipe);

    if (!this.total) {
      return;
    }
    if (this.exportLoading) {
      return;
    }
    this.exportLoading = true;
    spinnerService.activate(this.exportChannel);

    const length = 999;
    const totalPages = Math.ceil(this.total / length);
    const params = this.params;
    params.start = 0;
    params.pageSize = 999;
    params.index = 0;
    this.service
      .search(this.query.term, params, this.getExtraParams)
      .pipe(
        map((r: PaginatedListResults<any>) => {
          return r.data;
        }),
        // get the fist page
        expand((value, index) => {
          if (index <= totalPages) {
            params.index++;
            params.start = params.index * params.pageSize;
            return this.service.search(this.query.term, params, this.getExtraParams()).pipe(
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
          spinnerService.deactivate(this.exportChannel);
          throw error;
        })
      )
      .subscribe((dataToSave: any[]) => {
        const headers = this.getExportHeaders();
        Utilities.exportFile(dataToSave, headers, this.exportName, datePipe, type);
        spinnerService.deactivate(this.exportChannel);
        this.exportLoading = false;
      });
  }
}

/**
 *  Interface to handle methods of persisting
 */
export interface ISearchService<T> {
  search(term: string, params: any, extraParams: any): Observable<PaginatedListResults<T>>;
}
