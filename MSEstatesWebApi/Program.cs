using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.Identity.Web;
using Microsoft.AspNetCore.Authentication;
using Microsoft.Extensions.Azure;
using MarketPlaceHelper.Services;
using MSEstatesAppLibrary.DataAccess;
using MSEstatesAppLibrary.Services;
using MSEstatesAppLibrary.Services.MarketPlace;

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

builder.Services.AddSingleton<IDbConnection, DbConnection>();
builder.Services.AddSingleton<ICategoryData, MongoCategoryData>();
builder.Services.AddSingleton<ILocationData, MongoLocationData>();
builder.Services.AddSingleton<IAdvertisementTypeData, MongoAdvertisementTypeData>();
builder.Services.AddSingleton<ICompanyData, MongoCompanyData>();
builder.Services.AddSingleton<IAgentData, MongoAgentData>();
builder.Services.AddSingleton<IRealtorContactData, MongoRealtorContactData>();
builder.Services.AddSingleton<IListingData, MongoListingData>();
builder.Services.AddSingleton<IFacebookPostData, MongoFacebookPostData>();
builder.Services.AddSingleton<IFacebookTemplateData, MongoFacebookTemplateData>();
builder.Services.AddSingleton<IFacebookGroupData, MongoFacebookGroupData>();

builder.Services.AddSingleton<ListingService>();
builder.Services.AddSingleton<AzureBlobService>();
builder.Services.AddSingleton<FacebookTemplateService>();
builder.Services.AddSingleton<FacebookPostingService>();
builder.Services.AddScoped<SeleniumService>();
builder.Services.AddScoped<MarketPlacePostingService>();
builder.Services.AddScoped<MSEstatesAppLibrary.Services.MarketPlace.MarketPlaceHelper>();

var app = builder.Build();


//Attention
app.UseCors(options =>
{
    options.WithOrigins("https://www.ms-estates.net", "https://localhost:44390", "https://localhost:7043", "https://thankful-flower-02b259a00.5.azurestaticapps.net")
        .WithMethods("GET", "POST", "DELETE")
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