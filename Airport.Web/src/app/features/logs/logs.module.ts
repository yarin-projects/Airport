import { NgModule } from '@angular/core';
import { CommonModule, DatePipe } from '@angular/common';
import { LogsComponent } from './logs/logs.component';
import { LogEntryComponent } from './logs/log-entry/log-entry.component';
import { LogsRoutingModule } from './logs-routing.module';
import { NgxPaginationModule } from 'ngx-pagination';

@NgModule({
  declarations: [LogsComponent, LogEntryComponent],
  imports: [CommonModule, LogsRoutingModule, NgxPaginationModule],
  providers: [DatePipe],
  exports: [LogsComponent, LogEntryComponent],
})
export class LogsModule {}
