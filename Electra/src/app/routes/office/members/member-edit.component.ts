import { Component, OnInit } from '@angular/core';
import { FormBuilder, Validators } from '@angular/forms';
import { CommonDialogEditComponent } from '@core/forms/base-form-edit-dialog.component';
import { MemberItem } from '@core/models/member';
import { MembersEditService as MemberEditService } from './member-edit-service';
import { LookupCommandService } from '@core/services/lookup-commands.service';
import { Observable, forkJoin, map, tap } from 'rxjs';
import { Company, Department } from '@core/models/lookup';
import { ListResult } from '@core/core-model';

@Component({
  selector: 'app-register',
  templateUrl: './member-edit.component.html',
  styleUrls: ['./member-edit.component.scss'],
  providers: [MemberEditService],
})
export class AdministrationMemberEditComponent
  extends CommonDialogEditComponent<MemberItem, MemberItem, any>
  implements OnInit
{
  dialogData: MemberItem;
  optionsDepartments$: Observable<Department[]>;
  optionsCompanies$: Observable<Department[]>;

  constructor(
    protected fb: FormBuilder,
    private memberEditService: MemberEditService,
    private lookupCommandService: LookupCommandService
  ) {
    super(fb);
    this.form = this.fb.group({
      memberName: ['', [Validators.required, Validators.minLength(5), Validators.maxLength(50)]],
      company: ['', [Validators.required, Validators.maxLength(50)]],
      department: ['', [Validators.required]],
      isActive: [true],
    });
  }

  override loadCompleted(data: MemberItem) {
    this.form.patchValue({
      memberName: data.memberName,
      company: data.company,
      department: data.department,
      isActive: data.isActive ?? false,
    });
  }

  ngOnInit(): void {
    this.initializeDialogService(this.memberEditService);
    this.optionsDepartments$ = this.lookupCommandService
      .getDepartments()
      .pipe(map((result: ListResult<Department>) => result.data));
    this.optionsCompanies$ = this.lookupCommandService
      .getCompanies()
      .pipe(map((result: ListResult<Company>) => result.data));
    /* this.onLoad$ = forkJoin([
      this.lookupCommandService.getDepartments(),
      this.lookupCommandService.getCompanies(),
    ]).pipe(
      tap(([deparments, companies]: [ListResult<Department>, ListResult<Department>]) => {
        this.optionsDepartments = deparments.data;
        this.optionsCompanies = companies.data;
      })
    ); */
    this.load(this.dialogData).subscribe();
  }
}
