using System.Text;

namespace Basics.Block1;

// ТЕМА 1.12 — StringBuilder vs string
//
// 3 задачи. Реши САМ. Перед запуском впиши ПРЕДСКАЗАНИЕ.
// Запуск:  dotnet run --project Basics -- 1.12

public static class StringBuilderVsString
{
    public static void Run()
    {
        Task1();
        Task2();
        Task3();
    }

    // ── ЗАДАЧА 1: цена конкатенации в цикле — меряем GC ───────────────────
    // N = 30000.
    // Сценарий А (string): result = ""; в цикле result += "x";
    //   Замерь разницу GC.CollectionCount(0) вокруг цикла, выведи её и result.Length.
    // Сценарий Б (StringBuilder): sb.Append("x") в цикле, потом sb.ToString().
    //   Замерь разницу GC.CollectionCount(0), выведи её и длину результата.
    //
    // ПРЕДСКАЗАНИЕ (где сборок больше и почему): Сборок будет больше у string, так как в каждой итерации цикла будет создаваться
    // новый экземпляр строки за которым придется следить GC.
    private static void Task1()
    {
        Console.WriteLine("--- Задача 1 ---");

        var N = 30000;

        var result = "";

        Console.WriteLine($"GC.CollectionCount: {GC.CollectionCount(0)}");
        for (int i = 0; i < N; i++)
            result += "x";
        Console.WriteLine($"GC.CollectionCount: {GC.CollectionCount(0)}\nresult.Length: {result.Length}");

        var sb = new StringBuilder();

        Console.WriteLine($"GC.CollectionCount: {GC.CollectionCount(0)}");
        for (int i = 0; i < N; i++)
            sb.Append("x");
        sb.ToString();
        Console.WriteLine($"GC.CollectionCount: {GC.CollectionCount(0)}\nresult.Length: {sb.Length}");
    }

    // ── ЗАДАЧА 2: методы StringBuilder ────────────────────────────────────
    // 1) var sb = new StringBuilder();
    // 2) sb.Append("Hello");  sb.Append(", ");  sb.Append("world");
    // 3) sb.AppendLine("!");  sb.Append("вторая строка");
    // 4) Выведи sb.ToString().
    // 5) Проверь, что sb МЕНЯЕТСЯ на месте: выведи sb.Length, потом sb.Clear(),
    //    снова выведи sb.Length.
    //
    // ПРЕДСКАЗАНИЕ (текст после построения):     Hello, world!вторая строка
    // ПРЕДСКАЗАНИЕ (Length до Clear / после Clear): до - 26, после - 0
    private static void Task2()
    {
        Console.WriteLine("--- Задача 2 ---");

        var sb = new StringBuilder();
        sb.Append("Hello");
        sb.Append(", ");
        sb.Append("world");
        sb.Append("!");
        sb.Append("вторая строка");
        Console.WriteLine(sb.ToString());

        Console.WriteLine(sb.Length);
        sb.Clear();
        Console.WriteLine(sb.Length);
    }

    // ── ЗАДАЧА 3: string.Join + иммутабельность ───────────────────────────
    // 1) Дан массив string[] words = { "раз", "два", "три" }.
    //    Собери из него строку через string.Join(", ", words), выведи.
    // 2) Иммутабельность: string original = "hello";
    //    var upper = original.ToUpper();
    //    Выведи original и upper. В комментарии ответь: изменился ли original и почему? Не зименится, так как ToUpper возвращает новую строку.
    //
    // ПРЕДСКАЗАНИЕ (string.Join):        раз, два, три
    // ПРЕДСКАЗАНИЕ (original / upper):   hello / HELLO
    private static void Task3()
    {
        Console.WriteLine("--- Задача 3 ---");

        var str = "";

        string[] words = { "раз", "два", "три" };
        
        str = string.Join(", ", words);

        Console.WriteLine($"srt: {str}");

        string original = "hello";
        var upper = original.ToUpper();

        Console.WriteLine($"original: {original}");
        Console.WriteLine($"upper: {upper}");

    }
}
