import { Component, Input, OnInit } from '@angular/core';
import { ControlContainer, FormGroup, UntypedFormBuilder, UntypedFormGroup } from '@angular/forms';
import { LookUpNumber, VoucherConditionType, VoucherDeliveryTimeType } from '@core/models/lookup';
import { decimalNumberPrecision } from '@shared/validations/decimal-validator.component';
import { Observable, of } from 'rxjs';
/**
 *
 * Department component
 */
@Component({
  selector: 'app-voucher-special-details',
  templateUrl: './voucher-special-details.component.html',
  providers: [],
}) /* ,  */
export class VoucherSpecialsDetailsComponent implements OnInit {
  @Input() optionsDeliveryTimeType$: Observable<VoucherDeliveryTimeType[]>;
  @Input() optionConditionType$: Observable<VoucherConditionType[]>;
  private form: FormGroup;
  showDeliveryTimePanel = false;
  showCODAmountPanel = false;

  optionsYesNo$: Observable<LookUpNumber[]> = of([
    { id: 0, description: 'No' },
    { id: 1, description: 'Yes' },
  ]);

  constructor(protected controlContainer: ControlContainer) {}

  ngOnInit() {
    this.form = <UntypedFormGroup>this.controlContainer.control;
    this.startWatchFormChanges();
  }

  static createForm(fb: UntypedFormBuilder): FormGroup {
    return fb.group({
      voucherCondition: ['', []],
      voucherCodAmount: ['', [decimalNumberPrecision(2)]],
      voucherSaturdayDelivery: [0, []],
      voucherDeliveryTime: ['', []],
    });
  }

  private startWatchFormChanges() {
    this.form.controls['voucherSaturdayDelivery'].valueChanges.subscribe((data: number) => {
      if (data === 1) {
        this.showDeliveryTimePanel = true;
      } else {
        this.showDeliveryTimePanel = false;
      }
      this.form.controls['voucherDeliveryTime'].setValue(null);
    });

    this.form.controls['voucherCondition'].valueChanges.subscribe((data: string) => {
      if (data === 'COD') {
        this.showCODAmountPanel = true;
      } else {
        this.showCODAmountPanel = false;
      }
      this.form.controls['voucherCodAmount'].setValue(null);
    });
  }
}
