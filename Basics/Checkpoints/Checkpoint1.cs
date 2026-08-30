namespace Basics.Checkpoints;

// 🔁 КОНТРОЛЬНАЯ ТОЧКА 1 — смешанная задача по темам 1.1–1.13
//
// Формат другой, чем в обычных темах:
//   • в задачах 1 и 2 код уже написан — твоё дело ПРЕДСКАЗАТЬ вывод
//     и назвать, ИЗ КАКОЙ ТЕМЫ каждое поведение;
//   • в задаче 3 код надо дописать самому.
// Ни в один конспект не подглядывать — в этом весь смысл.
//
// Запуск:  dotnet run --project Basics -- R1

public static class Checkpoint1
{
    public static void Run()
    {
        Task1();
        Task2();
        Task3();
    }

    private class Config
    {
        public const string Version = "1.0";
        public readonly List<string> Items = new();
        public static int InstanceCount;

        public Config() => InstanceCount++;
    }

    // ── ЗАДАЧА 1: один класс — четыре темы ────────────────────────────────
    // Код ниже уже написан. Предскажи КАЖДУЮ строку вывода и рядом напиши
    // номер темы, которая это объясняет.
    //
    // ПРЕДСКАЗАНИЕ (1) c1.Items.Count после всего:            (тема: )
    // ПРЕДСКАЗАНИЕ (2) Config.InstanceCount:                  (тема: )
    // ПРЕДСКАЗАНИЕ (3) две закомментированные строки в конце — какие коды
    //                  ошибок компиляции и почему:
    //                  строка А:                              (тема: )
    //                  строка Б:                              (тема: )
    private static void Task1()
    {
        Console.WriteLine("--- Задача 1 ---");

        var c1 = new Config();
        var c2 = new Config();

        c1.Items.Add("первый");
        c1.Items.Add("второй");
        c2.Items.Add("чужой");

        Console.WriteLine($"c1.Items.Count = {c1.Items.Count}");
        Console.WriteLine($"Config.InstanceCount = {Config.InstanceCount}");
        Console.WriteLine($"Version = {Config.Version}");

        // Строка А — раскомментируй, посмотри ошибку, верни комментарий:
        // c1.Items = new List<string>();

        // Строка Б — то же самое:
        // Console.WriteLine(c1.Version);
    }

    private enum Status { Draft, Active, Closed }

    // ── ЗАДАЧА 2: коробки, enum и null ────────────────────────────────────
    // Тоже готовый код. Здесь есть строка, где «очевидный» ответ неверен.
    //
    // ПРЕДСКАЗАНИЕ (1) unboxed:                    (тема: )
    // ПРЕДСКАЗАНИЕ (2) s и Enum.IsDefined:         (тема: )
    // ПРЕДСКАЗАНИЕ (3) n после двух ??=:           (тема: )
    // ПРЕДСКАЗАНИЕ (4) b1 == b2:                   (тема: )
    // ПРЕДСКАЗАНИЕ (5) b1.Equals(b2):              (тема: )
    // ПРЕДСКАЗАНИЕ (6) что произойдёт, если раскомментировать строку В:
    private static void Task2()
    {
        Console.WriteLine("--- Задача 2 ---");

        object boxed = 42;
        int unboxed = (int)boxed;
        Console.WriteLine($"unboxed = {unboxed}");

        // Строка В — раскомментируй, посмотри, верни комментарий:
        // long asLong = (long)boxed;

        Status s = (Status)99;
        Console.WriteLine($"s = {s}");
        Console.WriteLine($"IsDefined = {Enum.IsDefined(typeof(Status), s)}");

        int? n = null;
        n ??= 5;
        n ??= 10;
        Console.WriteLine($"n = {n}");

        object b1 = 42;
        object b2 = 42;
        Console.WriteLine($"b1 == b2      : {b1 == b2}");
        Console.WriteLine($"b1.Equals(b2) : {b1.Equals(b2)}");
    }

    private struct Counter
    {
        public int Value;
    }

    // ── ЗАДАЧА 3: почини баг ──────────────────────────────────────────────
    // Дано: var list = new List<Counter> { new Counter { Value = 1 } };
    // Надо: увеличить Value первого элемента на 1.
    //
    // 1) Напиши список и убедись, что строка list[0].Value++; не компилируется.
    //    В комментарии — код ошибки и причина.
    // 2) Почини ДВУМЯ разными способами (оба оставь в коде, второй — на копии
    //    списка или на новом списке, чтобы не мешали друг другу).
    //    Способ 1 — не меняя тип Counter.
    //    Способ 2 — поменяв структуру данных или тип. В комментарии объясни,
    //    почему теперь работает.
    // 3) Напиши private static метод Bump, который принимает List<Counter>
    //    и индекс, увеличивает элемент и возвращает КОРТЕЖ (int Before, int After).
    //    Вызови его и деконструируй результат.
    //
    // ПРЕДСКАЗАНИЕ (код ошибки на list[0].Value++):
    // ПРЕДСКАЗАНИЕ (Before / After):
    private static void Task3()
    {
        Console.WriteLine("--- Задача 3 ---");

    }
}
