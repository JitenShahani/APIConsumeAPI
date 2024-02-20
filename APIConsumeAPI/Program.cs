var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();

builder.Services.AddSwaggerGen(options =>
{
	// Setup the Swagger document with below information
	options.SwaggerDoc("v1", new OpenApiInfo
	{
		Version = "v1",
		Title = "API Consume API v1",
		Description = "With this API i am consuming other services. I will consume this API in my frontend application.",
		TermsOfService = new Uri("http://www.sachinsimpex.com/serviceterms"),
		Contact = new OpenApiContact
		{
			Name = "Jiten Shahani",
			Url = new Uri("http://www.sachinsimpex.com"),
			Email = "info@sachinsimpex.com"
		},
		License = new OpenApiLicense
		{
			Name = "Registered to Sachins Impex",
			Url = new Uri("http://www.sachinsimpex.com/UniversityProject/license")
		}
	});

	// To enable endpoint metadata, install Swashbuckle.AspNetCore.Annotations
	options.EnableAnnotations();
});

// Global exception handling
builder.Services.AddTransient<GlobalExceptionHandling>();

// Basic health check + Ping Joke & University Endpoints
// Install AspNetCore.HealthChecks.SqlServer, to health check your database
builder.Services
	.AddHealthChecks()
	.AddCheck<JokeHealthCheck>("JokeEndpointHealth")
	.AddCheck<UniversityHealthCheck>("UniversityEndpointHealth")
	.AddSqlServer(builder.Configuration.GetConnectionString("Default")!);

// Create Health check UI - dashboard
builder.Services.AddHealthChecksUI().AddInMemoryStorage();

// Register CORS to enable cross domain access
builder.Services.AddCors(options =>
{
	options.AddPolicy("blazorApp", policyBuilder =>
	{
		policyBuilder.WithOrigins("http://localhost:5000"); // Due to change as per UI app.
		policyBuilder.AllowAnyHeader();
		policyBuilder.AllowAnyMethod();
	});
});

// Enable Rate limiting
builder.Services.AddRateLimiter(rateLimiterOptions =>
{
	rateLimiterOptions.RejectionStatusCode = StatusCodes.Status429TooManyRequests;

	rateLimiterOptions.AddTokenBucketLimiter("token", options =>
	{
		options.TokenLimit = 100;
		options.ReplenishmentPeriod = TimeSpan.FromSeconds(5);
		options.TokensPerPeriod = 10;
	});

	rateLimiterOptions.AddConcurrencyLimiter("concurrency", options =>
	{
		options.PermitLimit = 5;
		options.QueueProcessingOrder = QueueProcessingOrder.OldestFirst;
	});
});

// Register named HttpClient for universities related service
builder.Services.AddHttpClient("universities", options =>
{
	options.BaseAddress = new Uri(builder.Configuration["UniversityBaseAddress"]!);
});

// Register names HttpClient for jokes related service
builder.Services.AddHttpClient("jokes", options =>
{
	options.BaseAddress = new Uri(builder.Configuration["JokeBaseAddress"]!);
});

var app = builder.Build();

// Route Re-direct
app.UseRewriter(new RewriteOptions().AddRedirect("world", "hello"));

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger(options =>
	{
		options.RouteTemplate = "SachinsImpex/swagger/{documentName}/swagger.json";
	});

	// Setup Swagger UI with app name and version
    app.UseSwaggerUI(options =>
	{
		options.DocumentTitle = "API Consume API v1";
		options.RoutePrefix = "SachinsImpex/swagger";
		options.SwaggerEndpoint("/SachinsImpex/swagger/v1/swagger.json", $"{builder.Environment.ApplicationName} v1");
	});
}

// Initialize Global exception handler
app.UseMiddleware<GlobalExceptionHandling>();

// Enable CORS in middleware
app.UseCors("blazorApp");

// Health check endpoint
app.UseHealthChecks("/_health", new HealthCheckOptions
{
	ResponseWriter = UIResponseWriter.WriteHealthCheckUIResponse
});

// UI Dashboard for Health Check
app.MapHealthChecksUI();

// Enable Rate Limiter
app.UseRateLimiter();

// create instance of IHttpClientFactory
var _httpClientFactory = app.Services.GetRequiredService<IHttpClientFactory>();

// create instance of Logger
ILoggerFactory _loggerFactory = LoggerFactory.Create(builder =>
{
	builder.AddConsole();
	builder.AddDebug();
});

// Create instance of IConfiguration
IConfiguration _configuration = app.Services.GetRequiredService<IConfiguration>();

// Register my basic endpoints
new BasicEndpoints().ConfigureBasicEndpoints(app);

// Register university related endpoints
new ConsumeUniversityEndpoints(_httpClientFactory, _loggerFactory, _configuration).ConfigureUniversityEndpoints(app);

// Register jokes related endpoints
new ConsumeJokeEndpoints(_httpClientFactory, _loggerFactory, _configuration).ConfigureJokeEndpoints(app);

app.UseHttpsRedirection();

app.Run();