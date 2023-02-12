import { HttpClient } from '@angular/common/http';
import { Token } from '@angular/compiler';
import { Injectable } from '@angular/core';
import { SERVICE_VOUCHER_DATA } from '@core/data.service';
import { Observable, of } from 'rxjs';

@Injectable({
  providedIn: 'root',
})
export class VoucherService {
  constructor(protected http: HttpClient) {}

  loadServiceVoucher(): Observable<any[]> {
    return of(SERVICE_VOUCHER_DATA);
  }
}
