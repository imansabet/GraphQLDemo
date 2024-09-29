using GraphQLDemo.API.Schema.Mutations;
using GraphQLDemo.API.Schema.Queries;
using GraphQLDemo.API.Schema.Subscriptions;
using GraphQLDemo.API.Services;
using GraphQLDemo.API.Services.Courses;
using GraphQLDemo.API.Services.Instructors;
using Microsoft.EntityFrameworkCore;


var builder = WebApplication.CreateBuilder(args);

string connectionString = builder.Configuration.GetConnectionString("DefaultConnection");

builder.Services
    .AddGraphQLServer()
    .AddQueryType<Query>()
    .AddInMemorySubscriptions()
    .AddMutationType<Mutation>()
    .AddSubscriptionType<Subscription>();



builder.Services.AddPooledDbContextFactory<SchoolDbContext>(o => o.UseSqlite(connectionString));

builder.Services.AddScoped<CoursesRepository>();
builder.Services.AddScoped<InstructorRepository>();

var app = builder.Build();

using (var scope = app.Services.CreateScope())
{
    var dbContext = scope.ServiceProvider.GetRequiredService<SchoolDbContext>();
    dbContext.Database.Migrate();
}



app.UseRouting();
app.UseWebSockets();
app.MapGraphQL();

app.Run();
