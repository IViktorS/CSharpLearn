namespace Basics.Block1;

// 1.14 Pattern matching — задачи
//
// Правила прежние: сначала заполняешь ПРЕДСКАЗАНИЕ, потом запускаешь.
// Строки, помеченные «раскомментируй», — сначала предскажи код ошибки,
// потом проверь и верни комментарий на место.
//
// Запуск:  dotnet run --project Basics -- 1.14

public static class PatternMatching
{
    public static void Run()
    {
        Task1();
        Task2();
        Task3();
    }

    private enum Status { Draft, Active, Closed }

    // ── ЗАДАЧА 1: switch-выражение, порядок и полнота ─────────────────────
    // Метод Label ниже НЕ имеет ветки `_`.
    //
    // ПРЕДСКАЗАНИЕ (1) какой warning выдаст компилятор на Label
    //                  (код и в чём претензия):
    // ПРЕДСКАЗАНИЕ (2) Label(Status.Active):
    // ПРЕДСКАЗАНИЕ (3) что произойдёт при вызове Label((Status)99)
    //                  и почему такое значение вообще может оказаться
    //                  в переменной типа Status:
    // ПРЕДСКАЗАНИЕ (4) код ошибки для метода Broken (строка А):
    //
    // ЗАДАНИЕ: после того как всё предскажешь и проверишь — почини Label
    // так, чтобы warning исчез, а (Status)99 не ронял программу.
    private static void Task1()
    {
        Console.WriteLine("--- Задача 1 ---");

        Console.WriteLine($"Label(Active) = {Label(Status.Active)}");

        // Строка Б — раскомментируй, посмотри, верни комментарий:
        // Console.WriteLine($"Label((Status)99) = {Label((Status)99)}");
    }

    private static string Label(Status s) => s switch
    {
        Status.Draft => "черновик",
        Status.Active => "активен",
        Status.Closed => "закрыт",
    };

    // Строка А — раскомментируй метод целиком, посмотри ошибку, верни комментарий:
    // private static string Broken(int n) => n switch
    // {
    //     _   => "любое",
    //     > 0 => "положительное",
    // };

    // ── ЗАДАЧА 2: is-паттерны и точность типа ─────────────────────────────
    // Здесь важно не только «что вернёт», но и ПОЧЕМУ — по какому типу
    // принимается решение.
    //
    // ПРЕДСКАЗАНИЕ (1) a is long:
    // ПРЕДСКАЗАНИЕ (2) b is long:
    // ПРЕДСКАЗАНИЕ (3) a is int and > 3:
    // ПРЕДСКАЗАНИЕ (4) c is null / c is int:
    // ПРЕДСКАЗАНИЕ (5) что выведет ветка с d (и что окажется в str):
    // ПРЕДСКАЗАНИЕ (6) код ошибки на строке В и объяснение, почему
    //                  компилятор вообще не может это скомпилировать:
    private static void Task2()
    {
        Console.WriteLine("--- Задача 2 ---");

        object a = 5;
        object b = 5L;
        int? c = null;
        object d = "hello";

        Console.WriteLine($"a is long        : {a is long}");
        Console.WriteLine($"b is long        : {b is long}");
        Console.WriteLine($"a is int and > 3 : {a is int and > 3}");
        Console.WriteLine($"c is null        : {c is null}");
        Console.WriteLine($"c is int         : {c is int}");

        if (d is string { Length: > 3 } str)
            Console.WriteLine($"длинная строка   : {str.ToUpper()}");
        else
            Console.WriteLine("не подошло");

        // Строка В — раскомментируй, посмотри ошибку, верни комментарий:
        // Console.WriteLine($"d is {{ Length: 5 }} : {d is { Length: 5 }}");
    }

    // Мини-справка к задаче 3: тип может ПЕРЕГРУЗИТЬ операторы == и != —
    // объявить собственный метод `public static bool operator ==(...)`,
    // который вызывается вместо стандартного сравнения. Именно так устроен
    // string (поэтому `==` на строках сравнивает содержимое). Подробно
    // перегрузка операторов и правильное переопределение равенства — тема 2.6;
    // здесь достаточно знать, что такой метод бывает и его пишет автор типа.
    //
    // `#pragma warning disable` глушит предупреждение о том, что вместе с ==
    // положено переопределять ещё пару методов, — это тоже тема 2.6.

#pragma warning disable 660, 661
    private class Doc
    {
        public int Value;

        // Автор типа решил: документ с Value == 0 — это «пустой документ»,
        // и он должен считаться равным null.
        public static bool operator ==(Doc? left, Doc? right)
        {
            if (ReferenceEquals(left, null))
                return ReferenceEquals(right, null) || right!.Value == 0;
            if (ReferenceEquals(right, null))
                return left.Value == 0;
            return left.Value == right.Value;
        }

        public static bool operator !=(Doc? left, Doc? right) => !(left == right);
    }
#pragma warning restore 660, 661

    // ── ЗАДАЧА 3: != null против is not null ──────────────────────────────
    // Здесь «очевидный» ответ неверен как минимум в одной строке.
    // Объект `doc` создан через new — он совершенно точно существует.
    //
    // ПРЕДСКАЗАНИЕ (1) doc != null:
    // ПРЕДСКАЗАНИЕ (2) doc is not null:
    // ПРЕДСКАЗАНИЕ (3) od != null            (od — ТОТ ЖЕ объект, но object):
    // ПРЕДСКАЗАНИЕ (4) od is not null:
    // ПРЕДСКАЗАНИЕ (5) если (1) и (3) разошлись — что именно изменилось,
    //                  раз объект один и тот же?
    // ПРЕДСКАЗАНИЕ (6) код ошибки на строке Г:
    //
    // ЗАДАНИЕ: сформулируй в комментарии правило — когда `!= null` нельзя
    // доверять и почему `is not null` от этой проблемы свободен.
    private static void Task3()
    {
        Console.WriteLine("--- Задача 3 ---");

        var doc = new Doc { Value = 0 };
        object od = doc;

        Console.WriteLine($"doc != null      : {doc != null}");
        Console.WriteLine($"doc is not null  : {doc is not null}");
        Console.WriteLine($"od != null       : {od != null}");
        Console.WriteLine($"od is not null   : {od is not null}");

        // Строка Г — раскомментируй, посмотри ошибку, верни комментарий:
        // if (od is int n or long) Console.WriteLine(n);
    }
}
