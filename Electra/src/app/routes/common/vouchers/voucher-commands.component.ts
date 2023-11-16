import { Component, EventEmitter, Input, OnInit, Output } from '@angular/core';
import { DialogDynamicService } from '@shared/components/dialog/dialog.service';
import { VoucherDetailsCommonComponent } from 'app/routes/common/vouchers/voucher-details.component';
import { VoucherTrackCommonComponent } from './voucher-track.component';
import * as _ from 'lodash';
import { VoucherCommandService } from '@core/services/voucher-commands.service';
import { map, tap } from 'rxjs';
import { Utilities } from '@shared/utils/utilities';
import { NotificationService } from '@core/notification.service';

@Component({
  selector: 'app-common-voucher-commands',
  templateUrl: './voucher-commands.component.html',
})
export class VoucherCommandsComponent implements OnInit {
  constructor(
    protected dialogDynamicService: DialogDynamicService,
    protected voucherCommandService: VoucherCommandService,
    protected notification: NotificationService
  ) {}
  @Input() item: VoucherCommandData;
  @Input() canShowAdminCancelButton = false;
  @Output() cancelled = new EventEmitter<void>();

  private cancelStatus = [0];
  private traceStatus = [0, 90, 21, 22, 1];
  private printStatus = [90];
  private carrierVoucherNo = ['Queue', ''];
  private detailsStatus = [90];

  currentData: Date;
  isToday = false;
  isAdmin = true;
  cancelButton = false;
  cancelAdminButton = false;
  showTrackButton = false;
  voucherDetailsButton = false;

  ngOnInit(): void {
    // call setHours to take the time out of the comparison
    this.isToday =
      (_.isDate(this.item.createdAt)
        ? this.item.createdAt.setHours(0, 0, 0, 0)
        : new Date(this.item.createdAt)) === new Date().setHours(0, 0, 0, 0);
    this.cancelButton =
      this.cancelStatus.includes(this.item.systemStatus) &&
      this.isToday &&
      new Date().getHours() < 16;
    this.cancelAdminButton = this.isAdmin && new Date().getHours() > 16;
    this.showTrackButton = !this.traceStatus.includes(this.item.systemStatus);
    this.showTrackButton =
      !this.printStatus.includes(this.item.systemStatus) &&
      !this.carrierVoucherNo.includes(this.item.carrierVoucherNo);
    this.voucherDetailsButton = !this.detailsStatus.includes(this.item.systemStatus);
  }

  showVoucherDetails() {
    this.dialogDynamicService.open<VoucherDetailsCommonComponent, number, any>(
      VoucherDetailsCommonComponent,
      this.item.id,
      {
        titleKey: 'Voucher Details',
        acceptLabelKey: 'Ok',
        declineLabelKey: '',
      }
    );
  }

  showTrackDetails() {
    this.dialogDynamicService.open<VoucherTrackCommonComponent, number, any>(
      VoucherTrackCommonComponent,
      this.item.id,
      {
        titleKey: 'Voucher Track Details',
        acceptLabelKey: 'Ok',
        declineLabelKey: '',
      }
    );
  }

  printVoucher() {
    this.voucherCommandService.print(this.item.id).subscribe((file: any) => {
      Utilities.downloadFileOnNewTab(file, `Voucher_Print.pdf`);
    });
  }

  cancelVoucher() {
    this.voucherCommandService.cancel(this.item.id).subscribe((result: any) => {
      if (result) {
        this.notification.success(`Voucher ${result} Cancelled. Info is verified with Carrier.`);
        this.cancelled.emit();
      }
    });
  }
}

export interface VoucherCommandData {
  systemStatus: number;
  id: number;
  carrierVoucherNo: string;
  createdAt: Date;
}
