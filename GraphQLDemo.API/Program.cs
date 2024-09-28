using GraphQLDemo.API.Schema.Mutations;
using GraphQLDemo.API.Schema.Queries;
using GraphQLDemo.API.Schema.Subscriptions;

var builder = WebApplication.CreateBuilder(args);

builder.Services
    .AddGraphQLServer()
    .AddQueryType<Query>()
    .AddInMemorySubscriptions()
    .AddMutationType<Mutation>()
    .AddSubscriptionType<Subscription>();



var app = builder.Build();


app.UseRouting();
app.UseWebSockets();
app.MapGraphQL();

app.Run();
