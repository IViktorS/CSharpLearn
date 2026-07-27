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
    // ПРЕДСКАЗАНИЕ (шаг 1: HasValue, GetValueOrDefault): ...
    // ПРЕДСКАЗАНИЕ (шаг 2: HasValue, Value):             ...
    private static void Task1()
    {
        Console.WriteLine("--- Задача 1 ---");

        // TODO: твой код
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
    // ПРЕДСКАЗАНИЕ (len для p1): ...
    // ПРЕДСКАЗАНИЕ (len для p2): ...
    // ПРЕДСКАЗАНИЕ (len для p3): ...
    // ПРЕДСКАЗАНИЕ (safeLen для p1): ...
    private static void Task2()
    {
        Console.WriteLine("--- Задача 2 ---");

        // TODO: твой код
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
    // ПРЕДСКАЗАНИЕ (тип исключения в п.1):     ...
    // ПРЕДСКАЗАНИЕ (good в п.2):               ...
    // ПРЕДСКАЗАНИЕ (s после первого ??=):      ...
    // ПРЕДСКАЗАНИЕ (s после второго ??=):      ...
    private static void Task3()
    {
        Console.WriteLine("--- Задача 3 ---");

        // TODO: твой код
    }
}
