import { Component, OnInit, ChangeDetectionStrategy, ChangeDetectorRef } from '@angular/core';
import { VoucherSummary } from '@core/models/voucher-sumamry';
import { VoucherCommandService } from '@core/services/voucher-commands.service';

@Component({
  selector: 'app-dashboard',
  templateUrl: './dashboard.component.html',
  styleUrls: ['./dashboard.component.scss'],
})
export class DashboardComponent implements OnInit {
  constructor(
    private cdr: ChangeDetectorRef,
    private voucherCommandService: VoucherCommandService
  ) {}

  summaryData: VoucherSummary;

  ngOnInit() {
    this.voucherCommandService.getSummary().subscribe((data: VoucherSummary) => {
      this.summaryData = data;
    });
  }
}
