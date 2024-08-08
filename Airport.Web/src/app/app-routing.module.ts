import { NgModule } from '@angular/core';
import { RouterModule, Routes } from '@angular/router';

const routes: Routes = [
  {
    path: '',
    loadChildren: () =>
      import('./features/home/home.module').then((m) => m.HomeModule),
  },
  {
    path: 'home',
    loadChildren: () =>
      import('./features/home/home.module').then((m) => m.HomeModule),
  },
  {
    path: 'legs',
    loadChildren: () =>
      import('./features/legs/legs.module').then((m) => m.LegsModule),
  },
  {
    path: 'logs',
    loadChildren: () =>
      import('./features/logs/logs.module').then((m) => m.LogsModule),
  },
  {
    path: 'flights',
    loadChildren: () =>
      import('./features/flights/flights.module').then((m) => m.FlightsModule),
  },
  { path: '**', redirectTo: '', pathMatch: 'full' },
];

@NgModule({
  imports: [RouterModule.forRoot(routes)],
  exports: [RouterModule],
})
export class AppRoutingModule {}
