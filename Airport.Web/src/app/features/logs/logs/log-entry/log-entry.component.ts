import { Component, Input } from '@angular/core';
import { DatePipe } from '@angular/common';
import { Log } from '../../../../shared/interfaces/log.interface';

@Component({
  selector: 'app-log-entry',
  templateUrl: './log-entry.component.html',
  styleUrls: ['./log-entry.component.css'],
})
export class LogEntryComponent {
  @Input() log!: Log;
  constructor(private datePipe: DatePipe) {}
  formatDate(date: string): string {
    return this.datePipe.transform(date, 'HH:mm:ss.SSS - a -- d MMM, yyyy') ?? '';
  }
  isTimeExitValid(): boolean {
    return new Date(this.log.timeExit).getFullYear() > 2000;
  }
  getActionAndLeg(): string {
    if (this.log.isWaitingList) {
      return `Tried to enter Leg ${this.log.legId}`;
    } else if (this.isTimeExitValid()) {
      return `Exited Leg ${this.log.legId}`;
    } else {
      return `Entered Leg ${this.log.legId}`;
    }
  }
  getLogTime(): string {
    if (this.isTimeExitValid()) {
      return `${this.formatDate(this.log.timeExit)}`;
    } else {
      return `${this.formatDate(this.log.timeEntered)}`;
    }
  }
  getFlightName(): string {
    return this.log.flightId === 0
      ? `Flight-${this.log.flightNumber.substring(0, 5)}`
      : `Flight-${this.log.flightId}`;
  }
}
