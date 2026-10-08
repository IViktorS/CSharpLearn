using System.Reflection;

namespace Basics.Checkpoints;

// 🔁 РАЗБОР ОЧЕРЕДИ ПОВТОРЕНИЯ №2
//
// Не новая тема. Семь фактов, накопившихся в очереди, собраны по группам
// механизмов. Две задачи — находки карты Блока 1, остальные — строки,
// у которых подошёл срок или которые провалились в Q1.
//
// В конспекты не подглядывать.
//
// Запуск:  dotnet run --project Basics -- Q2

public static class QueueReview2
{
    public static void Run()
    {
        Task1();
        Task2();
        Task3();
        Task4();
        Task5();
    }

    // Поля, кроме A, никто не читает — они здесь ровно чтобы структура
    // весила 64 байта, поэтому CS0649 заглушён.
#pragma warning disable 649
    private struct Big
    {
        public double A, B, C, D, E, F, G, H;   // 64 байта данных
    }
#pragma warning restore 649

#pragma warning disable 649
    private class HasBig
    {
        public Big Value;   // поле нужно только ради размера объекта
    }
#pragma warning restore 649

    // ── ЗАДАЧА 1: где на самом деле лежит значимый тип ────────────────────
    //
    // Структура Big одна и та же во всех трёх случаях. Отличается только
    // место объявления. Замер считает выделенные в КУЧЕ байты.
    //
    // ПРЕДСКАЗАНИЕ (1) 100 000 ЛОКАЛЬНЫХ Big в цикле — сколько байт в куче?
    // ПРЕДСКАЗАНИЕ (2) 100 000 объектов класса с ПОЛЕМ Big — сколько?
    //                  И сколько примерно на один объект?
    // ПРЕДСКАЗАНИЕ (3) массив new Big[100000] — сколько? В куче он или нет?
    // ПРЕДСКАЗАНИЕ (4) сформулируй правило одной фразой: от чего зависит,
    //                  окажется значимый тип в стеке или в куче?
    //
    // ПРЕДСКАЗАНИЕ (5) int x = 7; object b = x; x = 99;
    //                  что напечатает x и что напечатает b? Почему?
    private static void Task1()
    {
        Console.WriteLine("--- Задача 1: где лежит значимый тип ---");

        const int N = 100_000;
        Big sink = default;
        HasBig[] keep;

        var a0 = GC.GetAllocatedBytesForCurrentThread();
        for (int i = 0; i < N; i++) { Big local = default; local.A = i; sink = local; }
        var a1 = GC.GetAllocatedBytesForCurrentThread();

        keep = new HasBig[N];
        var a2 = GC.GetAllocatedBytesForCurrentThread();
        for (int i = 0; i < N; i++) keep[i] = new HasBig();
        var a3 = GC.GetAllocatedBytesForCurrentThread();

        var a4 = GC.GetAllocatedBytesForCurrentThread();
        var arr = new Big[N];
        arr[0].A = 1;
        var a5 = GC.GetAllocatedBytesForCurrentThread();

        Console.WriteLine($"(1) {N} локальных Big      : {a1 - a0,10} байт");
        Console.WriteLine($"(2) {N} объектов с полем Big: {a3 - a2,10} байт  ({(a3 - a2) / (double)N} на объект)");
        Console.WriteLine($"(3) массив Big[{N}]    : {a5 - a4,10} байт");
        Console.WriteLine($"    (sink.A = {sink.A}, arr[0].A = {arr[0].A} — чтобы оптимизатор не выбросил)");

        int x = 7;
        object b = x;
        x = 99;
        Console.WriteLine($"(5) x = {x}, b = {b}");
    }

    // ── ЗАДАЧА 2: упаковка и Nullable — не одно и то же ───────────────────
    //
    // Обе конструкции называют «обёрткой вокруг значимого типа». Задача —
    // показать, что общего у них только это название.
    //
    // ПРЕДСКАЗАНИЕ (1) 100 000 раз  int? v = i;   сколько байт в куче?
    // ПРЕДСКАЗАНИЕ (2) 100 000 раз  object o = i; сколько байт в куче?
    // ПРЕДСКАЗАНИЕ (3) typeof(int?).IsValueType — что вернёт?
    // ПРЕДСКАЗАНИЕ (4) int? has = 5;  object o = has;
    //                  какой тип окажется ВНУТРИ коробки? Nullable<int> или int?
    // ПРЕДСКАЗАНИЕ (5) int? empty = null;  object o = empty;
    //                  что получится в o?
    // ПРЕДСКАЗАНИЕ (6) объясни разницу одной фразой: обёртка `Nullable<T>`
    //                  и коробка — это обёртки ГДЕ? (подсказка: не «в памяти»
    //                  обе, иначе ответ ничего не различает)
    private static void Task2()
    {
        Console.WriteLine("--- Задача 2: упаковка против Nullable ---");

        const int N = 100_000;
        int? nsink = null;
        object osink = null!;

        var b0 = GC.GetAllocatedBytesForCurrentThread();
        for (int i = 0; i < N; i++) { int? v = i; nsink = v; }
        var b1 = GC.GetAllocatedBytesForCurrentThread();
        for (int i = 0; i < N; i++) { object o = i; osink = o; }
        var b2 = GC.GetAllocatedBytesForCurrentThread();

        Console.WriteLine($"(1) {N} раз int? v = i   : {b1 - b0,10} байт");
        Console.WriteLine($"(2) {N} раз object o = i : {b2 - b1,10} байт  ({(b2 - b1) / (double)N} на штуку)");
        Console.WriteLine($"(3) typeof(int?).IsValueType = {typeof(int?).IsValueType}");

        int? has = 5;
        int? empty = null;
        object boxedHas = has;
        object? boxedEmpty = empty;

        Console.WriteLine($"(4) object o = (int?)5    -> внутри коробки: {boxedHas.GetType().Name}");
        Console.WriteLine($"(5) object o = (int?)null -> {(boxedEmpty is null ? "o == null, коробки нет" : "коробка есть")}");
        Console.WriteLine($"    (nsink={nsink}, osink={osink} — чтобы оптимизатор не выбросил)");
    }

    // ── ЗАДАЧА 3: что требует ограничения, а что нет ──────────────────────
    //
    // В классе Unconstrained ниже семь обобщённых методов. Ограничений
    // нет НИ ОДНОГО.
    //
    // ПРЕДСКАЗАНИЕ (1) какие из семи НЕ скомпилируются? Перечисли буквами.
    //                  Если считаешь, что все семь законны — так и напиши.
    // ПРЕДСКАЗАНИЕ (2) сформулируй правило: в каком случае ограничение
    //                  вообще требуется? Правило должно объяснять, почему
    //                  items.Length законно без него.
    //
    // Строка А ниже закомментирована: вызов Take<int?> у метода
    // с ограничением where T : struct.
    //
    // ПРЕДСКАЗАНИЕ (3) скомпилируется ли? Если нет — почему, ведь
    //                  int? это значимый тип (ты проверил это в задаче 2)?
    // ПРЕДСКАЗАНИЕ (4) чем эта ошибка ОТЛИЧАЕТСЯ от случая `T x = null`
    //                  в методе без ограничений? Это одна причина или
    //                  две разные?
    private static void Task3()
    {
        Console.WriteLine("--- Задача 3: когда нужно ограничение ---");

        Console.WriteLine($"Len:   {Unconstrained.Len(new[] { 1, 2, 3 })}");
        Console.WriteLine($"First: {Unconstrained.First(new[] { "a", "b" })}");
        Console.WriteLine($"Def:   [{Unconstrained.Def<int>()}]");
        Console.WriteLine($"Name:  {Unconstrained.Name<DateTime>()}");

        // Строка А — раскомментируй, посмотри ошибку, верни комментарий:
        // int? maybe = 5;
        // Console.WriteLine(Take(maybe));
    }

    private static string Take<T>(T value) where T : struct => $"взял {value}";

    // ── ЗАДАЧА 4: рефлексия против модификаторов свойства ─────────────────
    //
    // У класса Doc три свойства, все объявлены по-разному:
    //   Title  — public get, private set
    //   Author — public get, init
    //   Note   — public get, сеттера нет вообще
    //
    // Через рефлексию по каждому вызывается SetValue.
    //
    // ПРЕДСКАЗАНИЕ (1) Title (private set)  — получится записать или нет?
    // ПРЕДСКАЗАНИЕ (2) Author (init)        — получится или нет?
    // ПРЕДСКАЗАНИЕ (3) Note (нет сеттера)   — получится или нет?
    // ПРЕДСКАЗАНИЕ (4) сформулируй правило: на что SetValue смотрит,
    //                  а на что НЕ смотрит?
    // ПРЕДСКАЗАНИЕ (5) практический вывод: что из этого следует про
    //                  `private set` как средство защиты данных?
    private static void Task4()
    {
        Console.WriteLine("--- Задача 4: SetValue и модификаторы ---");

        var doc = new Doc();

        foreach (var name in new[] { "Title", "Author", "Note" })
        {
            var p = typeof(Doc).GetProperty(name)!;
            try
            {
                p.SetValue(doc, "изменено");
                Console.WriteLine($"  {name,-7} -> записалось, теперь: {p.GetValue(doc)}");
            }
            catch (Exception e)
            {
                Console.WriteLine($"  {name,-7} -> {e.GetType().Name}");
            }
        }

        Console.WriteLine();
        foreach (var p in typeof(Doc).GetProperties())
            Console.WriteLine($"  {p.Name,-7} CanWrite={p.CanWrite,-6} SetMethod={(p.SetMethod is null ? "null" : p.SetMethod.Name)}");
    }

    private class Doc
    {
        public string Title { get; private set; } = "исходный";
        public string Author { get; init; } = "аноним";
        public string Note { get; } = "только чтение";
    }

    // ── ЗАДАЧА 5 (ПРОВОКАЦИОННАЯ): три строки и два сравнения ─────────────
    //
    // Три переменные со значением «привет»:
    //   a, b — литералы;
    //   c    — собрана в рантайме из массива символов.
    // Затем ТЕ ЖЕ объекты кладутся в переменные типа object: oa, ob, oc.
    //
    // Восемь сравнений. Правдоподобный ответ неверен как минимум в одном.
    //
    // ПРЕДСКАЗАНИЕ (1) a == b:
    // ПРЕДСКАЗАНИЕ (2) a == c:
    // ПРЕДСКАЗАНИЕ (3) ReferenceEquals(a, b):
    // ПРЕДСКАЗАНИЕ (4) ReferenceEquals(a, c):
    // ПРЕДСКАЗАНИЕ (5) oa == ob:
    // ПРЕДСКАЗАНИЕ (6) oa == oc:
    // ПРЕДСКАЗАНИЕ (7) oa.Equals(oc):
    //
    // ПРЕДСКАЗАНИЕ (8) если (5) и (6) разошлись — объясни. Содержимое
    //                  в обеих парах одинаковое, тип переменных один и
    //                  тот же (object). Почему результат разный?
    //
    // ПРЕДСКАЗАНИЕ (9) (2) и (6) — это ОДНА И ТА ЖЕ пара объектов,
    //                  сравниваемая одним и тем же оператором ==.
    //                  Если результаты разошлись — что именно изменилось?
    private static void Task5()
    {
        Console.WriteLine("--- Задача 5: строки и два сравнения ---");

        string a = "привет";
        string b = "привет";
        string c = new string("привет".ToCharArray());

        object oa = a, ob = b, oc = c;

        Console.WriteLine($"(1) a == b                -> {a == b}");
        Console.WriteLine($"(2) a == c                -> {a == c}");
        Console.WriteLine($"(3) ReferenceEquals(a, b) -> {ReferenceEquals(a, b)}");
        Console.WriteLine($"(4) ReferenceEquals(a, c) -> {ReferenceEquals(a, c)}");
        Console.WriteLine($"(5) oa == ob              -> {oa == ob}");
        Console.WriteLine($"(6) oa == oc              -> {oa == oc}");
        Console.WriteLine($"(7) oa.Equals(oc)         -> {oa.Equals(oc)}");
    }
}

// Материал задачи 3. Ограничений нет ни одного.
internal static class Unconstrained
{
    public static int Len<T>(T[] items) => items.Length;            // А
    public static T First<T>(T[] items) => items[0];                // Б
    public static T Def<T>() => default!;                           // В
    public static T[] Make<T>(int n) => new T[n];                   // Г
    public static bool IsNull<T>(T v) => v is null;                 // Д
    public static string Name<T>() => typeof(T).Name;               // Е
    public static void Swap<T>(ref T a, ref T b) { (a, b) = (b, a); } // Ж
}
