import { Injectable } from '@angular/core';
import { ControlContainer } from '@angular/forms';
import { GlobalEventsService } from '@core/global-events.service';
import { TranslateService } from '@ngx-translate/core';
import { Observable, map, of } from 'rxjs';
import { ModalMessageAttribute } from '../dialog/dialog-message/dialog-message.component';
import { DialogDynamicService } from '../dialog/dialog.service';

@Injectable({ providedIn: 'root' })
export class UnsavedChangesService {
  public isActive = true;
  private readonly controlContainer: Record<string, ControlContainer> = {};

  constructor(
    private readonly globalEventsService: GlobalEventsService,
    private readonly translateService: TranslateService,
    private readonly dialogService: DialogDynamicService
  ) {
    globalEventsService.beforeUnload$.subscribe(event => this.onUnload(event));
  }

  watch(formId: string, form: ControlContainer): void {
    this.controlContainer[formId] = form;
  }

  unWatch(formId: string): void {
    delete this.controlContainer[formId];
  }

  canDeactivate(): Observable<boolean> {
    return this.ignoreChanges();
  }

  ignoreChanges(formIds?: string[]): Observable<boolean> {
    if (this.hasPendingChanges(formIds)) {
      const dialogOptions = {
        acceptLabelKey: 'lblYes',
        declineLabelKey: 'lblNo',
        blockClose: true,
        position: { top: '0px' },
      };
      const dialog = this.dialogService.openConfirmationDialog(
        new ModalMessageAttribute(
          this.translateService.instant('unsavedChanges'),
          this.translateService.instant('unsavedChangesTitle')
        ),
        dialogOptions
      );
      dialog.updatePosition({ top: '20px' });

      return dialog.afterClosed().pipe(
        map((answer: any) => {
          if (answer) {
            return true;
          }
          return false;
        })
      );
    } else {
      return of(true);
    }
  }

  private onUnload(event: BeforeUnloadEvent): string | null {
    if (this.hasPendingChanges()) {
      const confirmationMessage = this.message();
      event.returnValue = confirmationMessage;
      return confirmationMessage;
    }
    return null;
  }

  private hasPendingChanges(ids: string[] = Object.keys(this.controlContainer)): boolean {
    const includesPendingChanges =
      Object.keys(this.controlContainer).filter(
        formId => ids.includes(formId) && this.controlContainer[formId].dirty
      ).length > 0;
    return this.isActive && includesPendingChanges;
  }

  private message(): string {
    return this.translateService.instant('i18n.validation.unsavedChanges');
  }
}
