import { Component, Input } from '@angular/core';
import { FormGroup, UntypedFormBuilder, Validators } from '@angular/forms';
import { VoucherCarrier } from '@core/models/carrier';
import { Company, Department, LookUpNumber, LookUpString, VoucherType } from '@core/models/lookup';
import { decimalNumberPrecision } from '@shared/validations/decimal-validator.component';
import { integerNumber } from '@shared/validations/integer-validator.component copy';
import { Observable } from 'rxjs';
/**
 *
 * Department component
 */
@Component({
  selector: 'app-voucher-details',
  templateUrl: './voucher-details.component.html',
  providers: [],
}) /* ,  */
export class VoucherDetailsComponent {
  @Input() optionsDeliverTo$: Observable<LookUpString[]>;
  @Input() optionsAction$: Observable<LookUpNumber[]>;
  @Input() optionsCarrier$: Observable<VoucherCarrier[]>;
  @Input() optionsDepartment$: Observable<Department[]>;
  @Input() optionsMember$: Observable<LookUpNumber[]>;
  @Input() optionsCompany$: Observable<Company[]>;
  @Input() optionsType$: Observable<VoucherType[]>;
  @Input() optionsProject$: Observable<any[]>;
  @Input() showProject = false;

  static createForm(fb: UntypedFormBuilder): FormGroup {
    return fb.group({
      voucherAction: [0, [Validators.required]],
      voucherCarrier: ['', [Validators.required]],
      voucherCompany: ['', [Validators.required]],
      voucherDepartment: ['', [Validators.required]],
      voucherMember: ['', [Validators.required]],
      voucherProject: ['', []],
      voucherType: ['', []],
      voucherDeliverTo: ['person', []],
      voucherWeight: [
        0.01,
        [
          decimalNumberPrecision(2),
          Validators.required,
          Validators.min(0.01),
          Validators.max(500000),
        ],
      ],
      voucherQuantities: [
        1,
        [Validators.required, Validators.min(1), Validators.max(500000), integerNumber()],
      ],
      comments: ['', [Validators.maxLength(500)]],
      voucherReference: ['', [Validators.maxLength(20)]],
    });
  }
}
