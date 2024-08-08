import { Pipe, PipeTransform } from '@angular/core';

@Pipe({
  name: 'timespan',
})
export class FormatTimeSpanPipe implements PipeTransform {
  transform(value: string): string {
    if (!value) return value;

    const [hours, minutes, seconds] = value.split(':').map(Number);

    const formattedHours = hours > 0 ? `${hours}h ` : '';
    const formattedMinutes = minutes > 0 ? `${minutes}m ` : '';
    const formattedSeconds = `${seconds}s`;

    return `${formattedHours}${formattedMinutes}${formattedSeconds}`;
  }
}
