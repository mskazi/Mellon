import { NgModule } from '@angular/core';
import { Routes, RouterModule } from '@angular/router';
import { OfficeMemberListComponent } from './members/member-list.component';
import { OfficeContactListComponent } from './contact/contact-list.component';

const routes: Routes = [
  { path: 'members', component: OfficeMemberListComponent },
  { path: 'contacts', component: OfficeContactListComponent },
];

@NgModule({
  imports: [RouterModule.forChild(routes)],
  exports: [RouterModule],
})
export class OfficeRoutingModule {}
