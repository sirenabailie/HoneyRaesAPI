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
    return serviceTickets.Select(st => new
    {
        st.Id,
        st.Description,
        st.Emergency,
        st.DateCompleted,
        Customer = customers.FirstOrDefault(c => c.Id == st.CustomerId),
        Employee = employees.FirstOrDefault(e => e.Id == st.EmployeeId)
    });
});

// Get a single service ticket with employee and customer details
app.MapGet("/servicetickets/{id}", (int id) =>
{
    ServiceTicket serviceTicket = serviceTickets.FirstOrDefault(st => st.Id == id);
    if (serviceTicket == null)
    {
        return Results.NotFound();
    }

    serviceTicket.Employee = employees.FirstOrDefault(e => e.Id == serviceTicket.EmployeeId);
    serviceTicket.Customer = customers.FirstOrDefault(c => c.Id == serviceTicket.CustomerId);

    return Results.Ok(serviceTicket);
});

app.MapGet("/employees", () =>
{
    return employees;
});

// Get an employee by ID including assigned service tickets
app.MapGet("/employees/{id}", (int id) =>
{
    Employee employee = employees.FirstOrDefault(e => e.Id == id);
    if (employee == null)
    {
        return Results.NotFound();
    }

    employee.ServiceTickets = serviceTickets.Where(st => st.EmployeeId == id).ToList();

    return Results.Ok(new
    {
        employee.Id,
        employee.Name,
        employee.Specialty,
        ServiceTickets = employee.ServiceTickets.Select(st => new
        {
            st.Id,
            st.Description,
            st.Emergency,
            st.DateCompleted
        })
    });
});

app.MapGet("/customers", () =>
{
    return customers;
});

// Get a customer by ID, including their service tickets
app.MapGet("/customers/{id}", (int id) =>
{
    Customer customer = customers.FirstOrDefault(c => c.Id == id);
    if (customer == null)
    {
        return Results.NotFound();
    }

    var customerTickets = serviceTickets.Where(st => st.CustomerId == id).ToList();

    return Results.Ok(new
    {
        customer.Id,
        customer.Name,
        customer.Address,
        ServiceTickets = customerTickets.Select(st => new
        {
            st.Id,
            st.Description,
            st.Emergency,
            st.DateCompleted,
            Employee = employees.FirstOrDefault(e => e.Id == st.EmployeeId)
        })
    });
});

app.MapPost("/servicetickets", (ServiceTicket serviceTicket) =>
{
    // creates a new id (When we get to it later, our SQL database will do this for us like JSON Server did!)
    serviceTicket.Id = serviceTickets.Max(st => st.Id) + 1;
    serviceTickets.Add(serviceTicket);
    return serviceTicket;
});

app.MapDelete("/servicetickets/{id}", (int id) =>
{
    var serviceTicket = serviceTickets.FirstOrDefault(st => st.Id == id);
    
    if (serviceTicket == null)
    {
        return Results.NotFound();
    }

    serviceTickets.Remove(serviceTicket);
    return Results.NoContent(); // NoContent tells us deletion was successful 
});

app.Run();
