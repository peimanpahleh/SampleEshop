global using MediatR;
global using Products.Api;
global using Serilog;
global using Serilog.Enrichers.Span;
global using Products.Application;
global using Products.Infrastructure;
global using Microsoft.AspNetCore.Mvc;
global using Products.Api.Controllers.Base;
global using Products.Application.Products.User.Queries;
global using Microsoft.AspNetCore.Authentication.JwtBearer;
global using System.IdentityModel.Tokens.Jwt;
global using EventBus;
global using EventBus.Abstractions;
global using MassTransit;
global using Products.Application.IntegrationEventsHandler;
global using EventBus.Configurations;
global using Grpc.Core;
global using Products.Api.Services;
global using Consul;
global using System.Text.Json;
global using System.Net;
global using System.Text;
global using Microsoft.Extensions.Options;
global using Winton.Extensions.Configuration.Consul;


