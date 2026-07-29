namespace Basics.Block1;

// ТЕМА 1.5 — null, Nullable<T>, ?., ??, ??=, NRT
//
// 3 задачи. Реши САМ. Перед запуском впиши ПРЕДСКАЗАНИЕ.
// Когда закончишь — скажи, проверю код и предсказания.
//
// Запуск:  dotnet run --project Basics -- 1.5

public static class NullHandling
{
    // Класс со ссылочным nullable-полем — для задач 2 и 3.
    public class Person
    {
        public string? Name;
    }

    public static void Run()
    {
        Task1();
        Task2();
        Task3();
    }

    // ── ЗАДАЧА 1: Nullable<int> — HasValue / Value / GetValueOrDefault ────
    // 1) int? x = null;
    //    Выведи x.HasValue и x.GetValueOrDefault().
    // 2) Присвой x = 7;
    //    Выведи x.HasValue и x.Value.
    //
    // Вопрос в комментарии: что вернёт x.Value, если HasValue == false, —
    // и почему GetValueOrDefault() безопаснее?
    //
    // ПРЕДСКАЗАНИЕ (шаг 1: HasValue, GetValueOrDefault): HasValue = false, GetValueOrDefault = 0 - дефолтное значение для int.
    // ПРЕДСКАЗАНИЕ (шаг 2: HasValue, Value):             HasValue = true, GetValueOrDefault = 7.
    private static void Task1()
    {
        Console.WriteLine("--- Задача 1 ---");

        int? x = null;
        Console.WriteLine($"x.HasValue: {x.HasValue}");
        Console.WriteLine($"x.GetValueOrDefault(): {x.GetValueOrDefault()}");

        x = 7;
        Console.WriteLine($"x.HasValue: {x.HasValue}");
        Console.WriteLine($"x.GetValueOrDefault(): {x.GetValueOrDefault()}");
    }

    // ── ЗАДАЧА 2: цепочка ?. и оператор ?? ────────────────────────────────
    // Даны три случая (создай объекты):
    //   a) Person? p1 = null;
    //   b) Person p2 = new() { Name = null };
    //   c) Person p3 = new() { Name = "abc" };
    //
    // Для каждого вычисли длину имени безопасно через цепочку:
    //   int? len = pX?.Name?.Length;
    // и выведи len для p1, p2, p3.
    //
    // Затем для p1 выведи вариант с ??:  int safeLen = p1?.Name?.Length ?? -1;
    //
    // Вопрос в комментарии: почему тип pX?.Name?.Length — это int?, а не int?
    //
    // ПРЕДСКАЗАНИЕ (len для p1): null так как p1 = null.
    // ПРЕДСКАЗАНИЕ (len для p2): null так как Name = null.
    // ПРЕДСКАЗАНИЕ (len для p3): 3 так как Name = "abc" - длина = 3.
    // ПРЕДСКАЗАНИЕ (safeLen для p1): -1, так как p1 = null и используется ??.
    private static void Task2()
    {
        Console.WriteLine("--- Задача 2 ---");

        Person? p1 = null;
        Person p2 = new() { Name = null };
        Person p3 = new() { Name = "abc" };

        int? len1 = p1?.Name?.Length;
        Console.WriteLine(len1);

        int? len2 = p2?.Name?.Length;
        Console.WriteLine(len2);

        int? len3 = p3?.Name?.Length;
        Console.WriteLine(len3);

        int safeLen = p1?.Name?.Length ?? -1;
        Console.WriteLine(safeLen);
    }

    // ── ЗАДАЧА 3: ?. спасает от NRE + оператор ??= ────────────────────────
    // Часть А — сравни поведение прямого доступа и ?.:
    //   Person? p = null;
    //   1) В try/catch попробуй int bad = p.Name.Length; — поймай исключение,
    //      выведи его тип (ex.GetType().Name).
    //   2) Безопасно: int? good = p?.Name?.Length; выведи good (ожидаемо пусто/null).
    //
    // Часть Б — оператор ??=:
    //   string? s = null;
    //   s ??= "default";  выведи s
    //   s ??= "other";    выведи s   (подумай: изменится ли?)
    //
    // ПРЕДСКАЗАНИЕ (тип исключения в п.1):     NullReferenceException, так как до выполнения Length не дойдет и упадет на p.Name, по причине того, что у p нет ссылки на объект.
    // ПРЕДСКАЗАНИЕ (good в п.2):               Null? так как мы используем оператор?.
    // ПРЕДСКАЗАНИЕ (s после первого ??=):      default
    // ПРЕДСКАЗАНИЕ (s после второго ??=):      default
    private static void Task3()
    {
        Console.WriteLine("--- Задача 3 ---");

        Person? p = null;

        try
        {
            int bad = p.Name.Length;
        }
        catch (Exception ex)
        {
            Console.WriteLine($"Тип Exception: {ex.GetType().Name}");
        }

        int? good = p?.Name?.Length;
        Console.WriteLine($"good: {good}");

        string? s = null;

        s ??= "default";
        Console.WriteLine($"s ??= \"default\" : {s}");

        s ??= "other";
        Console.WriteLine($"s ??= \"other\": {s}");
    }
}
