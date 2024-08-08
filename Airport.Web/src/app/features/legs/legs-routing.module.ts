import { NgModule } from '@angular/core';
import { RouterModule, Routes } from '@angular/router';
import { LegsComponent } from './legs/legs.component';

const routes: Routes = [{ path: '', component: LegsComponent }];

@NgModule({
  declarations: [],
  imports: [RouterModule.forChild(routes)],
  exports: [RouterModule],
})
export class LegsRoutingModule {}
