import { Component, OnInit } from '@angular/core';
import { FormGroup, UntypedFormBuilder, Validators } from '@angular/forms';
import { BaseFormEditComponent } from '@core/forms/base-form-edit.component';
import { VoucherCreateRoleType } from '@core/models/voucher-create';
import { ContactsCommandService } from '@core/services/contact-commands.service';
import { DialogDynamicService } from '@shared/components/dialog/dialog.service';
import { finalize, mergeMap } from 'rxjs';
import { VoucherServiceOrder } from './voucher-order';
import {
  VoucherCreateNewFromServiceComponent,
  VoucherServiceOrderCreateContactComponentData,
} from './voucher-order-create.component';
import { ContactEditItem } from '@core/models/contact-edit-item';

@Component({
  selector: 'app-voucher-service-order',
  templateUrl: './voucher-order.component.html',
  providers: [VoucherServiceOrder],
}) /* ,  */
export class ServiceOrderComponent extends BaseFormEditComponent<any> implements OnInit {
  form: FormGroup;
  working = false;
  constructor(
    fb: UntypedFormBuilder,
    private voucherServiceOrder: VoucherServiceOrder,
    private dialogDynamicService: DialogDynamicService,
    private contactsCommandService: ContactsCommandService
  ) {
    super(fb);
    this.form = this.fb.group({
      order: ['', [Validators.required, Validators.maxLength(50)]],
    });
  }
  reset() {
    this.form.controls['order'].setValue(null);
    this.form.markAsPristine();
  }

  ngOnInit() {
    this.init(this.voucherServiceOrder);
    this.load({}).subscribe();
  }

  createOrder() {
    if (!this.form.valid) {
      this.markTouchedFields(this.form);
      this.form.updateValueAndValidity();
      return;
    }

    if (this.working) {
      return;
    }
    this.working = true;

    this.contactsCommandService
      .getContact(this.form.controls['order'].value)
      .pipe(
        mergeMap((contactEditItem: ContactEditItem) => {
          const ref = this.dialogDynamicService.open<
            VoucherCreateNewFromServiceComponent,
            VoucherServiceOrderCreateContactComponentData,
            any
          >(
            VoucherCreateNewFromServiceComponent,
            { contact: contactEditItem, roleType: VoucherCreateRoleType.SERVICE },
            {
              titleKey: 'New Voucher',
              acceptLabelKey: 'Save',
              declineLabelKey: 'Cancel',
            }
          );
          return ref.afterClosed();
        }),
        finalize(() => {
          this.working = false;
        })
      )
      .subscribe((response: any) => {
        if (response) {
          this.reset();
        }
      });
  }
}
