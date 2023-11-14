import { HttpClient } from '@angular/common/http';
import { Injectable } from '@angular/core';
import { PaginatedListResults } from '@core/core-model';
import { ISearchService } from '@core/forms/base-form-search.component';
import { ContactEditItem } from '@core/models/contact-edit-item';
import { ContactSearchItem } from '@core/models/contact-search-item';
import { MemberItem } from '@core/models/member';
import { environment } from '@env/environment';
import { Utilities } from '@shared/utils/utilities';
import { Observable } from 'rxjs';

@Injectable({
  providedIn: 'root',
})
export class ContactsCommandService implements ISearchService<ContactSearchItem> {
  constructor(private http: HttpClient) {}

  search(
    term: string = '',
    params: any = {},
    extraParams: any = {}
  ): Observable<PaginatedListResults<ContactSearchItem>> {
    let query = Utilities.paginatedQueryParams(params) + '&';
    if (params) {
      query += Utilities.orderQueryParams(params) + '&';
    }
    if (term) {
      query += `term=${term.toString()}`;
    }

    return this.http.get<PaginatedListResults<ContactSearchItem>>(
      `${environment.serviceContactsUrl}/list/${extraParams.mode}?${query}`
    );
  }
  getContact(id: number): Observable<ContactEditItem> {
    return this.http.get<ContactEditItem>(`${environment.serviceContactsUrl}/${id}`);
  }

  getContactByOrder(order: string): Observable<ContactEditItem> {
    return this.http.get<ContactEditItem>(`${environment.serviceContactsUrl}/navision/${order}`);
  }

  saveContact(data: ContactEditItem): Observable<ContactEditItem> {
    const httpRequest = data.id
      ? this.http.put<ContactEditItem>(`${environment.serviceContactsUrl}/${data.id}`, data)
      : this.http.post<ContactEditItem>(`${environment.serviceContactsUrl}`, data);
    return httpRequest;
  }
}
