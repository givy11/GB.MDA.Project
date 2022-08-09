using System.Diagnostics;
using GB.MDA.Project.Model;

Console.OutputEncoding = System.Text.Encoding.UTF8;
var rest = new Restaurant();
while (true)
{
    Console.WriteLine("Привет! Желаете забронировать столик? 1 - асинхронно. 2 - синхронно.");
    if (!int.TryParse(Console.ReadLine(), out var choice) && choice != 1 && choice != 2)
    {
        Console.WriteLine("Введите пожалуйста 1 или 2");
        continue;
    }

    var stopwatch = new Stopwatch();
    stopwatch.Start();
    if (choice == 1)
    {
        rest.BookFreeTableAsync(1);
    }
    else
    {
        rest.BookFreeTable(1);
    }

    Console.WriteLine("Спасибо за обращение");
    stopwatch.Stop();
    var ts = stopwatch.Elapsed;
    Console.WriteLine($"{ts.Seconds:00}:{ts.Milliseconds:00}");
}