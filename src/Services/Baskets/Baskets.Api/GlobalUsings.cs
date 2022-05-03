﻿global using MediatR;
global using Serilog;
global using Serilog.Enrichers.Span;
global using Serilog.Sinks.SystemConsole.Themes;
global using Microsoft.AspNetCore.Mvc;
global using Microsoft.AspNetCore.Authentication.JwtBearer;
global using System.IdentityModel.Tokens.Jwt;
global using Microsoft.AspNetCore.Authorization;
global using Baskets.Api.Models.Baskets;
global using Baskets.Application.Baskets.Commands;
global using Baskets.Application.Baskets.Queries;
global using Baskets.Application.Models.Dto;
global using Baskets.Api;
global using Baskets.Application;
global using Baskets.Infrastructure;
global using System.Net;
global using Baskets.Api.Models.Response;
global using Baskets.Application.Configuration.Services;
global using Baskets.Application.Models.Dto.User;
global using Baskets.Application.IntegrationEvents.EventHandler;
global using EventBus;
global using EventBus.Abstractions;
global using MassTransit;
global using EventBus.Configurations;
global using Microsoft.AspNetCore.Diagnostics;
global using Baskets.Api.Errors;
global using Baskets.Application.Configuration.Exceptions;
global using FluentValidation;
global using Baskets.Api.Controllers.Base;
global using Baskets.Api.Filters;
global using Consul;
global using FluentValidation.AspNetCore;
global using System.Text;
global using System.Text.Json;
global using Winton.Extensions.Configuration.Consul;