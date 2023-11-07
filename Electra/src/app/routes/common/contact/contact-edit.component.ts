import { Component, OnInit } from '@angular/core';
import {
  AbstractControl,
  FormBuilder,
  FormGroup,
  ValidationErrors,
  ValidatorFn,
  Validators,
} from '@angular/forms';
import { ListResult } from '@core/core-model';
import { CommonDialogEditComponent } from '@core/forms/base-form-edit-dialog.component';
import { ContactMode } from '@core/models/contac';
import { ContactEditItem } from '@core/models/contact-edit-item';
import { ContactSearchItem } from '@core/models/contact-search-item';
import { Country, Department } from '@core/models/lookup';
import { LookupCommandService } from '@core/services/lookup-commands.service';
import { Observable, map } from 'rxjs';
import { ContactEditService } from './contact-edit-service';
import * as _ from 'lodash';

@Component({
  selector: 'app-contract-edit',
  templateUrl: './contact-edit.component.html',
  styleUrls: ['./contact-edit.component.scss'],
  providers: [ContactEditService],
})
export class ContactEditComponent
  extends CommonDialogEditComponent<ContactEditModalData, ContactEditItem, any>
  implements OnInit
{
  dialogData: ContactEditModalData;
  optionsCountries$: Observable<Country[]>;

  constructor(
    protected fb: FormBuilder,
    private contactEditService: ContactEditService,
    private lookupCommandService: LookupCommandService
  ) {
    super(fb);
    this.form = this.fb.group({
      voucherName: ['', [Validators.minLength(5), Validators.maxLength(500)]],
      voucherContact: ['', [Validators.maxLength(100)]],
      voucherAddress: [
        '',
        [Validators.required, Validators.minLength(5), Validators.maxLength(100)],
      ],
      voucherCity: ['', [Validators.required, Validators.minLength(5), Validators.maxLength(100)]],
      voucherPostCode: [
        '',
        [Validators.required, Validators.minLength(5), Validators.maxLength(6)],
      ],
      voucherCountry: ['', [Validators.required, Validators.minLength(2), Validators.maxLength(6)]],
      voucherPhoneNo: ['', [Validators.maxLength(50), this.atLeastOnePhoneRequired()]],
      voucherMobileNo: ['', [Validators.maxLength(50), this.atLeastOnePhoneRequired()]],
      contactNotes: ['', [Validators.maxLength(250)]],
      active: [true],
    });
  }

  atLeastOnePhoneRequired(): ValidatorFn {
    return (control: AbstractControl): ValidationErrors | null => {
      if (!this.form) {
        return null;
      }
      if (
        this.form.controls['voucherPhoneNo'].value ||
        this.form.controls['voucherMobileNo'].value
      ) {
        return null;
      }
      return { phoneOrMobile: true };
    };
  }

  override loadCompleted(data: ContactEditItem) {
    this.form.patchValue({
      voucherName: data.voucherName,
      voucherContact: data.voucherContact,
      voucherAddress: data.voucherAddress,
      voucherCity: data.voucherCity,
      voucherPostCode: data.voucherPostCode,
      voucherCountry: data.voucherCountry,
      voucherPhoneNo: data.voucherPhoneNo,
      voucherMobileNo: data.voucherMobileNo,
      contactNotes: data.contactNotes,
      active: data.active ?? true,
    });

    this.form.controls['voucherMobileNo'].valueChanges.subscribe((data: string) => {
      this.form.controls['voucherPhoneNo'].markAsTouched();
      this.form.controls['voucherPhoneNo'].updateValueAndValidity({ emitEvent: false });
    });

    this.form.controls['voucherPhoneNo'].valueChanges.subscribe((data: string) => {
      this.form.controls['voucherMobileNo'].markAsTouched();
      this.form.controls['voucherMobileNo'].updateValueAndValidity({ emitEvent: false });
    });
  }

  ngOnInit(): void {
    this.initializeDialogService(this.contactEditService);
    this.optionsCountries$ = this.lookupCommandService
      .getCountries()
      .pipe(map((result: ListResult<Country>) => result.data));

    this.load(this.dialogData.item).subscribe();
  }

  override patchObject() {
    const data = _.assignIn(this.data, this.form.value);
    data.mode = this.dialogData.mode;
    return data;
  }
}

export interface ContactEditModalData {
  item: ContactSearchItem;
  mode: ContactMode;
}
