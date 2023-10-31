import { UntypedFormBuilder, UntypedFormGroup, UntypedFormControl, UntypedFormArray, AbstractControl, ValidationErrors, Validators } from "@angular/forms";
import { Observable } from "rxjs";

export abstract class BaseFormComponent {

  constructor(protected fb: UntypedFormBuilder) {
  }

  /**
   * Marks all form as touched
   * @param formGroup
   */
  markTouchedFields(formGroup: UntypedFormGroup) {
    formGroup.markAsTouched({ onlySelf: true });
    Object.keys(formGroup.controls).forEach(field => {
      const control = formGroup.get(field);
      if (control instanceof UntypedFormControl) {
        control.markAsTouched({ onlySelf: true });
      } else if (control instanceof UntypedFormGroup) {
        control.markAsTouched({ onlySelf: true });
        this.markTouchedFields(control);
      } else if (control instanceof UntypedFormArray) {
        if(control.validator !== null) {
          control.markAsPending();
        }
        control.markAsTouched();
        for (let controlGroup of control.controls) {
          controlGroup.markAsTouched();
          this.markTouchedFields(controlGroup as UntypedFormGroup);
        }
      }
    });
  }
  /**
   * Add id and version formGroup
   * @param required specify if the field will be required.
   * @returns version form group the created form group.
   */
  addFormGroup(required?: boolean): UntypedFormGroup {
    let validators: ((control: AbstractControl) => ValidationErrors | null)[] = [];
    if (required) {
      validators = [Validators.required];
    }
    return this.fb.group({ id: [null, validators], version: null });
  }
  /**
   * Clears a form array
   * @param {FormArray} formArray
   * @memberof BaseFormComponent
   */
  clearFormArray(formArray: UntypedFormArray) {
    if (!formArray) {
      return;
    }
    while (formArray.length !== 0) {
      formArray.removeAt(0);
    }
  }
}



/**
 *  Interface to handle methods of persisting
 */
export interface IPersistObjectService<T> {
  updateValueChange?(active: any): unknown;
  valueChange?: boolean;
  load(params: any): Observable<T>;
  save(data: T, id?: string, valueChange?: boolean, extraParams?: any): Observable<T>;
}
