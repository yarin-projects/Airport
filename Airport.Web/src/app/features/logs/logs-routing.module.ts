import { NgModule } from '@angular/core';
import { RouterModule, Routes } from '@angular/router';
import { LogsComponent } from './logs/logs.component';

const routes: Routes = [{ path: '', component: LogsComponent }];

@NgModule({
  declarations: [],
  imports: [RouterModule.forChild(routes)],
  exports: [RouterModule],
})
export class LogsRoutingModule {}
