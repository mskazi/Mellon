import { Component, Input, OnInit } from "@angular/core";
import { ControlContainer, UntypedFormGroup } from "@angular/forms";

@Component({
  selector: 'app-textarea-field',
  templateUrl: './textarea-field.component.html'
})
export class TextareaFieldComponent implements OnInit {
  // the label of the field
  @Input() public label: string;
  // the property name where the input field will be bind to
  @Input() public propertyName: string;
  // specifies if field is mandatory
  @Input() public mandatory: boolean = false;
  // specifies the rows of the text field
  @Input() public rows: number = 3;
  // the hint of the field(text under input)
  @Input() public hint: string;
  // specifies if hint must be shown.
  @Input() public showHint: boolean = true;
  @Input() inputGroup: UntypedFormGroup;
  // the parent form group
  public form: UntypedFormGroup;
  constructor(protected controlContainer: ControlContainer) {

  }
  // On init of component
  ngOnInit() {
    // access parent form group control
    this.form = <UntypedFormGroup>this.controlContainer.control;
  }
}
