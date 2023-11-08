import { AfterViewInit, Component, OnDestroy, OnInit } from '@angular/core';
import { UntypedFormBuilder } from '@angular/forms';
import { Voucher, VoucherTrack } from '@core/models/voucher-details-item';
import { VoucherCommandService } from '@core/services/voucher-commands.service';
import { TranslateService } from '@ngx-translate/core';
import { DialogResponse, IDialogDataComponent } from '@shared/components/dialog/dialog';
import { SpinnerService, SpinnerType } from '@shared/components/spinner/spinner.service';
import { Utilities } from '@shared/utils/utilities';
import { Observable, finalize, of } from 'rxjs';

/**
 *
 * Department component
 */
@Component({
  selector: 'app-common-voucher-track',
  templateUrl: './voucher-track.component.html',
}) /* ,  */
export class VoucherTrackCommonComponent
  implements OnInit, AfterViewInit, OnDestroy, IDialogDataComponent<number, any, any>
{
  voucher: VoucherTrack;
  spinnerType = SpinnerType.CIRCLE;
  channel = Utilities.GenGUI();
  constructor(
    fb: UntypedFormBuilder,
    private translateService: TranslateService,
    private voucherCommandService: VoucherCommandService,
    private spinnerService: SpinnerService
  ) {}

  ngOnInit() {}
  ngAfterViewInit() {
    this.spinnerService.activate(this.channel);
    this.voucherCommandService
      .getVoucherTrack(this.dialogData)
      .pipe(
        finalize(() => {
          this.spinnerService.deactivate(this.channel);
        })
      )
      .subscribe((data: VoucherTrack) => {
        this.voucher = data;
      });
  }

  dialogData: number;
  dialogServices: any;

  getAcceptedDialogData(): Observable<DialogResponse<any>> {
    return of({
      close: true,
      dataDialog: {},
    });
  }

  canClose(): Observable<boolean> {
    return of(true);
  }

  ngOnDestroy() {}
}
