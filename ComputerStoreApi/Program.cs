
using Autofac;
using Autofac.Extensions.DependencyInjection;
using ComputerStoreApi.AutoFacConfiguration;
using Infrastructure.BootstrapingExtensions;
using Infrastructure.DatabaseConfiguration;
using Infrastructure.MappingProfiles;
using Infrastructure.Middlewares;
using Microsoft.Extensions.Caching.Distributed;

var builder = WebApplication.CreateBuilder(args);



builder.Services.AddSwaggerGen();

builder.Services.AddInitialDependencies(builder.Configuration);
builder.Services.AddAutoMapper(typeof(MappingProfile));

builder.Host.UseServiceProviderFactory(new AutofacServiceProviderFactory());
builder.Host.ConfigureContainer<ContainerBuilder>(builder => builder.RegisterModule(new AutoFacConfiguratior()));


var app = builder.Build();

app.UseMiddleware<ExceptionMiddleware>();
app.UseCors();
app.Services.MigrateDb();

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
