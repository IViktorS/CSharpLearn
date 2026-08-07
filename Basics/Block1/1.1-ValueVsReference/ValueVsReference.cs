namespace Basics.Block1;

// ТЕМА 1.1 — Value types vs reference types
//
// Ниже описаны 3 задачи. Реши их САМ, без подсказок.
// Когда закончишь — скажи, и я проверю (не только код, но и твои предсказания).
//
// Запуск:  dotnet run --project Basics -- 1.1

public static class ValueVsReference
{
    // Тип-значение и тип-ссылка с одинаковым полем — для сравнения поведения.
    public struct PointStruct
    {
        public int X;
    }

    public class PointClass
    {
        public int X;
    }

    public static void Run()
    {
        Task1();
        Task2();
        Task3();
    }

    // ── ЗАДАЧА 1 ─────────────────────────────────────────────────────────
    // Есть метод, который принимает объект и увеличивает его поле X на 1.
    // Реализуй ДВЕ версии: одну для PointStruct, одну для PointClass.
    // Затем в Task1: создай по одному экземпляру каждого (X = 10),
    // вызови соответствующий Increment, и выведи X ДО и ПОСЛЕ вызова.
    //
    // ПЕРЕД запуском впиши сюда предсказание (что выведется и почему):
    // ПРЕДСКАЗАНИЕ: PointStruct это значимы тип и находится в данном случае внутри класса. Значит хрантися в куче. При передаче в метод Increment создается копия.
    // соответственно при X++ значение X в методе Increment увеличится, но в исходном экземпляре PointStruct значение X останется прежним.
    // PointClass это ссылочный тип. Соответственно в метод Increment мы передаем ссылку на объект. В следствии чего при X++ мы меняем непосредстенно сам объект.
    // Значит изменится его значение X в исходном экземпляре PointClass.
    private static void Task1()
    {
        Console.WriteLine("--- Задача 1 ---");

        var pointStruct = new PointStruct { X = 10 };
        var pointClass = new PointClass { X = 10 };

        Console.WriteLine($"PointStruct значение до Increment: {pointStruct.X}");
        Console.WriteLine($"PointClass значение до Increment: {pointClass.X}");

        Increment(pointStruct);
        Console.WriteLine($"PointStruct значение после Increment: {pointStruct.X}");

        Increment(pointClass);
        Console.WriteLine($"PointClass значение после Increment: {pointClass.X}");
    }

    private static void Increment(PointStruct p)
    {
        p.X++;
    }

    private static void Increment(PointClass p)
    {
        p.X++;
    }

    // Реализуй эти два метода:
    // private static void Increment(PointStruct p) { ... }
    // private static void Increment(PointClass p)  { ... }

    // ── ЗАДАЧА 2 ─────────────────────────────────────────────────────────
    // Массивы — ссылочный тип. Напиши метод DoubleAll(int[] arr),
    // который умножает каждый элемент на 2.
    // В Task2: создай массив { 1, 2, 3 }, передай его в DoubleAll,
    // и выведи содержимое массива ПОСЛЕ вызова.
    //
    // ПРЕДСКАЗАНИЕ: Массив изменится, так как это ссылочный тип.
    // В метод DoubleAll мы передаем ссылку на этот масси в и внутри умножаем каждое значение на 2.
    // Соответственно после выхода из метода DoubleAll массив будет изменен.
    private static void Task2()
    {
        Console.WriteLine("--- Задача 2 ---");

        var arr = new[] { 1, 2, 3 };

        Console.WriteLine("Значения массива до передачи в метод DoubleAll");
        foreach (var item in arr)
            Console.WriteLine(item);

        DoubleAll(arr);

        Console.WriteLine("Значения массива после передачи в метод DoubleAll");
        foreach (var item in arr)
            Console.WriteLine(item);
    }

    private static void DoubleAll(int[] arr)
    {
        for (int i = 0; i < arr.Length; i++)
        {
            arr[i] *= 2;
        }
    }

    // ── ЗАДАЧА 3 (на понимание string) ───────────────────────────────────
    // Напиши метод TryAppend(string s), который внутри делает s = s + "!!!".
    // В Task3: заведи строку "hello", передай в TryAppend,
    // и выведи строку ПОСЛЕ вызова.
    //
    // ПРЕДСКАЗАНИЕ: Строка снаружи не изменится. Хоть это и ссылочный тип, но при присвоении s + "!!!" создается новая строка, а изночальная не меняется.
    private static void Task3()
    {
        Console.WriteLine("--- Задача 3 ---");
        
        var str = "hello";

        Console.WriteLine($"Строка до вызова TryAppend: {str}");
        TryAppend(str);
        Console.WriteLine($"Строка после вызова TryAppend: {str}");
    }

    private static void TryAppend(string s)
    {
        s = s + "!!!";
    }
}
