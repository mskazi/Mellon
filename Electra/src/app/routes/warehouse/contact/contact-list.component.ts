import { Component } from '@angular/core';
import { ContactMode } from '@core/models/contac';
@Component({
  selector: 'app-warehouse-contact-list',
  templateUrl: './contact-list.component.html',
  styleUrls: [],
})
export class WarehouseContactListComponent {
  contactMode = ContactMode.Warehouse;
  constructor() {}
}
