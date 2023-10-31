import { NgModule } from '@angular/core';
import { SharedModule } from '@shared';
import { RoutesCommonModule } from '../common/common.module';
import { AdministrationRoutingModule } from './administration-routing.module';
import { AdministrationMemberListComponent } from './members/member-list.component';
import { AdministrationMemberEditComponent } from './members/member-edit.component';

const COMPONENTS: any[] = [AdministrationMemberListComponent, AdministrationMemberEditComponent];
const COMPONENTS_DYNAMIC: any[] = [];

@NgModule({
  imports: [SharedModule, RoutesCommonModule, AdministrationRoutingModule],
  declarations: [...COMPONENTS, ...COMPONENTS_DYNAMIC],
})
export class AdministrationModule {}
