import { Component } from '@angular/core';

@Component({
  selector: 'app-error-unauthorized',
  template: `
    <error-code
      code="Unauthorized"
      title="No Account or Active account"
      message="Please call your system administrator."
      [showBackButton]="false"
    >
    </error-code>
  `,
})
export class UnauthorizedComponent {}
