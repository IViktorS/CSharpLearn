using System.Runtime.CompilerServices;
using System.Security.Cryptography.X509Certificates;

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
        Task4();
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
    // ПРЕДСКАЗАНИЕ (1) c1.Items.Count после всего: 2           (тема: 1.8)
    // ПРЕДСКАЗАНИЕ (2) Config.InstanceCount: 2                 (тема: 1.10)
    // ПРЕДСКАЗАНИЕ (3) две закомментированные строки в конце — какие коды
    //                  ошибок компиляции и почему:
    //                  строка А: CS0119 по тому что поле Items является readonly, значение ему можно присвоить или в самом классе или в конструкторе класса                            (тема: 1.8)
    //                  строка Б: CS0176 нельзя обратиться к статическойц переменной по ссылке , только через тип                              (тема: 1.9)
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
        //c1.Items = new List<string>();

        // Строка Б — то же самое:
        //Console.WriteLine(c1.Version);
    }

    private enum Status { Draft, Active, Closed }

    // ── ЗАДАЧА 2: коробки, enum и null ────────────────────────────────────
    // Тоже готовый код. Здесь есть строка, где «очевидный» ответ неверен.
    //
    // ПРЕДСКАЗАНИЕ (1) unboxed: 42                    (тема: 1.4)
    // ПРЕДСКАЗАНИЕ (2) s и Enum.IsDefined: s = 99, Enum.IsDefined = false       (тема: 1.7)
    // ПРЕДСКАЗАНИЕ (3) n после двух ??=: 5          (тема: 1.5)
    // ПРЕДСКАЗАНИЕ (4) b1 == b2: true                  (тема: 1.1)
    // ПРЕДСКАЗАНИЕ (5) b1.Equals(b2): false             (тема: 1.1)
    // ПРЕДСКАЗАНИЕ (6) что произойдёт, если раскомментировать строку В: Тут я не уверен, но думаю, что будет ошибка каста, так как в boxed лежит int, а мы  строго проверяем можно ли сделать unboxing в long.
    // Сработало бы, если написать long asLong = (long)(int)boxed; Сперва бы мы сделали unboxing, а потом сделали каст к long.
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
    //    Способ 1 — не меняя тип Counter. этот вариант я увидел на сайте https://learn.microsoft.com/, когда смотрел расшифровку ошибки, так что не совсем сам вспомнил.
    //    Способ 2 — поменяв структуру данных или тип. В комментарии объясни,
    //    почему теперь работает. Так как массив дает доступ напрямую к месту хранения этого значения, а не его копию, насколько помню.
    // 3) Напиши private static метод Bump, который принимает List<Counter>
    //    и индекс, увеличивает элемент и возвращает КОРТЕЖ (int Before, int After).
    //    Вызови его и деконструируй результат.
    //
    // ПРЕДСКАЗАНИЕ (код ошибки на list[0].Value++):CS1612 мы не можем таким образом изменить значение поля структуры, так в List[0] мы пытаемся обратиться к скопированному значению.
    // ПРЕДСКАЗАНИЕ (Before / After):1 / 2
    private static void Task3()
    {
        Console.WriteLine("--- Задача 3 ---");

        var list = new List<Counter> { new Counter { Value = 1 } };

        //list[0].Value++;

        Counter counter = list[0];
        counter.Value++;
        list[0] = counter;

        var arr = new []{ new Counter { Value = 1 } };
        arr[0].Value++;

        var newlist = new List<Counter> { new Counter { Value = 1 } };
        var bump = Bump(newlist, 0);

        var (before, after) = bump;

        Console.WriteLine($"bump.Before: {before}");
        Console.WriteLine($"bump.After: {after}");
    }

    private static (int Before, int After) Bump(List<Counter> list, int index)
    {
        var before = list[index].Value;

        Counter counter = list[index];
        counter.Value++;
        list[index] = counter;

        return (before, list[index].Value);
    }

    // ── ЗАДАЧА 4: разбор ошибки по == / Equals ────────────────────────────
    // Добавлена после проверки: в задаче 2 предсказания (4) и (5) оказались
    // перевёрнуты. Цель — не повторить правильный ответ, а увидеть, где
    // прежняя модель даёт заведомо неверный результат.
    //
    // Код ниже уже написан, раскомментируй его и предскажи КАЖДУЮ строку
    // ДО запуска. Обрати внимание: в блоке Б и в блоке В участвуют
    // ОДНИ И ТЕ ЖЕ два объекта — меняется только объявленный тип переменной.
    //
    // ПРЕДСКАЗАНИЕ (1) s1 == s2: true
    // ПРЕДСКАЗАНИЕ (2) o1 == o2: false
    // ПРЕДСКАЗАНИЕ (3) o1.Equals(o2): true
    // ПРЕДСКАЗАНИЕ (4) x == y: true
    //
    // И один вопрос словами, ответ пиши здесь же:
    // ПРЕДСКАЗАНИЕ (5) предсказания (1) и (2) — про одну и ту же пару объектов.
    //                  Если они разошлись, то ЧТО именно изменилось между
    //                  строками, раз объекты те же самые? Пока у объектов был тип string, оператор == был перегружен под данный тип и сравнивал по значению, а когда мы сравниваем object, == уже по ссылкам.
    private static void Task4()
    {
        Console.WriteLine("--- Задача 4 ---");

        // Блок А: две строки с одинаковым содержимым, но собранные по-разному,
        // чтобы вторая гарантированно не была тем же объектом, что первая.
        string s1 = "hello";
        string s2 = new string(new[] { 'h', 'e', 'l', 'l', 'o' });

        // Блок Б: сравниваем как string
        Console.WriteLine($"s1 == s2        : {s1 == s2}");

        // Блок В: те же два объекта, но переменные объявлены как object
        object o1 = s1;
        object o2 = s2;
        Console.WriteLine($"o1 == o2        : {o1 == o2}");
        Console.WriteLine($"o1.Equals(o2)   : {o1.Equals(o2)}");

        // Блок Г: одна коробка, две ссылки на неё
        object x = 42;
        object y = x;
        Console.WriteLine($"x == y          : {x == y}");
    }
}
