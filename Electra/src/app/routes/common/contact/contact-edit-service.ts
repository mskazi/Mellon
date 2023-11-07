import { Injectable } from '@angular/core';
import { IPersistObjectService } from '@core/forms/base-form.component';
import { ContactEditItem } from '@core/models/contact-edit-item';
import { ContactsCommandService } from '@core/services/contact-commands.service';
import { Observable, of } from 'rxjs';

@Injectable()
export class ContactEditService implements IPersistObjectService<ContactEditItem> {
  constructor(private contactsCommandService: ContactsCommandService) {}

  save(data: ContactEditItem): Observable<ContactEditItem> {
    return this.contactsCommandService.saveContact(data);
  }

  load(params: any): Observable<ContactEditItem> {
    return params.id ? this.contactsCommandService.getContact(params.id) : of(params);
  }
}
