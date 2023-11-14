import { Component, Input } from '@angular/core';
import { FormGroup, UntypedFormBuilder, Validators } from '@angular/forms';
import { Country } from '@core/models/lookup';
import { Observable } from 'rxjs';
/**
 *
 * Department component
 */
@Component({
  selector: 'app-voucher-send-to-details',
  templateUrl: './voucher-send-to-details.component.html',
  providers: [],
}) /* ,  */
export class VoucherSendToDetailsComponent {
  @Input() optionsCountries$: Observable<Country[]>;

  static createForm(fb: UntypedFormBuilder): FormGroup {
    return fb.group({
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
      voucherPhoneNo: ['', [Validators.maxLength(50)]],
      voucherMobileNo: ['', [Validators.maxLength(50)]],
      contactNotes: ['', [Validators.maxLength(250)]],
      active: [true],
    });
  }
}
