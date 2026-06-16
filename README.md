# Football Prediction Analyzer

[![Backend Tests](https://github.com/Temelkov84/football-prediction-analyzer-api/actions/workflows/backend-tests.yml/badge.svg)](https://github.com/Temelkov84/football-prediction-analyzer-api/actions/workflows/backend-tests.yml)

Football Prediction Analyzer is a QA automation portfolio project built around a realistic football prediction workflow.

The application includes an ASP.NET Core Web API backend, a React/Vite frontend, Entity Framework Core database flow, admin data management, CSV prediction import, backend validation, automated prediction calculation and a public weekly predictions page.

The goal of this project is not to present a final commercial prediction formula, but to provide a realistic application for practicing and demonstrating QA automation skills across multiple levels: API testing, integration testing, validation testing, frontend workflow testing, Playwright E2E testing and CI.

## Tech Stack

* ASP.NET Core Web API (.NET 8)
* Entity Framework Core
* SQL Server / LocalDB for local development
* React
* Vite
* NUnit
* WebApplicationFactory
* EF Core InMemory database for integration tests
* Playwright
* GitHub Actions

## Main Features

* Admin management for leagues, teams, matches and match statistics.
* CSV import workflow for creating matches, statistics and predictions.
* All-or-nothing CSV import strategy.
* Backend validation for important business rules.
* Automated prediction calculation.
* Public weekly predictions page.
* Admin CSV upload UI.
* JSON error responses for validation failures.

## Prediction Engine

The prediction engine uses a weighted multi-factor model based on team statistics and match context.

This public version is used as a demo and portfolio engine. The project focuses on application workflow, validation, testing and automation rather than exposing or finalizing a commercial prediction formula.

## Testing

The project includes backend API/integration tests and frontend Playwright E2E tests.

### Backend Tests

The backend test project covers:

* Public weekly predictions endpoint.
* Admin prediction calculation.
* CSV prediction import workflow.
* JSON prediction import workflow.
* Validation errors for important backend rules.
* Database state assertions after API actions.
* Weekly data cleanup workflow.

Integration tests use `WebApplicationFactory`, `HttpClient` and an EF Core InMemory test database. This allows the tests to run both locally and in GitHub Actions without depending on SQL Server LocalDB.

Backend tests can be run from the solution root:

```bash
dotnet test
```

### Playwright E2E Tests

The frontend Playwright tests cover:

* Public weekly predictions page loading.
* Weekly predictions section and empty state handling.
* Admin Import Predictions tab and upload form.
* CSV upload through the frontend UI.
* Successful CSV import flow.
* Public page visibility after successful import.
* Negative validation scenarios for invalid CSV data.

The strongest E2E flow covers:

```text
Admin CSV upload
→ backend validation
→ match/statistics/prediction creation
→ public weekly predictions display
```

Playwright tests can be run from the frontend project:

```bash
cd FootballPredictionTracker.Client
npm run test:e2e
```

The frontend test suite currently runs locally. Playwright CI is planned as a future improvement after backend/database setup for stable end-to-end execution in GitHub Actions.

## Continuous Integration

GitHub Actions is configured to automatically:

* restore dependencies;
* build the solution;
* run the backend test suite.

The workflow runs on every push and pull request to the `main` branch.

## Current QA Focus

This project is being used as a practical QA automation training and portfolio project.

Current testing areas include:

* API testing
* Integration testing
* CSV import validation
* Backend validation testing
* Database state verification
* Frontend workflow validation
* Playwright end-to-end testing
* CI pipeline execution

## Project Status

The project currently supports the core backend and frontend workflow:

```text
CSV data import
→ backend validation
→ match/statistics creation
→ prediction calculation
→ public weekly predictions display
```

The current portfolio version includes backend API/integration test coverage, Playwright frontend E2E coverage and GitHub Actions backend CI.

Future improvements may include broader Playwright coverage, Playwright CI setup, more advanced frontend workflows and a private version of the prediction engine for further product development.
