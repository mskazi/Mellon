import { Component } from '@angular/core';
import { ComponentFixture, TestBed } from '@angular/core/testing';
import { MAT_DIALOG_DATA, MatDialogRef } from '@angular/material/dialog';
import { NoopAnimationsModule } from '@angular/platform-browser/animations';
import { Observable, of } from 'rxjs';
import { SubmissTestingMockModule } from 'src/app/spec/mock.module.spec';
import { DialogResponse, IDialogDataComponent } from './dialog';
import { DialogDynamicComponent } from './dialog.component';

describe('DialogDynamicComponent', () => {
  let component: DialogDynamicComponent;
  let fixture: ComponentFixture<DialogDynamicComponent>;
  let dialogRef: Partial<MatDialogRef<HostedComponent>>;
  let h1: HTMLElement;


  @Component({
    selector: "test-hosted",
    template: "this is my template"
  })
  class HostedComponent implements IDialogDataComponent<string, boolean> {
    dialogData: string;
    getAcceptedDialogData(): Observable<DialogResponse<boolean>> {
      return of({
        close: true,
        dataDialog: true
      });
    }
    // can close modal
    canClose() {
      return of(true);
    }

    closeFn() {

    }
  }

  beforeEach(async () => {
    const dialogData: any = {
      component: HostedComponent,
      componentData: 'myData',
      data: {

      },
      options: {
        titleKey: 'test',
      }
    };

    await TestBed.configureTestingModule({
      declarations: [DialogDynamicComponent],
      imports: [SubmissTestingMockModule, NoopAnimationsModule],
      providers: [
        { provide: MatDialogRef, useValue: dialogRef },
        { provide: MAT_DIALOG_DATA, useValue: dialogData },
      ],
    }).compileComponents();
  });

  beforeEach(() => {
    fixture = TestBed.createComponent(DialogDynamicComponent);
    component = fixture.componentInstance;
    dialogRef = {
      keydownEvents: () => of({ key: 'Escape' }),
      close: () => { },
      backdropClick: () => of({}),
    } as MatDialogRef<HostedComponent>;
    fixture.detectChanges();
  });

  it('should create the component', () => {
    expect(component).toBeTruthy();
  });

  it('should initialize component properties', () => {
    h1 = fixture.nativeElement.querySelector('h1');
    const mockComponentRef = jasmine.createSpyObj('ComponentRef', ['instance']);
    spyOn(component.viewContainerRef, 'createComponent').and.returnValue(mockComponentRef);
    expect(component.viewContainerRef).toBeDefined();
    expect(h1.textContent).toContain(component.data.options.titleKey);
    expect((component as any).componentRef.instance.action).toBeDefined();
    expect((component as any).componentRef.instance.setFormMode).toBeDefined();
    expect((component as any).componentRef.instance.setFooterTemplate).toBeDefined();
    expect((component as any).componentRef.instance.closeModal).toBeDefined();

  });

  it('should destroy the component', () => {
    spyOn(component, 'ngOnDestroy').and.callThrough();
    component.ngOnDestroy();
    expect(component.ngOnDestroy).toHaveBeenCalled();
  });


  it('should close the dialog on action', () => {
    spyOn((component as any).dialogRef, 'close');
    const mockResponse = { close: true, dataDialog: {} };
    spyOn((component as any).componentRef.instance, 'getAcceptedDialogData').and.returnValue(of(mockResponse));
    component.action();

    expect((component as any).dialogRef.close).toHaveBeenCalledWith(mockResponse.dataDialog);
  });

  it('should close the dialog with override of close fn', () => {
    spyOn((component as any).componentRef.instance, 'closeFn');
    component.close();
    expect((component as any).componentRef.instance.closeFn).toHaveBeenCalled();
  });

  /*  it('should close the dialog if component allows closing', () => {
     spyOn((component as any).dialogRef, 'close');
     spyOn((component as any).componentRef.instance, 'canClose').and.returnValue(of(true));
     component.closeModal();
     expect((component as any).dialogRef.close).toHaveBeenCalled();
   });
  */
  /*   it('should not close the dialog if component does not allow closing', () => {
      spyOn((component as any).dialogRef, 'close');
      spyOn((component as any).componentRef.instance, 'canClose').and.returnValue(of(false));
      component.closeModal();
      expect((component as any).dialogRef.close).not.toHaveBeenCalled();
    }); */

});

