import {
  AfterViewInit,
  Component,
  ComponentRef,
  Inject,
  TemplateRef,
  ViewChild,
  ViewContainerRef,
} from '@angular/core';
import { FormGroup } from '@angular/forms';
import { MatDialogRef, MAT_DIALOG_DATA } from '@angular/material/dialog';
import * as _ from 'lodash';
import { Subscription, finalize } from 'rxjs';
import { DialogResponse } from './dialog';
import { SpinnerService, SpinnerType } from '../spinner/spinner.service';
import { Utilities } from '@shared/utils/utilities';

/**
 * Component for dynamic loading components inside a dialog
 *
 * @export
 * @class DialogDynamicComponent
 * @implements {AfterViewInit}
 */
@Component({
  selector: 'app-dynamic-dialog',
  templateUrl: './dialog.component.html',
  styleUrls: ['./dialog.component.scss'],
})
export class DialogDynamicComponent implements AfterViewInit {
  // target of the template where a component can be loaded
  @ViewChild('target', { read: ViewContainerRef }) viewContainerRef: ViewContainerRef;
  // dynamic component reference
  private componentRef: ComponentRef<any>;
  // subscription of component data
  private getComponentDataSubscription: Subscription;
  // subscription of closing dialog
  private canCloseDialogSubscription: Subscription;
  // subscription of keydown Events
  private keydownEventsSubscription: Subscription;
  // subscription of backdrop click
  private backdropClickSubscription: Subscription;
  // boolean to show or hide the action button in case of invsible action button
  private showActionButton = false;
  blockClose = false;
  form: FormGroup;
  channel = Utilities.GenGUI();
  working = false;
  spinnerType = SpinnerType.CIRCLE;
  overrideFooterTemplate: TemplateRef<any>;
  /**
   * Creates an instance of DialogDynamicComponent.
   * @param {MatDialogRef<DialogDynamicComponent>} dialogRef
   * @param {ComponentFactoryResolver} resolver
   * @param {*} data
   * @memberof DialogDynamicComponent
   */
  constructor(
    private dialogRef: MatDialogRef<DialogDynamicComponent>,
    @Inject(MAT_DIALOG_DATA) public data: any,
    protected spinnerService: SpinnerService
  ) {}
  /**
   * After view initialization
   * @memberof DialogDynamicComponent
   */
  ngAfterViewInit(): void {
    this.componentRef = this.viewContainerRef.createComponent(this.data.component);
    this.componentRef.instance.dialogData = _.cloneDeep(this.data.componentData);
    this.componentRef.instance.dialogServices = this.data.componentServices;
    this.componentRef.instance.action = this.action.bind(this);
    this.componentRef.instance.setFormMode = this.setFormMode.bind(this);
    this.componentRef.instance.setFooterTemplate = this.setFooterTemplate.bind(this);
    this.componentRef.instance.closeModal = this.closeModal.bind(this);

    if (this.data?.options?.blockClose) {
      this.blockClose = true;
    }

    if (this.dialogRef && _.isFunction(this.dialogRef.keydownEvents)) {
      this.keydownEventsSubscription = this.dialogRef.keydownEvents().subscribe(event => {
        if (event.key === 'Escape' && !this.blockClose) {
          this.closeModal();
        }
      });
    }
    if (this.dialogRef && _.isFunction(this.dialogRef.backdropClick)) {
      this.backdropClickSubscription = this.dialogRef.backdropClick().subscribe(() => {
        if (!this.blockClose) {
          this.closeModal();
        }
      });
    }
  }
  // on component destroy
  ngOnDestroy() {
    if (this.componentRef) {
      this.componentRef.destroy();
    }

    if (this.getComponentDataSubscription) {
      this.getComponentDataSubscription.unsubscribe();
    }

    if (this.canCloseDialogSubscription) {
      this.canCloseDialogSubscription.unsubscribe();
    }

    if (this.keydownEventsSubscription) {
      this.keydownEventsSubscription.unsubscribe();
    }

    if (this.backdropClickSubscription) {
      this.backdropClickSubscription.unsubscribe();
    }
  }
  /**
   * on accept action
   * @memberof DialogDynamicComponent
   */
  action() {
    if (this.working) {
      return;
    }
    this.working = true;
    this.activate();
    this.getComponentDataSubscription = this.componentRef.instance
      .getAcceptedDialogData()
      .pipe(
        finalize(() => {
          this.working = false;
          this.deactivate();
        })
      )
      .subscribe((response: DialogResponse<any>) => {
        if (response.close) {
          this.dialogRef.close(response.dataDialog);
        }
      });
  }

  activate() {
    this.spinnerService.activate(this.channel);
  }

  deactivate() {
    this.spinnerService.deactivate(this.channel);
  }

  /**
   * dialog close method
   * @memberof DialogDynamicComponent
   */
  close() {
    this.componentRef.instance.closeFn ? this.componentRef.instance.closeFn() : this.closeModal();
  }

  /**
   * set the form mode of the dialog
   * @memberof DialogDynamicComponent
   */
  setFormMode(form: FormGroup) {
    this.form = form;
  }

  /**
   * override footer component
   */
  setFooterTemplate(templateRef: TemplateRef<any>) {
    this.overrideFooterTemplate = templateRef;
  }

  /**
   * Setter for showActionButton
   * @param show
   * @memberof DialogDynamicComponent
   */
  set showAction(show: boolean) {
    this.showActionButton = show;
  }

  /**
   * Getter for showActionButton
   * @memberof DialogDynamicComponent
   */
  get showAction() {
    return this.showActionButton;
  }

  closeModal(data?: any) {
    if (this.working) {
      return;
    }
    this.working = true;
    this.activate();
    if (!this.componentRef.instance.canClose) {
      this.working = false;
      this.deactivate();
      this.dialogRef.close(data);
      return;
    }
    this.canCloseDialogSubscription = this.componentRef.instance
      .canClose()
      .subscribe((close: boolean) => {
        this.working = false;
        this.deactivate();
        if (close) {
          this.dialogRef.close(data);
        }
      });
  }
}
