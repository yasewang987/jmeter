FROM mcr.microsoft.com/dotnet/core/aspnet:2.2
WORKDIR /app
EXPOSE 80
COPY ./bin/Release/netcoreapp2.2/publish .
RUN apt-get update && apt-get install libltdl7 -y
ENTRYPOINT ["dotnet", "jmeterSite.dll"]
