import { AbstractControl, ValidationErrors, ValidatorFn } from '@angular/forms';
import { environment } from '../../../environments/environment';

/**
 * Input validation for decimal with given decimal number precision
 * @param maxPrecision the given max validation precision
 * @param preventZero the boolean value to specify if zero is allowed
 */
export function decimalNumberPrecision(maxPrecision: number, preventZero?: boolean): ValidatorFn {
  return (control: AbstractControl): ValidationErrors | null => {
    return checkDecimalNumber(control, maxPrecision, preventZero);
  };
}

/**
 * function that checks if given decimal number in a form control is valid with the given precision
 */
function checkDecimalNumber(
  control: AbstractControl,
  precision?: number,
  preventZero?: boolean
): ValidationErrors | null {
  if (control.value === undefined || control.value === null || control.value.length == 0) {
    return null;
  }
  const v: any = control.value;
  if (preventZero && (v === '0' || v === 0 || v === '0.0' || v === '0.00')) {
    return {
      numberNotZero: {},
    };
  }
  const finalPrecision = precision ? precision : 2;
  const pattern = new RegExp(
    `^[+-]?(?!\\.(?!\\d))(\\d{1,3}(?:’?\\d{3})*|\\d+)?(?:\\.\\d{0,${finalPrecision}})?$`
  );
  return pattern.test(v)
    ? null
    : {
        decimalNumber: {
          precision: finalPrecision,
        },
      };
}
