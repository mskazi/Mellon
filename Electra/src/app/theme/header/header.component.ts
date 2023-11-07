import {
  AfterViewInit,
  Component,
  EventEmitter,
  HostBinding,
  Input,
  Output,
  ViewEncapsulation,
} from '@angular/core';
import { ActivatedRoute, Router } from '@angular/router';
import { Subscription } from 'rxjs/internal/Subscription';
import screenfull from 'screenfull';

@Component({
  selector: 'app-header',
  templateUrl: './header.component.html',
  styleUrls: ['./header.component.scss'],
  encapsulation: ViewEncapsulation.None,
})
export class HeaderComponent implements AfterViewInit {
  @HostBinding('class') class = 'matero-header';

  @Input() showToggle = true;
  @Input() showBranding = false;

  @Output() toggleSidenav = new EventEmitter<void>();
  @Output() toggleSidenavNotice = new EventEmitter<void>();
  private routeSubscription: Subscription;

  constructor(
    private router: Router,
    protected route: ActivatedRoute
  ) {}

  ngAfterViewInit(): void {
    this.routeSubscription = this.route.queryParams.subscribe((queryParam: any) => {
      this.searchTerm = queryParam['searchTerm'];
    });
  }
  ngOnDestroy() {
    if (this.routeSubscription) {
      this.routeSubscription.unsubscribe();
    }
  }

  searchTerm: string;
  toggleFullscreen() {
    if (screenfull.isEnabled) {
      screenfull.toggle();
    }
  }

  search() {
    if (!this.searchTerm) {
      return;
    }

    this.router.navigate([`search`], { queryParams: { searchTerm: this.searchTerm } });
  }
}
