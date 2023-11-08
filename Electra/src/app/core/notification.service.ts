import { Injectable } from '@angular/core';
import { TranslateService } from '@ngx-translate/core';
import { ToastrService } from 'ngx-toastr';
/**
 * Notification service for showing errors , info and warnings
 */
@Injectable({ providedIn: 'root' })
export class NotificationService {
  constructor(
    private readonly toastrService: ToastrService,
    private translationService: TranslateService
  ) {}
  /**
   * Success message
   * @param message
   * @param title
   */
  success(message: string, title?: string) {
    this.toastrService.success(message, title);
  }
  /**
   * Info message
   * @param message
   * @param title
   */
  info(message: string, title?: string) {
    this.toastrService.info(message, title);
  }
  /**
   * Error message
   * @param message
   * @param title
   */
  error(message: string, title?: string) {
    this.toastrService.error(message, title, {
      timeOut: 6000,
    });
  }
  /**
   * Warning message
   * @param message
   * @param title
   */
  warning(message: string, title?: string) {
    this.toastrService.warning(message, title);
  }
}
/**
 * Object that contains the properties for notification.
 */
export class NotificationMessageAttribute {
  message: string;

  title: string;

  titleParams?: any;

  messageParams?: any;

  constructor(message: string, title: string, titleParams?: any, messageParams?: any) {
    this.message = message;
    this.title = title;
    this.titleParams = titleParams;
    this.messageParams = messageParams;
  }
}
