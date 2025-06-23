# InfoMedics Patient Portal Backend

A backend RESTful API for managing patients, appointments, dentists, and treatments for the InfoMedics Patient Portal. Built with ASP.NET Core 9.0, this solution provides endpoints for CRUD operations and statistics, using in-memory storage for rapid development and testing.

## Solution Structure

- **InfomedicsPortal/**: Main Web API project
  - `Controllers/`: API endpoints for Patients, Appointments, Dentists, Treatments, and Stats
  - `Middlewares/`: Custom middleware for error handling
  - `Program.cs`: Application entry point and service configuration
  - `appsettings.json`: Application configuration
  - `InfomedicsPortal.http`: Example HTTP requests for API testing
- **InfomedicsPortal.Core/**: Core business logic and models
- **InfomedicsPortal.Infrastructure/**: Infrastructure and in-memory data repositories
- **InfomedicsPortal.UnitTests/**: Unit tests for core features

## Features

- **Patients**: Create, retrieve, and list patients
- **Appointments**: Schedule and list appointments, including by patient
- **Dentists**: List available dentists
- **Treatments**: List available treatments
- **Stats**: Retrieve system statistics
- **Error Handling**: Consistent JSON error responses via middleware
- **CORS**: Enabled for local development
- **OpenAPI**: Swagger documentation in development mode

## Getting Started

### Prerequisites
- [.NET 9.0 SDK](https://dotnet.microsoft.com/en-us/download/dotnet/9.0)

### Running the API
```bash
# From the solution root
cd InfomedicsPortal/InfomedicsPortal
# Run the API
 dotnet run
```
The API will be available at `https://localhost:5001` or `http://localhost:5000` (see `Properties/launchSettings.json`).

### API Documentation
When running in development, Swagger/OpenAPI docs are available at `/swagger`.

## Example API Endpoints

- `GET    /patients` — List all patients
- `POST   /patients` — Create a new patient
- `GET    /patients/{id}` — Get patient details and appointments
- `GET    /appointments` — List all appointments
- `POST   /appointments` — Schedule a new appointment
- `GET    /appointments/{patientId}` — List appointments for a patient
- `GET    /dentists` — List all dentists
- `GET    /treatments` — List all treatments
- `GET    /stats` — Get system statistics

See `InfomedicsPortal.http` for example requests.

## Error Handling
All errors are returned as JSON with a message and status code, handled by custom middleware.

## Testing
Run unit tests from the solution root:
```bash
cd InfomedicsPortal/InfomedicsPortal.UnitTests
dotnet test
```

## Notes
- Data is stored in-memory (no external database required).
- For production, replace in-memory repositories with persistent storage (e.g., PostgreSQL).

## Contributing
Pull requests are welcome! For major changes, please open an issue first to discuss what you would like to change.

## License
[MIT](LICENSE)
