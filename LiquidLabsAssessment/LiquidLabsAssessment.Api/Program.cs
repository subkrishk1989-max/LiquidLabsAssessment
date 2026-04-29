using LiquidLabsAssessment.Api.Middleware;
using LiquidLabsAssessment.Application.Common.Behaviors;
using LiquidLabsAssessment.Application.Interfaces;
using LiquidLabsAssessment.Infrastructure.Configurations;
using LiquidLabsAssessment.Infrastructure.External;
using LiquidLabsAssessment.Infrastructure.Persistence;
using MediatR;
using Serilog;
using System.Reflection;

var builder = WebApplication.CreateBuilder(args);

builder.Services.Configure<ExternalApiSettings>(
    builder.Configuration.GetSection("ExternalApi"));

Log.Logger = new LoggerConfiguration()
    .WriteTo.Console()
    .CreateLogger();

builder.Host.UseSerilog();

builder.Services
    .AddControllers()
    .AddNewtonsoftJson();

builder.Services.AddSwaggerGen();

builder.Services.AddMediatR(cfg =>
    cfg.RegisterServicesFromAssembly(
        Assembly.Load("LiquidLabsAssessment.Application")));

builder.Services.AddTransient(
    typeof(IPipelineBehavior<,>),
    typeof(LoggingBehavior<,>));

builder.Services.AddScoped<
    IProductRepository,
    ProductRepository>();

builder.Services.AddHttpClient<
    IExternalProductService,
    ExternalProductService>();


var app = builder.Build();

app.UseMiddleware<ExceptionMiddleware>();
app.UseSwagger();
app.UseSwaggerUI();

app.MapControllers();

app.Run();
