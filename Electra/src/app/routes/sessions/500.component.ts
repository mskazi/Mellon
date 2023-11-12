import { Component } from '@angular/core';

@Component({
  selector: 'app-error-500',
  template: `
    <error-code
      code="500"
      title="Server went wrong!"
      message="Please call your system administrator in order to resolve it"
    >
    </error-code>
  `,
})
export class Error500Component {}
