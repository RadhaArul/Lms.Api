using Microsoft.EntityFrameworkCore;
using Lms.Data.Data;
using Lms.Api.Extensions;
using Lms.Data.Data.MapperProfile;
using Lms.Core.Repositories;
using Lms.Data.Repositories;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddDbContext<LmsApiContext>(options =>
    options.UseSqlServer(builder.Configuration.GetConnectionString("LmsApiContext")));

// Add services to the container.
//These extensions help us map to Json and Xml.
builder.Services.AddControllers(opt=>opt.ReturnHttpNotAcceptable=true)
    .AddNewtonsoftJson()
    .AddXmlDataContractSerializerFormatters();
builder.Services.AddAutoMapper(typeof(LmsMappings));
builder.Services.AddScoped<IUnitOfWork, UnitOfWork>();

// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();
var app = builder.Build();



//Seed
app.SeedDataAsync().GetAwaiter().GetResult();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();

app.Run();
