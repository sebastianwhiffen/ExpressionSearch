using System.Linq.Dynamic.Core;
using System.Linq.Expressions;

Person[] people =
{
    new Person { fName = "John", lName = "smith" },
    new Person { fName = "Jane", lName = "Doe" },
    new Person { fName = "John", lName = "Doe" },
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
    if(expression == null) return people;

    return people.Where(ParseExpression(expression!));
});

app.Run();

static Func<Person, bool> ParseExpression(string expressionString)
{
    try
    {
        ParameterExpression parameter = Expression.Parameter(typeof(Person), "x");

        var expression = DynamicExpressionParser.ParseLambda(new[] { parameter }, typeof(bool), expressionString);

        return (Func<Person, bool>)expression.Compile();
    }
    catch
    {
        Console.WriteLine("Invalid expression");
        return x => true;
    }
}

class Person
{
    public string fName { get; set; }
    public string lName { get; set; }
}