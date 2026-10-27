using System.Diagnostics;
using System.Reflection;

namespace Basics.Block1;

// ТЕМА 1.20 — Рефлексия: чтение метаданных, цена, когда не надо
//
// Конспект: 1.20-Reflection.md в этой же папке.
// Запуск:   dotnet run --project Basics -- 1.20
//
// Все три задачи — предсказания. Отдельного шага с вопросами нет.
//
// Члены классов в задачах намеренно НЕ те, что в таблицах конспекта:
// ответ надо вывести из механизма, а не найти в таблице.
//
// Сразу знаешь ответ без раздумий — скажи, заменю задачу.

public static class ReflectionBasics
{
    public static void Run()
    {
        Task1();
        Task2();
        Task3();
    }

    // Вспомогательное: выполняет действие и печатает «ок» или имя исключения.
    // Чёрный ящик, разбирать не нужно.
    private static string Try(Action action)
    {
        try { action(); return "ок"; }
        catch (Exception ex) { return ex.GetType().Name; }
    }

    // ── ЗАДАЧА 1 (ПРОВОКАЦИОННАЯ): что рефлексия может, а компилятор нет ──
    //
    // Внизу файла — класс Badge. У него шесть членов, и каждый чем-то
    // «защищён»: private, static, readonly, init, private set, const,
    // вычисляемое свойство.
    //
    // Код ниже пытается записать в каждый из них новое значение через
    // рефлексию. Обычным кодом ни одна из этих записей не скомпилировалась бы.
    //
    // По каждой строке предскажи: «ок» или исключение (какое — примерно,
    // своими словами, если не помнишь имени) — и какое значение напечатается.
    // Главное — ПОЧЕМУ.
    //
    // ПРЕДСКАЗАНИЕ (1) private readonly _code    → : ok, Code = 10. Метод GetField с флагом PrivateInstance может вернуть приватное поле. Запись значения также доступна в этом случае.
    // ПРЕДСКАЗАНИЕ (2) private static _counter   → : ok, Counter = 20. Метод GetField с флагом PrivateStatic может вернуть приватное статическое поле. Запись значения также доступна.
    // ПРЕДСКАЗАНИЕ (3) init-свойство Init        → : исключение, Init = 3. GetProperty возвращает свойство, а не поле. Соответственно все правила для свойства остаются в силе и init не позволит присвоить значение вне инициализации.
    // Поправка : init-свойство можно изменить через рефлексию, так что запись пройдет и Init = 30. У init и private set есть настоящий метод записи, а их ограничения проверяет только компилятор
    // ПРЕДСКАЗАНИЕ (4) private set PrivSet       → : исключение, PrivSet = 4. GetProperty возвращает свойство, а не поле. Соответственно все правила для свойства остаются в силе и private set не позволит присвоить значение вне класса.
    // Поправка : private set можно изменить через рефлексию, так что запись пройдет и PrivSet = 40. У init и private set есть настоящий метод записи, а их ограничения проверяет только компилятор
    // ПРЕДСКАЗАНИЕ (5) const Label               → : исключение, Label = "пропуск". Константа не может быть изменена после компиляции и это не какое то отдельное поле.
    // ПРЕДСКАЗАНИЕ (6) вычисляемое свойство Code → : исключение, Code = 10. Это вычисляемое свойство без сеттера.
    //
    // ПРЕДСКАЗАНИЕ (7) одной фразой: что общего у тех строк, где запись
    //                  прошла, и что общего у тех, где нет? Отличается тем, есть ли у члена реальный сеттер или поле, которое можно изменить.
    // ПРЕДСКАЗАНИЕ (8) в какой строке уверенность ниже всего? Номером. 6
    //
    private static void Task1()
    {
        Console.WriteLine("--- Задача 1 ---");

        var t = typeof(Badge);
        var b = new Badge();
        const BindingFlags PrivateInstance = BindingFlags.NonPublic | BindingFlags.Instance;
        const BindingFlags PrivateStatic = BindingFlags.NonPublic | BindingFlags.Static;

        Console.WriteLine($"(1) private readonly _code    -> {Try(() => t.GetField("_code", PrivateInstance)!.SetValue(b, 10))}, Code = {b.Code}");
        Console.WriteLine($"(2) private static _counter   -> {Try(() => t.GetField("_counter", PrivateStatic)!.SetValue(null, 20))}, Counter = {Badge.Counter}");
        Console.WriteLine($"(3) init-свойство Init        -> {Try(() => t.GetProperty("Init")!.SetValue(b, 30))}, Init = {b.Init}");
        Console.WriteLine($"(4) private set PrivSet       -> {Try(() => t.GetProperty("PrivSet")!.SetValue(b, 40))}, PrivSet = {b.PrivSet}");
        Console.WriteLine($"(5) const Label               -> {Try(() => t.GetField("Label")!.SetValue(null, "x"))}, Label = {Badge.Label}");
        Console.WriteLine($"(6) вычисляемое свойство Code -> {Try(() => t.GetProperty("Code")!.SetValue(b, 50))}, Code = {b.Code}");
    }

    // ── ЗАДАЧА 2: фактический тип и «все свойства объекта» ────────────────
    //
    // Часть А — предсказать три строки (код ниже уже написан).
    //
    // ПРЕДСКАЗАНИЕ (А1) object o = 3.14;      o.GetType().Name                 : double так как GetType возвращает фактическийй тип объекта.
    // ПРЕДСКАЗАНИЕ (А2) object s = "x";       typeof(object) == s.GetType()    : false, так как s.GetType() возвращает фактический тип объекта, а typeof(object) вернет object.
    // ПРЕДСКАЗАНИЕ (А3) int? n = 7;           n.GetType() == typeof(int?)      : false, так как n.GetType() возврщает фактический тип обекта и вернет int, а typeof(int?) вернет NUllable<int>.
    //
    // Часть Б — написать самому.
    //
    // Напиши метод Dump(object obj), который печатает объект в виде
    //     ИмяТипа { Свойство1 = значение1, Свойство2 = значение2 }
    // Бери свойства через obj.GetType().GetProperties() — без флагов.
    // Подсказка по форме: string.Join (1.12) склеит пары через запятую.
    //
    // Вызови его на объекте Gauge (класс внизу файла). Посмотри, какие у
    // Gauge члены: свойство, ПОЛЕ, закрытое свойство, вычисляемое свойство,
    // статическое свойство.
    //
    // ПРЕДСКАЗАНИЕ (Б1) какие члены Gauge попадут в вывод, а какие нет —
    //                   и почему для каждого из пяти: Title - публичное свойство, Show - публичное вычисляемое свойство, Instances - публичное статическое свойство попадут в вывод.
    //                   Соответственно их можно получить через GetProperties, так как метод возвращает публичные поля.   Value является полем и GetProperties его не вернет. Secret - приватное свойство и GetProperties его тоже не вернет.
    // ПРЕДСКАЗАНИЕ (Б2) ДО запуска: почему Dump принимает object, а не
    //                   обобщённый T — что дала бы здесь обобщённость (1.17)
    //                   и чего бы она не дала? Уверенности нет, но я предположу.
    // Dump должен принимать любой объект в принципе. T принимает строго типизированный объект.
    // Возможно обобщенность дала бы возможность использовать типизированные свойства, хотя тут они вроде бы и не нужны. 
    //
    private static void Task2()
    {
        Console.WriteLine("--- Задача 2 ---");

        object o = 3.14;
        object s = "x";
        int? n = 7;
        Console.WriteLine($"(А1) o.GetType().Name              = {o.GetType().Name}");
        Console.WriteLine($"(А2) typeof(object) == s.GetType() = {typeof(object) == s.GetType()}");
        Console.WriteLine($"(А3) n.GetType() == typeof(int?)   = {n.GetType() == typeof(int?)}");

        Dump(new Gauge());
    }

    private static void Dump(object obj)
    {
        var str = obj.GetType().Name;

        Console.WriteLine($"{str} {{{string.Join(", ", obj.GetType().GetProperties().Select(p => $"{p.Name} = {p.GetValue(obj)}"))}}}");
    }

    // ── ЗАДАЧА 3: сколько стоит ───────────────────────────────────────────
    //
    // Замер уже написан (метод Measure — чёрный ящик). У класса Meter три
    // члена: свойство double, свойство string и метод с ДВУМЯ аргументами int.
    //
    // В конспекте замерены int-свойство (24 байта на вызов) и метод с ОДНИМ
    // аргументом (80 байт). Здесь числа другие — выведи их из механизма.
    // Как раскладывать число по слагаемым — грабля 3 конспекта, «Разбор по
    // шагам» и «Правило подсчёта» (дописано 2026-10-05: раньше там стояли
    // только готовые цифры, и вывести из них новый случай было нельзя).
    // Размеры, если нужны: коробка для int или double — 24 байта; массив
    // object[] из N элементов — 24 + 8·N байт.
    //
    // ПРЕДСКАЗАНИЕ (1) байт на вызов, lv.GetValue(m), double : 24
    // ПРЕДСКАЗАНИЕ (2) байт на вызов, un.GetValue(m), string : 0
    // ПРЕДСКАЗАНИЕ (3) байт на вызов, sum.Invoke(m, { 1, 2 }) : 16 заголовок + 8 длина массива + 8*2 элементов массива = 40
    //                  разложи число по слагаемым — что из чего берётся
    // Поправка 16 заголовок + 8 длина массива + 8*2 элементов массива = 40 + 2*24 упаковка для каждого элемента в массиве + 24 результат упаковки на выходе.
    // ПРЕДСКАЗАНИЕ (4) какая из трёх строк по наносекундам окажется самой
    //                  дорогой и почему: 3 так как больше операций
    // ПРЕДСКАЗАНИЕ (5) в каком из предсказаний уверенность ниже всего? 3 и 2. Больше в 3.
    //
    private static void Task3()
    {
        Console.WriteLine("--- Задача 3 ---");

        const int N = 10_000_000;
        var m = new Meter();
        var level = typeof(Meter).GetProperty(nameof(Meter.Level))!;
        var unit = typeof(Meter).GetProperty(nameof(Meter.Unit))!;
        var sum = typeof(Meter).GetMethod(nameof(Meter.Sum))!;

        Measure("lv.GetValue(m)     double", N, k =>
        {
            long x = 0;

            for (int i = 0; i < k; i++)
            {
                x += (long)(double)level.GetValue(m)!;
            }

            return x;
        });
        Measure("un.GetValue(m)     string", N, k => { long x = 0; for (int i = 0; i < k; i++) x += ((string)unit.GetValue(m)!).Length; return x; });
        Measure("sum.Invoke(m, {1, 2})", N, k => { long x = 0; for (int i = 0; i < k; i++) x += (int)sum.Invoke(m, new object[] { 1, 2 })!; return x; });
    }

    // Замер: прогрев, затем N повторов; печатает наносекунды и байты на вызов.
    // Чёрный ящик, разбирать не нужно.
    private static void Measure(string label, int n, Func<int, long> body)
    {
        body(1000);
        GC.Collect();
        long before = GC.GetAllocatedBytesForCurrentThread();
        var sw = Stopwatch.StartNew();
        long r = body(n);
        sw.Stop();
        long after = GC.GetAllocatedBytesForCurrentThread();
        Console.WriteLine($"{label,-28} {sw.Elapsed.TotalMilliseconds * 1_000_000 / n,7:F2} нс/вызов   {(double)(after - before) / n,6:F1} байт/вызов   (r={r})");
    }

    // ── материал к задаче 1 ───────────────────────────────────────────────

    /// <summary>Шесть «защищённых» членов.</summary>
    public class Badge
    {
        /// <summary>Закрытое поле только для чтения.</summary>
        private readonly int _code = 1;

        /// <summary>Закрытое статическое поле (не readonly).</summary>
        private static int _counter = 2;

        /// <summary>Свойство, которое можно задать только при создании.</summary>
        public int Init { get; init; } = 3;

        /// <summary>Свойство с закрытым сеттером.</summary>
        public int PrivSet { get; private set; } = 4;

        /// <summary>Константа.</summary>
        public const string Label = "пропуск";

        /// <summary>Вычисляемое свойство — читает _code.</summary>
        public int Code => _code;

        /// <summary>Вычисляемое статическое свойство — читает _counter.</summary>
        public static int Counter => _counter;
    }

    // ── материал к задаче 2 ───────────────────────────────────────────────

    /// <summary>Пять членов разного рода — для Dump.</summary>
    public class Gauge
    {
        /// <summary>Обычное свойство.</summary>
        public string Title { get; set; } = "датчик";

        /// <summary>ПОЛЕ, не свойство.</summary>
        public double Value = 36.6;

        /// <summary>Закрытое свойство.</summary>
        private int Secret { get; set; } = 7;

        /// <summary>Вычисляемое свойство.</summary>
        public string Shown => $"{Title}: {Value}";

        /// <summary>Статическое свойство.</summary>
        public static int Instances { get; set; } = 1;
    }

    // ── материал к задаче 3 ───────────────────────────────────────────────

    /// <summary>Три члена для замера.</summary>
    public class Meter
    {
        /// <summary>Свойство значимого типа.</summary>
        public double Level { get; set; } = 1.5;

        /// <summary>Свойство ссылочного типа.</summary>
        public string Unit { get; set; } = "бар";

        /// <summary>Метод с двумя аргументами.</summary>
        public int Sum(int a, int b) => a + b;
    }
}
