﻿global using System;
global using System.Collections.Generic;
global using System.Linq;
global using System.Threading.Tasks;
global using Orders.Saga;
global using Orders.Saga.Models;
global using Orders.Saga.StateMachines;
global using Automatonymous;
global using MassTransit;
global using Orders.Saga.SagaSchedules;
global using EventBus.IntegrationEvents.OrderSaga;
global using GreenPipes;
global using EventBus.IntegrationEvents.Payments;
global using EventBus.IntegrationEvents.Products;
global using Orders.Saga.SagaActivity;
global using EventBus.IntegrationEvents.Orders;
global using MassTransit.Definition;
global using EventBus.IntegrationEvents.Baskets;
global using MassTransit.Saga;
global using MongoDB.Bson.Serialization.Attributes;
global using EventBus;
global using EventBus.Abstractions;
global using Consul;
global using EventBus.Configurations;
global using System.Net;
global using System.Text;
global using System.Text.Json;