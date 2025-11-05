mkdir .coverage

dotnet test Prana.ServiceGateway.UnitTest.csproj --no-build /p:CollectCoverage=true /p:CoverletOutputFormat=opencover /p:CoverletOutput=".coverage/coverage.opencover.xml"

dotnet tool install --global dotnet-reportgenerator-globaltool

reportgenerator "-reports:.coverage/coverage.opencover.xml" "-targetdir:.coverage/coverage-report" -reporttypes:HTML;

PAUSE