namespace ExpressionVisitorExample.Models;

internal record Employee(int Id, string Name, string Surname, int Age)
{
    public Employee() : this (0, string.Empty, string.Empty, 0) { }

    public override string ToString() => $"Id: {Id}, Name: {Name}, Surname: {Surname}, Age: {Age}";    
}