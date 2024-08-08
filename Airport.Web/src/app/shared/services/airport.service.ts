import { HttpClient, HttpParams } from '@angular/common/http';
import { Injectable } from '@angular/core';
import { lastValueFrom } from 'rxjs';
import { Leg } from '../interfaces/leg.interface';
import { Log } from '../interfaces/log.interface';
import { Flight } from '../interfaces/flight.interface';
import { config } from '../config/config.env';
 
@Injectable({
  providedIn: 'root',
})
export class AirportService {
  baseUrl = config.apiUrl;

  constructor(private airportApi: HttpClient) {}
  async getLegs(): Promise<Leg[]> {
    const response = this.airportApi.get<Leg[]>(this.baseUrl + 'legs');
    return await lastValueFrom(response);
  }
  async getLogs(lastLogId: number): Promise<Log[]> {
    const params = new HttpParams().set('lastLogId', lastLogId.toString());
    const response = this.airportApi.get<Log[]>(this.baseUrl + 'logs', {
      params,
    });
    return await lastValueFrom(response);
  }
  async getFlights(): Promise<Flight[]> {
    const response = this.airportApi.get<Flight[]>(this.baseUrl + 'flights');
    return await lastValueFrom(response);
  }
}
