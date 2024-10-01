using System.Net;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.HttpOverrides;
using Microsoft.Identity.Web;
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

builder.Services.Configure<ForwardedHeadersOptions>(options =>
{
    options.KnownProxies.Add(IPAddress.Parse("15.235.206.108"));
});

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
builder.Services.AddScoped<MarketPlaceHelper>();

builder.Services.AddCors(options =>
{
    options.AddPolicy("AllowLocalhost5100",
        policyBuilder => policyBuilder.WithOrigins("http://localhost:5100")
            .AllowAnyHeader()
            .AllowAnyMethod());
});

var app = builder.Build();

app.UseCors("AllowLocalhost5100");

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
    app.UseResponseCaching();
}

app.UseForwardedHeaders(new ForwardedHeadersOptions
{
    ForwardedHeaders = ForwardedHeaders.XForwardedFor | ForwardedHeaders.XForwardedProto
});

app.UseAuthentication();

app.UseAuthorization();

app.MapControllers();

app.Run();