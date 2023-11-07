import { NgModule } from '@angular/core';
import { SharedModule } from '@shared';
import { RoutesCommonModule } from '../common/common.module';
import { OfficeRoutingModule } from './office-routing.module';
import { OfficeMemberEditComponent } from './members/member-edit.component';
import { OfficeMemberListComponent } from './members/member-list.component';
import { OfficeContactListComponent } from './contact/contact-list.component';

const COMPONENTS: any[] = [
  OfficeMemberListComponent,
  OfficeMemberEditComponent,
  OfficeContactListComponent,
];
const COMPONENTS_DYNAMIC: any[] = [];

@NgModule({
  imports: [SharedModule, RoutesCommonModule, OfficeRoutingModule],
  declarations: [...COMPONENTS, ...COMPONENTS_DYNAMIC],
})
export class OfficeModule {}
