import { Component, Input } from '@angular/core';
import { Leg } from '../../../../shared/interfaces/leg.interface';
import { Flight } from '../../../../shared/interfaces/flight.interface';
import { config } from '../../../../shared/config/config.env';

@Component({
  selector: 'app-leg-entry',
  templateUrl: './leg-entry.component.html',
  styleUrl: './leg-entry.component.css',
})
export class LegEntryComponent {
  flightStatusLabels = config.flightStatusLabels;
  legTypeLabels = config.legTypeLabels;
  @Input() leg!: Leg;
  selectedFlight!: Flight;
}
