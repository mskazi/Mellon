import { NgModule } from '@angular/core';
import { SharedModule } from '@shared';
import { RoutesCommonModule } from '../common/common.module';
import { OfficeContactListComponent } from './contact/contact-list.component';
import { OfficeMemberEditComponent } from './members/member-edit.component';
import { OfficeMemberListComponent } from './members/member-list.component';
import { OfficeRoutingModule } from './office-routing.module';
import { OfficeVoucherListComponent } from './voucher/voucher-list.component';
import { OfficePrintCriteriaComponent } from './print/print-criteria.component';

const COMPONENTS: any[] = [
  OfficeMemberListComponent,
  OfficeMemberEditComponent,
  OfficeContactListComponent,
  OfficeVoucherListComponent,
  OfficePrintCriteriaComponent,
];
const COMPONENTS_DYNAMIC: any[] = [];

@NgModule({
  imports: [SharedModule, RoutesCommonModule, OfficeRoutingModule],
  declarations: [...COMPONENTS, ...COMPONENTS_DYNAMIC],
})
export class OfficeModule {}
