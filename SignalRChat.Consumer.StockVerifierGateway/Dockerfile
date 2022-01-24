#See https://aka.ms/containerfastmode to understand how Visual Studio uses this Dockerfile to build your images for faster debugging.

FROM mcr.microsoft.com/dotnet/aspnet:6.0 AS base
WORKDIR /app
EXPOSE 80
EXPOSE 443

FROM mcr.microsoft.com/dotnet/sdk:6.0 AS build
WORKDIR /src
COPY ["SignalRChat.Consumer.StockVerifierGateway/SignalRChat.Consumer.StockVerifierGateway.csproj", "SignalRChat.Consumer.StockVerifierGateway/"]
RUN dotnet restore "SignalRChat.Consumer.StockVerifierGateway/SignalRChat.Consumer.StockVerifierGateway.csproj"
COPY . .
WORKDIR "/src/SignalRChat.Consumer.StockVerifierGateway"
RUN dotnet build "SignalRChat.Consumer.StockVerifierGateway.csproj" -c Release -o /app/build

FROM build AS publish
RUN dotnet publish "SignalRChat.Consumer.StockVerifierGateway.csproj" -c Release -o /app/publish

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "SignalRChat.Consumer.StockVerifierGateway.dll"]