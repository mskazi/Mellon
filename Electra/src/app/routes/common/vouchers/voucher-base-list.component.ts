import { DatePipe } from '@angular/common';
import { BaseSearchFormComponent, ISearchService } from '@core/forms/base-form-search.component';
import { DialogDynamicService } from '@shared/components/dialog/dialog.service';
import { VoucherDetailsCommonComponent } from 'app/routes/common/vouchers/voucher-details.component';

export abstract class BaseVoucherListComponent<T> extends BaseSearchFormComponent<T> {
  constructor(
    protected service: ISearchService<T>,
    protected dialogDynamicService: DialogDynamicService,
    protected forceLoad = true
  ) {
    super(service, forceLoad);
  }

  showVoucherDetails(item: number) {
    this.dialogDynamicService.open<VoucherDetailsCommonComponent, number, any>(
      VoucherDetailsCommonComponent,
      item,
      {
        titleKey: 'Voucher Details',
        acceptLabelKey: 'Ok',
        declineLabelKey: 'Cancel',
      }
    );
  }
}
