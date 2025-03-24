import { NgModule } from '@angular/core';
import { RouterModule, Routes } from '@angular/router';
import { UserGamificationComponent } from './user-gamification/user-gamification.component';
import { LeaderboardComponent } from './leaderboard/leaderboard.component';

const routes: Routes = [
  { path: '', component: UserGamificationComponent },
  { path: 'leaderboard', component: LeaderboardComponent }
];

@NgModule({
  imports: [RouterModule.forChild(routes)],
  exports: [RouterModule]
})
export class GamificationRoutingModule { }
