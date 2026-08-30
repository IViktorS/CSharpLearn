namespace Basics.Block1;

// ТЕМА 1.13 — Кортежи (tuples) и деконструкция
//
// 3 задачи. Реши САМ. Перед запуском впиши ПРЕДСКАЗАНИЕ.
// Запуск:  dotnet run --project Basics -- 1.13

public static class Tuples
{
    public static void Run()
    {
        Task1();
        Task2();
        Task3();
    }

    // ── ЗАДАЧА 1: метод, возвращающий кортеж + деконструкция ──────────────
    // 1) Ниже объяви private static метод Stats, который принимает int[] и
    //    возвращает кортеж из ТРЁХ именованных элементов: Min, Max, Sum.
    // 2) Дан массив int[] data = { 7, -3, 12, 5 }.
    // 3) Вызови Stats и деконструируй результат в три переменные через var (...).
    //    Выведи их.
    // 4) Вызови Stats ещё раз, но возьми ТОЛЬКО Sum — остальные два элемента
    //    выброси через discard. Выведи сумму.
    // 5) Вызови Stats в третий раз БЕЗ деконструкции — присвой в одну переменную
    //    и обратись к элементам по именам. Выведи Max.
    //
    // ПРЕДСКАЗАНИЕ (Min / Max / Sum): Min = -3, Max = 12, Sum = 21
    // ПРЕДСКАЗАНИЕ (сколько раз выполнится тело Stats за задачу): 3
    private static void Task1()
    {
        Console.WriteLine("--- Задача 1 ---");

        int[] data = { 7, -3, 12, 5 };

        var (min, max, sum) = Stats(data);
        Console.WriteLine(
            $"min = {min}\n " +
            $"max = {max}\n " +
            $"sum = {sum}");

        var(_,_,sum2) = Stats(data);
        Console.WriteLine($"sum2 = {sum2}");

        var result = Stats(data);
        Console.WriteLine(
            $"min = {result.Min}\n " +
            $"max = {result.Max}\n " +
            $"sum = {result.Sum}");
    }
    private static (int Min, int Max, int Sum) Stats(int[] arr)
    {
        var min = arr[0];
        var max = arr[0];
        var sum = 0;

        foreach (var item in arr)
        {
            if (item < min)
                min = item;

            if (item > max)
                max = item;

            sum += item;
        }

        return (min, max, sum); 
    }

    // ── ЗАДАЧА 2: кортеж — это struct ─────────────────────────────────────
    // 1) var a = (Id: 1, Name: "первый");
    //    var b = a;
    //    b.Id = 99;
    //    Выведи a.Id и b.Id.
    // 2) Имена — сахар компилятора. Объяви (int Code, string Text) c = a;
    //    (да, присваивание из a — с другими именами элементов).
    //    Выведи c.Code и c.Text. В комментарии ответь: почему это компилируется? типов не два, а один: оба ValueTuple<int, string>
    // 3) Выведи Console.WriteLine(a) — просто сам кортеж. Посмотри, что напечатается.
    // 4) Равенство: выведи результат сравнения a == c.
    // 5) Массив против списка. Создай:
    //        var arr  = new[] { (X: 1, Y: 2), (X: 3, Y: 4) };
    //        var list = new List<(int X, int Y)> { (1, 2), (3, 4) };
    //    Попробуй написать arr[0].X = 100;  и  list[0].X = 100;
    //    Одна из строк не скомпилируется — закомментируй её и в комментарии
    //    напиши код ошибки и причину.
    //
    // ПРЕДСКАЗАНИЕ (a.Id / b.Id): a.Id = 1, b.Id = 99 
    // ПРЕДСКАЗАНИЕ (что напечатает Console.WriteLine(a)):(1,первый)
    // ПРЕДСКАЗАНИЕ (a == c): true
    // ПРЕДСКАЗАНИЕ (какая из двух строк не скомпилируется): описано ниже в методе
    private static void Task2()
    {
        Console.WriteLine("--- Задача 2 ---");

        var a = (Id: 1, Name: "первый");
        var b = a;
        b.Id = 99;

        Console.WriteLine($"a.Id = {a.Id}");
        Console.WriteLine($"b.Id = {b.Id}");

        (int Code, string Text) c = a;
        Console.WriteLine($"c.Code = {c.Code}");
        Console.WriteLine($"c.Text = {c.Text}");

        Console.WriteLine(a);

        Console.WriteLine($"a == c : {a == c}");

        var arr = new[] { (X: 1, Y: 2), (X: 3, Y: 4) };
        var list = new List<(int X, int Y)> { (1, 2), (3, 4) };

        arr[0].X = 100;
        // list[0].X = 100; CS1612 так как тут нам вернулась копия, которой мы пытаемся присвоить значение.
    }

    // ── ЗАДАЧА 3: свой Deconstruct + порядок вычисления ───────────────────
    // 1) Ниже (за пределами метода, но внутри файла) объяви класс Rectangle
    //    со свойствами Width и Height (get/init) и конструктором.
    //    Добавь ему метод Deconstruct на два out-параметра.
    //    Добавь ВТОРУЮ перегрузку Deconstruct на три out-параметра:
    //    ширина, высота и площадь.
    // 2) Создай Rectangle(3, 4). Деконструируй его сначала в две переменные,
    //    потом в три. Выведи результаты.
    //    В комментарии ответь: как компилятор выбирает нужную перегрузку? Компилятор выбирает по числу переменных слева.
    // 3) Порядок вычисления. Объяви int x = 1, y = 2, z = 3;
    //    Выполни одной строкой циклический сдвиг: (x, y, z) = (z, x, y);
    //    Выведи x, y, z.
    //
    // ПРЕДСКАЗАНИЕ (деконструкция на 3 переменные): width1 =3, height1 = 4, square = 12
    // ПРЕДСКАЗАНИЕ (x / y / z после сдвига): z = 3, x = 1, y = 2
    private static void Task3()
    {
        Console.WriteLine("--- Задача 3 ---");

        var rectangle = new Rectangle(3, 4);

        var (width, height) = rectangle;
        var (width1, height1, square) = rectangle;

        Console.WriteLine(
            $"width = {width}\n " +
            $"height = {height}");

        Console.WriteLine(
            $"width1 = {width1}\n " +
            $"height1 = {height1}\n " +
            $"square = {square}");

        int x = 1, y = 2, z = 3;
        (x, y, z) = (z, x, y);
        Console.WriteLine(
            $"x = {x}\n " +
            $"y = {y}\n " +
            $"z = {z}");
    }

    private class Rectangle
    {
        public int Width {  get; init; }
        public int Height { get; init; }

        public Rectangle(int width, int height)
        {
            Width = width;
            Height = height;
        }

        public void Deconstruct (out int width, out int height)
        {
            width = Width;
            height = Height;
        }

        public void Deconstruct(out int width, out int height, out int square)
        {
            width = Width;
            height = Height;
            square = Width * Height;
        }
    }
}
