import { ComponentType } from '@angular/cdk/portal';
import { Injectable, TemplateRef } from '@angular/core';
import { MatDialog, MatDialogConfig } from '@angular/material/dialog';
import { DialogOptions, IDialogConfig, IDialogDataComponent } from './dialog';
import { DialogDynamicComponent } from './dialog.component';
import {
  DialogMessageComponent,
  ModalMessageAttribute,
  ModalMultipleOptionsAttribute,
} from './dialog-message/dialog-message.component';
import { mergeMap } from 'rxjs/operators';
import { Observable, of } from 'rxjs';
import { DialogMessageMultipleOptionsComponent } from './dialog-message-multiple-options/dialog-message-multiple-options.component';
/**
 * Root service for loading types of dialog
 *
 * @export
 * @class DialogDynamicService
 */
@Injectable({
  providedIn: 'root',
})
export class DialogDynamicService {
  /**
   * Creates an instance of DialogDynamicService.
   * @param {MatDialog} dialog
   * @memberof DialogDynamicService
   */
  constructor(public dialog: MatDialog) {}
  // default configuration
  private defaultConfiguration = { height: 'auto', width: 'auto', disableClose: true };
  private defaultOptions = {
    acceptLabelKey: 'lblSave',
    declineLabelKey: 'lblCancel',
    cancelSkipDirtyCheck: true,
  };

  /**
   * Open custom template
   * @template T the type of the component
   * @template Z the type of data set on  dynamic component
   * @template X the type of data to retrieve on accept action
   * @param {(ComponentType<T> | TemplateRef<T>)} component
   * @param {Z} [data]
   * @param {string} [titleKey]
   * @param {string} [cancelKey]
   * @param {string} [acceptKey]
   * @return {*}
   * @memberof DialogDynamicService
   */
  public open<T extends IDialogDataComponent<Z, X>, Z, X>(
    component: ComponentType<T> | TemplateRef<T>,
    data?: Z,
    options?: DialogOptions
  ) {
    const conf: MatDialogConfig<IDialogConfig<T, Z, undefined>> = {
      autoFocus: true,
      restoreFocus: true,
      ...this.defaultConfiguration,
    };
    const defaultOptions = { ...options };
    conf.data = {
      component,
      componentData: data,
      options: {
        ...defaultOptions,
      },
    };
    return this.dialog.open(DialogDynamicComponent, conf);
  }
  /**
   * Open custom template with non clonable service
   * @template T the type of the component
   * @template Z the type of data set on  dynamic component
   * @template X the type of data to retrieve on accept action
   * @param {(ComponentType<T> | TemplateRef<T>)} component
   * @param {Z} [data]
   * @param {string} [titleKey]
   * @param {string} [cancelKey]
   * @param {string} [acceptKey]
   * @return {*}
   * @memberof DialogDynamicService
   */
  public openWithService<T extends IDialogDataComponent<Z, X, S>, Z, X, S>(
    component: ComponentType<T> | TemplateRef<T>,
    data?: Z,
    services?: S,
    options?: DialogOptions
  ) {
    const conf: MatDialogConfig<IDialogConfig<T, Z, S>> = {
      autoFocus: true,
      restoreFocus: true,
      ...this.defaultConfiguration,
    };
    const defaultOptions = { ...this.defaultOptions, ...options };
    conf.data = {
      component,
      componentData: data,
      componentServices: services,
      options: {
        ...defaultOptions,
      },
    };
    return this.dialog.open(DialogDynamicComponent, conf);
  }
  /**
   * Opens the common message dialog.
   * @param attribute
   * @returns {MatDialogRef<DialogDynamicComponent, any>}
   */

  openConfirmationDialog(attribute: ModalMessageAttribute, overrideOptions?: DialogOptions) {
    const dialogOptions = {
      titleKey: attribute.title,
      acceptLabelKey: overrideOptions?.acceptLabelKey ?? 'lblSave',
      declineLabelKey: overrideOptions?.declineLabelKey ?? 'lblCancel',
      titleParams: overrideOptions?.titleParams ?? attribute.titleParams,
      blockClose: overrideOptions?.blockClose,
      position: overrideOptions?.position,
    };
    return this.open<DialogMessageComponent, ModalMessageAttribute, boolean>(
      DialogMessageComponent,
      attribute,
      dialogOptions
    );
  }

  /**
   * Opens the common message dialog with three options yes,no,cancel
   * @param attribute
   * @returns {MatDialogRef<DialogDynamicComponent, any>}
   */

  openMultipleOptionsDialog(attribute: ModalMultipleOptionsAttribute) {
    const dialogOptions = {
      titleKey: attribute.title,
    };
    return this.open<DialogMessageMultipleOptionsComponent, ModalMultipleOptionsAttribute, boolean>(
      DialogMessageMultipleOptionsComponent,
      attribute,
      dialogOptions
    );
  }

  /**
   * Confirmation dialog
   * @param messageLabel
   * @param messageParams
   * @param callback
   * @param callbackParams
   * @returns
   */
  titleConfirmationDialog(
    messageLabel: string,
    messageParams?: object,
    callback?: any,
    callbackParams?: any
  ): Observable<any> {
    const ref = this.openConfirmationDialog(
      new ModalMessageAttribute(
        messageLabel,
        'lblConfirmationDialog',
        callback,
        undefined,
        messageParams,
        callbackParams
      )
    );
    return ref.afterClosed().pipe(
      mergeMap((_titleData: any[]) => {
        if (_titleData) {
          return of(true);
        }
        return of(false);
      })
    );
  }

  customTitleConfirmationDialog(
    messageLabel: string,
    titleLabel: string,
    titleParams?: any,
    messageParams?: any,
    callback?: any,
    callbackParams?: any
  ): Observable<any> {
    const ref = this.openConfirmationDialog(
      new ModalMessageAttribute(
        messageLabel,
        titleLabel,
        callback,
        titleParams,
        messageParams,
        callbackParams
      )
    );
    return ref.afterClosed().pipe(
      mergeMap((_customTitleData: any[]) => {
        if (_customTitleData) {
          return of(true);
        }
        return of();
      })
    );
  }

  infoConfirmationDialog(
    messageLabel: string,
    titleLabel: string,
    acceptLabelKey = 'i18n.common.close',
    titleParams?: any,
    messageParams?: object,
    titleExtras?: string
  ): Observable<any> {
    const attribute = new ModalMessageAttribute(
      messageLabel,
      titleLabel,
      undefined,
      titleParams,
      messageParams,
      undefined,
      titleExtras
    );
    const dialogOptions = {
      titleKey: attribute.title,
      acceptLabelKey: acceptLabelKey,
      titleParams: attribute.titleParams,
      titleExtras: attribute.titleExtras,
    };
    const ref = this.open<DialogMessageComponent, ModalMessageAttribute, boolean>(
      DialogMessageComponent,
      attribute,
      dialogOptions
    );
    return ref.afterClosed().pipe(
      mergeMap((infoConfirmationData: any[]) => {
        if (infoConfirmationData) {
          return of(true);
        }
        return of();
      })
    );
  }
}
