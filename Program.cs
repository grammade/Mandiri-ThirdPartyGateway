using CSRequest;
using Newtonsoft.Json;
using ThirdPartyGateway;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();

app.MapGet("/nutritionValue/{food}", async (string food) =>
{
    var ep = builder.Configuration.GetValue<string>("Spoonacular:endpoint");
    var key = builder.Configuration.GetValue<string>("Spoonacular:APIKey");

    using var client = new HttpClient();
    var req = await new Request(ep + "/recipes/guessNutrition", client)
        .WithQuery(new {apiKey = key, title = food})
        .GetAsync();
    var a = await req.Content.ReadAsStringAsync();
    var obj = JsonConvert.DeserializeObject<NutValues>(await req.Content.ReadAsStringAsync());

    return obj;
});
app.Run();