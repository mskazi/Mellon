import { Component } from '@angular/core';
import { ContactMode } from '@core/models/contac';
@Component({
  selector: 'app-office-contact-list',
  templateUrl: './contact-list.component.html',
  styleUrls: [],
})
export class OfficeContactListComponent {
  contactMode = ContactMode.Office;
  constructor() {}
}
