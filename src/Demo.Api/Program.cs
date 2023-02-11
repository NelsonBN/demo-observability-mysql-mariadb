using System;
using System.Collections.Generic;
using System.Data;
using Dapper;
using Demo.Api;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using MySql.Data.MySqlClient;

var builder = WebApplication.CreateBuilder(args);



builder.Services
    .AddEndpointsApiExplorer()
    .AddSwaggerGen()
    .AddScoped<Func<string, IDbConnection>>(sp =>
        database =>
        {
            var name = database.ToDatabaseName();
            if(name is null)
            {
                throw new NotImplementedException();
            };

            var connectionString = sp.GetRequiredService<IConfiguration>().GetConnectionString(name);
            var connection = new MySqlConnection(connectionString);
            connection.Open();

            return connection;
        });

builder.Logging.ClearProviders();
builder.Logging.AddConsole();

var app = builder.Build();

app.UseSwagger()
   .UseSwaggerUI();



app.MapPost("/{database}",
    Results<Ok<ulong>, BadRequest<string>> (Func<string, IDbConnection> providor, string database, [FromBody] Product product) =>
    {
        if(string.IsNullOrWhiteSpace(product.Name))
        {
            return TypedResults.BadRequest("Name is required");
        }

        if(database.ToDatabaseName() is null)
        {
            return TypedResults.BadRequest("Invalid database name");
        }

        var connection = providor(database);

        var id = connection.QuerySingle<ulong>(
            """
            INSERT `Product` (`Name`, `Quantity`)
                       VALUE (@Name , @Quantity );
            SELECT CAST(LAST_INSERT_ID() AS UNSIGNED INTEGER);
            """,
            product);

        return TypedResults.Ok(id);
    });


app.MapGet("/{database}",
    Results<Ok<IEnumerable<Product>>, BadRequest<string>> (Func<string, IDbConnection> providor, string database) =>
    {
        if(database.ToDatabaseName() is null)
        {
            return TypedResults.BadRequest("Invalid database name");
        }

        var connection = providor(database);

        var result = connection.Query<Product>(
            """
            SELECT 
                `Id`,
                `Name`,
                `Quantity`
            FROM `Product` ;
            """
        );

        return TypedResults.Ok(result);
    });


app.MapGet("/{database}/{id:int}",
    Results<Ok<Product>, BadRequest<string>> (Func<string, IDbConnection> providor, string database, uint id) =>
    {
        if(database.ToDatabaseName() is null)
        {
            return TypedResults.BadRequest("Invalid database name");
        }

        var connection = providor(database);

        var result = connection.QuerySingleOrDefault<Product>(
            """
            SELECT 
                `Id`,
                `Name`,
                `Quantity`
            FROM `Product`
            WHERE `Id` = @Id
            """,
            new { Id = id });

        return TypedResults.Ok(result);
    });


app.MapPut("/{database}/{id:int}",
    Results<NoContent, BadRequest<string>> (Func<string, IDbConnection> providor, string database, uint id, [FromBody] Product product) =>
    {
        if(string.IsNullOrWhiteSpace(product.Name))
        {
            return TypedResults.BadRequest("Name is required");
        }

        if(database.ToDatabaseName() is null)
        {
            return TypedResults.BadRequest("Invalid database name");
        }

        var connection = providor(database);

        product.Id = id;

        connection.Execute(
            """
            UPDATE `Product` SET
                `Name` = @Name,
                `Quantity` = @Quantity
            WHERE `id` = @Id ;
            """,
            product);

        return TypedResults.NoContent();
    });


app.MapDelete("/{database}/{id:int}",
    Results<NoContent, BadRequest<string>> (Func<string, IDbConnection> providor, string database, uint id) =>
    {
        if(database.ToDatabaseName() is null)
        {
            return TypedResults.BadRequest("Invalid database name");
        }

        var connection = providor(database);

        connection.Execute(
            """
            DELETE FROM `Product`
            WHERE `id` = @Id ;
            """,
            new { Id = id });

        return TypedResults.NoContent();
    });


app.Run();
