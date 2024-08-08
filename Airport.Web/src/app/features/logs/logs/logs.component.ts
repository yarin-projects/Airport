import { Component, OnDestroy, OnInit } from '@angular/core';
import { Log } from '../../../shared/interfaces/log.interface';
import { interval, Subscription, switchMap } from 'rxjs';
import { AirportService } from '../../../shared/services/airport.service';
import { config } from '../../../shared/config/config.env';

@Component({
  selector: 'app-logs',
  templateUrl: './logs.component.html',
  styleUrls: ['./logs.component.css'],
})
export class LogsComponent implements OnInit, OnDestroy {
  lastLogId: number = 0;
  waitingFlightsLogs: Log[] = [];
  unassignedFlightsLogs: Log[] = [];
  assignedFlightsLogs: Log[] = [];
  displayedLogs: Log[] = [];
  updateSubscription: Subscription = new Subscription();
  activeLogType: string = 'assignedFlightsLogs';
  page: number = 1;

  constructor(private airportService: AirportService) {}

  ngOnInit(): void {
    this.updateSubscription = interval(config.interval)
      .pipe(switchMap(() => this.airportService.getLogs(this.lastLogId)))
      .subscribe((logs: Log[]) => {
        this.lastLogId = logs[logs.length - 1]?.logId || this.lastLogId;
        this.updateLogs(logs);
      });
  }
  updateLogs(logs: Log[]): void {
    this.waitingFlightsLogs = [
      ...this.waitingFlightsLogs,
      ...logs.filter((log) => log.isWaitingList && log.flightId !== 0),
    ];
    this.unassignedFlightsLogs = [
      ...this.unassignedFlightsLogs,
      ...logs.filter((log) => log.flightId === 0),
    ];
    this.assignedFlightsLogs = [
      ...this.assignedFlightsLogs,
      ...logs.filter((log) => !log.isWaitingList && log.flightId > 0),
    ];
    this.updateDisplayedLogs();
  }
  toggleLogType(logType: string): void {
    this.activeLogType = logType;
    this.page = 1;
    this.updateDisplayedLogs();
  }
  updateDisplayedLogs(): void {
    switch (this.activeLogType) {
      case 'waitingFlightsLogs':
        this.displayedLogs = this.waitingFlightsLogs;
        break;
      case 'unassignedFlightsLogs':
        this.displayedLogs = this.unassignedFlightsLogs;
        break;
      case 'assignedFlightsLogs':
        this.displayedLogs = this.assignedFlightsLogs;
        break;
      default:
        this.displayedLogs = [];
    }
    this.displayedLogs.sort((logX, logY) => logX.logId - logY.logId).reverse();
  }
  ngOnDestroy(): void {
    if (this.updateSubscription) {
      this.updateSubscription.unsubscribe();
    }
  }
}
