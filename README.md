# Airport Management System

## Overview

The Airport Management System is designed to manage an airport's flight operations, including flight movements through various legs (sections) of the airport. The system comprises multiple services and controllers that handle the flight lifecycle, from arrival to departure.

The project was completed on August, 2024.

### Key Components

- **FlightMovementService**
- **LegService**
- **AirportService**
- **AirportRepository**

## Main Application

The main entry point of the application configures services and middleware, including:

- **Database Context:** Uses SQLite via `AirportContext`.
- **Controllers:** Configures JSON options to ignore cycles in object graphs.
- **CORS:** Allows any origin, header, and method.
- **Swagger:** Enables API documentation.
- **Services:** Registers various services and hosted services.
- **Database Initialization:** Ensures database migration and starts moving active flights if necessary.

## FlightsController

This controller handles API endpoints related to flights:

- `POST /api/post/flight`: Adds a new flight.
- `GET /api/get/logs`: Retrieves logs based on the last log ID.
- `GET /api/get/legs`: Retrieves all legs.
- `GET /api/get/flights`: Retrieves all flights.
- `PUT /api/complete/moving/flights`: Completes all flights currently moving in the airport.
- `DELETE /api/reset/database`: Resets the database.

## LegService

Handles operations related to legs, including:

- Checking if moving flights exceed a threshold.
- Determining the starting and next legs for a flight.
- Managing leg availability and flight handling.

## FlightMovementService

Manages the movement of flights through the airport:

- Moving flights to the first or next leg.
- Completing flights and assigning them to specific legs.
- Detaching flights from their current leg.

## AirportService

Oversees overall airport operations, such as:

- Adding flights and processing their routes.
- Handling waiting flights and assigning them to legs.
- Managing the waiting list and logging actions.

## AirportRepository

Handles data access and transactions, including:

- Managing flights, logs, legs, and waiting flights.
- Starting active flights and ensuring database migration.
- Resetting the database.

## WaitingFlightProcessingHostedService

A hosted service that periodically processes waiting flights using a timer.

- **Methods:**
  - `StartAsync(CancellationToken cancellationToken)`
  - `ProcessWaitingFlights()`
  - `StopAsync(CancellationToken cancellationToken)`
  - `Dispose()`
  - `Dispose(bool disposing)`

## Utility Classes and Enums

- **LegHelpers:** Contains utility methods and properties related to legs.
- **AirportStatus:** Manages the airport's availability status.
- **Log:** Represents a log entry.
- **Flight:** Represents a flight.
- **Leg:** Represents a leg.
- **WaitingFlight:** Represents a waiting flight.

## Airport.Http.Client

### Models

- **FlightStatusDto:** An enumeration representing the status of a flight.
- **FlightDto:** A Data Transfer Object (DTO) representing a flight.

### API

- **FlightsHttpClient:** A client class responsible for sending flight data to the HTTP endpoint.

## Airport.Flight.Simulator

- **Program:** The entry point of the flight simulator application.
- **FlightGenerator:** A class responsible for generating random flight data.
  - **Methods:**
    - `GetNewFlight()`
    - `CreateFlightDto()`
- **FlightsTimer:** A class responsible for periodically generating and sending flight data.
  - **Methods:**
    - `Start()`
    - `CreateFlights()`

## Airport Control Tower Angular Project

### Overview

This Angular project is designed for monitoring and managing airport operations, including tracking and managing flights, legs, and logs.

### Airport Service

- **File:** `airport.service.ts`
- **Purpose:** Provides methods to fetch data related to legs, logs, and flights from the backend API.

### Routes

- **Home Route (`/`):** Welcome page with navigation cards linking to Legs, Flights, and Logs sections.
- **Legs Route (`/legs`):** Displays information about airport legs, with real-time updates.
- **Flights Route (`/flights`):** Displays information about flights, with options to toggle between Moving, Completed, and Unassigned flights.
- **Logs Route (`/logs`):** Displays logs of various airport operations, with real-time updates.
