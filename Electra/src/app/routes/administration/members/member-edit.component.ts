import { Component, OnInit } from '@angular/core';
import { FormBuilder, Validators } from '@angular/forms';
import { CommonDialogEditComponent } from '@core/forms/base-form-edit-dialog.component';
import { MemberItem } from '@core/models/member';
import { MembersEditService as MemberEditService } from './member-edit-service';

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

  constructor(
    protected fb: FormBuilder,
    private memberEditService: MemberEditService
  ) {
    super(fb);
    this.form = this.fb.group({
      memberName: ['', [Validators.required, Validators.maxLength(5), Validators.maxLength(50)]],
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
    this.load(this.dialogData).subscribe();
  }
}
