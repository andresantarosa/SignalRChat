#See https://aka.ms/containerfastmode to understand how Visual Studio uses this Dockerfile to build your images for faster debugging.

FROM mcr.microsoft.com/dotnet/aspnet:6.0 AS base
WORKDIR /app
EXPOSE 80
EXPOSE 443

FROM mcr.microsoft.com/dotnet/sdk:6.0 AS build
WORKDIR /src
COPY ["SignalRChat.UI/SignalRChat.UI.csproj", "SignalRChat.UI/"]
RUN dotnet restore "SignalRChat.UI/SignalRChat.UI.csproj"
COPY . .
WORKDIR "/src/SignalRChat.UI"
RUN dotnet build "SignalRChat.UI.csproj" -c Release -o /app/build

FROM build AS publish
RUN dotnet publish "SignalRChat.UI.csproj" -c Release -o /app/publish

FROM base as final
WORKDIR /app
RUN Scripts/wait-for-it.sh rabbitmq:1433 -t 120

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "SignalRChat.UI.dll"]