var builder = WebApplication.CreateBuilder(args);

builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

var app = builder.Build();


app.UseSwagger();
app.UseSwaggerUI();

app.MapPost("/checkout", async (CheckoutRequest request) =>
{
    // Simulate external payment provider latency
    await CallPaymentProviderAsync(request.CardNumber);

    var lastFour = request.CardNumber.Substring(request.CardNumber.Length - 4);

    return Results.Ok(new
    {
        status = "ok",
        card = $"**** **** **** {lastFour}"
    });
});

app.Run();

static async Task CallPaymentProviderAsync(string cardNumber)
{
    // Slow path for specific cards
    if (cardNumber.StartsWith("9999"))
    {
        await Task.Delay(2000); // external provider is slow
        return;
    }

    await Task.Delay(200); // normal latency
}

record CheckoutRequest(string CardNumber);