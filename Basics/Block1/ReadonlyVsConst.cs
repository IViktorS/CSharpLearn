namespace Basics.Block1;

// ТЕМА 1.9 — readonly vs const
//
// 3 задачи. Реши САМ. Перед запуском впиши ПРЕДСКАЗАНИЕ.
// Часть проверок — закомментированные строки: раскомментируй → увидишь ошибку.
//
// Запуск:  dotnet run --project Basics -- 1.9

public static class ReadonlyVsConst
{
    // Для задачи 1: const (статик, компиляция).
    public class MathStuff
    {
        public const double Pi = 3.14159;

        // Почему так нельзя? (ответишь в задаче 1)
        // public const DateTime Now = DateTime.Now;
    }

    // Для задачи 2: readonly-поля экземпляра, задаются в конструкторе.
    public class Order
    {
        public readonly int Id;
        public readonly DateTime CreatedAt;

        public Order(int id)
        {
            Id = id;
            CreatedAt = DateTime.Now;
        }
    }

    // Для задачи 3: readonly ссылочного типа.
    public class Basket
    {
        public readonly List<string> Items = new();
    }

    public static void Run()
    {
        Task1();
        Task2();
        Task3();
    }

    // ── ЗАДАЧА 1: const статичен и известен на компиляции ─────────────────
    // 1) Выведи Pi, обратившись к нему ПРАВИЛЬНО — через тип или через экземпляр?
    //    (напиши рабочий вариант и выведи значение)
    // 2) В комментарии ответь: почему строка // public const DateTime Now = DateTime.Now;
    //    в классе MathStuff не скомпилировалась бы? Потому что она вычисляется в рантайме, а не при компиляции.
    //
    // ПРЕДСКАЗАНИЕ (как обращаемся к Pi — через тип/экземпляр и значение): Обращаемся через тип.
    private static void Task1()
    {
        Console.WriteLine("--- Задача 1 ---");

        Console.WriteLine($"MathStuff.Pi = {MathStuff.Pi}");
    }

    // ── ЗАДАЧА 2: readonly — своё у каждого экземпляра, менять нельзя ──────
    // 1) Создай order1 = new Order(1) и order2 = new Order(2).
    //    Выведи order1.Id и order2.Id.
    // 2) В комментарии ответь: почему Id у них разный, хотя поле readonly? Так как мы создаем разные экземпляры класса и в каждом задаем значение id
    // в конструкторе.
    // 3) Раскомментируй строку ниже — что скажет компилятор и почему?
    //    // order1.Id = 99; Скажет, что нельзя присвоить значение readonly полю вне конструктора или сеттера внутри класса.
    //
    // ПРЕДСКАЗАНИЕ (order1.Id / order2.Id):        order1.Id = 1, order2.Id = 2
    // ПРЕДСКАЗАНИЕ (скомпилируется ли order1.Id = 99): нет
    private static void Task2()
    {
        Console.WriteLine("--- Задача 2 ---");

        var order1 = new Order(1);
        var order2 = new Order(2);

        Console.WriteLine($"order1.Id = {order1.Id}");
        Console.WriteLine($"order2.Id = {order2.Id}");

    }

    // ── ЗАДАЧА 3: readonly ссылочного типа — ссылка vs содержимое ─────────
    // 1) Создай Basket, добавь в Items "apple" и "banana".
    //    Выведи количество элементов и сами элементы.
    // 2) Раскомментируй строку ниже — скомпилируется ли? Почему?
    //    // basket.Items = new List<string>(); Так как мы не можем переприсвоить ссылку на новый объект, так как поле Items объявлено как readonly.
    //    Но мы можем изменять содержимое списка, на который ссылается Items.
    //
    // ПРЕДСКАЗАНИЕ (кол-во и элементы):                     2 элемента: apple, banana
    // ПРЕДСКАЗАНИЕ (скомпилируется ли переприсваивание Items): нет
    private static void Task3()
    {
        Console.WriteLine("--- Задача 3 ---");

        var basket = new Basket();
        basket.Items.Add("apple");
        basket.Items.Add("banana");

        Console.WriteLine($"Количество элементов: {basket.Items.Count}");
        Console.WriteLine("Элементы:");
        foreach (var item in basket.Items)
        {
            Console.WriteLine($"  {item}");
        }
    }
}
