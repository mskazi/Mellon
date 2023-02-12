import { HttpClient } from '@angular/common/http';
import { Component, OnInit } from '@angular/core';
import { ActivatedRoute } from '@angular/router';
import { catchError, mergeMap, of } from 'rxjs';
import { environment } from 'src/environments/environment';
import * as _ from 'lodash';
import { FormBuilder, FormGroup, Validators } from '@angular/forms';
import * as moment from 'moment';
@Component({
  selector: 'app-document',
  templateUrl: './document.component.html',
})
export class AppDocumentComponent implements OnInit {
  loading = true;
  approvalStatusEnum = ApprovalStatusEnum;
  messageTypeEnum = MessageTypeEnum;
  message?: Message = undefined;
  approvalOrder?: IApprovalOrder;
  openApproval?: IApproval;
  form: FormGroup;

  constructor(private httpClient: HttpClient, private activatedRoute: ActivatedRoute, protected fb: FormBuilder) { }
  ngOnInit() {
    this.form = this.fb.group({
      comment: [null, [Validators.maxLength(115)]],
    });

    this.activatedRoute.params.pipe(
      mergeMap((params: any) => {
        const documentToken = params.documentToken;

        if (!documentToken) {
          this.message = { code: '1', title: 'Something went wrong', message: 'Missing document token.Please check your url for token parameter', type: MessageTypeEnum.Error };
          this.loading = false;
          throw new Error('Missing Document Token')
        }
        return this.httpClient.get<IApprovalOrder>(`${environment.serviceApprovalsUrl}/${documentToken}`);
      }),
      catchError((error: any) => {
        this.loading = false;
        this.showErrorMessages(error);
        throw error;
      })
    ).subscribe((approvalOrder: IApprovalOrder) => {
      approvalOrder.approvals = _.orderBy(approvalOrder.approvals, "approvalSequence").map((approval: IApproval) => {
        approval.approvalResponsible = approval.approvalResponsible.replace('MELLONGROUP\\', '');
        return approval;
      });
      approvalOrder.approvalLines = _.orderBy(approvalOrder.approvalLines, "lineNo");
      approvalOrder.mainApprovalLine = approvalOrder.approvalLines[0];
      approvalOrder.totalAmount = _.sumBy(approvalOrder.approvalLines, "lineAmount");
      approvalOrder.orderDate = moment(approvalOrder.orderDate).add(moment(approvalOrder.orderDate).utcOffset(), 'minute').toDate();
      this.approvalOrder = approvalOrder;
      this.loading = false;
    });
  }

  action(decision: ApprovalStatusEnum) {
    if (!this.form.valid) {
      this.form.markAllAsTouched();
      return;
    }
    this.loading = true;
    const data = {
      decision: decision,
      comment: this.form.get('comment')?.value,
    };

    return this.httpClient.post(`${environment.serviceApprovalsUrl}/Decision/${this.approvalOrder?.documentToken}`, data).pipe(
      catchError((error: any) => {
        this.loading = false;
        this.showErrorMessages(error);
        throw error;
      })).subscribe(() => {
        this.loading = false;
        this.message = { code: '', title: 'Action submitted successfully', message: `Action  ${decision === ApprovalStatusEnum.Approved ? 'Approved' : 'Rejected'} has been submitted.Thank you.`, type: MessageTypeEnum.Success };
      });
  }

  private showErrorMessages(error: any) {

    if (error.status === 500 && error?.error?.Code === 1009) {
      this.message = { code: error?.error?.Code, title: 'Something went wrong with ERP', message: error.error?.detail ?? error?.message, type: MessageTypeEnum.Error };
      throw error;
    }
    if (error.status === 404) {
      this.message = { code: '1004', title: 'Something went wrong', message: error.error?.detail ?? error?.message, type: MessageTypeEnum.Error };
      throw error;
    }
    if (error?.error?.Code === 1008) {
      this.message = { code: '1004', title: 'Action Done', message: 'You have already taken action for this request', type: MessageTypeEnum.Error };
      throw error;
    }
    this.message = { code: error.code, title: 'Something went wrong', message: error.error?.detail ?? error?.message, type: MessageTypeEnum.Error };
  }
}

export interface IApprovalOrder {
  id: string,
  erpCountry: string,
  erpCompany: string,
  documentNo: string,
  documentType: string,
  sourceName: string,
  orderDateMoment: any;
  orderDate: Date,
  bu: string,
  buName: string,
  bl: string,
  blName: string,
  plLines: string,
  plLinesName: string,
  ergo: string,
  ergoName: string,
  erpTimeStamp: string,
  notificationMail: string,
  currency: string,
  status: string,
  comment: string,
  rowVersion: string,
  approvals: IApproval[],
  approvalLines: IApprovalLine[]
  mainApprovalLine: IApprovalLine;
  totalAmount: number;
  documentToken: string,
  documentOwner: string,
  approvalProcessComment: string
}

export interface IApprovalLine {
  id: string,
  erpCompany: string,
  documentType: string,
  documentNo: string,
  lineNo: number,
  description: string,
  lineAmount: number,
  bu: string,
  buName: string,
  bl: string,
  blName: string,
  plLines: string,
  plLinesName: string,
  ergo: string,
  ergoName: string,
  unitPrice: number,
  quantity: number,
  erpTimeStamp: string,
  rowVersion: string
}

export interface IApproval {
  id: number,
  erpCountry: string,
  erpCompany: string,
  documentType: string,
  documentNo: string,
  approvalSequence: number,
  documentOwner: string,
  requestNo: number,
  approvalResponsible: string,
  email: string,
  documentOwnerEmail: string,
  status: ApprovalStatusEnum,
  documentToken: string,
  erpTimeStamp: string,
  approvalProcessComment: string,
  approvalRequestComment: string,
  rowVersion: string,

}

export interface Message {
  code: string;
  message: string;
  title: string;
  type: MessageTypeEnum
}

export enum ApprovalStatusEnum {
  Requested,
  Open,
  Created,
  Canceled,
  Rejected,
  Approved,
}

export enum MessageTypeEnum {
  Error,
  Success
}

