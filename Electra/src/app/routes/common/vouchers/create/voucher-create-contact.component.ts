import { Component, OnInit } from '@angular/core';
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
import * as _ from 'lodash';
import { Observable, forkJoin, map, merge, mergeMap, of, tap } from 'rxjs';
import {
  IBaseVoucherBaseNewContactComponentData,
  VoucherCreateBaseComponent,
} from '../common/voucher-base.component';
import { VoucherDetailsComponent } from '../common/voucher-details.component';
import { VoucherSendToDetailsComponent } from '../common/voucher-send-to-details.component';
import { VoucherSpecialsDetailsComponent } from '../common/voucher-special-details.component';
import { VoucherCreateContactService } from './voucher-create-contact';
/**
 *
 * Department component
 */
@Component({
  selector: 'app-create-voucher-new-contact',
  templateUrl: './voucher-create-contact.component.html',
  providers: [VoucherCreateContactService],
}) /* ,  */
export class VoucherCreateNewContactComponent
  extends VoucherCreateBaseComponent<VoucherBaseNewContactComponentData>
  implements OnInit
{
  constructor(
    protected fb: UntypedFormBuilder,
    private voucherCommandService: VoucherCommandService,
    private carrierCommandService: CarrierCommandService,
    private voucherCreateContactService: VoucherCreateContactService,
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

  ngOnInit() {
    this.startWatchFormChanges();
    this.loadLists();
    this.form.controls['contactForm'].disable();
    this.voucherCreateContactService.setMode(this.dialogData.roleType);
    this.initializeDialogService(this.voucherCreateContactService);
    // before load
    this.onLoad$ = forkJoin([
      this.contactsCommandService.getContact(this.dialogData.contactId),
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
    return { contactId: this.dialogData.contactId, voucherDetails: voucherDetails };
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

export interface VoucherBaseNewContactComponentData
  extends IBaseVoucherBaseNewContactComponentData {
  contactId: number;
}
