import { Component, OnInit, AfterViewInit } from '@angular/core';
import { PreloaderService } from '@core';

@Component({
  selector: 'app-root',
  template: '<router-outlet  *ngIf="!isIframe"></router-outlet>',
})
export class AppComponent implements OnInit, AfterViewInit {
  isIframe = false;
  constructor(private preloader: PreloaderService) {}

  ngOnInit() {
    this.isIframe = window !== window.parent && !window.opener;
  }

  ngAfterViewInit() {
    this.preloader.hide();
  }
}
