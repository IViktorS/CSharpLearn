namespace Basics.Block1;

// ТЕМА 1.11 — out / ref / in параметры
//
// 3 задачи. Реши САМ. Перед запуском впиши ПРЕДСКАЗАНИЕ.
// Запуск:  dotnet run --project Basics -- 1.11

public static class RefOutIn
{
    // Большой значимый тип — чтобы показать смысл in (не копировать).
    public struct BigStruct
    {
        public double A, B, C, D;
        public double Sum => A + B + C + D;
    }

    public static void Run()
    {
        Task1();
        Task2();
        Task3();
    }

    // ── ЗАДАЧА 1: ref — обмен значениями (классика) ───────────────────────
    // 1) Напиши метод Swap(ref int a, ref int b), который меняет их местами.
    // 2) Заведи x = 1, y = 2; выведи до обмена.
    // 3) Вызови Swap(ref x, ref y); выведи после.
    //
    // ПРЕДСКАЗАНИЕ (x, y после Swap): x = 2, y = 1
    private static void Task1()
    {
        Console.WriteLine("--- Задача 1 ---");

        var x = 1;
        var y = 2;

        Swap(ref x, ref y);

        Console.WriteLine($"x = {x}\ny = {y}");
    }

    public static void Swap(ref int a, ref int b) => (b, a) = (a, b);

    // ── ЗАДАЧА 2: out — вернуть «удалось + результат» ─────────────────────
    // 1) Напиши метод bool TryDivide(int a, int b, out int result):
    //    если b == 0 — result = 0, вернуть false; иначе result = a / b, вернуть true.
    // 2) Вызови для (10, 2) и (10, 0). Для каждого выведи и возвращённый bool, и result.
    //
    // ПРЕДСКАЗАНИЕ (10 / 2 — bool и result): true, 5
    // ПРЕДСКАЗАНИЕ (10 / 0 — bool и result): false, 0
    private static void Task2()
    {
        Console.WriteLine("--- Задача 2 ---");

        int result;

        var tryDivide = TryDivide(10, 2, out result);
        Console.WriteLine($"tryDivide = {tryDivide}, result = {result}");

        tryDivide = TryDivide(10, 0, out result);
        Console.WriteLine($"tryDivide = {tryDivide}, result = {result}");
    }

    public static bool TryDivide(int a, int b, out int result)
    {
        if (b == 0)
        {
            result = 0;
            return false;
        }

        result = a / b;
        return true;
    }

    // ── ЗАДАЧА 3: ref со строкой (возврат к теме 1.1) + in ────────────────
    // Часть А:
    //   Напиши метод Append(ref string s), который делает s = s + "!!!".
    //   Заведи str = "hello", вызови Append(ref str), выведи str.
    //   (Сравни с TryAppend из темы 1.1, где БЕЗ ref строка не менялась.)
    //
    // Часть Б (in):
    //   Напиши метод double SumOf(in BigStruct s), который возвращает s.Sum
    //   (менять s внутри нельзя — попробуй в комментарии объяснить почему). Нельзя менять, так как метод испольщует параметр in, который передает метод
    //   по ссылке, но не дает менять внутри
    //   Создай BigStruct { A=1, B=2, C=3, D=4 }, вызови SumOf, выведи результат.
    //
    // ПРЕДСКАЗАНИЕ (str после Append): hello!!!
    // ПРЕДСКАЗАНИЕ (SumOf для {1,2,3,4}): 10
    private static void Task3()
    {
        Console.WriteLine("--- Задача 3 ---");

        var str = "hello";

        Append(ref str);
        Console.WriteLine($"str {str}");

        var big = new BigStruct { A = 1, B = 2, C = 3, D = 4 };
        Console.WriteLine($"bigSumOf: {SumOf(big)}");
    }

    public static void Append(ref string s)
    {
        s = s + "!!!";
    }

    public static double SumOf(in BigStruct s)
    {
        return s.Sum;
    }
}
