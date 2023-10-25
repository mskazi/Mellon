import { Component, OnInit, AfterViewInit } from '@angular/core';
import { PreloaderService, SettingsService } from '@core';
import 'reflect-metadata';

@Component({
  selector: 'app-root',
  template: '<router-outlet *ngIf="!isIframe"></router-outlet>',
})
export class AppComponent implements OnInit, AfterViewInit {
  isIframe = false;

  constructor(
    private preloader: PreloaderService,
    private settings: SettingsService
  ) {}

  ngOnInit() {
    this.isIframe = window !== window.parent && !window.opener;
    this.settings.setDirection();
    this.settings.setTheme();
  }

  ngAfterViewInit() {
    this.preloader.hide();
  }
}
