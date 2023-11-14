import {
  ChangeDetectorRef,
  Component,
  ElementRef,
  EventEmitter,
  Input,
  OnInit,
  Output,
  ViewChild,
} from '@angular/core';
import { ControlContainer, UntypedFormGroup } from '@angular/forms';
import { MatSelect } from '@angular/material/select';
import { TranslateService } from '@ngx-translate/core';
import { Utilities } from '@shared/utils/utilities';
import * as _ from 'lodash';
import { BehaviorSubject, Subscription, tap } from 'rxjs';
import { Observable } from 'rxjs/internal/Observable';

/**
 * Component for displaying select-field-error combination
 */
@Component({
  selector: 'app-select-field',
  templateUrl: './select-field.component.html',
  styleUrls: ['./select-field.component.scss'],
})
export class SelectFieldComponent implements OnInit {
  @Input() public label: string;
  // options to be loaded on select field
  @Input() public options$: Observable<any[]>;
  // specifies if field is mandatory
  @Input() public mandatory: boolean = false;
  // specifies if allows empty value
  @Input() public allowEmpty: boolean = true;
  // the extra property to display (instead of name)
  @Input() public displayProperty: string;
  // the extra property to display (instead of name)
  @Input() public controlName: string = 'id';

  @Input() displayFn?: any;
  // the placeholder name of the input field
  @Input() public placeholder: string;
  /**
   * pass a subject for clearing the values of the component
   * @memberof SelectFieldComponent
   */
  @Input() clearValuesSubject = new BehaviorSubject<boolean>(false);

  clearValues$: Observable<any>;

  // the parent form group
  public form: UntypedFormGroup;
  options: any[] = [];
  selectedOptions: any[] = [];
  private optionSubscription: Subscription;
  @Output() valueChanged = new EventEmitter<any>();
  @ViewChild(MatSelect) public matSelect: MatSelect;
  @ViewChild('search', { read: ElementRef }) searchInput: ElementRef;
  constructor(
    protected controlContainer: ControlContainer,
    protected cdr: ChangeDetectorRef,
    protected translate: TranslateService
  ) {}
  // On init of component
  ngOnInit() {
    // access parent form group control
    this.form = <UntypedFormGroup>this.controlContainer.control;
    // on lang change mark for changes
    // on options loaded
    this.optionSubscription = this.options$?.subscribe(options => {
      this.options = options;
      this.selectedOptions = options;
    });
    this.initResetValues();
  }

  initResetValues() {
    this.clearValues$ = this.clearValuesSubject.asObservable();
    this.clearValues$
      .pipe(
        tap((data: boolean) => {
          if (data) {
            this.onTextClear();
            this.valueChanged.emit(this.selectedOptions);
          }
        })
      )
      .subscribe();
  }

  onKey(value: any) {
    this.selectedOptions = this.search(value);
  }

  onKeyDown($event: any) {
    Utilities.allowArrowNavigation($event);
  }

  // Filter the states list and send back to populate the selectedStates**
  search(event: any) {
    if (!event.target?.value) {
      return this.options;
    }
    let filter = event.target?.value.toLowerCase();
    let filteredOptions;

    if (this.displayProperty) {
      filteredOptions = this.options.filter(
        option => option[this.displayProperty]?.toLowerCase().includes(filter)
      );
    } else {
      filteredOptions = this.options.filter(
        option => _.toString(option.display)?.toLowerCase().includes(filter)
      );
    }
    return filteredOptions;
  }

  openedChange(event: any) {
    if (this.searchInput) {
      this.searchInput.nativeElement.focus();
    }
    if (event === false) {
      const inputElements = document.querySelectorAll('.filter');
      _.each(inputElements, (inputElement: any) => {
        inputElement.value = '';
      });
      this.onKey('');
    }
  }
  /**
   * On selection change set version input fi
   * @param {*} event
   * @memberof SelectFieldComponent
   */
  selectionChange(event: any) {
    if (event) {
      const option = this.options.find(o => o[this.displayProperty] === event.value);
      this.valueChanged.emit(option);
    }
  }

  customDisplay(option: any) {
    return this.displayProperty
      ? option[this.displayProperty] + ', ' + option.display
      : option.display;
  }

  getOptionDisplayValue(option: any) {
    if (this.displayProperty) {
      return option[this.displayProperty];
    }
    return option;
  }

  onTextClear() {
    this.form.markAsDirty();
    this.form.controls[this.controlName].markAsTouched();
    this.form.controls[this.controlName].patchValue(null);
    this.valueChanged.emit(null);
  }

  // On destroy of component
  ngOnDestroy() {
    if (this.optionSubscription) {
      this.optionSubscription.unsubscribe();
    }
  }
}
