FROM mcr.microsoft.com/dotnet/runtime:6.0.3-focal-amd64 AS base
WORKDIR /app
EXPOSE 80
EXPOSE 443

FROM mcr.microsoft.com/dotnet/sdk:6.0.201-focal-amd64 AS build
WORKDIR /src
COPY ["HeadlessBrowserAudioVideoCapturingService/HeadlessBrowserAudioVideoCapturingService.csproj", "HeadlessBrowserAudioVideoCapturingService/"]
RUN dotnet restore "HeadlessBrowserAudioVideoCapturingService/HeadlessBrowserAudioVideoCapturingService.csproj"
COPY . .
WORKDIR "/src/HeadlessBrowserAudioVideoCapturingService"
RUN dotnet build "HeadlessBrowserAudioVideoCapturingService.csproj" -c Release -o /app/build

FROM build AS publish
RUN dotnet publish "HeadlessBrowserAudioVideoCapturingService.csproj" -r linux-x64 -c Release -o /app/publish

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "HeadlessBrowserAudioVideoCapturingService.dll"]

## The builder image
#FROM mcr.microsoft.com/dotnet/sdk:6.0.201-focal-amd64 AS builder
#
#WORKDIR /sln
#
## Just copy everything
#COPY . .
#
## Do the restore/publish/build in one step
#RUN dotnet publish -c Release -r linux-x64 -o /sln/artifacts -p:PublishTrimmed=True
#
## The deployment image
#FROM mcr.microsoft.com/dotnet/runtime-deps:6.0.3-focal-amd64
#
## Copy across the published app
#WORKDIR /app
#ENTRYPOINT ["dotnet", "HeadlessBrowserAudioVideoCapturingService.dll"]
#COPY --from=builder ./sln/artifacts .
