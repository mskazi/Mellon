import { Directive, ElementRef, OnDestroy, OnInit, Optional } from '@angular/core';
import { ValidationErrors } from '@angular/forms';
import { TranslatePipe, TranslateService } from '@ngx-translate/core';
import { filter, takeUntil, tap } from 'rxjs/operators';
import { Subject } from 'rxjs';
import { ErrorMessagesDirective } from './error-messages.directive';

@Directive({
  // eslint-disable-next-line @angular-eslint/directive-selector
  selector: 'mat-error',
})
export class ErrorDirective implements OnInit, OnDestroy {
  private readonly pipe: TranslatePipe;
  private errors: ValidationErrors = {};
  private readonly unsubscribe = new Subject<void>();

  constructor(
    @Optional() private readonly control: ErrorMessagesDirective,
    private readonly el: ElementRef,
    translate: TranslateService
  ) {
    if (this.control) {
      translate.onLangChange.subscribe(() => this.showErrors(this.errors || {}));
    }
  }

  ngOnInit(): void {
    if (this.control) {
      this.control.errors$
        .pipe(
          filter(errors => !!errors),
          tap(errors => (this.errors = errors)),
          takeUntil(this.unsubscribe)
        )
        .subscribe(errors => this.showErrors(errors));
    }
  }

  ngOnDestroy(): void {
    this.unsubscribe.next();
    this.unsubscribe.complete();
  }

  private showErrors(errors: ValidationErrors): void {
    this.el.nativeElement.innerText = Object.keys(errors)
      .map(key => this.pipe.transform(`i18n.validation.${key}`, errors[key]))
      .join('\n');
  }
}
