import { HttpClient } from '@angular/common/http';
import { Injectable } from '@angular/core';
import { ListResult } from '@core/core-model';
import {
  Company,
  Country,
  Department,
  VoucherConditionType,
  VoucherDeliveryTimeType,
  VoucherDepartmentType,
  VoucherType,
} from '@core/models/lookup';
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

  getCompanies(refresh?: boolean): Observable<ListResult<Company>> {
    if (refresh || !this.cacheCompanies$) {
      (this.cacheCompanies$ = this.http.get<ListResult<Company>>(
        `${environment.serviceLookupUrl}/companies`
      )).pipe(
        map((res: ListResult<Company>) => {
          return res.data;
        })
      );
    }
    return this.cacheCompanies$;
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

  getTypesOffice(refresh?: boolean): Observable<ListResult<VoucherType>> {
    if (refresh || !this.cacheVoucherTypes$) {
      (this.cacheVoucherTypes$ = this.http.get<ListResult<VoucherType>>(
        `${environment.serviceLookupUrl}/types/office`
      )).pipe(
        map((res: ListResult<VoucherType>) => {
          return res.data;
        })
      );
    }
    return this.cacheVoucherTypes$;
  }

  getDeliveryTimeOffice(refresh?: boolean): Observable<ListResult<VoucherDeliveryTimeType>> {
    if (refresh || !this.cacheVoucherDeliveryTimeType$) {
      (this.cacheVoucherDeliveryTimeType$ = this.http.get<ListResult<VoucherDeliveryTimeType>>(
        `${environment.serviceLookupUrl}/deliveryTimes/office`
      )).pipe(
        map((res: ListResult<VoucherDeliveryTimeType>) => {
          return res.data;
        })
      );
    }
    return this.cacheVoucherDeliveryTimeType$;
  }

  getVoucherConditionTypeOffice(refresh?: boolean): Observable<ListResult<VoucherConditionType>> {
    if (refresh || !this.cacheVoucherConditionType$) {
      (this.cacheVoucherConditionType$ = this.http.get<ListResult<VoucherConditionType>>(
        `${environment.serviceLookupUrl}/conditions/office`
      )).pipe(
        map((res: ListResult<VoucherConditionType>) => {
          return res.data;
        })
      );
    }
    return this.cacheVoucherConditionType$;
  }

  getVoucherDepartmentsOffice(refresh?: boolean): Observable<ListResult<VoucherDepartmentType>> {
    if (refresh || !this.cacheVoucherDepartmentType$) {
      (this.cacheVoucherDepartmentType$ = this.http.get<ListResult<VoucherDepartmentType>>(
        `${environment.serviceLookupUrl}/departments/office`
      )).pipe(
        map((res: ListResult<VoucherDepartmentType>) => {
          return res.data;
        })
      );
    }
    return this.cacheVoucherDepartmentType$;
  }
}
