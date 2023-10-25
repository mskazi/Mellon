import { DialogResponse, IDialogDataComponent } from '../dialog';
import { Observable } from 'rxjs/internal/Observable';
import { Component, OnInit } from '@angular/core';
import { map } from 'rxjs/operators';
import { of } from 'rxjs';

/**
 * Object that contains the properties for the popup.
 */
export class ModalMessageAttribute {

  message: string;

  title: string;

  titleParams?: object;

  messageParams?: object;

  callBackParams?: object;

  // the callback function for the accepted user action.
  callBack?: any;

  titleExtras?: string;

  constructor(message: string, title: string, callBack?: any, titleParams?: object, messageParams?: object, callBackParams?: object, titleExtras?: string) {
    this.message = message;
    this.title = title;
    this.callBack = callBack;
    this.titleParams = titleParams;
    this.messageParams = messageParams;
    this.callBackParams = callBackParams;
    this.titleExtras = titleExtras;
  }
}

/**
 * Object that contains the properties for the popup of multiple options dialog.
 */
export class ModalMultipleOptionsAttribute extends ModalMessageAttribute {

  acceptLabelKey: string;
  declineLabelKey: string;
  cancelLabelKey: string;

  constructor(declineLabelKey: string, acceptLabelKey: string, cancelLabelKey: string, modalAttributes: ModalMessageAttribute) {
    super(modalAttributes.message, modalAttributes.title, modalAttributes.callBack, modalAttributes.titleParams, modalAttributes.messageParams, modalAttributes.callBackParams, modalAttributes.titleExtras);
    this.declineLabelKey = declineLabelKey;
    this.acceptLabelKey = acceptLabelKey;
    this.cancelLabelKey = cancelLabelKey;
  }

}

/**
 * Component that opens a dialog containing a given message and callback function which is triggered after user action.
 */
@Component({
  selector: 'app-edit-dialog-message',
  templateUrl: './dialog-message.component.html',
})
export class DialogMessageComponent implements IDialogDataComponent<any, boolean>, OnInit {

  /**
   * The input of the dialog component.
   * @type {ModalMessageAttribute}
   */
  dialogData: ModalMessageAttribute;

  callBack: any;

  message: string;

  messageParams: object | undefined;

  callBackParams: object | undefined;

  /**
   * The data when the dialog component closes.
   */
  getAcceptedDialogData(): Observable<DialogResponse<boolean>> {
    const action = (this.callBack && this.callBack(this.callBackParams)) ?? of(true);
    return action.pipe(map((data: boolean): DialogResponse<boolean> => {
      return {
        close: true,
        dataDialog: data
      };
    }));
  }
  // can close modal
  canClose() {
    return of(true);
  }
  /**
   * Initialization of component
   */
  ngOnInit() {
    // initiate the message derived from dialogData
    this.message = this.dialogData.message;
    // initiate the message params derived from dialogData
    this.messageParams = this.dialogData.messageParams;
    // initiate the callback derived from dialogData
    this.callBack = this.dialogData.callBack;
    // initiate the callback params derived from dialogData
    this.callBackParams = this.dialogData.callBackParams;
  }
}
