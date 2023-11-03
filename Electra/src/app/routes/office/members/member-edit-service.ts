import { Injectable } from '@angular/core';
import { IPersistObjectService } from '@core/forms/base-form.component';
import { MemberItem } from '@core/models/member';
import { MembersCommandService } from '@core/services/member-commands.service';
import { Observable, of } from 'rxjs';

@Injectable()
export class MembersEditService implements IPersistObjectService<MemberItem> {
  constructor(private membersCommandService: MembersCommandService) {}

  save(data: MemberItem): Observable<MemberItem> {
    return this.membersCommandService.saveMember(data);
  }

  load(params: any): Observable<MemberItem> {
    return params.id ? this.membersCommandService.getMember(params.id) : of(params);
  }
}
