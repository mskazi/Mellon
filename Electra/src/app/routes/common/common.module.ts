import { NgModule } from '@angular/core';
import { SharedModule } from '@shared/shared.module';
import { VoucherDetailsCommonComponent } from './vouchers/voucher-details.component';

const COMPONENTS: any[] = [VoucherDetailsCommonComponent];
const COMPONENTS_DYNAMIC: any[] = [];

@NgModule({
  imports: [SharedModule],
  declarations: [...COMPONENTS, ...COMPONENTS_DYNAMIC],
})
export class RoutesCommonModule {}
