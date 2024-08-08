import { Component, OnDestroy, OnInit } from '@angular/core';
import { Flight } from '../../../shared/interfaces/flight.interface';
import { interval, Subscription, switchMap } from 'rxjs';
import { AirportService } from '../../../shared/services/airport.service';
import { config } from '../../../shared/config/config.env';

@Component({
  selector: 'app-flights',
  templateUrl: './flights.component.html',
  styleUrl: './flights.component.css',
})
export class FlightsComponent implements OnInit, OnDestroy {
  movingFlights: Flight[] = [];
  unassignedFlights: Flight[] = [];
  completedFlights: Flight[] = [];
  displayedFlights: Flight[] = [];
  updateSubscription: Subscription = new Subscription();
  activeFlightType: string = 'movingFlights';
  page: number = 1;
  itemsPerPage: number = 12;

  constructor(private airportService: AirportService) {}
  ngOnInit(): void {
    this.updateSubscription = interval(config.interval)
      .pipe(switchMap(() => this.airportService.getFlights()))
      .subscribe((flights: Flight[]) => {
        this.updateFlights(flights);
      });
  }
  updateFlights(flights: Flight[]): void {
    this.movingFlights = flights.filter((f) => f.flightId !== 0 && !f.isDone);
    this.unassignedFlights = flights.filter((f) => f.flightId === 0);
    this.completedFlights = flights.filter((f) => f.isDone);
    this.updateDisplayedFlights();
  }
  toggleFlightType(flightType: string): void {
    this.activeFlightType = flightType;
    this.page = 1;
    this.updateDisplayedFlights();
  }
  updateDisplayedFlights(): void {
    switch (this.activeFlightType) {
      case 'movingFlights':
        this.displayedFlights = this.movingFlights;
        break;
      case 'unassignedFlights':
        this.displayedFlights = this.unassignedFlights;
        break;
      case 'completedFlights':
        this.displayedFlights = this.completedFlights;
        break;
      default:
        this.displayedFlights = [];
    }
    this.displayedFlights
      .sort((flightX, flightY) => flightX.flightId - flightY.flightId)
      .reverse();
  }
  ngOnDestroy(): void {
    if (this.updateSubscription) {
      this.updateSubscription.unsubscribe();
    }
  }
}
