# RadioScheduler

RadioScheduler to aplikacja napisana w .NET 8 do zarządzania harmonogramem audycji radiowych, zbudowana zgodnie z zasadami Clean Architecture. Obsługuje tworzenie audycji, wykrywanie kolizji w harmonogramie, wysyłanie powiadomień, logowanie błędów oraz generowanie raportów dziennych.

## Struktura projektu

Projekt jest oparty na Clean Architecture i składa się z czterech warstw:

- **RadioScheduler.Domain**: Zawiera encje (`Show`) oraz interfejsy (`IShowRepository`, `INotificationService`, `ILogger`).
- **RadioScheduler.Application**: Zawiera logikę biznesową, komendy (`CreateShowCommand`), zapytania (`GetShowByIdQuery`, `GetShowsByDateQuery`, `GetDailyReportQuery`) oraz DTO (`ShowDto`, `DailyReportDto`).
- **RadioScheduler.Infrastructure**: Implementuje repozytoria (`InMemoryShowRepository`), usługi (`ConsoleNotificationService`) oraz logowanie (`FileLogger`).
- **RadioScheduler.Api**: Udostępnia endpointy REST API poprzez `ShowsController` oraz integruje Swagger dla dokumentacji API.
- **RadioScheduler.Tests**: Zawiera testy jednostkowe z użyciem xUnit i Moq.

## Wymagania wstępne

- [.NET 8 SDK](https://dotnet.microsoft.com/download/dotnet/8.0)
- [Docker](https://www.docker.com/get-started)
- WSL z Ubuntu-18.04 (opcjonalnie, dla programowania w systemie Linux)
- Visual Studio 2022 (opcjonalnie, dla programowania w systemie Windows)

## Konfiguracja

Aplikacja używa pliku `appsettings.json` do konfiguracji logowania i Swaggera. Plik trzeba stworzyć w projekcie *.Api:

```json
{
  "AppLogging": {
    "LoggerType": "File",
    "FileLogger": {
      "Path": "logs/error_logs.txt"
    }
  },
  "Logging": {
    "LogLevel": {
      "Default": "Information",
      "Microsoft.AspNetCore": "Warning"
    }
  },
  "AllowedHosts": "*",
  "Serilog": {
    "MinimumLevel": {
      "Default": "Error"
    },
    "WriteTo": [
      {
        "Name": "File",
        "Args": {
          "path": "serilog_errors.txt",
          "rollingInterval": "Day",
          "outputTemplate": "[{Timestamp:yyyy-MM-dd HH:mm:ss}] [{Level:u3}] {Message:lj}{NewLine}{Exception}"
        }
      }
    ]
  },
  "Swagger": {
    "EndpointUrl": "/swagger/v1/swagger.json",
    "ApiName": "RadioScheduler API v1"
  }
}
```

Upewnij się, że plik `error_logs.txt` ma uprawnienia do zapisu w katalogu aplikacji.

## Uruchomienie lokalne

1. **Sklonuj repozytorium**:
   ```bash
   git clone [<adres-repozytorium>](https://github.com/VladShpilMan/RadioScheduler)
   cd RadioScheduler
   ```

2. **Zbuduj projekt**:
   ```bash
   make build
   ```

3. **Uruchom testy**:
   ```bash
   make test
   ```

4. **Uruchom aplikację**:
   ```bash
   make run
   ```

5. **Dostęp do API**:
   - Otwórz w przeglądarce: `https://localhost:5001/swagger`, aby zobaczyć dokumentację Swagger.

## Uruchomienie w Dockerze

1. **Zbuduj obraz Docker**:
   ```bash
   make docker-build
   ```

2. **Uruchom kontener Docker**:
   ```bash
   make docker-run
   ```

3. **Dostęp do API**:
   - Otwórz w przeglądarce: `http://localhost:8080/swagger`.


## Endpointy API i przykłady żądań `curl`

### 1. Tworzenie audycji
- **Metoda**: POST
- **Ścieżka**: `/api/shows`
- **Opis**: Tworzy nową audycję, sprawdzając kolizje w harmonogramie.
- **Przykład**:
  ```bash
  curl -X POST "https://localhost:5001/api/shows" \
  -H "Content-Type: application/json" \
  -d '{
    "title": "Poranny Program",
    "presenter": "Jan Kowalski",
    "startTime": "2025-07-14T08:00:00",
    "durationMinutes": 60
  }'
  ```

### 2. Pobieranie harmonogramu na dany dzień
- **Metoda**: GET
- **Ścieżka**: `/api/shows?date=YYYY-MM-DD`
- **Opis**: Zwraca harmonogram audycji dla wybranego dnia.
- **Przykład**:
  ```bash
  curl -X GET "https://localhost:5001/api/shows?date=2025-07-14"
  ```

### 3. Pobieranie audycji po ID
- **Metoda**: GET
- **Ścieżka**: `/api/shows/{id}`
- **Opis**: Zwraca szczegóły konkretnej audycji.
- **Przykład**:
  ```bash
  curl -X GET "https://localhost:5001/api/shows/123e4567-e89b-12d3-a456-426614174000"
  ```

### 4. Generowanie raportu dziennego
- **Metoda**: GET
- **Ścieżka**: `/api/shows/daily-report?date=YYYY-MM-DD`
- **Opis**: Generuje raport dzienny z liczbą audycji i ich całkowitym czasem trwania.
- **Przykład**:
  ```bash
  curl -X GET "https://localhost:5001/api/shows/daily-report?date=2025-07-14"
  ```

## Polecenia Makefile

- `make build`: Buduje projekt.
- `make test`: Uruchamia testy jednostkowe.
- `make run`: Uruchamia API.
- `make docker-build`: Buduje obraz Docker.
- `make docker-run`: Uruchamia kontener Docker.

## Logowanie błędów

Błędy biznesowe (np. kolizje harmonogramu, HTTP 400) są zapisywane do pliku `error_logs.txt` (konfiguracja w `AppLogging`) w formacie:
```
YYYY-MM-DD HH:mm:ss ERROR: Kolizja audycji
```

Błędy aplikacji są zapisywane do pliku `serilog_errors.txt` (konfiguracja w `Serilog`) z rótacją dzienną w formacie:
```
[{Timestamp:yyyy-MM-dd HH:mm:ss}] [{Level:u3}] {Message:lj}{NewLine}{Exception}
```

## Uwagi

- Projekt używa repozytorium w pamięci (`InMemoryShowRepository`) dla uproszczenia.
- Dokumentacja Swagger jest dostępna w trybie deweloperskim pod adresem `/swagger`.
