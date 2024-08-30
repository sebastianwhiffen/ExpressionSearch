using System.Linq.Dynamic.Core;
using System.Linq.Expressions;

Person[] people =
{
    new Person { fName = "John", lName = "Smith", DOB = new DateTime(1980, 1, 1) },
    new Person { fName = "Jane", lName = "Doe", DOB = new DateTime(1985, 2, 15) },
    new Person { fName = "John", lName = "Doe", DOB = new DateTime(1990, 3, 20) },
    new Person { fName = "Seb", lName = "Whiff", DOB = new DateTime(1995, 4, 25) },
    new Person { fName = "Kalea", lName = "CartWright", DOB = new DateTime(2000, 5, 30) },
    new Person { fName = "Romy", lName = "O'leary", DOB = new DateTime(2005, 6, 10) },
};
var builder = WebApplication.CreateBuilder(args);

builder.Services.AddCors(o => o.AddPolicy("MyPolicy", builder =>
    {
        builder.WithOrigins("*")
               .AllowAnyMethod()
               .AllowAnyHeader();
    }));

builder.WebHost.UseUrls("http://localhost:5001");

var app = builder.Build();

app.UseCors("MyPolicy");

app.MapGet("/", (string? expression) =>
{
    if (expression == null) return people;

    return people.Where(ParseExpression(expression!));
});

app.Run();

static Func<Person, bool> ParseExpression(string expressionString)
{
    try
    {
        ParameterExpression parameter = Expression.Parameter(typeof(Person), "x");

        var expression = DynamicExpressionParser.ParseLambda([parameter], typeof(bool), expressionString);

        return (Func<Person, bool>)expression.Compile();
    }
    catch
    {
        Console.WriteLine("Invalid expression");
        return x => true;
    }
}

public class Person
{
    public string fName { get; set; }
    public string lName { get; set; }
    public DateTime DOB { get; set; }
}