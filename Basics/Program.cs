// Раннер тем.
//   Без аргумента (или F5 в Visual Studio) — запускается текущая (последняя) тема.
//   dotnet run --project Basics -- 1.1     — запуск конкретной темы
//   dotnet run --project Basics -- list    — список всех тем

using Basics;

var topics = TopicRegistry.All;

if (topics.Length == 0)
{
    Console.WriteLine("Темы ещё не зарегистрированы в TopicRegistry.");
    return;
}

string key = args.Length > 0 ? args[0] : topics[^1].Id; // по умолчанию — последняя тема

if (key == "list")
{
    Console.WriteLine("Доступные темы:");
    foreach (var (id, title, _) in topics)
        Console.WriteLine($"  {id,-6} {title}");
    return;
}
в
var topic = topics.FirstOrDefault(t => t.Id == key);
if (topic.Run is null)
{
    Console.WriteLine($"Тема '{key}' не найдена. Запусти без аргумента, чтобы увидеть список.");
    return;
}

Console.WriteLine($"=== {topic.Id}  {topic.Title} ===\n");
topic.Run();
