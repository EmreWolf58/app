Prerequisites: IIS + Hosting Bundle

Publish steps (API/MVC)

IIS setup (ports 5000/5001)

Set ASPNETCORE_ENVIRONMENT=Production

Config files:

API: appsettings.Production.json (connection string, jwt)

MVC: appsettings.Production.json (Api:BaseUrl)

Troubleshooting:

500.19 → hosting bundle

502.5 → app crash, Event Viewer

Logs permission