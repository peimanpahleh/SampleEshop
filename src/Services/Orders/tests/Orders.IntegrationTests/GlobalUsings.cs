﻿global using System;
global using System.Collections.Generic;
global using System.Linq;
global using System.Threading.Tasks;
global using Orders.Domain.SeedWork;
global using MediatR;
global using System.Reflection;
global using Orders.Domain.Exceptions;
global using Xunit;
global using FluentAssertions;
global using Microsoft.Extensions.Configuration;
global using Microsoft.Extensions.DependencyInjection;
global using MongoDB.Bson.Serialization;
global using MongoDB.Bson.Serialization.IdGenerators;
global using MongoDB.Driver;
global using Orders.IntegrationTests.Fixtures;
global using Orders.Domain.Orders;
global using Orders.Infrastructure;
global using Orders.Infrastructure.Persistence.Mongo;
global using Moq;
global using EventBus.Abstractions;
global using MassTransit.Testing;
global using Orders.Application.Orders.Commands;
global using EventBus;
global using Orders.Application.Configuration.Context;
global using Orders.Domain.Buyers;
global using Orders.Infrastructure.Context;