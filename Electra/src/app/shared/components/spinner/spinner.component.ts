import { animate, keyframes, state, style, transition, trigger } from '@angular/animations';
import { Component, ElementRef, Input, OnDestroy, OnInit, ViewChild, ViewEncapsulation } from '@angular/core';
import { ReplaySubject, Subject } from 'rxjs';
import { delay, filter, takeUntil, tap } from 'rxjs/operators';
import { SpinnerEvent, SpinnerService, SpinnerType } from './spinner.service';

@Component({
    selector: 'spinner',
    exportAs: 'spinner',
    templateUrl: './spinner.component.html',
    styleUrls: ['./spinner.component.scss'],
    encapsulation: ViewEncapsulation.None,
    animations: [
        trigger('inOut', [
            state('in', style({ opacity: 1, display: 'block' })),
            transition('out => in', [
                style({ display: 'block' }), // As we can not animate the `display` property, we modify it before starting the next animation.
                animate('250ms ease-in-out', keyframes([style({ offset: 0, opacity: 0 }), style({ offset: 1, opacity: 1 })]))
            ]),
            state('out', style({ opacity: 0, display: 'none' })),
            transition('in => out', [animate('250ms ease-in-out', keyframes([style({ offset: 0, opacity: 1 }), style({ offset: 1, opacity: 0 })]))])
        ])
    ],
    host: { class: 'ob-spinner' }
})
export class SpinnerComponent implements OnInit, OnDestroy {
    @Input() channel: string = SpinnerService.CHANNEL;
    @Input() fixed = false;
    @Input() type: SpinnerType = SpinnerType.PROGRESS_BAR;
    spinnerTypes = SpinnerType;
    @ViewChild('spinnerContainer') spinnerContainer: ElementRef;
    $state = 'out';
    private readonly unsubscribe = new ReplaySubject<void>();

    constructor(private readonly spinnerService: SpinnerService, private readonly element: ElementRef) { }

    ngOnInit(): void {
        this.element.nativeElement.parentElement.classList.add('ob-has-overlay');
        this.spinnerService.events$
            .pipe(
                tap((event: SpinnerEvent) => {
                    console.log(this.channel);
                    console.log(event.channel);
                }),
                takeUntil(this.unsubscribe),
                filter(event => event.channel === this.channel),
                //delay(0)
                tap((event: SpinnerEvent) => {
                    console.log("channel state" + this.channel + " " + event.active);
                    this.$state = event.active ? 'in' : 'out'
                })
            )
            .subscribe((event: SpinnerEvent) => {

            });
    }

    ngOnDestroy(): void {
        this.unsubscribe.next();
        this.unsubscribe.complete();
    }
}