import { HttpClient, HttpParams } from '@angular/common/http';
import { Injectable } from '@angular/core';
import { ListResult } from '@core/core-model';
import {
  Company,
  Country,
  Department,
  LookUpString,
  VoucherConditionType,
  VoucherDeliveryTimeType,
  VoucherDepartmentType,
  VoucherType,
} from '@core/models/lookup';
import { VoucherCreateRoleType } from '@core/models/voucher-create';
import { environment } from '@env/environment';
import { Observable, map } from 'rxjs';

@Injectable({
  providedIn: 'root',
})
export class LookupCommandService {
  constructor(private http: HttpClient) {}

  private cacheDepartments$: Observable<ListResult<Department>>;
  private cacheCompanies$: Observable<ListResult<Company>>;
  private cacheCountries$: Observable<ListResult<Country>>;
  private cacheVoucherTypes$: Observable<ListResult<VoucherType>>;
  private cacheVoucherDeliveryTimeType$: Observable<ListResult<VoucherDeliveryTimeType>>;
  private cacheVoucherConditionType$: Observable<ListResult<VoucherConditionType>>;
  private cacheVoucherDepartmentType$: Observable<ListResult<VoucherDepartmentType>>;

  getDepartments(refresh?: boolean): Observable<ListResult<Department>> {
    if (refresh || !this.cacheDepartments$) {
      (this.cacheDepartments$ = this.http.get<ListResult<Department>>(
        `${environment.serviceLookupUrl}/departments`
      )).pipe(
        map((res: ListResult<Department>) => {
          return res.data;
        })
      );
    }
    return this.cacheDepartments$;
  }

  getCompanies(
    roleType: VoucherCreateRoleType = VoucherCreateRoleType.NONE
  ): Observable<ListResult<Company>> {
    let params = new HttpParams();
    params = params.append('roleType', roleType);

    return this.http.get<ListResult<Company>>(`${environment.serviceLookupUrl}/companies`, {
      params: params,
    });
  }

  getCountries(refresh?: boolean): Observable<ListResult<Country>> {
    if (refresh || !this.cacheCountries$) {
      (this.cacheCountries$ = this.http.get<ListResult<Country>>(
        `${environment.serviceLookupUrl}/countries`
      )).pipe(
        map((res: ListResult<Country>) => {
          return res.data;
        })
      );
    }
    return this.cacheCountries$;
  }

  getTypes(refresh?: boolean): Observable<ListResult<VoucherType>> {
    if (refresh || !this.cacheVoucherTypes$) {
      (this.cacheVoucherTypes$ = this.http.get<ListResult<VoucherType>>(
        `${environment.serviceLookupUrl}/types`
      )).pipe(
        map((res: ListResult<VoucherType>) => {
          return res.data;
        })
      );
    }
    return this.cacheVoucherTypes$;
  }

  getDeliveryTime(refresh?: boolean): Observable<ListResult<VoucherDeliveryTimeType>> {
    if (refresh || !this.cacheVoucherDeliveryTimeType$) {
      (this.cacheVoucherDeliveryTimeType$ = this.http.get<ListResult<VoucherDeliveryTimeType>>(
        `${environment.serviceLookupUrl}/deliveryTimes`
      )).pipe(
        map((res: ListResult<VoucherDeliveryTimeType>) => {
          return res.data;
        })
      );
    }
    return this.cacheVoucherDeliveryTimeType$;
  }

  getVoucherConditionType(refresh?: boolean): Observable<ListResult<VoucherConditionType>> {
    if (refresh || !this.cacheVoucherConditionType$) {
      (this.cacheVoucherConditionType$ = this.http.get<ListResult<VoucherConditionType>>(
        `${environment.serviceLookupUrl}/conditions`
      )).pipe(
        map((res: ListResult<VoucherConditionType>) => {
          return res.data;
        })
      );
    }
    return this.cacheVoucherConditionType$;
  }

  getVoucherDepartments(
    roleType: VoucherCreateRoleType
  ): Observable<ListResult<VoucherDepartmentType>> {
    let params = new HttpParams();
    params = params.append('roleType', roleType);

    return this.http.get<ListResult<VoucherDepartmentType>>(
      `${environment.serviceLookupUrl}/departments`,
      { params: params }
    );
  }

  getOfficeCompanies(): Observable<ListResult<string>> {
    return this.http.get<ListResult<string>>(`${environment.serviceLookupUrl}/office/project`);
  }
}
