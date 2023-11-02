import { HttpClient } from '@angular/common/http';
import { Injectable } from '@angular/core';
import { PaginatedListResults } from '@core/core-model';
import { Voucher } from '@core/models/voucher-details-item';
import { VoucherServiceItem } from '@core/models/voucher-search-item';
import { environment } from '@env/environment';
import { Utilities } from '@shared/utils/utilities';
import { Observable } from 'rxjs';

@Injectable({
  providedIn: 'root',
})
export class VoucherCommandService {
  constructor(private http: HttpClient) {}

  getVoucherList(
    term: string = '',
    params: any = {}
  ): Observable<PaginatedListResults<VoucherServiceItem>> {
    let query = Utilities.paginatedQueryParams(params) + '&';
    if (params) {
      query += Utilities.orderQueryParams(params) + '&';
    }
    if (term) {
      query += `term=${term.toString()}`;
    }

    return this.http.get<PaginatedListResults<any>>(
      `${environment.serviceRoleUrl}/service/vouchers?${query}`
    );
  }

  getVoucherDetails(id: number): Observable<Voucher> {
    return this.http.get<Voucher>(`${environment.serviceRoleUrl}/${id}`);
  }
}
