#!/bin/sh
export PATH="$PATH:/root/.dotnet/tools"
mkdir -p /app/Output/AppTests
rm -Rf /app/Output/AppTests
cd /app/Api
dotnet build Api.sln
cd /app/AppTests
dotnet test --collect:"XPlat Code Coverage" --results-directory /app/Output/AppTests/TestResults --settings CodeCoverage.runsettings.xml
guid=$(ls /app/Output/AppTests/TestResults/)
report=$(ls /app/Output/AppTests/TestResults/$guid/coverage.cobertura.xml)
reportgenerator -reports:$report -targetdir:"/app/Output/AppTests/TestResults/coveragereport" -reporttypes:Html