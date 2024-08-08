import { LegType } from '../models/enums/leg-type.enum';
import { Flight } from './flight.interface';

export interface Leg {
  legId: number;
  legType: LegType;
  crossingTime: string;
  isOccupied: boolean;
  flightId: number;
  flight: Flight;
  rowVersion: string;
}
