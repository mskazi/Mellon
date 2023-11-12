import { AbstractControl, ValidationErrors, ValidatorFn } from '@angular/forms';

/**
 * Input validation for decimal with given decimal number precision
 * @param maxPrecision the given max validation precision
 * @param preventZero the boolean value to specify if zero is allowed
 */
export function integerNumber(): ValidatorFn {
  return (control: AbstractControl): ValidationErrors | null => {
    return checkIntegerNumber(control);
  };
}

/**
 * function that checks if given decimal number in a form control is valid with the given precision
 */
function checkIntegerNumber(
  control: AbstractControl,
  precision?: number,
  preventZero?: boolean
): ValidationErrors | null {
  if (control.value === undefined || control.value === null || control.value.length == 0) {
    return null;
  }
  const v: any = control.value;

  const pattern = new RegExp(`^[0-9]*$`);
  return pattern.test(v)
    ? null
    : {
        integerNumber: true,
      };
}
