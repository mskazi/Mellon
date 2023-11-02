import { ChangeDetectorRef, Component, EventEmitter, Input, OnInit, Output } from '@angular/core';
import { ControlContainer, UntypedFormGroup } from '@angular/forms';

/**
 * Component for displaying input-field
 */
@Component({
  selector: 'app-input-field',
  templateUrl: './input-field.component.html',
  /*  changeDetection: ChangeDetectionStrategy.OnPush */
})
export class InputFieldComponent implements OnInit {
  // the label of the field
  @Input() public label: string;
  // the property name where the input field will be bind to
  @Input() public controlName: string;
  // specifies if field is mandatory
  @Input() public mandatory: boolean = false;
  // the hint of the field(text under input)
  @Input() public hint: string;
  // specifies if hint must be shown.
  @Input() public showHint: boolean = true;
  // the type of the field
  @Input() public type: string = 'text';
  // specifies the step if the input field is number
  @Input() public step: string;
  // specifies the min value if the input field is number
  @Input() public min: string;
  // specifies if the field is read-only
  @Input() public readOnly: boolean = false;

  @Input() inputGroup: UntypedFormGroup;

  @Input() placeHolder: string;

  @Output() valueChangedEvent = new EventEmitter<string>();

  // the parent form group
  public form: UntypedFormGroup;
  constructor(
    protected controlContainer: ControlContainer,
    protected cdr: ChangeDetectorRef
  ) {}
  // On init of component
  ngOnInit() {
    // access parent form group control
    this.form = <UntypedFormGroup>this.controlContainer.control;
  }

  valueChanged(event: any) {
    if (event) {
      this.form.markAsDirty();
      this.valueChangedEvent.emit(this.form.get(this.controlName)?.value);
    }
  }
}
