import { Component, OnInit } from '@angular/core';
import { VoucherServiceCreateOrderService } from './voucher-order-create';
import { UntypedFormBuilder } from '@angular/forms';
import { ListResult } from '@core/core-model';
import { VoucherCarrier } from '@core/models/carrier';
import { ContactEditItem } from '@core/models/contact-edit-item';
import { MemberItem } from '@core/models/member';
import { Voucher } from '@core/models/voucher-details-item';
import { CarrierCommandService } from '@core/services/carrier-commands.service';
import { ContactsCommandService } from '@core/services/contact-commands.service';
import { LookupCommandService } from '@core/services/lookup-commands.service';
import { MembersCommandService } from '@core/services/member-commands.service';
import { VoucherCommandService } from '@core/services/voucher-commands.service';
import { DialogResponse } from '@shared/components/dialog/dialog';
import { Utilities } from '@shared/utils/utilities';
import {
  IBaseVoucherBaseNewContactComponentData,
  VoucherCreateBaseComponent,
} from 'app/routes/common/vouchers/common/voucher-base.component';
import { VoucherDetailsComponent } from 'app/routes/common/vouchers/common/voucher-details.component';
import { VoucherSendToDetailsComponent } from 'app/routes/common/vouchers/common/voucher-send-to-details.component';
import { VoucherSpecialsDetailsComponent } from 'app/routes/common/vouchers/common/voucher-special-details.component';
import { VoucherCreateContactService } from 'app/routes/common/vouchers/create/voucher-create-contact';
import * as _ from 'lodash';
import { forkJoin, tap, mergeMap, merge, map, Observable, of } from 'rxjs';
import { VoucherCreateRoleType } from '@core/models/voucher-create';
import { LookUpNumber } from '@core/models/lookup';
/**
 *
 * Department component
 */
@Component({
  selector: 'app-voucher-service-order-create',
  templateUrl: './voucher-order-create.component.html',
  providers: [VoucherServiceCreateOrderService],
}) /* ,  */
export class VoucherCreateNewFromServiceComponent
  extends VoucherCreateBaseComponent<VoucherServiceOrderCreateContactComponentData>
  implements OnInit
{
  constructor(
    protected fb: UntypedFormBuilder,
    private voucherCommandService: VoucherCommandService,
    private carrierCommandService: CarrierCommandService,
    private voucherServiceCreateOrderService: VoucherServiceCreateOrderService,
    private contactsCommandService: ContactsCommandService,
    protected lookupCommandService: LookupCommandService,
    private membersCommandService: MembersCommandService
  ) {
    super(fb, lookupCommandService);

    this.form = this.fb.group({
      contactForm: VoucherSendToDetailsComponent.createForm(fb),
      voucherDetails: VoucherDetailsComponent.createForm(fb),
      specialDetails: VoucherSpecialsDetailsComponent.createForm(fb),
    });
  }

  override optionsAction$: Observable<LookUpNumber[]> = of([{ id: 0, description: 'Person' }]);

  ngOnInit() {
    this.getTypes();
    this.getVoucherDepartments();
    this.getVoucherConditionType();
    this.getCompanies();

    this.voucherServiceCreateOrderService.setMode(this.dialogData.roleType);
    this.initializeDialogService(this.voucherServiceCreateOrderService);
    // before load
    this.onLoad$ = forkJoin([
      //this.contactsCommandService.getContact(this.dialogData.contactId),
    ]).pipe(
      tap(([contact]: [ContactEditItem]) => {
        this.contactForm.patchValue(contact);
      }),
      mergeMap(([contact]: [ContactEditItem]) => {
        return forkJoin([
          this.carrierCommandService.getVoucherCarriers(
            contact.voucherPostCode,
            this.dialogData.roleType
          ),
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
    this.load({}).subscribe();
  }

  override loadCompleted(data: any) {}

  override patchObject() {
    const voucherDetails = {
      ...this.voucherDetailsForm.getRawValue(),
      ...this.specialDetailsForm.getRawValue(),
    };
    return { voucherDetails: voucherDetails };
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

export interface VoucherServiceOrderCreateContactComponentData
  extends IBaseVoucherBaseNewContactComponentData {
  contact: ContactEditItem;
}
