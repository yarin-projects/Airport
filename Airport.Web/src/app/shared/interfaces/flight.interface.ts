import { FlightStatus } from '../models/enums/flight-status.enum';
import { Leg } from './leg.interface';

export interface Flight {
  flightId: number;
  number: string;
  passengersCount: number;
  model: string;
  flightStatus: FlightStatus;
  isDone: boolean;
  timeCompleted: string;
  legId: number;
  leg: Leg;
}
