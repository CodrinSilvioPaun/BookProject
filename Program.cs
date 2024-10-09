using BookProject.Middlewares;
using BookProject.Services;

var builder = WebApplication.CreateBuilder(args);


builder.Services.AddControllers();
// Change the routing to be lowercase
builder.Services.AddRouting(options => options.LowercaseUrls = true);
// Add the Book service
builder.Services.AddScoped<IBookService, BookService>();
// Add the custom exception handler middleware
builder.Services.AddTransient<GlobalExceptionHandlerMiddleware>();

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
app.UseGlobalExceptionHandlerMiddleware();
app.MapControllers();

app.Run();
