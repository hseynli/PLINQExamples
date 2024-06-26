using ExpressionVisitorExample.Models;
using PLINQExamples;
using System.Diagnostics;

internal class Program : QueryRunner
{
    IEnumerable<Employee> employees = new MockDataGenerator().GenerateEmployees(10);

    public override void Run()
    {
        // ASlowQueryAppeared();
        RunInParallel();
        // PreserveOrdering();
        // LimitParallelization();
        // UseForAll();
        // MergingThreadResults();
        // ReturnToSequential();
    }

    public static void Main()
    {
        new Program().Run();

        Console.WriteLine("\nDone!");
    }

    /// <summary>
    /// This query is artificially slow, taking 1s per item in the source collection.
    /// </summary>
    void ASlowQueryAppeared()
    {
        var query =
            from employee in employees
            where SlowCondition(employee.Name.Length >  5)
            select employee;

        PrintAll(query);
    }

    /// <summary>
    /// By running in parallel, we can speed up results.
    /// </summary>
    void RunInParallel()
    {
        var stopWatch = new Stopwatch();
        stopWatch.Start();

        var query = employees
                    .AsParallel()
                    .Where(employee => SlowCondition(employee.Name.Length >  5));

        PrintAll(query);

        Console.WriteLine($"Execution time: {stopWatch.ElapsedMilliseconds} ms");
    }

    /// <summary>
    /// If we need to keep the order of the source intact
    /// </summary>
    void PreserveOrdering()
    {
        var stopWatch = new Stopwatch();
        stopWatch.Start();


        var query = employees
                    .AsParallel()
                    .AsOrdered()
                    .Where(employee => SlowCondition(employee.Name.Length >  5));

        PrintAll(query);

        Console.WriteLine($"Execution time: {stopWatch.ElapsedMilliseconds} ms");
    }

    /// <summary>
    /// Sometimes we want to limit the number of threads.
    /// </summary>
    void LimitParallelization()
    {
        var stopWatch = new Stopwatch();
        stopWatch.Start();

        var query = employees
                    .AsParallel()
                    .WithDegreeOfParallelism(6)
                    .Where(employee => SlowCondition(employee.Name.Length >  5));

        PrintAll(query);

        Console.WriteLine($"Execution time: {stopWatch.ElapsedMilliseconds} ms");
    }

    /// <summary>
    /// Instead of iterating, the pipeline can remain parallel.
    /// </summary>
    void UseForAll()
    {
        var stopWatch = new Stopwatch();
        stopWatch.Start();

        employees
            .AsParallel()
            .Where(employee => SlowCondition(employee.Name.Length > 5))
            .ForAll(Console.WriteLine);

        Console.WriteLine($"Execution time: {stopWatch.ElapsedMilliseconds} ms");
    }

    /// <summary>
    /// MergeOptions control how results are returned.
    /// </summary>
    void MergingThreadResults()
    {
        var stopWatch = new Stopwatch();
        stopWatch.Start();

        var query = employees
                .AsParallel()
                .WithDegreeOfParallelism(6)
                .WithMergeOptions(ParallelMergeOptions.FullyBuffered)
                .Where(employee => SlowCondition(employee.Name.Length >  5));

        PrintAll(query);

        Console.WriteLine($"Execution time: {stopWatch.ElapsedMilliseconds} ms");
    }

    /// <summary>
    /// You can run the rest of the query in sequence if needed.
    /// </summary>
    void ReturnToSequential()
    {
        var stopWatch = new Stopwatch();
        stopWatch.Start();

        var query = employees
            .AsParallel()
            .Where(employee => SlowCondition(employee.Name.Length >  5))
            .AsSequential()
            .Where(employee => SlowCondition(employee.Age > 30));

        PrintAll(query);

        Console.WriteLine($"Execution time: {stopWatch.ElapsedMilliseconds} ms");
    }

    private bool SlowCondition(bool inputCondition)
    {
        Thread.Sleep(1000);
        return inputCondition;
    }
}