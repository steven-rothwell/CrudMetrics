using CrudMetrics.Api.Options;
using CrudMetrics.Api.Preservers;
using CrudMetrics.Api.Preservers.MongoDb;
using CrudMetrics.Api.Validators;
using MongoDB.Bson;
using MongoDB.Bson.Serialization;
using MongoDB.Bson.Serialization.Conventions;
using MongoDB.Bson.Serialization.Serializers;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddScoped<IValidator, Validator>();
builder.Services.AddScoped<IPreserver, Preserver>();

var conventionPack = new ConventionPack
{
    //new CamelCaseElementNameConvention(),  // Uncommenting this will cause reads not to work. Would need to change nameof(IExternalEntity.ExternalId) to camelCase.
    new IgnoreExtraElementsConvention(true)
};
ConventionRegistry.Register("conventionPack", conventionPack, t => true);

BsonDefaults.GuidRepresentationMode = GuidRepresentationMode.V3;
BsonSerializer.RegisterSerializer(new GuidSerializer(GuidRepresentation.Standard));

builder.Services.Configure<MongoDbOptions>(builder.Configuration.GetSection(nameof(MongoDbOptions)));
builder.Services.Configure<ApplicationOptions>(builder.Configuration.GetSection(nameof(ApplicationOptions)));

builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

var app = builder.Build();

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
