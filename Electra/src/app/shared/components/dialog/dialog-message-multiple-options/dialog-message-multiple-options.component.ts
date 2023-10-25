import { AfterViewInit, Component, OnInit, TemplateRef, ViewChild } from '@angular/core';
import { IDialogDataComponent } from '../dialog';
import { DialogMessageComponent, ModalMultipleOptionsAttribute } from '../dialog-message/dialog-message.component';

/**
 * Component that opens a dialog containing a given message and callback function which is triggered after user action.
 * the footer is overriden wtih 3 options
 */
@Component({
  selector: 'app-edit-dialog-message-multiple-options',
  templateUrl: './dialog-message-multiple-options.component.html',
})
export class DialogMessageMultipleOptionsComponent extends DialogMessageComponent implements IDialogDataComponent<any, boolean>, OnInit, AfterViewInit {

  @ViewChild('overrideFooterTemplate') overrideFooterTemplate: TemplateRef<any>;

  override dialogData: ModalMultipleOptionsAttribute;

  /**
 * Initialization of component
 */
  override ngOnInit() {
    super.ngOnInit();
  }

  ngAfterViewInit(): void {
    this.setFooterTemplate(this.overrideFooterTemplate);
  }

  /**
 * override footer component
 */
  setFooterTemplate(templateRef: TemplateRef<any>) {
    this.overrideFooterTemplate = templateRef;
  }

  closeModal(data?: any) {
    return data;
  }

  decline() {
    this.closeModal(false);
  }

  accept() {
    this.closeModal(true);
  }

  cancel() {
    this.closeModal(null);
  }
}
