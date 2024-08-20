using System.Linq.Dynamic.Core;
using System.Linq.Expressions;

Person[] people =
{
    new Person { Name = "Alice", Age = 30, DateOfBirth = new DateTime(1993, 1, 1) },
    new Person { Name = "Bob", Age = 25, DateOfBirth = new DateTime(1998, 1, 1) },
    new Person { Name = "Charlie", Age = 35, DateOfBirth = new DateTime(1988, 1, 1) },
    new Person { Name = "David", Age = 40, DateOfBirth = new DateTime(1983, 1, 1) },
    new Person { Name = "Eve", Age = 28, DateOfBirth = new DateTime(1995, 1, 1) }
};

while (true)
{
    Console.WriteLine("Enter an expression where x is the predicate. eg: x.Age > 30");
    var expression = Console.ReadLine();

    people.Where(ParseExpression(expression!)).ToList().ForEach(person => Console.WriteLine($"{person.Name}, {person.Age}, {person.DateOfBirth.ToShortDateString()}"));
}

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
    public string Name { get; set; }
    public int Age { get; set; }
    public DateTime DateOfBirth { get; set; }
}