import { NgModule } from '@angular/core';
import { CommonModule } from '@angular/common';

import { AiRoutingModule } from './ai-routing.module';
import { RecommendationComponent } from './recommendation/recommendation.component';
import { SharedModule } from 'src/app/shared/shared.module';

@NgModule({
  declarations: [
    RecommendationComponent
  ],
  imports: [
    CommonModule,
    AiRoutingModule,
    SharedModule
  ]
})
export class AiModule { }
