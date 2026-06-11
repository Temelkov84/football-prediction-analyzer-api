# Football Prediction Analyzer

[![Backend Tests](https://github.com/Temelkov84/football-prediction-analyzer-api/actions/workflows/backend-tests.yml/badge.svg)](https://github.com/Temelkov84/football-prediction-analyzer-api/actions/workflows/backend-tests.yml)

Football Prediction Analyzer is a QA automation portfolio project built around a realistic football prediction workflow.

The application includes an ASP.NET Core backend, a React frontend, SQL Server database support, admin data management, CSV prediction import, backend validation, automated prediction calculation and a public weekly predictions page.

The goal of this project is not to present a final commercial prediction formula, but to provide a realistic application for practicing and demonstrating QA automation skills across multiple levels: API testing, integration testing, validation testing, frontend workflow testing and CI.

## Tech Stack

* ASP.NET Core Web API (.NET 8)
* Entity Framework Core
* SQL Server / LocalDB for local development
* React
* Vite
* NUnit
* WebApplicationFactory
* EF Core InMemory database for integration tests
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

The backend test project includes:

* Unit tests for prediction calculation logic.
* API/integration tests for public weekly predictions.
* Admin prediction calculation tests.
* JSON prediction import tests.
* CSV prediction import tests.
* Validation tests for important backend workflows.

Integration tests use `WebApplicationFactory` and an EF Core InMemory test database. This allows the tests to run both locally and in GitHub Actions without depending on SQL Server LocalDB.

## Continuous Integration

GitHub Actions is configured to automatically:

* restore dependencies;
* build the solution;
* run the backend test suite.

The workflow runs on every push and pull request to the `main` branch.

## Current QA Focus

This project is being used as a practical QA automation training and portfolio project.

Current and planned testing areas include:

* API testing
* Integration testing
* CSV import validation
* Frontend workflow validation
* Playwright end-to-end testing
* CI pipeline execution

## Project Status

The project currently supports the core backend and frontend workflow:

CSV data import
→ backend validation
→ match/statistics creation
→ prediction calculation
→ public weekly predictions display

The next development focus is adding Playwright end-to-end tests for the main user and admin workflows.
