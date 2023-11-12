import { Injectable } from '@angular/core';
import {
  HttpErrorResponse,
  HttpEvent,
  HttpHandler,
  HttpInterceptor,
  HttpRequest,
} from '@angular/common/http';
import { Router } from '@angular/router';
import { Observable, throwError } from 'rxjs';
import { catchError } from 'rxjs/operators';
import { ToastrService } from 'ngx-toastr';
import { NotificationService } from '@core/notification.service';
import { MatDialog } from '@angular/material/dialog';

export enum STATUS {
  UNAUTHORIZED = 401,
  FORBIDDEN = 403,
  NOT_FOUND = 404,
  INTERNAL_SERVER_ERROR = 500,
}

@Injectable()
export class ErrorInterceptor implements HttpInterceptor {
  private errorPages = [STATUS.FORBIDDEN, STATUS.NOT_FOUND, STATUS.INTERNAL_SERVER_ERROR];

  private getMessage = (error: HttpErrorResponse) => {
    if (error.error?.message) {
      return error.error.message;
    }

    if (error.error?.msg) {
      return error.error.msg;
    }

    return `${error.status} ${error.statusText}`;
  };

  private getSubTitle = (error: HttpErrorResponse) => {
    if (error.error?.detail) {
      return `(${error.error?.Code ?? '-'}) ${error.error.detail}`;
    }
    return ``;
  };

  constructor(
    private router: Router,
    private toast: NotificationService,
    public dialog: MatDialog
  ) {}

  intercept(request: HttpRequest<unknown>, next: HttpHandler): Observable<HttpEvent<unknown>> {
    return next
      .handle(request)
      .pipe(catchError((error: HttpErrorResponse) => this.handleError(error)));
  }

  private handleError(error: HttpErrorResponse) {
    if (this.errorPages.includes(error.status)) {
      this.dialog.closeAll();
      this.router.navigateByUrl(`/${error.status}`, {
        skipLocationChange: true,
      });
    } else if (error.status === 400) {
      this.toast.error(this.getSubTitle(error), this.getMessage(error));
    } else {
      console.error('ERROR', error);
      this.toast.error(this.getMessage(error));
    }
    return throwError(() => error);
  }
}
