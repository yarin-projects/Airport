import { Component, Input } from '@angular/core';
import { Flight } from '../../../../shared/interfaces/flight.interface';
import { DatePipe } from '@angular/common';
import { config } from '../../../../shared/config/config.env';

@Component({
  selector: 'app-flight-entry',
  templateUrl: './flight-entry.component.html',
  styleUrls: ['./flight-entry.component.css'],
})
export class FlightEntryComponent {
  @Input() flight!: Flight;
  @Input() activeFlightType!: string;
  flightStatusLabels = config.flightStatusLabels;

  constructor(private datePipe: DatePipe) {}
  getFlightId(): string {
    return this.flight.flightId === 0 ? 'N/A' : this.flight.flightId.toString();
  }
  getFlightTimeCompleted(): string {
    if (this.isTimeCompleteValid()) {
      return this.formatDate(this.flight.timeCompleted);
    } else {
      return '';
    }
  }
  formatDate(date: string): string {
    return this.datePipe.transform(date, 'HH:mm:ss a -- d MMM, yyyy') ?? '';
  }
  isTimeCompleteValid(): boolean {
    return new Date(this.flight.timeCompleted).getFullYear() > 2000;
  }
}
