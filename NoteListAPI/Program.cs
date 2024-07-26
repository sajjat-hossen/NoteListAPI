using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using NoteListAPI.DomainLayer.Data;
using NoteListAPI.RepositoryLayer.IRepositories;
using NoteListAPI.RepositoryLayer.Repositories;
using NoteListAPI.ServiceLayer.IServices;
using NoteListAPI.ServiceLayer.MappingProfiles;
using NoteListAPI.ServiceLayer.Services;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddDbContext<ApplicationDbContext>(options => options.UseSqlServer(builder.Configuration.GetConnectionString("DefaultConnection")));

builder.Services.AddIdentity<IdentityUser<int>, IdentityRole<int>>(options =>
{
    options.Password.RequireDigit = true;
    options.Password.RequiredLength = 8;
    options.Password.RequireNonAlphanumeric = true;
    options.Password.RequireUppercase = true;
    options.Password.RequireLowercase = true;
    options.Password.RequiredUniqueChars = 4;
}).AddEntityFrameworkStores<ApplicationDbContext>()
    .AddDefaultTokenProviders();

builder.Services.AddAuthorization(options =>
{
    options.AddPolicy("DeleteNotePolicy", policy => policy.RequireClaim("Delete Note"));
    options.AddPolicy("EditNotePolicy", policy => policy.RequireClaim("Edit Note"));
    options.AddPolicy("CreateNotePolicy", policy => policy.RequireClaim("Create Note"));
    options.AddPolicy("ViewNotePolicy", policy => policy.RequireClaim("View Note"));

    options.AddPolicy("DeleteTodoListPolicy", policy => policy.RequireClaim("Delete TodoList"));
    options.AddPolicy("EditTodoListPolicy", policy => policy.RequireClaim("Edit TodoList"));
    options.AddPolicy("CreateTodoListPolicy", policy => policy.RequireClaim("Create TodoList"));
    options.AddPolicy("ViewTodoListPolicy", policy => policy.RequireClaim("View TodoList"));
});

builder.Services.AddAutoMapper(typeof(Mappings));

builder.Services.AddScoped<INoteRepository, NoteRepository>();
builder.Services.AddScoped<INoteService, NoteService>();
builder.Services.AddScoped<ITodoListRepository, TodoListRepository>();
builder.Services.AddScoped<ITodoListService, TodoListService>();
builder.Services.AddScoped<IRoleService, RoleService>();
builder.Services.AddScoped<IAccountService, AccountService>();
builder.Services.AddScoped<IClaimService, ClaimService>();
builder.Services.AddScoped<IAdministrationService, AdministrationService>();

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

app.UseAuthentication();
app.UseAuthorization();

app.MapControllers();

app.Run();
