import { UntypedFormGroup } from '@angular/forms';
import { SpinnerService } from '@shared/components/spinner/spinner.service';
import { UnsavedChangesService } from '@shared/components/unsavedChanges/unsaved-changes.service';
import { Utilities } from '@shared/utils/utilities';
import * as _ from 'lodash';
import { Observable, Subject, catchError, finalize, map, mergeMap, of, tap } from 'rxjs';
import { BaseFormComponent, IPersistObjectService } from './base-form.component';
import { InjectorService } from '@core/injector.service';
import { NotificationService } from '@core/notification.service';

export abstract class BaseFormEditComponent<T> extends BaseFormComponent {
  readonly formId = Utilities.GenGUI();

  saveCompleted(_data: T): void {
    const not = InjectorService.injector.get(NotificationService);
    not.success('Save completed');
    //do nothing
  }
  loadCompleted(_item: T): void {
    //do nothing
  }
  abstract form: UntypedFormGroup;

  private errorMessagesSubject = new Subject<any[]>();
  protected service: IPersistObjectService<T>;

  protected data: T;
  private initData: T;
  private enableSpinner = false;
  private workingSaveForm = false;

  get errorMessages$(): Observable<any> {
    return this.errorMessagesSubject;
  }

  setErrorMessages(errors: any[]) {
    this.errorMessagesSubject.next(errors);
  }

  clearErrors() {
    this.errorMessagesSubject.next([]);
  }

  constructNewClass$: Observable<T>;
  onSave$: Observable<any>;
  onLoad$: Observable<any>;

  init(service: IPersistObjectService<T>, enableSpinner = false) {
    this.service = service;
    this.enableSpinner = enableSpinner;
  }

  load(params = {} as any): Observable<T | undefined> {
    return (this.onLoad$ || of(true)).pipe(
      mergeMap(() => {
        return this.service.load(params).pipe(
          map((response: T) => {
            this.initData = _.cloneDeep(Object.freeze(response));
            this.data = _.cloneDeep(response);
            this.loadCompleted(this.data);
            return this.data;
          })
        );
      })
    );
  }

  save(): Observable<T> {
    if (this.workingSaveForm) {
      return of();
    }

    this.activateSpinner();
    if (!this.form.valid) {
      this.markTouchedFields(this.form);
      this.form.updateValueAndValidity();
      return of();
    }
    return (this.onSave$ || of(true)).pipe(
      mergeMap(() => {
        const data = this.patchObject();
        return this.service.save(data).pipe(
          tap((response: T) => {
            this.saveCompleted(response);
            this.form.markAsPristine();
          }),
          finalize(() => {
            this.deactivateSpinner();
          }),
          catchError((errorResponse: any) => {
            this.saveCatchError(errorResponse);
            throw of(errorResponse);
          })
        );
      })
    );
  }

  activateSpinner() {
    this.workingSaveForm = true;
    if (this.enableSpinner) {
      const spinnerService = InjectorService.injector.get(SpinnerService);
      spinnerService.activate(this.formId);
    }
  }

  deactivateSpinner() {
    this.workingSaveForm = false;
    if (this.enableSpinner) {
      const spinnerService = InjectorService.injector.get(SpinnerService);
      spinnerService.deactivate(this.formId);
    }
  }

  /**
   * Method to handle the http error messages
   * @param errorResponse the error
   */
  saveCatchError(errorResponse: any) {
    if (errorResponse.status === 400 && errorResponse.error && _.isArray(errorResponse.error)) {
      const errors = _.uniqBy(errorResponse.error, 'message');
      this.errorMessagesSubject.next(errors);
    }
  }

  /**
   * Default patch form to object
   * @returns {any}
   */
  patchObject(): T {
    return _.assignIn(this.data, this.form.value);
  }

  // handle cancel form after data are unsaved
  cancel() {
    const unsavedChangesService = InjectorService.injector.get(UnsavedChangesService);
    unsavedChangesService.ignoreChanges([this.formId]).subscribe((done: boolean) => {
      if (done) {
        this.afterCancel();
      }
    });
  }

  //Method to override to handle after confirmation of the unsaved changes dialog
  afterCancel() {
    //do nothing
  }
}
