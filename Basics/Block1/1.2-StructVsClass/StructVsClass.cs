namespace Basics.Block1;

// ТЕМА 1.2 — struct vs class: когда что выбирать
//
// Ниже 3 задачи. Реши их САМ, без подсказок.
// Перед каждым запуском впиши ПРЕДСКАЗАНИЕ (что выведется и почему).
// Когда закончишь — скажи, я проверю и код, и предсказания.
//
// Запуск:  dotnet run --project Basics -- 1.2

public static class StructVsClass
{
    // Мутирующий значимый тип — специально, чтобы поймать ловушку копий.
    public struct Counter
    {
        public int Value;
        public void Increment() => Value++;
    }

    // Ссылочный аналог.
    public class CounterClass
    {
        public int Value;
        public void Increment() => Value++;
    }

    public static void Run()
    {
        Task1();
        Task2();
        Task3();
    }

    // ── ЗАДАЧА 1: копия при чтении из коллекции ───────────────────────────
    // Дано: массив из ДВУХ Counter, у обоих Value = 0.
    // 1) Пройди по массиву обычным for и для каждого элемента ВЫЗОВИ arr[i].Increment().
    // 2) Выведи Value обоих элементов после цикла.
    // 3) Затем сделай то же самое с массивом CounterClass (foreach или for) и выведи Value.
    //
    // Подсказка по механике: подумай, что делает arr[i].Increment() для struct —
    // на чём именно вызывается метод.
    //
    // ПРЕДСКАЗАНИЕ (struct): arr[i].Increment() вызывается на копии элемента, поэтому Value не изменится.
    // ПРЕДСКАЗАНИЕ (class):  arr[i].Increment() вызывается на ссылке на объекте, соответственно Value изменится для каждого элемента на 1.
    private static void Task1()
    {
        Console.WriteLine("--- Задача 1 ---");

        var arrStruct = new[] {new Counter{Value = 0}, new Counter{Value = 0}};

        for (int i = 0; i < arrStruct.Length; i++)
            arrStruct[i].Increment();

        foreach (var item in arrStruct)
            Console.WriteLine(item.Value);

        var arrClass = new[] { new CounterClass { Value = 0 }, new CounterClass { Value = 0 } };

        for (int i = 0; i < arrClass.Length; i++)
            arrClass[i].Increment();

        foreach (var item in arrClass)
            Console.WriteLine(item.Value);
    }

    // ── ЗАДАЧА 2: значение по умолчанию (default), null, Nullable ─────────
    // 1) Выведи default(Counter).Value.
    // 2) Заведи переменную CounterClass? c = null; выведи, равна ли она null.
    // 3) Заведи Counter? nc = null; (Nullable<Counter>) — выведи nc.HasValue.
    //    Потом присвой nc = new Counter { Value = 5 }; и выведи nc.Value.
    //
    // Вопрос на подумать (ответь текстом в комментарии): почему у Counter
    // не бывает состояния "не создан", а у CounterClass бывает?
    //
    // ПРЕДСКАЗАНИЕ: У Counter не бывает состояния не создан, так как он является значимым типом и всегда имеет значение по умолчанию.
    // Как я для себя понял, значимый тип всегда имеет какое то значение.
    // У CounterClass может быть null так как это ссылочный тип и он может не указывать на объект в памяти, например, когда он не создан.
    private static void Task2()
    {
        Console.WriteLine("--- Задача 2 ---");

        Console.WriteLine($"default(Counter).Value: {default(Counter).Value}");

        CounterClass? c = null;
        Console.WriteLine($"CounterClass? c = null: {c == null}");

        Counter? nc = null;
        Console.WriteLine($"Counter? nc = null: {nc == null}");

        nc = new Counter { Value = 5 };
        Console.WriteLine($"nc.HasValue: {nc.HasValue}");
    }

    // ── ЗАДАЧА 3: равенство по умолчанию ──────────────────────────────────
    // Для struct:
    //   var a = new Counter { Value = 1 };
    //   var b = new Counter { Value = 1 };
    //   Выведи a.Equals(b).
    // Для class:
    //   var x = new CounterClass { Value = 1 };
    //   var y = new CounterClass { Value = 1 };
    //   var z = x;
    //   Выведи x.Equals(y) и x.Equals(z).
    //
    // ПРЕДСКАЗАНИЕ (a.Equals(b)):  Так как мы сравниваем два разных экзмепляра структуры,
    // то сравнение идет по значениям полей. В данном случае значения одинаковые и, соответственно равенство будет true.
    // ПРЕДСКАЗАНИЕ (x.Equals(y)):  Здесь у нас происходит сравнение ссылок, так как это экземпляры классов. Соответственно ссылки разные и равенства не будет.
    // ПРЕДСКАЗАНИЕ (x.Equals(z)):  Здесь равенство будет, так как мы присвоили ссылку x к z и теперь обе переменные указывают на один и тот же объект куче.
    private static void Task3()
    {
        Console.WriteLine("--- Задача 3 ---");

        var a = new Counter { Value = 1 };
        var b = new Counter { Value = 1 };

        Console.WriteLine(a.Equals(b));

        var x = new CounterClass { Value = 1 };
        var y = new CounterClass { Value = 1 };
        var z = x;

        Console.WriteLine(x.Equals(y));
        Console.WriteLine(x.Equals(z));
    }
}
