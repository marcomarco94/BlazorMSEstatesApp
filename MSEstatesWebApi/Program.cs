using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.Identity.Web;
using MSEstatesAppLibrary.DataAccess;
using Microsoft.AspNetCore.Authentication;
using Microsoft.Extensions.Azure;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
    .AddMicrosoftIdentityWebApi(builder.Configuration.GetSection("AzureAd"));

builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

builder.Services.AddMemoryCache();
builder.Services.AddResponseCaching();

// Attention 
builder.Services.AddCors();

builder.Services.AddTransient<IDbConnection, DbConnection>();
builder.Services.AddTransient<ICategoryData, MongoCategoryData>();
builder.Services.AddTransient<ILocationData, MongoLocationData>();
builder.Services.AddTransient<IAdvertisementTypeData, MongoAdvertisementTypeData>();
builder.Services.AddTransient<ICompanyData, MongoCompanyData>();
builder.Services.AddTransient<IAgentData, MongoAgentData>();
builder.Services.AddTransient<IRealtorContactData, MongoRealtorContactData>();
builder.Services.AddTransient<IListingData, MongoListingData>();
builder.Services.AddTransient<IFacebookData, MongoFacebookData>();

var app = builder.Build();

//Attention
app.UseCors(options =>
{
    options.WithOrigins("https://www.ms-estates.net", "https://localhost:44344")
        .WithMethods("GET", "POST")
        .AllowAnyHeader();
});


// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
    app.UseResponseCaching();
}

app.UseHttpsRedirection();

app.UseAuthentication();

app.UseAuthorization();

app.MapControllers();

app.Run();