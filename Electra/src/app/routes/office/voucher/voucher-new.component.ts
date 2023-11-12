import { Component, OnInit } from '@angular/core';
import { FormGroup, UntypedFormBuilder, Validators } from '@angular/forms';
import { CommonDialogEditComponent } from '@core/forms/base-form-edit-dialog.component';
import { VoucherCommandService } from '@core/services/voucher-commands.service';
import { TranslateService } from '@ngx-translate/core';
import { OfficeVoucherNewService } from './voucher-new-service';
import { ListResult } from '@core/core-model';
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
import { ContactsCommandService } from '@core/services/contact-commands.service';
import { LookupCommandService } from '@core/services/lookup-commands.service';
import {
  Observable,
  forkJoin,
  map,
  mergeMap,
  of,
  tap,
  merge,
  Subject,
  BehaviorSubject,
} from 'rxjs';
import { ContactEditItem } from '@core/models/contact-edit-item';
import { CarrierCommandService } from '@core/services/carrier-commands.service';
import { VoucherCarrier } from '@core/models/carrier';
import * as _ from 'lodash';
import { MembersCommandService } from '@core/services/member-commands.service';
import { MemberItem } from '@core/models/member';
import { decimalNumberPrecision } from '@shared/validations/decimal-validator.component';
import { integerNumber } from '@shared/validations/integer-validator.component copy';
import { DialogResponse } from '@shared/components/dialog/dialog';
import { Voucher } from '@core/models/voucher-details-item';
import { Utilities } from '@shared/utils/utilities';

/**
 *
 * Department component
 */
@Component({
  selector: 'app-office-voucher-new',
  templateUrl: './voucher-new.component.html',
  providers: [OfficeVoucherNewService],
}) /* ,  */
export class OfficeVoucherNewComponent
  extends CommonDialogEditComponent<any, any, any>
  implements OnInit
{
  optionsAction$: Observable<LookUpNumber[]> = of([
    { id: 0, description: 'Person' },
    { id: 1, description: 'PickUp' },
  ]);
  optionsDeliverTo$: Observable<LookUpString[]> = of([
    { id: 'person', description: 'Deliver' },
    { id: 'po_box', description: 'PO Box' },
  ]);

  optionsYesNo$: Observable<LookUpNumber[]> = of([
    { id: 0, description: 'No' },
    { id: 1, description: 'Yes' },
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

  showDeliveryTimePanel = false;
  showCODAmountPanel = false;
  constructor(
    fb: UntypedFormBuilder,
    private voucherCommandService: VoucherCommandService,
    private carrierCommandService: CarrierCommandService,
    private officeVoucherNewService: OfficeVoucherNewService,
    private contactsCommandService: ContactsCommandService,
    private lookupCommandService: LookupCommandService,
    private membersCommandService: MembersCommandService
  ) {
    super(fb);
    this.form = this.fb.group({
      contactForm: this.fb.group({
        voucherName: ['', [Validators.minLength(5), Validators.maxLength(500)]],
        voucherContact: ['', [Validators.maxLength(100)]],
        voucherAddress: [
          '',
          [Validators.required, Validators.minLength(5), Validators.maxLength(100)],
        ],
        voucherCity: [
          '',
          [Validators.required, Validators.minLength(5), Validators.maxLength(100)],
        ],
        voucherPostCode: [
          '',
          [Validators.required, Validators.minLength(5), Validators.maxLength(6)],
        ],
        voucherCountry: [
          '',
          [Validators.required, Validators.minLength(2), Validators.maxLength(6)],
        ],
        voucherPhoneNo: ['', [Validators.maxLength(50)]],
        voucherMobileNo: ['', [Validators.maxLength(50)]],
        contactNotes: ['', [Validators.maxLength(250)]],
        active: [true],
      }),
      voucherDetails: this.fb.group({
        voucherAction: [0, [Validators.required]],
        voucherCarrier: ['', [Validators.required]],
        voucherCompany: ['', [Validators.required]],
        voucherDepartment: ['', [Validators.required]],
        voucherMember: ['', [Validators.required]],
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

        voucherCondition: ['', []],
        voucherCodAmount: ['', [decimalNumberPrecision(2)]],
        voucherSaturdayDelivery: [0, []],
        voucherDeliveryTime: ['', []],
      }),
    });
  }

  get contactForm(): FormGroup {
    return this.form.controls['contactForm'] as FormGroup;
  }

  get voucherDetailsForm(): FormGroup {
    return this.form.controls['voucherDetails'] as FormGroup;
  }

  ngOnInit() {
    this.startWatchFormChanges();
    this.loadLists();
    this.form.controls['contactForm'].disable();
    this.initializeDialogService(this.officeVoucherNewService);
    // before load
    this.onLoad$ = forkJoin([this.contactsCommandService.getContact(this.dialogData)]).pipe(
      tap(([contact]: [ContactEditItem]) => {
        this.contactForm.patchValue(contact);
      }),
      mergeMap(([contact]: [ContactEditItem]) => {
        return forkJoin([
          this.carrierCommandService.getVoucherCarriers(contact.voucherPostCode),
          this.carrierCommandService.getVoucherCarrier(contact.voucherPostCode),
        ]);
      }),
      tap(([voucherCarriers, selectedVoucherCarrier]: [VoucherCarrier[], VoucherCarrier]) => {
        if (selectedVoucherCarrier && voucherCarriers) {
          const exist = _.find(voucherCarriers, x => x.id === selectedVoucherCarrier.id);
          if (exist) {
            this.voucherDetailsForm.controls['voucherCarrier'].setValue(exist.id);
          }
        }
        this.optionsCarrierSubject.next(voucherCarriers);
      })
    );
    this.load(this.dialogData.item).subscribe();
  }

  private loadLists() {
    this.lookupCommandService
      .getCountries()
      .pipe(
        tap((result: ListResult<Country>) => {
          this.optionsCountriesSubject.next(result.data);
        })
      )
      .subscribe();

    this.lookupCommandService
      .getCompanies()
      .pipe(
        tap((result: ListResult<Company>) => {
          if (result.data?.length > 0 && result.data[0].id) {
            this.voucherDetailsForm.controls['voucherCompany'].setValue(result.data[0].id);
          }
          this.optionsCompanySubject.next(result.data);
        })
      )
      .subscribe();

    this.lookupCommandService
      .getVoucherDepartmentsOffice()
      .pipe(
        tap((result: ListResult<Department>) => {
          this.optionsDepartmentSubject.next(result.data);
        })
      )
      .subscribe();

    this.lookupCommandService
      .getTypesOffice()
      .pipe(
        tap((result: ListResult<VoucherType>) => {
          if (result.data?.length > 0 && result.data[0].id) {
            this.voucherDetailsForm.controls['voucherType'].setValue(result.data[0].id);
          }
          this.optionsTypeSubject.next(result.data);
        })
      )
      .subscribe();

    this.lookupCommandService
      .getDeliveryTimeOffice()
      .pipe(
        tap((result: ListResult<VoucherDeliveryTimeType>) => {
          const data: VoucherDeliveryTimeType[] = [];
          data.push({ id: 0, description: `* ANY *` });
          _.each(result.data, i => {
            data.push({ id: i.id, description: `${i.description} (${i.id})` });
          });
          if (result.data?.length > 0 && result.data[0].id) {
            this.voucherDetailsForm.controls['voucherDeliveryTime'].setValue(data[0].id);
          }
          this.optionsDeliveryTimeTypeSubject.next(data);
        })
      )
      .subscribe();

    this.lookupCommandService
      .getVoucherConditionTypeOffice()
      .pipe(
        tap((result: ListResult<VoucherConditionType>) => {
          const data: VoucherConditionType[] = [];
          data.push({ id: 'ZZZ', description: `Simple Delivery` });
          _.each(result.data, i => {
            data.push({ id: i.id, description: `${i.description} (${i.id})` });
          });
          this.optionConditionTypeSubject.next(data);
          if (result.data?.length > 0 && result.data[0].id) {
            this.voucherDetailsForm.controls['voucherCondition'].setValue(data[0].id);
          }
        })
      )
      .subscribe();
  }

  override loadCompleted(data: any) {}

  override patchObject() {
    return { contactId: this.dialogData, voucherDetails: this.voucherDetailsForm.getRawValue() };
  }

  private startWatchFormChanges() {
    merge(
      this.voucherDetailsForm.controls['voucherCompany'].valueChanges,
      this.voucherDetailsForm.controls['voucherDepartment'].valueChanges
    ).subscribe(() => {
      if (
        this.voucherDetailsForm.controls['voucherCompany'].value &&
        this.voucherDetailsForm.controls['voucherDepartment'].value
      ) {
        this.membersCommandService
          .getMembers(
            this.voucherDetailsForm.controls['voucherCompany'].value,
            this.voucherDetailsForm.controls['voucherDepartment'].value
          )
          .pipe(
            map((result: ListResult<MemberItem>) =>
              _.map(result.data, x => {
                return { id: x.id, description: x.memberName };
              })
            ),
            tap((data: any) => {
              this.voucherDetailsForm.controls['voucherMember'].setValue(null);
              this.optionsMemberSubject.next(data);
            })
          )
          .subscribe();
      } else {
        this.voucherDetailsForm.controls['voucherMember'].setValue(null);
        this.optionsMemberSubject.next([]);
      }
    });

    this.voucherDetailsForm.controls['voucherSaturdayDelivery'].valueChanges.subscribe(
      (data: number) => {
        if (data === 1) {
          this.showDeliveryTimePanel = true;
        } else {
          this.showDeliveryTimePanel = false;
        }
        this.voucherDetailsForm.controls['voucherDeliveryTime'].setValue(null);
      }
    );

    this.voucherDetailsForm.controls['voucherCondition'].valueChanges.subscribe((data: string) => {
      if (data === 'COD') {
        this.showCODAmountPanel = true;
      } else {
        this.showCODAmountPanel = false;
      }
      this.voucherDetailsForm.controls['voucherCodAmount'].setValue(null);
    });
  }

  /**
   * Method to close dialog and save data
   */
  getAcceptedDialogData(): Observable<DialogResponse<any>> {
    return this.save().pipe(
      mergeMap((data: Voucher) => {
        if (data.carrierActionType !== 1) {
          return this.voucherCommandService.print(data.id).pipe(
            map((file: any) => {
              Utilities.downloadFileOnNewTab(file, `Voucher_Print.pdf`);
              return data;
            })
          );
        }
        return of(data);
      }),
      map((data: Voucher): DialogResponse<Voucher> => {
        return {
          close: true,
          dataDialog: data,
        };
      })
    );
  }
}
