using System.Collections;

namespace Basics.Block1;

// ТЕМА 1.4 — Boxing / Unboxing
//
// 3 задачи. Реши САМ. Перед запуском впиши ПРЕДСКАЗАНИЕ.
// Когда закончишь — скажи, проверю код и предсказания.
//
// Запуск:  dotnet run --project Basics -- 1.4

public static class BoxingUnboxing
{
    public static void Run()
    {
        Task1();
        Task2();
        Task3();
    }

    // ── ЗАДАЧА 1: коробка — независимая копия ─────────────────────────────
    // 1) int n = 42;
    // 2) object o = n;   // boxing
    // 3) n = 100;        // меняем ОРИГИНАЛ
    // 4) Выведи n и (int)o.
    //
    // ПРЕДСКАЗАНИЕ (n):      n будет равно 100.
    // ПРЕДСКАЗАНИЕ ((int)o): Будет равно 42, так как в object o лежит копия значени n на момент упаковки.
    private static void Task1()
    {
        Console.WriteLine("--- Задача 1 ---");

        int n = 42;
        object o = n;
        n = 100;

        Console.WriteLine($"n = {n}");
        Console.WriteLine($"(int)o = {(int)o}");
    }

    // ── ЗАДАЧА 2: unboxing требует ТОЧНОГО совпадения типа ────────────────
    // 1) object o = 11.11;   // в коробке лежит double
    // 2) Попробуй int x = (int)o; ВНУТРИ try/catch — поймай нужное исключение,
    //    выведи его тип (ex.GetType().Name). (без try/catch программа упадёт)
    // 3) Теперь распакуй правильно: int y = (int)(double)o; выведи y.
    // 4) Безопасный вариант: проверь o is int (ожидаемо false),
    //    и o is double d (ожидаемо true) — выведи оба результата.
    //
    // ПРЕДСКАЗАНИЕ (что произойдёт на (int)o и почему): Мы получим InvalidCastException, так как в рантайме проверяется строго, лежит ли в o double.
    // ПРЕДСКАЗАНИЕ (y):            y = 11. InvalidCastException не произойдет, так как сперва мы делаем unboxing к (double), а уже получившееся значение кастуем к  int,
    // в этом случае у нас отпадет часть после запятой, так как int это целое число без дробной части.
    // ПРЕДСКАЗАНИЕ (o is int):     Проверка покажет false.
    // ПРЕДСКАЗАНИЕ (o is double):  Проверка покажет true.
    private static void Task2()
    {
        Console.WriteLine("--- Задача 2 ---");

        object o = 11.11;

        try
        {
            int x = (int)o;
        }
        catch (Exception ex)
        {
            Console.WriteLine($"Тип исключения: {ex.GetType().Name}");
        }

        int y = (int)(double)o;
        Console.WriteLine($"y = {y}");

        if(o is int)
            Console.WriteLine("o is int: true");
        else
            Console.WriteLine("o is int: false");

        if(o is double d)
            Console.WriteLine($"o is double: true, значение: {d}");
        else
            Console.WriteLine("o is double: false");
    }

    // ── ЗАДАЧА 3: цена boxing — снова меряем GC ───────────────────────────
    // N = 5_000_000.
    // Сценарий А (boxing): ArrayList list = new(); в цикле list.Add(i) — каждый
    //   int упаковывается. Замерь разницу GC.CollectionCount(0) вокруг цикла.
    // Сценарий Б (без boxing): List<int> list = new(); тот же цикл list.Add(i).
    //   Замерь разницу GC.CollectionCount(0).
    //
    // (для чистоты можешь задать ёмкость заранее или нет — попробуй без неё)
    //
    // ПРЕДСКАЗАНИЕ (где сборок больше и почему): Сборок больше будет в сценприи с boxing.
    // Так как каждый boxing создаст отдельный объект в куче, за которыми придется следить GC.
    // С List<int> такой проблемы не будет, так как он сразу хранит в себе значимые типы.
    private static void Task3()
    {
        Console.WriteLine("--- Задача 3 ---");

        var N = 5000000;

        var before = GC.CollectionCount(0);
        Console.WriteLine($"Сборок до: {before}");

        ArrayList list = [];
        for (var i = 0; i < N; i++)
            list.Add(i);

        var after = GC.CollectionCount(0);
        Console.WriteLine($"Сборок после: {after - before}");

        before = GC.CollectionCount(0);
        Console.WriteLine($"Сборок до: {before}");

        List<int> list2 = [];
        for (var i = 0; i < N; i++)
            list2.Add(i);

        after = GC.CollectionCount(0);
        Console.WriteLine($"Сборок после: {after - before}");
    }
}
