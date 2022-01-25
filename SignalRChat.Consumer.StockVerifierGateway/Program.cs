using SignalRChat.Consumer.StockVerifierGateway.Service;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();
builder.Services.AddHttpClient();
builder.Services.AddScoped<StockService>();

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();

app.UseHttpsRedirection();

app.MapGet("/GetStock/{stockCode}/{caller}", async (StockService stockService, string stockCode, string caller) =>
{
    var result = await stockService.GetStock(stockCode, caller);
    return result.Close == "N/D" ? Results.BadRequest("Stock not found") : Results.Ok(result);
})
.WithName("GetStock")
.Produces(200)
.Produces(400);
app.Run();
