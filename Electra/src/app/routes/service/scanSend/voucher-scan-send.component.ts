import { Component, OnInit } from '@angular/core';
import { FormGroup, UntypedFormBuilder, Validators } from '@angular/forms';
import { ListResult } from '@core/core-model';
import { BaseFormEditComponent } from '@core/forms/base-form-edit.component';
import { LookUpNumber, VoucherConditionType, VoucherDeliveryTimeType } from '@core/models/lookup';
import { VoucherCreateRoleType } from '@core/models/voucher-create';
import { CarrierCommandService } from '@core/services/carrier-commands.service';
import { LookupCommandService } from '@core/services/lookup-commands.service';
import { VoucherCommandService } from '@core/services/voucher-commands.service';
import { decimalNumberPrecision } from '@shared/validations/decimal-validator.component';
import * as _ from 'lodash';
import { BehaviorSubject, Observable, Subject, of, tap } from 'rxjs';
import { VoucherServiceScanSendService } from './voucher-scan-send';

@Component({
  selector: 'app-voucher-service-scan-send',
  templateUrl: './voucher-scan-send.component.html',
  providers: [VoucherServiceScanSendService],
}) /* ,  */
export class ServiceScanSendComponent extends BaseFormEditComponent<any> implements OnInit {
  optionsYesNo$: Observable<LookUpNumber[]> = of([
    { id: 0, description: 'No' },
    { id: 1, description: 'Yes' },
  ]);

  optionsDeliveryTimeTypeSubject = new BehaviorSubject<VoucherDeliveryTimeType[]>([]);
  optionsDeliveryTimeType$: Observable<VoucherDeliveryTimeType[]> =
    this.optionsDeliveryTimeTypeSubject.asObservable();

  optionConditionTypeSubject = new Subject<VoucherConditionType[]>();
  optionConditionType$: Observable<VoucherConditionType[]> =
    this.optionConditionTypeSubject.asObservable();

  form: FormGroup;
  showDeliveryTimePanel = false;
  showCODAmountPanel = false;

  initValueVoucherDeliveryTime: number;
  constructor(
    fb: UntypedFormBuilder,
    private voucherCommandService: VoucherCommandService,
    private carrierCommandService: CarrierCommandService,
    private voucherServiceScanSendService: VoucherServiceScanSendService,
    private lookupCommandService: LookupCommandService
  ) {
    super(fb);
    this.form = this.fb.group({
      scanSerial: ['', [Validators.required, Validators.maxLength(50)]],
      voucherCondition: ['', []],
      voucherCodAmount: ['', [decimalNumberPrecision(2)]],
      voucherSaturdayDelivery: [0, []],
      voucherDeliveryTime: ['', []],
    });
    this.form.controls['voucherCondition'].disable();
  }

  reset() {
    this.form.controls['scanSerial'].setValue(null);
    this.form.controls['voucherSaturdayDelivery'].setValue(0);
    this.form.controls['voucherCodAmount'].setValue(0);
    this.form.controls['voucherDeliveryTime'].setValue(this.initValueVoucherDeliveryTime);
  }

  ngOnInit() {
    this.init(this.voucherServiceScanSendService);
    this.startWatchFormChanges();
    this.loadLists();
    this.load({}).subscribe();
  }

  private loadLists() {
    this.lookupCommandService
      .getDeliveryTime()
      .pipe(
        tap((result: ListResult<VoucherDeliveryTimeType>) => {
          const data: VoucherDeliveryTimeType[] = [];
          data.push({ id: 0, description: `* ANY *` });
          _.each(result.data, i => {
            data.push({ id: i.id, description: `${i.description} (${i.id})` });
          });
          if (result.data?.length > 0 && result.data[0].id) {
            this.initValueVoucherDeliveryTime = data[0].id;
            this.form.controls['voucherDeliveryTime'].setValue(data[0].id);
          }
          this.optionsDeliveryTimeTypeSubject.next(data);
        })
      )
      .subscribe();

    this.lookupCommandService
      .getVoucherConditionType()
      .pipe(
        tap((result: ListResult<VoucherConditionType>) => {
          const data: VoucherConditionType[] = [];
          data.push({ id: 'ZZZ', description: `Simple Delivery` });
          _.each(result.data, i => {
            data.push({ id: i.id, description: `${i.description} (${i.id})` });
          });
          this.optionConditionTypeSubject.next(data);
          if (result.data?.length > 0 && result.data[0].id) {
            this.form.controls['voucherCondition'].setValue(data[0].id);
          }
        })
      )
      .subscribe();
  }
  scan() {
    super.save().subscribe();
  }

  override loadCompleted(data: any) {}

  override patchObject() {
    return this.form.getRawValue();
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

export interface VoucherCreateNewContactComponentData {
  contactId: number;
  roleType: VoucherCreateRoleType;
}
