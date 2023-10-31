import { CommonModule, DatePipe } from '@angular/common';
import { NgModule } from '@angular/core';
import { FormsModule, ReactiveFormsModule } from '@angular/forms';
import { RouterModule } from '@angular/router';

import { MaterialExtensionsModule } from '../material-extensions.module';
import { MaterialModule } from '../material.module';

import { TranslateModule } from '@ngx-translate/core';
import { NgxPermissionsModule } from 'ngx-permissions';
import { NgProgressModule } from 'ngx-progressbar';
import { NgProgressHttpModule } from 'ngx-progressbar/http';
import { NgProgressRouterModule } from 'ngx-progressbar/router';
import { ToastrModule } from 'ngx-toastr';

import { BreadcrumbComponent } from './components/breadcrumb/breadcrumb.component';
import { DialogMessageMultipleOptionsComponent } from './components/dialog/dialog-message-multiple-options/dialog-message-multiple-options.component';
import { DialogMessageComponent } from './components/dialog/dialog-message/dialog-message.component';
import { DialogDynamicComponent } from './components/dialog/dialog.component';
import { ErrorCodeComponent } from './components/error-code/error-code.component';
import { ErrorMessagesDirective } from './components/error-messages/error-messages.directive';
import { ErrorDirective } from './components/error-messages/mat-error.directive';
import { PageHeaderComponent } from './components/page-header/page-header.component';
import { SpinnerComponent } from './components/spinner/spinner.component';
import { DisableControlDirective } from './directives/disable-control.directive';
import { SafeUrlPipe } from './pipes/safe-url.pipe';
import { ToObservablePipe } from './pipes/to-observable.pipe';
import { UnsavedChangesDirective } from './components/unsavedChanges/unsaved-changes.directive';

const MODULES: any[] = [
  CommonModule,
  RouterModule,
  ReactiveFormsModule,
  FormsModule,
  MaterialModule,
  MaterialExtensionsModule,
  NgProgressModule,
  NgProgressRouterModule,
  NgProgressHttpModule,
  NgxPermissionsModule,
  ToastrModule,
  TranslateModule,
];
const COMPONENTS: any[] = [
  BreadcrumbComponent,
  PageHeaderComponent,
  ErrorCodeComponent,
  SpinnerComponent,
  DialogDynamicComponent,
  DialogMessageMultipleOptionsComponent,
  DialogMessageComponent,
  ErrorDirective,
  ErrorMessagesDirective,
  UnsavedChangesDirective,
];
const COMPONENTS_DYNAMIC: any[] = [];
const DIRECTIVES: any[] = [DisableControlDirective];
const PIPES: any[] = [SafeUrlPipe, ToObservablePipe];
@NgModule({
  imports: [...MODULES],
  exports: [...MODULES, ...COMPONENTS, ...DIRECTIVES, ...PIPES],
  declarations: [...COMPONENTS, ...COMPONENTS_DYNAMIC, ...DIRECTIVES, ...PIPES],
  providers: [DatePipe],
})
export class SharedModule {}
