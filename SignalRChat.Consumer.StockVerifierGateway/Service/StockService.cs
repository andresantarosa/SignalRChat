﻿using RabbitMQ.Client;
using SignalRChat.Consumer.StockVerifierGateway.CsvParserConfig;
using SignalRChat.Consumer.StockVerifierGateway.Dto;
using SignalRChat.Domain.Dto.External;
using System.Text;
using System.Text.Json;
using TinyCsvParser;

namespace SignalRChat.Consumer.StockVerifierGateway.Service
{
    public class StockService
    {
        private readonly IHttpClientFactory _httpClientFactory;

        public StockService(IHttpClientFactory httpClientFactory)
        {
            _httpClientFactory = httpClientFactory;
        }

        public async Task<StockCsvDto> GetStock(string stockCode, string caller)
        {
            var httpClient = _httpClientFactory.CreateClient();
            using (var stream = await httpClient.GetStreamAsync($"https://stooq.com/q/l/?s={stockCode}&f=sd2t2ohlcv&h&e=csv"))
            {
                StreamReader reader = new StreamReader(stream);
                string text = reader.ReadToEnd();

                CsvParserOptions csvParserOptions = new CsvParserOptions(true, ',');
                CsvReaderOptions csvReaderOptions = new CsvReaderOptions(new[] { Environment.NewLine });
                CsvPersonMapping csvMapper = new CsvPersonMapping();
                CsvParser<StockCsvDto> csvParser = new CsvParser<StockCsvDto>(csvParserOptions, csvMapper);

                var result = csvParser.ReadFromString(csvReaderOptions, text).FirstOrDefault()!.Result;
                var message = new StockMessageDto
                {
                    StockCode = stockCode,
                    Caller = caller,
                    StockValue = result.Close,
                };

                SendToRabbit(message);
                return result;
            }
        }

        private void SendToRabbit(StockMessageDto stockMessage)
        {
            try
            {
                string queueName = "stock-queue";
                string connectionString = "amqp://user:password@localhost:5672/jobsity_host";
                var message = new { StockCode = stockMessage.StockCode, StockValue = stockMessage.StockValue, Caller = stockMessage.Caller };
                var factory = new ConnectionFactory()
                {
                    Uri = new Uri(connectionString)
                };
                using var connection = factory.CreateConnection();
                using var channel = connection.CreateModel();

                channel.QueueDeclare(queue: queueName,
                                     durable: false,
                                     exclusive: false,
                                     autoDelete: false,
                                     arguments: null);

                channel.BasicPublish(exchange: "",
                                     routingKey: queueName,
                                     basicProperties: null,
                                     body: Encoding.UTF8.GetBytes(JsonSerializer.Serialize(message)));
            }
            catch (Exception e)
            {

                //throw;
            }
        }
    }
}