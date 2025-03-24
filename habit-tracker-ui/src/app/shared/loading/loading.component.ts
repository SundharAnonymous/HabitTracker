import { Component } from '@angular/core';
import { Observable } from 'rxjs';
import { LoadingService } from '../loading.service';

@Component({
  selector: 'app-loading',
  template: `
    <div class="overlay" *ngIf="loading$ | async">
      <mat-progress-spinner mode="indeterminate" color="accent"></mat-progress-spinner>
    </div>
  `,
  styleUrls: ['./loading.component.scss']
})
export class LoadingComponent {
  loading$: Observable<boolean>;

  constructor(private loadingService: LoadingService) {
    this.loading$ = this.loadingService.loading$;
  }
}
