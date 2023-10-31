import { Injectable } from '@angular/core';
import { Observable, Subject } from 'rxjs';

/**
 * SpinnerService (TODO: Rethink this concept)
 *
 */
@Injectable({ providedIn: 'root' })
export class SpinnerService {
  /**
   * The channel name where spinner events will be broadcasted to.
   */
  public static CHANNEL = 'default';

  public readonly events$: Observable<SpinnerEvent>;

  private calls: Record<string, number> = {};
  private readonly events: Subject<SpinnerEvent> = new Subject<SpinnerEvent>();

  constructor() {
    this.events$ = this.events.asObservable();
  }

  public activate(channel = SpinnerService.CHANNEL): void {
    if (this.increase(channel) >= 1) {
      console.log('broadcast increase' + channel);
      console.log('broadcast number' + this.calls[channel]);
      this.broadcast({
        active: true,
        channel,
      });
    }
  }

  public deactivate(channel = SpinnerService.CHANNEL): void {
    if (this.decrease(channel) === 0) {
      console.log('broadcast decrease' + channel);
      console.log('broadcast number' + this.calls[channel]);
      this.broadcast({
        active: false,
        channel,
      });
    }
  }

  public forceDeactivate(channel = SpinnerService.CHANNEL): void {
    this.calls[channel] = 0;
    this.deactivate(channel);
  }

  private broadcast(event: SpinnerEvent): void {
    this.events.next(event);
  }

  private increase(channel: string = SpinnerService.CHANNEL): number {
    this.calls[channel] = (this.calls[channel] || 0) + 1;
    return this.calls[channel];
  }

  private decrease(channel: string = SpinnerService.CHANNEL): number {
    if (this.calls[channel] === 0 || this.calls[channel] === undefined) {
      return this.calls[channel];
    }
    this.calls[channel] = (this.calls[channel] || 0) - 1;
    return this.calls[channel];
  }
}

export interface SpinnerEvent {
  active: boolean;
  channel: string;
}

export enum SpinnerType {
  PROGRESS_BAR,
  CIRCLE,
  FIXED_CIRCLE,
}
