using Microsoft.AspNetCore.Mvc;

var builder = WebApplication.CreateBuilder(args);
var app = builder.Build();

// --- 1. DATA MODEL & MOCK DB ---
var users = new List<User>
{
    new User(1, "Alice", "alice@example.com"),
    new User(2, "Bob", "bob@example.com")
};

// --- 2. MIDDLEWARE (5pts) ---
// Custom Logging Middleware: Logs every request to the console
app.Use(async (context, next) =>
{
    Console.WriteLine($"[{DateTime.Now}] Request: {context.Request.Method} {context.Request.Path}");
    await next.Invoke();
});

// --- 3. CRUD ENDPOINTS (5pts) ---

// GET: Get all users
app.MapGet("/users", () => Results.Ok(users));

// GET: Get user by ID
app.MapGet("/users/{id}", (int id) => 
    users.FirstOrDefault(u => u.Id == id) is User user ? Results.Ok(user) : Results.NotFound());

// POST: Create a user (With 4. VALIDATION - 5pts)
app.MapPost("/users", (User newUser) =>
{
    // Validation logic
    if (string.IsNullOrEmpty(newUser.Name) || !newUser.Email.Contains("@"))
    {
        return Results.BadRequest("Invalid data: Name is required and Email must be valid.");
    }

    var id = users.Max(u => u.Id) + 1;
    var userToAdd = newUser with { Id = id };
    users.Add(userToAdd);
    
    return Results.Created($"/users/{id}", userToAdd);
});

// PUT: Update a user
app.MapPut("/users/{id}", (int id, User updatedUser) =>
{
    var index = users.FindIndex(u => u.Id == id);
    if (index == -1) return Results.NotFound();

    users[index] = updatedUser with { Id = id };
    return Results.NoContent();
});

// DELETE: Remove a user
app.MapDelete("/users/{id}", (int id) =>
{
    var user = users.FirstOrDefault(u => u.Id == id);
    if (user is null) return Results.NotFound();
    
    users.Remove(user);
    return Results.NoContent();
});

app.Run();

// Data structure
public record User(int Id, string Name, string Email);