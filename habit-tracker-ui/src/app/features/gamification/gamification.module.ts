import { NgModule } from '@angular/core';
import { CommonModule } from '@angular/common';

import { GamificationRoutingModule } from './gamification-routing.module';
import { UserGamificationComponent } from './user-gamification/user-gamification.component';
import { LeaderboardComponent } from './leaderboard/leaderboard.component';
import { SharedModule } from 'src/app/shared/shared.module';
import { MatChipsModule } from '@angular/material/chips';

@NgModule({
  declarations: [
    UserGamificationComponent,
    LeaderboardComponent
  ],
  imports: [
    CommonModule,
    GamificationRoutingModule,
    SharedModule,
    MatChipsModule
  ]
})
export class GamificationModule { }
