FROM mcr.microsoft.com/dotnet/aspnet:6.0 AS base
WORKDIR /app
EXPOSE 80
EXPOSE 443

FROM mcr.microsoft.com/dotnet/sdk:6.0 AS build
WORKDIR /src
COPY ["HeadlessBrowserAudioVideoCapturingService/HeadlessBrowserAudioVideoCapturingService.csproj", "HeadlessBrowserAudioVideoCapturingService/"]
RUN dotnet restore "HeadlessBrowserAudioVideoCapturingService/HeadlessBrowserAudioVideoCapturingService.csproj"
COPY . .
WORKDIR "/src/HeadlessBrowserAudioVideoCapturingService"
RUN dotnet build "HeadlessBrowserAudioVideoCapturingService.csproj" -c Release -o /app/build

FROM build AS publish
RUN dotnet publish "HeadlessBrowserAudioVideoCapturingService.csproj" -c Release -o /app/publish

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "HeadlessBrowserAudioVideoCapturingService.dll"]
