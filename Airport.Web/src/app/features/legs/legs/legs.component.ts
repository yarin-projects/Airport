import { Component, OnDestroy, OnInit } from '@angular/core';
import { Leg } from '../../../shared/interfaces/leg.interface';
import { AirportService } from '../../../shared/services/airport.service';
import { interval, Subscription, switchMap } from 'rxjs';
import { config } from '../../../shared/config/config.env';

@Component({
  selector: 'app-legs',
  templateUrl: './legs.component.html',
  styleUrl: './legs.component.css',
})
export class LegsComponent implements OnInit, OnDestroy {
  legs: Leg[] = [];
  private updateSubscription: Subscription = new Subscription();

  constructor(private airportService: AirportService) {}
  ngOnInit(): void {
    this.updateSubscription = interval(config.interval)
      .pipe(switchMap(() => this.airportService.getLegs()))
      .subscribe((legs: Leg[]) => {
        this.legs = legs;
      });
  }
  ngOnDestroy(): void {
    if (this.updateSubscription) {
      this.updateSubscription.unsubscribe();
    }
  }
}
