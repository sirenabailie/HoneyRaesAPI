using HoneyRaesAPI.Models;

List<Customer> customers = new List<Customer>
{
    new Customer { Id = 1, Name = "T. Kingfisher", Address = "123 Greensboro St" },
    new Customer { Id = 2, Name = "N.K. Jemisin", Address = "456 Fifth Season Ave" },
    new Customer { Id = 3, Name = "Fonda Lee", Address = "789 Janloon Rd" }
};

List<Employee> employees = new List<Employee>
{
    new Employee { Id = 1, Name = "Bob Ross", Specialty = "Painting" },
    new Employee { Id = 2, Name = "Beatrix Farrand", Specialty = "Landscaping" }
};

List<ServiceTicket> serviceTickets = new List<ServiceTicket>
{
    new ServiceTicket { Id = 1, CustomerId = 1, EmployeeId = 1, Description = "Touchup Exterior Paint", Emergency = false, DateCompleted = DateTime.Now },
    new ServiceTicket { Id = 2, CustomerId = 2, EmployeeId = 2, Description = "Trim Hedges", Emergency = false },
    new ServiceTicket { Id = 3, CustomerId = 3, EmployeeId = 1, Description = "Repaint Office Walls", Emergency = true, DateCompleted = DateTime.Now },
    new ServiceTicket { Id = 4, CustomerId = 1, Description = "Replace Fuse Box", Emergency = true }, 
    new ServiceTicket { Id = 5, CustomerId = 2, Description = "Build Custom Bookshelf", Emergency = false } 
};

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
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

app.MapGet("/servicetickets", () =>
{
    return serviceTickets;
});

app.MapGet("/servicetickets/{id}", (int id) =>
{
    var ticket = serviceTickets.FirstOrDefault(st => st.Id == id);
    return ticket is not null ? Results.Ok(ticket) : Results.NotFound();
});

app.MapGet("/employees", () =>
{
    return employees;
});

app.MapGet("/employees/{id}", (int id) =>
{
    var employee = employees.FirstOrDefault(e => e.Id == id);
    return employee is not null ? Results.Ok(employee) : Results.NotFound();
});

app.MapGet("/customers", () =>
{
    return customers;
});

app.MapGet("/customers/{id}", (int id) =>
{
    var customer = customers.FirstOrDefault(c => c.Id == id);
    return customer is not null ? Results.Ok(customer) : Results.NotFound();
});

app.Run();
