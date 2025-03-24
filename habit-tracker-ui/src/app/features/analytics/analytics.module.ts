import { NgModule } from '@angular/core';
import { CommonModule } from '@angular/common';

import { AnalyticsRoutingModule } from './analytics-routing.module';
import { ReportComponent } from './report/report.component';
import { SharedModule } from 'src/app/shared/shared.module';

@NgModule({
  declarations: [
    ReportComponent
  ],
  imports: [
    CommonModule,
    AnalyticsRoutingModule,
    SharedModule
  ]
})
export class AnalyticsModule { }
