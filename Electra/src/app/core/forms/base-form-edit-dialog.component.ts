import { UntypedFormBuilder, UntypedFormGroup } from '@angular/forms';
import { Observable, map, of } from 'rxjs';
import { InjectorService } from '../injector.service';
import { BaseFormEditComponent } from './base-form-edit.component';
import { IPersistObjectService } from './base-form.component';
import { IDialogDataComponent, DialogResponse } from '@shared/components/dialog/dialog';
import { UnsavedChangesService } from '@shared/components/unsavedChanges/unsaved-changes.service';
import { NotificationService } from '@core/notification.service';

export abstract class CommonDialogEditComponent<T, K, S>
  extends BaseFormEditComponent<K>
  implements IDialogDataComponent<T, K, S>
{
  form: UntypedFormGroup;
  dialogData: T;
  dialogServices?: S;
  setFormMode: any;

  protected constructor(fb: UntypedFormBuilder) {
    super(fb);
  }

  initializeDialogService(service: IPersistObjectService<K>) {
    super.init(service);
    this.setFormMode(this.form);
  }
  /**
   * Method to close dialog and save data
   */
  getAcceptedDialogData(): Observable<DialogResponse<K>> {
    return this.save().pipe(
      map((data: K): DialogResponse<K> => {
        return {
          close: true,
          dataDialog: data ?? ({} as K),
        };
      })
    );
  }

  // can close modal
  canClose(): Observable<boolean> {
    const unsavedChangesService = InjectorService.injector.get(UnsavedChangesService);
    return unsavedChangesService.ignoreChanges([this.formId]);
  }

  /**
   * virtual class on save completed
   * @param data
   */
  override saveCompleted(_data: K): void {
    const not = InjectorService.injector.get(NotificationService);
    not.success('Save completed');
  }
}
