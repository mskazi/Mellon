import { HttpClient } from '@angular/common/http';
import { Injectable } from '@angular/core';
import { PaginatedListResults } from '@core/core-model';
import { ISearchService } from '@core/forms/base-form-search.component';
import { Voucher, VoucherTrack } from '@core/models/voucher-details-item';
import {
  VoucherOfficeItem,
  VoucherSearchItem,
  VoucherServiceItem,
  VoucherWarehouseItem,
} from '@core/models/voucher-search-item';
import { VoucherSummary } from '@core/models/voucher-sumamry';
import { environment } from '@env/environment';
import { Utilities } from '@shared/utils/utilities';
import { Observable } from 'rxjs';

@Injectable({
  providedIn: 'root',
})
export class VoucherCommandService {
  warehouseCommands: VoucherWarehouseCommands;
  searchCommands: VoucherSearchCommands;
  serviceCommands: VoucherServiceCommands;
  officeCommands: VoucherOfficeCommands;

  constructor(private http: HttpClient) {
    this.warehouseCommands = new VoucherWarehouseCommands(http);
    this.searchCommands = new VoucherSearchCommands(http);
    this.officeCommands = new VoucherOfficeCommands(http);
    this.serviceCommands = new VoucherServiceCommands(http);
  }

  getVoucherDetails(id: number): Observable<Voucher> {
    return this.http.get<Voucher>(`${environment.serviceRoleUrl}/details/${id}`);
  }

  getVoucherTrack(id: number): Observable<VoucherTrack> {
    return this.http.get<VoucherTrack>(`${environment.serviceRoleUrl}/track/${id}`);
  }

  getSummary(): Observable<VoucherSummary> {
    return this.http.get<VoucherSummary>(`${environment.serviceRoleUrl}/summary`);
  }

  print(id: number): Observable<any> {
    return this.http.post(
      `${environment.serviceRoleUrl}/print/${id}`,
      {},
      {
        responseType: 'blob',
      }
    );
  }
}

export class VoucherWarehouseCommands implements ISearchService<VoucherWarehouseItem> {
  constructor(private http: HttpClient) {}

  search(
    term: string,
    params: any,
    extraParams: any
  ): Observable<PaginatedListResults<VoucherWarehouseItem>> {
    let query = Utilities.paginatedQueryParams(params) + '&';
    if (params) {
      query += Utilities.orderQueryParams(params) + '&';
    }
    if (term) {
      query += `term=${term.toString()}`;
    }

    return this.http.get<PaginatedListResults<VoucherWarehouseItem>>(
      `${environment.serviceRoleUrl}/warehouse?${query}`
    );
  }
}

export class VoucherSearchCommands implements ISearchService<VoucherSearchItem> {
  constructor(private http: HttpClient) {}

  search(term: string, params: any): Observable<PaginatedListResults<VoucherSearchItem>> {
    let query = Utilities.paginatedQueryParams(params) + '&';
    if (params) {
      query += Utilities.orderQueryParams(params) + '&';
    }
    if (term) {
      query += `term=${term.toString()}`;
    }

    return this.http.get<PaginatedListResults<VoucherSearchItem>>(
      `${environment.serviceRoleUrl}/search?${query}`
    );
  }
}

export class VoucherOfficeCommands implements ISearchService<VoucherOfficeItem> {
  constructor(private http: HttpClient) {}

  search(term: string, params: any): Observable<PaginatedListResults<VoucherOfficeItem>> {
    let query = Utilities.paginatedQueryParams(params) + '&';
    if (params) {
      query += Utilities.orderQueryParams(params) + '&';
    }
    if (term) {
      query += `term=${term.toString()}`;
    }

    return this.http.get<PaginatedListResults<VoucherOfficeItem>>(
      `${environment.serviceRoleUrl}/office?${query}`
    );
  }
  createVoucher(data: any): Observable<any> {
    return this.http.post<any>(
      `${environment.serviceRoleUrl}/office/new/${data.contactId}`,
      data.voucherDetails
    );
  }
}

export class VoucherServiceCommands implements ISearchService<VoucherServiceItem> {
  constructor(private http: HttpClient) {}

  search(term: string, params: any): Observable<PaginatedListResults<VoucherServiceItem>> {
    let query = Utilities.paginatedQueryParams(params) + '&';
    if (params) {
      query += Utilities.orderQueryParams(params) + '&';
    }
    if (term) {
      query += `term=${term.toString()}`;
    }

    return this.http.get<PaginatedListResults<VoucherServiceItem>>(
      `${environment.serviceRoleUrl}/service?${query}`
    );
  }
}
