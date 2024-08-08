import { NgModule } from '@angular/core';
import { CommonModule, DatePipe } from '@angular/common';
import { FlightsComponent } from './flights/flights.component';
import { FlightEntryComponent } from './flights/flight-entry/flight-entry.component';
import { FlightsRoutingModule } from './flights-routing.module';
import { NgxPaginationModule } from 'ngx-pagination';

@NgModule({
  declarations: [FlightsComponent, FlightEntryComponent],
  imports: [CommonModule, FlightsRoutingModule, NgxPaginationModule],
  providers: [DatePipe],
  exports: [FlightsComponent, FlightEntryComponent],
})
export class FlightsModule {}
