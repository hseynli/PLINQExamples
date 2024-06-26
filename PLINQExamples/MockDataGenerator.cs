using Bogus;
using ExpressionVisitorExample.Models;

namespace PLINQExamples;

internal class MockDataGenerator
{
    internal IEnumerable<Employee> GenerateEmployees(int count)
    {
        Faker<Employee> people = new Faker<Employee>()
                    .RuleFor(p => p.Id, f => f.IndexFaker)
                    .RuleFor(p => p.Name, f => f.Name.FirstName())
                    .RuleFor(p => p.Surname, f => f.Name.LastName())
                    .RuleFor(p => p.Age, f => f.Random.Number(18, 65));

        return people.Generate(count);
    }
}