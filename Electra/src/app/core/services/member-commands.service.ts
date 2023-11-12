import { HttpClient, HttpParams } from '@angular/common/http';
import { Injectable } from '@angular/core';
import { ListResult, PaginatedListResults } from '@core/core-model';
import { MemberItem } from '@core/models/member';
import { environment } from '@env/environment';
import { Utilities } from '@shared/utils/utilities';
import { Observable } from 'rxjs';

@Injectable({
  providedIn: 'root',
})
export class MembersCommandService {
  constructor(private http: HttpClient) {}

  getMemberList(term: string = '', params: any = {}): Observable<PaginatedListResults<MemberItem>> {
    let query = Utilities.paginatedQueryParams(params) + '&';
    if (params) {
      query += Utilities.orderQueryParams(params) + '&';
    }
    if (term) {
      query += `term=${term.toString()}`;
    }

    return this.http.get<PaginatedListResults<MemberItem>>(
      `${environment.servicesMembersUrl}/list?${query}`
    );
  }
  getMember(id: number): Observable<MemberItem> {
    return this.http.get<MemberItem>(`${environment.servicesMembersUrl}/${id}`);
  }

  saveMember(data: MemberItem): Observable<MemberItem> {
    const httpRequest = data.id
      ? this.http.put<MemberItem>(`${environment.servicesMembersUrl}/${data.id}`, data)
      : this.http.post<MemberItem>(`${environment.servicesMembersUrl}`, data);
    return httpRequest;
  }

  getMembers(company: string, department: string): Observable<ListResult<MemberItem>> {
    let params = new HttpParams();
    params = params.append('company', company);
    params = params.append('department', department);

    return this.http.get<ListResult<MemberItem>>(`${environment.servicesMembersUrl}/all/company`, {
      params: params,
    });
  }
}
