# Carparkr: A toy project demonstrating how to build a car parking API with a TDD approach

All code changes were subject to a failing test. The commit history shows the approach to building up a working solution.

## Running locally

This project uses an im-memory DB and seeds the car park on start-up, so no set-up is required.

- Using the `dotnet` CLI: `cd .\Carparkr.API\` then `dotnet run`
- Using Rider/VS etc: run the Carparkr.API project.

## Assumptions

- The charge for a given minute in the car park is applied when the minute starts (the first minute is charged on the 1st second for instance)
- The 5 min charge is applied at the end of the 5 min period, not at the start of the 5th minute.
- There should be scope for managing multiple car parks.
- THe number of spaces would be configurable in future, but we've defaulted to 100 spaces for the purpose of this project.
