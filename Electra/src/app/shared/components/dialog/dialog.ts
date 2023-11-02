import { ComponentType } from '@angular/cdk/portal';
import { TemplateRef } from '@angular/core';
import { FormGroup } from '@angular/forms';
import { DialogPosition } from '@angular/material/dialog';
import { Observable } from 'rxjs/internal/Observable';
/**
 * Interface that need to be implemented by any component that we want to dynamic load on dialog
 *
 * @export
 * @interface IDialogDataComponent
 * @template T
 * @template K
 */
export interface IDialogDataComponent<T, K, S = {}> {
  // data on load when component is initialized.Can be accessed on ngInit or constructor
  dialogData: T;
  // data on load when component is initialized.Can be accessed on ngInit or constructor
  dialogServices?: S;
  // retrieve data on action accept
  getAcceptedDialogData(): Observable<DialogResponse<K>>;
  // can close
  canClose(): Observable<boolean>;
  // the close function to override the default one
  closeFn?: any;
  // execute manual command action
  action?: any;
  // disables action button
  setFormMode?(form: FormGroup): void;
  // action to override footer;
  setFooterTemplate?(templateRef: TemplateRef<any>): void;
}

/**
 * Interface that combines all the information for a component to load on a dialog
 * @export
 * @interface IDialogConfig
 * @template T
 * @template K
 */
export interface IDialogConfig<T, K, S> {
  // the component type to load
  component: ComponentType<T> | TemplateRef<T>;
  // the data on initialization
  componentData?: K;
  // services
  componentServices?: S;
  // dialog data
  options: DialogOptions;
}

export interface DialogResponse<K> {
  close: boolean;
  dataDialog?: K;
}

export interface DialogOptions {
  // the title of dialog
  titleKey?: string;
  // the decline action button
  declineLabelKey?: string;
  // the accept action button
  acceptLabelKey?: string;
  // title parameters
  titleParams?: object;
  // title extra text
  titleExtras?: string;
  // can close modal
  blockClose?: boolean;
  // boolean to have the action button hidden and show it when a condition is met
  invisibleActionButton?: boolean;
  //override footer template
  overrideFooter?: boolean;
  //whenever save button will be available without checking if form is dirty
  saveSkipDirtyCheck?: boolean;
  //whenever cancel button will be available without checking if form is dirty
  cancelSkipDirtyCheck?: boolean;

  position?: DialogPosition;
}
