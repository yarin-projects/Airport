import { FlightStatus } from "../models/enums/flight-status.enum";
import { LegType } from "../models/enums/leg-type.enum";

export const config = {
    apiUrl: `http://localhost:5204/api/get/`,
    interval: 500,
    flightStatusLabels: {
      [FlightStatus.Landing]: 'Landing',
      [FlightStatus.Departing]: 'Departing',
    },
    legTypeLabels: {
      [LegType.Arrival]: 'Arrival',
      [LegType.Departure]: 'Departure',
      [LegType.Both]: 'Departure, Arrival',
    }
  };
  