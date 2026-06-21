


// var builder = WebApplication.CreateBuilder(args);

// // Add services to the container.

// builder.Services.AddControllers();

// var app = builder.Build();

// // Configure the HTTP request pipeline.

// app.UseHttpsRedirection();

// app.UseAuthorization();

// app.MapControllers();

// app.Run();


// var builder = WebApplication.CreateBuilder(args);

// // Add authentication services (even if minimal, the pipeline needs this)
// // For this exercise, we're adding the services but authentication may be minimal
// builder.Services.AddAuthentication();
// builder.Services.AddAuthorization();

// var app = builder.Build();

// // TODO 1: Register routing FIRST - it needs to know where the request is going
// app.UseRouting();

// // TODO 2: Authentication and Authorization MUST come BEFORE the endpoint
// // These are the security gates
// app.UseAuthentication();  // "Who are you?"
// app.UseAuthorization();   // "Are you allowed here?"

// // TODO 3: Map the endpoint AFTER security middleware
// // Now it's protected - only authenticated users reach this point
// app.MapGet("/api/assessments/results", () => Results.Ok(new
// {
//     courseCode = "CS-101",
//     studentId = "S-001",
//     letterGrade = "A"
// }))
// .RequireAuthorization();  // Explicitly require authorization for this route

// app.Run();

using TmsApi;
using Scalar.AspNetCore;
using Microsoft.AspNetCore.Mvc;

var builder = WebApplication.CreateBuilder(args);


builder.Services.AddAuthentication();
builder.Services.AddAuthorization();


builder.Services.AddOptions<PaymentOptions>()
    .BindConfiguration("Payments")
    .ValidateDataAnnotations()
    .ValidateOnStart();

builder.Services.AddSingleton<IEnrollmentService, EnrollmentService>();
//builder.Services.AddSingleton<EnrollmentWorker>();
// builder.Services.AddScoped<IEnrollmentService, EnrollmentService>();
builder.Services.AddControllers(); 

builder.Host.UseDefaultServiceProvider(options =>
{
    options.ValidateScopes = true;
    options.ValidateOnBuild = true;
});

// builder.Services.AddOpenApi();


builder.Services.AddProblemDetails();


var app = builder.Build();

app.UseMiddleware<RequestLoggingMiddleware>();  // 👈 Wraps everything

// if (app.Environment.IsDevelopment())
// {
//     // Development: expose OpenAPI + Scalar explorer
//     app.MapOpenApi();
//     app.MapScalarApiReference();
// }
// else
// {
//     // Production: hide explorers, use exception handler
//     app.UseExceptionHandler();
// }

app.UseStatusCodePages();
app.UseExceptionHandler();        
app.UseHttpsRedirection();     
app.UseRouting();
app.UseAuthentication();
app.MapControllers(); 
app.UseAuthorization();


// This makes EVERY endpoint require authorization unless specified otherwise


// app.MapGet("/api/enrollments/worker-smoke", (EnrollmentWorker worker) =>
// {
//     worker.ProcessBatch();
//     return Results.Ok("processed");
// });

app.MapGet("/api/enrollments/worker-smoke", ([FromServices] EnrollmentWorker worker) =>
{
    worker.ProcessBatch();
    return Results.Ok("processed");
});


app.MapGet("/api/assessments/results", () => Results.Ok(new
{
    courseCode = "CS-101",
    studentId = "S-001",
    letterGrade = "A"
}));

app.MapGet("/api/error", () =>
{
    throw new TmsDatabaseException("Simulated database failure for ProblemDetails testing");
});

app.Run();


// var builder = WebApplication.CreateBuilder(args);

// // ✅ Services: add authentication/authorization if required
// builder.Services.AddAuthentication("Bearer") // or your cohort’s scheme
//     .AddJwtBearer("Bearer", options =>
//     {
//         // minimal config for now; facilitator may guide
//         options.Authority = "https://your-auth-server";
//         options.Audience = "tmsapi";
//     });

// builder.Services.AddAuthorization();

// var app = builder.Build();

// // TODO 1: Register routing
// app.UseRouting();

// // TODO 2: Register authentication and authorization BEFORE mapping endpoints
// app.UseAuthentication();
// app.UseAuthorization();

// // TODO 3: Map GET /api/assessments/results with placeholder response, require authorization
// app.MapGet("/api/assessments/results", () => Results.Ok(new
// {
//     courseCode = "CS-101",
//     studentId = "S-001",
//     letterGrade = "A"
// }))
// .RequireAuthorization(); // 👈 ensures 401 for anonymous callers

// app.Run();


