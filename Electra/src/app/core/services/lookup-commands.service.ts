import { HttpClient } from '@angular/common/http';
import { Injectable } from '@angular/core';
import { ListResult } from '@core/core-model';
import { Company, Country, Department } from '@core/models/lookup';
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

  getDepartments(refresh?: boolean): Observable<ListResult<Department>> {
    if (refresh || !this.cacheDepartments$) {
      (this.cacheDepartments$ = this.http.get<ListResult<Department>>(
        `${environment.serviceLookupUrl}/departments`
      )).pipe(
        map((res: ListResult<Department>) => {
          //   res.data = res.data.sort((a, b) => a.description.localeCompare(b.description));
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
          //   res.data = res.data.sort((a, b) => a.description.localeCompare(b.description));
          return res.data;
        })
      );
    }
    return this.cacheCompanies$;
  }

  getCountries(refresh?: boolean): Observable<ListResult<Country>> {
    if (refresh || !this.cacheCompanies$) {
      (this.cacheCountries$ = this.http.get<ListResult<Country>>(
        `${environment.serviceLookupUrl}/countries`
      )).pipe(
        map((res: ListResult<Country>) => {
          //   res.data = res.data.sort((a, b) => a.description.localeCompare(b.description));
          return res.data;
        })
      );
    }
    return this.cacheCountries$;
  }
}
