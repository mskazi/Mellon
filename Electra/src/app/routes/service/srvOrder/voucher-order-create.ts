import { Injectable } from '@angular/core';
import { IPersistObjectService } from '@core/forms/base-form.component';
import { VoucherCreateRoleType } from '@core/models/voucher-create';
import { VoucherCommandService } from '@core/services/voucher-commands.service';
import { Observable, of } from 'rxjs';

@Injectable()
export class VoucherServiceCreateOrderService implements IPersistObjectService<any> {
  constructor(private voucherCommandService: VoucherCommandService) {}

  private roleType: VoucherCreateRoleType;

  setMode(roleType: VoucherCreateRoleType) {
    this.roleType = roleType;
  }

  save(data: any): Observable<any> {
    return this.voucherCommandService.createNewVoucherContact(data, this.roleType);
  }

  load(params: any): Observable<any> {
    return of({} as any);
  }
}
