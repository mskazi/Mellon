import { Injectable } from '@angular/core';
import { FormGroup, UntypedFormBuilder } from '@angular/forms';
import { ListResult } from '@core/core-model';
import { CommonDialogEditComponent } from '@core/forms/base-form-edit-dialog.component';
import { VoucherCarrier } from '@core/models/carrier';
import {
  Company,
  Country,
  Department,
  LookUpNumber,
  LookUpString,
  VoucherConditionType,
  VoucherDeliveryTimeType,
  VoucherType,
} from '@core/models/lookup';
import { VoucherCreateRoleType } from '@core/models/voucher-create';
import { LookupCommandService } from '@core/services/lookup-commands.service';
import * as _ from 'lodash';
import { BehaviorSubject, Observable, Subject, of, tap } from 'rxjs';
import { VoucherDetailsComponent } from './voucher-details.component';
import { VoucherSendToDetailsComponent } from './voucher-send-to-details.component';
import { VoucherSpecialsDetailsComponent } from './voucher-special-details.component';
/**
 *
 * Department component
 */
@Injectable()
export abstract class VoucherCreateBaseComponent<
  T extends IBaseVoucherBaseNewContactComponentData,
> extends CommonDialogEditComponent<T, any, any> {
  optionsAction$: Observable<LookUpNumber[]> = of([
    { id: 0, description: 'Person' },
    { id: 1, description: 'PickUp' },
  ]);
  optionsDeliverTo$: Observable<LookUpString[]> = of([
    { id: 'person', description: 'Deliver' },
    { id: 'po_box', description: 'PO Box' },
  ]);

  optionsCountriesSubject = new Subject<Country[]>();
  optionsCountries$: Observable<Country[]> = this.optionsCountriesSubject.asObservable();

  optionsCarrierSubject = new Subject<VoucherCarrier[]>();
  optionsCarrier$: Observable<VoucherCarrier[]> = this.optionsCarrierSubject.asObservable();

  optionsCompanySubject = new Subject<Company[]>();
  optionsCompany$: Observable<Company[]> = this.optionsCompanySubject.asObservable();

  optionsDepartmentSubject = new Subject<Department[]>();
  optionsDepartment$: Observable<Department[]> = this.optionsDepartmentSubject.asObservable();
  optionsTypeSubject = new Subject<VoucherType[]>();
  optionsType$: Observable<VoucherType[]> = this.optionsTypeSubject.asObservable();

  optionsDeliveryTimeTypeSubject = new BehaviorSubject<VoucherDeliveryTimeType[]>([]);
  optionsDeliveryTimeType$: Observable<VoucherDeliveryTimeType[]> =
    this.optionsDeliveryTimeTypeSubject.asObservable();

  optionConditionTypeSubject = new Subject<VoucherConditionType[]>();
  optionConditionType$: Observable<VoucherConditionType[]> =
    this.optionConditionTypeSubject.asObservable();

  optionsMemberSubject = new Subject<LookUpNumber[]>();
  optionsMember$: Observable<LookUpNumber[]> = this.optionsMemberSubject.asObservable();

  constructor(
    protected fb: UntypedFormBuilder,
    protected lookupCommandService: LookupCommandService
  ) {
    super(fb);
    this.form = this.fb.group({
      contactForm: VoucherSendToDetailsComponent.createForm(fb),
      voucherDetails: VoucherDetailsComponent.createForm(fb),
      specialDetails: VoucherSpecialsDetailsComponent.createForm(fb),
    });
  }

  get contactForm(): FormGroup {
    return this.form.controls['contactForm'] as FormGroup;
  }

  get voucherDetailsForm(): FormGroup {
    return this.form.controls['voucherDetails'] as FormGroup;
  }

  get specialDetailsForm(): FormGroup {
    return this.form.controls['specialDetails'] as FormGroup;
  }

  public loadLists() {
    this.getCountries();
    this.getCompanies();
    this.getVoucherDepartments();
    this.getTypes();
    this.getDeliveryTime();
    this.getVoucherConditionType();
  }

  getCountries() {
    this.lookupCommandService
      .getCountries()
      .pipe(
        tap((result: ListResult<Country>) => {
          this.optionsCountriesSubject.next(result.data);
        })
      )
      .subscribe();
  }
  getCompanies() {
    this.lookupCommandService
      .getCompanies(this.dialogData.roleType)
      .pipe(
        tap((result: ListResult<Company>) => {
          if (result.data?.length > 0 && result.data[0].id) {
            this.voucherDetailsForm.controls['voucherCompany'].setValue(result.data[0].id);
          }
          this.optionsCompanySubject.next(result.data);
        })
      )
      .subscribe();
  }
  getVoucherDepartments() {
    this.lookupCommandService
      .getVoucherDepartments(this.dialogData.roleType)
      .pipe(
        tap((result: ListResult<Department>) => {
          this.optionsDepartmentSubject.next(result.data);
        })
      )
      .subscribe();
  }

  getTypes() {
    this.lookupCommandService
      .getTypes()
      .pipe(
        tap((result: ListResult<VoucherType>) => {
          if (result.data?.length > 0 && result.data[0].id) {
            this.voucherDetailsForm.controls['voucherType'].setValue(result.data[0].id);
          }
          this.optionsTypeSubject.next(result.data);
        })
      )
      .subscribe();
  }

  getDeliveryTime() {
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
            this.specialDetailsForm.controls['voucherDeliveryTime'].setValue(data[0].id);
          }
          this.optionsDeliveryTimeTypeSubject.next(data);
        })
      )
      .subscribe();
  }

  getVoucherConditionType() {
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
            this.specialDetailsForm.controls['voucherCondition'].setValue(data[0].id);
          }
        })
      )
      .subscribe();
  }
}

export interface IBaseVoucherBaseNewContactComponentData {
  roleType: VoucherCreateRoleType;
}
