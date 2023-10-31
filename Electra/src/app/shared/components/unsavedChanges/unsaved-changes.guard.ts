import { Injectable } from "@angular/core";
import { CanDeactivate } from "@angular/router";
import { Observable } from "rxjs";
import { UnsavedChangesService } from "./unsaved-changes.service";

@Injectable({ providedIn: 'root' })
export class UnsavedChangesGuard implements CanDeactivate<any> {
    // NOTE: Currently this guard has to be added on every route where it's used
    //      If this Issue: https://github.com/angular/angular/issues/11836  is resolved, we can rewrite it

    constructor(private readonly unsavedChangesService: UnsavedChangesService) { }

    canDeactivate(): Observable<boolean> {
        return this.unsavedChangesService.canDeactivate();
    }
}

