global using System;
global using System.Collections.Generic;
global using System.Linq;
global using System.Threading.Tasks;
global using Orders.Domain.SeedWork;
global using MediatR;
global using System.Reflection;
global using Orders.Domain.Exceptions;
global using Microsoft.AspNetCore.Http;
global using Orders.Application.Configuration.Services;
global using MongoDB.Driver;
global using Orders.Application.Models;
global using Orders.Application.ReadSide;
global using Orders.Domain.Orders;
global using Microsoft.Extensions.Configuration;
global using Microsoft.Extensions.DependencyInjection;
global using MongoDB.Bson.Serialization;
global using MongoDB.Bson.Serialization.IdGenerators;
global using Orders.Infrastructure.Persistence.Mongo;
global using Orders.Infrastructure.Services;
global using Microsoft.Extensions.Options;
global using Orders.Application.Configuration.Context;
global using Orders.Domain.Buyers;
global using Consul;
global using Orders.Infrastructure.Context;
global using Orders.Infrastructure.Settings;
global using System.Text.Json;