import { NgModule } from '@angular/core';
import { RouterModule, Routes } from '@angular/router';

const routes: Routes = [
  { path: '', redirectTo: 'auth/login', pathMatch: 'full' },
  { path: 'auth', loadChildren: () => import('./features/auth/auth.module').then(m => m.AuthModule) },
  { path: 'habits', loadChildren: () => import('./features/habits/habits.module').then(m => m.HabitsModule) },
  { path: 'notifications', loadChildren: () => import('./features/notifications/notifications.module').then(m => m.NotificationsModule) },
  { path: 'gamification', loadChildren: () => import('./features/gamification/gamification.module').then(m => m.GamificationModule) },
  { path: 'analytics', loadChildren: () => import('./features/analytics/analytics.module').then(m => m.AnalyticsModule) },
  { path: 'ai', loadChildren: () => import('./features/ai/ai.module').then(m => m.AiModule) }
];

@NgModule({
  imports: [RouterModule.forRoot(routes)],
  exports: [RouterModule]
})
export class AppRoutingModule { }
