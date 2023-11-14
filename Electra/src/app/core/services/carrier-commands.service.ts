import { HttpClient, HttpParams } from '@angular/common/http';
import { Injectable } from '@angular/core';
import { ListResult } from '@core/core-model';
import { VoucherCarrier } from '@core/models/carrier';
import { VoucherCreateRoleType } from '@core/models/voucher-create';
import { environment } from '@env/environment';
import { Observable, map } from 'rxjs';

@Injectable({
  providedIn: 'root',
})
export class CarrierCommandService {
  constructor(private http: HttpClient) {}

  getVoucherCarriers(
    postalCode: string,
    roleType: VoucherCreateRoleType
  ): Observable<VoucherCarrier[]> {
    let params = new HttpParams();
    params = params.append('postalCode', postalCode);
    params = params.append('roleType', roleType);

    return this.http
      .get<ListResult<VoucherCarrier>>(`${environment.serviceCarriersUrl}/list`, {
        params: params,
      })
      .pipe(
        map((res: ListResult<VoucherCarrier>) => {
          return res.data;
        })
      );
  }

  getVoucherCarrier(postalCode: string): Observable<VoucherCarrier> {
    let params = new HttpParams();
    params = params.append('postalCode', postalCode);
    return this.http.get<VoucherCarrier>(`${environment.serviceCarriersUrl}/parameters`, {
      params: params,
    });
  }
}
