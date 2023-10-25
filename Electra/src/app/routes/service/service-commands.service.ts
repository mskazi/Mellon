import { HttpClient } from '@angular/common/http';
import { Injectable } from '@angular/core';
import { PaginatedListResults } from '@core/table-model';
import { environment } from '@env/environment';
import { Utilities } from '@shared/utils/utilities';
import { Observable } from 'rxjs';

@Injectable()
export class ServiceCommandsService {
  constructor(private http: HttpClient) {}

  getVoucherList(term: string = '', params: any = {}): Observable<PaginatedListResults<any>> {
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
}

export interface VoucherServiceItem {
  id: number;
  voucherName: string;
  systemStatus: number;
  voucherContact?: string;
  voucherAddress: string;
  voucherCity: string;
  voucherPostCode: string;
  voucherPhoneNo: string;
  voucherMobileNo: string;
  voucherDescription?: string;
  serialNo: string;
  systemCompany: string;
  navisionServiceOrderNo: string;
  navisionSalesOrderNo: string;
  actionTypeDescription: string;
  carrierVoucherNo: string;
  statusDescription: string;
  orderedBy: string;
  mellonProject: string;
  carrierName: string;
  createdBy: string;
  createdAt: Date;
}
