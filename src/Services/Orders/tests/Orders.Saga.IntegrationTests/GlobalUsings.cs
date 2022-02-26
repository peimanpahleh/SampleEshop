global using System;
global using System.Collections.Generic;
global using System.Linq;
global using System.Threading.Tasks;
global using System.Reflection;
global using NUnit;
global using NUnit.Framework;
global using FluentAssertions;
global using Microsoft.Extensions.Configuration;
global using Microsoft.Extensions.DependencyInjection;
global using MongoDB.Bson.Serialization;
global using MongoDB.Bson.Serialization.IdGenerators;
global using MongoDB.Driver;
global using EventBus.Abstractions;
global using MassTransit.Testing;
global using Orders.Saga.Models;
global using Orders.Saga.StateMachines;
global using MassTransit;
global using EventBus.IntegrationEvents.Orders;
global using EventBus.IntegrationEvents.Baskets;
global using Automatonymous;
global using MassTransit.Context;
global using MassTransit.ExtensionsDependencyInjectionIntegration;
global using Microsoft.Extensions.Logging;
global using Quartz;
global using Orders.Saga.IntegrationTests.Internals;
global using NUnit.Framework.Internal;
global using Orders.Saga.IntegrationTests.Fixture;



