import { Injectable } from '@angular/core';
import { IPersistObjectService } from '@core/forms/base-form.component';
import { VoucherCommandService } from '@core/services/voucher-commands.service';
import { Observable, of } from 'rxjs';

@Injectable()
export class VoucherServiceScanSendService implements IPersistObjectService<any> {
  constructor(private voucherCommandService: VoucherCommandService) {}

  save(data: any): Observable<any> {
    return this.voucherCommandService.serviceCommands.scan(data);
  }

  load(params: any): Observable<any> {
    return of({} as any);
  }
}
