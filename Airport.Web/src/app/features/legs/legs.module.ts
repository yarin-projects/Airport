import { NgModule } from '@angular/core';
import { CommonModule } from '@angular/common';
import { LegsComponent } from './legs/legs.component';
import { LegEntryComponent } from './legs/leg-entry/leg-entry.component';
import { LegsRoutingModule } from './legs-routing.module';
import { SharedModule } from '../../shared/shared.module';

@NgModule({
  declarations: [LegsComponent, LegEntryComponent],
  imports: [CommonModule, LegsRoutingModule, SharedModule],
  exports: [LegsComponent, LegEntryComponent],
})
export class LegsModule {}
