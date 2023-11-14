import { COMMA, ENTER } from '@angular/cdk/keycodes';
import { Component, ElementRef, OnInit, ViewChild } from '@angular/core';
import { FormControl, FormGroup, UntypedFormBuilder, Validators } from '@angular/forms';
import { MatAutocompleteSelectedEvent } from '@angular/material/autocomplete';
import { MatChipInputEvent } from '@angular/material/chips';
import { ListResult } from '@core/core-model';
import { BaseFormComponent } from '@core/forms/base-form.component';
import { LookUpString } from '@core/models/lookup';
import { NotificationService } from '@core/notification.service';
import { LookupCommandService } from '@core/services/lookup-commands.service';
import { VoucherCommandService } from '@core/services/voucher-commands.service';
import { DialogDynamicService } from '@shared/components/dialog/dialog.service';
import { Utilities } from '@shared/utils/utilities';
import * as _ from 'lodash';
import { Observable, Subject, finalize, map, mergeMap, startWith, tap } from 'rxjs';
@Component({
  selector: 'app-office-pritn-criteria',
  templateUrl: './print-criteria.component.html',
  providers: [],
}) /* ,  */
export class OfficePrintCriteriaComponent extends BaseFormComponent implements OnInit {
  optionsCompanySubject = new Subject<LookUpString[]>();
  optionsCompany$: Observable<LookUpString[]> = this.optionsCompanySubject.asObservable();
  formId = Utilities.GenGUI();
  form: FormGroup;
  working = false;

  optionsVouchers: any[] = [];
  separatorKeysCodes: number[] = [ENTER, COMMA];
  filteredVouchers: Observable<any[]>;
  vouchers: any[] = [];
  @ViewChild('voucherInput') voucherInput: ElementRef<HTMLInputElement>;
  voucherCtrl = new FormControl('');
  constructor(
    fb: UntypedFormBuilder,
    private lookupCommandService: LookupCommandService,
    private dialogDynamicService: DialogDynamicService,
    private voucherCommandService: VoucherCommandService,
    private notificationService: NotificationService
  ) {
    super(fb);
    this.form = this.fb.group({
      company: ['', [Validators.required]],
    });

    this.filteredVouchers = this.voucherCtrl.valueChanges.pipe(
      startWith(null),
      map((voucher: string | null) =>
        voucher ? this._filter(voucher) : this.optionsVouchers.slice()
      )
    );
  }

  reset() {
    this.form.controls['company'].setValue(null);
    this.form.markAsPristine();
  }

  ngOnInit() {
    this.getCompanies();
    this.form.controls['company'].valueChanges
      .pipe(
        mergeMap((selectedCompany: string) => {
          return this.voucherCommandService.officeCommands.getVouchersPrint(selectedCompany);
        }),
        tap((response: any) => {
          this.optionsVouchers = response.data;
          this.voucherCtrl.setValue(null);
          this.vouchers = [];
        })
      )
      .subscribe();
  }

  getCompanies() {
    this.lookupCommandService
      .getOfficeCompanies()
      .pipe(
        tap((result: ListResult<string>) => {
          if (result.data?.length > 0 && result.data[0]) {
            this.form.controls['company'].setValue(result.data[0]);
          }
          this.optionsCompanySubject.next(
            result.data.map(i => {
              return { id: i, description: i } as LookUpString;
            })
          );
        })
      )
      .subscribe();
  }

  add(event: MatChipInputEvent): void {
    const value = (event.value || '').trim();

    // Add our fruit
    if (value) {
      this.vouchers.push(value);
    }

    // Clear the input value
    event.chipInput!.clear();

    this.voucherCtrl.setValue(null);
  }

  remove(voucher: any): void {
    const index = this.vouchers.indexOf(voucher);
    if (index >= 0) {
      this.vouchers.splice(index, 1);
      const find = this.optionsVouchers.find(x => x.voucherNo === voucher.voucherNo);
      if (find) {
        find.selected = false;
      }
    }
  }

  selected(event: MatAutocompleteSelectedEvent): void {
    this.vouchers.push(event.option.value);
    this.voucherInput.nativeElement.value = '';
    this.voucherCtrl.setValue(null);

    const find = this.optionsVouchers.find(x => x.voucherNo === event.option.value.voucherNo);
    if (find) {
      find.selected = true;
    }
  }

  private _filter(value: string): string[] {
    if (_.isObject(value)) {
      return this.optionsVouchers;
    }
    const filterValue = value.toLowerCase();

    return this.optionsVouchers.filter(
      voucher =>
        voucher?.voucherNo?.toLowerCase().includes(filterValue) ||
        voucher?.voucherName?.toLowerCase().includes(filterValue) ||
        voucher?.voucherCity?.toLowerCase().includes(filterValue) ||
        voucher?.voucherContact?.toLowerCase().includes(filterValue)
    );
  }

  print() {
    if (!this.form.valid) {
      this.markTouchedFields(this.form);
      this.form.updateValueAndValidity();
      return;
    }

    if (this.vouchers && this.vouchers.length === 0) {
      this.notificationService.error('You need to select at least 1 voucher for printing');
      return;
    }

    if (this.working) {
      return;
    }
    this.working = true;
    this.voucherCommandService.officeCommands
      .print(this.vouchers.map(x => x.voucherNo))
      .pipe(
        finalize(() => {
          this.working = false;
        })
      )
      .subscribe(file => {
        this.vouchers = [];
        _.forEach(this.optionsVouchers, (x: any) => {
          x.selected = false;
        });
        this.voucherCtrl.setValue(null);
        Utilities.downloadFileOnNewTab(file, `Voucher_Print.pdf`);
      });
  }
}
