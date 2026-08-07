namespace Basics.Block1;

// ТЕМА 1.10 — static (класс / метод / поле / конструктор)
//
// 3 задачи. Реши САМ. Перед запуском впиши ПРЕДСКАЗАНИЕ.
// Запуск:  dotnet run --project Basics -- 1.10

public static class StaticMembers
{
    // Для задачи 1: static-поле общее для всех, экземплярное — своё.
    public class Widget
    {
        public static int Count;   // одно на весь тип
        public int Id;             // своё у каждого объекта

        public Widget()
        {
            Count++;               // общий счётчик растёт при каждом создании
            Id = Count;            // фиксируем номер этого экземпляра
        }
    }

    // Для задачи 2: статический конструктор — один раз, лениво.
    public class Settings
    {
        public static readonly string LoadedAt;

        static Settings()
        {
            Console.WriteLine("  [сработал static-конструктор Settings]");
            LoadedAt = "инициализировано";
        }
    }

    // Для задачи 3: статический класс-утилита.
    public static class StringUtils
    {
        public static string Repeat(string s, int times) =>
            string.Concat(Enumerable.Repeat(s, times));
    }

    public static void Run()
    {
        Task1();
        Task2();
        Task3();
    }

    // ── ЗАДАЧА 1: static-поле общее для всех экземпляров ──────────────────
    // 1) Создай ТРИ Widget подряд.
    // 2) Для каждого выведи его Id сразу после создания.
    // 3) В конце выведи Widget.Count (обращайся через ТИП, не через объект).
    //
    // ПРЕДСКАЗАНИЕ (Id трёх виджетов): widget1.Id = 1, widget2.Id = 2, widget3.Id = 3
    // ПРЕДСКАЗАНИЕ (Widget.Count в конце): Widget.Count = 3
    private static void Task1()
    {
        Console.WriteLine("--- Задача 1 ---");

        var widget1 = new Widget();
        Console.WriteLine($"widget1.Id: {widget1.Id}");

        var widget2 = new Widget();
        Console.WriteLine($"widget2.Id: {widget2.Id}");

        var widget3 = new Widget();
        Console.WriteLine($"widget3.Id: {widget3.Id}");

        Console.WriteLine($"Widget.Count: {Widget.Count}");
    }

    // ── ЗАДАЧА 2: когда срабатывает static-конструктор ────────────────────
    // 1) Выведи строку "до первого обращения к Settings".
    // 2) Обратись к Settings.LoadedAt и выведи его.
    // 3) Обратись к Settings.LoadedAt ЕЩЁ РАЗ и выведи снова.
    //
    // Следи по выводу: в какой момент печатается "[сработал static-конструктор]"
    // и сколько раз.
    //
    // ПРЕДСКАЗАНИЕ (сколько раз сработает static-конструктор и когда именно): Сработает 1 раз при первом обращении к Settings.LoadedAt
    private static void Task2()
    {
        Console.WriteLine("--- Задача 2 ---");

        Console.WriteLine("До первого обращения к Settings");
        Console.WriteLine($"Settings.LoadedAt: {Settings.LoadedAt}");
        Console.WriteLine($"Settings.LoadedAt: {Settings.LoadedAt}");
    }

    // ── ЗАДАЧА 3: статический класс-утилита ───────────────────────────────
    // 1) Вызови StringUtils.Repeat("ab", 3) и выведи результат.
    // 2) В комментарии ответь: почему статический метод Repeat НЕ смог бы
    //    обратиться к нестатическому полю какого-либо класса напрямую? Потому что у него нет указателя this и он не знает к какому экземпляру обращаться.
    //
    // ПРЕДСКАЗАНИЕ (StringUtils.Repeat("ab", 3)): ababab
    private static void Task3()
    {
        Console.WriteLine("--- Задача 3 ---");

        var repeat = StringUtils.Repeat("ab", 3);

        Console.WriteLine($"StringUtils.Repeat(\"ab\", 3): {repeat}");
    }
}
