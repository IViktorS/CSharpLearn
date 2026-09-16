using System.Text;

namespace Basics.Block1;

// ТЕМА 1.16 — Методы расширения
//
// Режим ускоренный: отдельного шага с вопросами нет, предсказания его
// заменяют. Сразу знаешь ответ без раздумий — скажи, заменю задачу.
//
// Запуск:  dotnet run --project Basics -- 1.16

public static class ExtensionMethods
{
    public static void Run()
    {
        Task1();
        Task2();
        Task3();
    }

    // ── ЗАДАЧА 1: написать два расширения ─────────────────────────────────
    // Напиши в статическом классе MyExtensions (он ниже, пустой) два метода:
    //
    //   1) Truncate(this string s, int max) — если длина больше max,
    //      вернуть первые max символов, иначе исходную строку.
    //      Про null реши сам и объясни решение в комментарии.
    //
    //   2) Repeat(this int n, string text) — вернуть text, повторённый n раз.
    //      Внутри используй то, что подходит для склейки в цикле (тема 1.12).
    //
    // Вызови оба ТОЧЕЧНОЙ записью и выведи результат.
    //
    // ПРЕДСКАЗАНИЕ (1) "Привет, мир".Truncate(6): Привет
    // ПРЕДСКАЗАНИЕ (2) 3.Repeat("ля"): ляляля
    // ПРЕДСКАЗАНИЕ (3) законна ли запись MyExtensions.Truncate("абвгд", 3) —
    //                  то есть вызов того же метода как обычного статического?
    //                  Почему? Законна. Компилятор сразу видит, откуда вызывать метод и искать его не нужно.
    private static void Task1()
    {
        Console.WriteLine("--- Задача 1 ---");

        Console.WriteLine("Привет, мир".Truncate(6));

        Console.WriteLine(3.Repeat("ля"));
    }

    // ── ЗАДАЧА 2 (ПРОВОКАЦИОННАЯ): кто побеждает и что с null ─────────────
    // В КОНЦЕ ФАЙЛА объявлен класс ExtensionMethodsBox со СВОИМ методом
    // Name(), а в классе DemoExtensions рядом с ним — расширение Name()
    // для того же типа и с той же сигнатурой.
    //
    // Оба «очевидных» ответа здесь неверны. Заполни предсказания ДО запуска.
    //
    // ПРЕДСКАЗАНИЕ (1) что напечатает new ExtensionMethodsBox().Name() —
    //                  EXTENSION? И будет ли предупреждение при сборке
    //                  о том, что два метода конфликтуют? Напечатает INSTANCE. Так как метод экземпляра имеет приоритет над методом расширения.
    // ПРЕДСКАЗАНИЕ (2) строка nil равна null. Что произойдёт на nil.Tag()?
    //                  Если считаешь, что упадёт — назови исключение
    //                  и строку, на которой это случится: ошибки не будет, вернется строка EXT:<null>
    // ПРЕДСКАЗАНИЕ (3) если (2) НЕ падает — объясни почему, опираясь
    //                  на то, во что компилятор переписывает вызов: вызов будет выглядеть как DemoExtensions.Tag(nil) и расширение тело метода не трогает. По этому и не упадет до момента обращения к переменной внутри, если нет проверки.
    // ПРЕДСКАЗАНИЕ (4) практический вывод: чем это опасно при отладке,
    //                  если расширение всё-таки обратится к s.Length внутри? если обратится к s.Length, то метод упадет и стектрейс будет указывать внутрь расширения, а не там, где его вызвали.
    
    private static void Task2()
    {
        Console.WriteLine("--- Задача 2 ---");

        Console.WriteLine($"ExtensionMethodsBox().Name() : {new ExtensionMethodsBox().Name()}");

        string nil = null!;
        Console.WriteLine($"nil.Tag()        : {nil.Tag()}");
    }

    // ── ЗАДАЧА 3: по какому типу выбирается метод ─────────────────────────
    // Ниже ДВА расширения с одинаковым именем Describe — одно для string,
    // другое для object. Строка одна и та же, переменные разные.
    //
    // ПРЕДСКАЗАНИЕ (1) "абв".Describe(): string-ext
    // ПРЕДСКАЗАНИЕ (2) asObj.Describe(), где asObj — та же строка,
    //                  но переменная объявлена как object: object-ext
    // ПРЕДСКАЗАНИЕ (3) если (1) и (2) разошлись — что именно решило выбор,
    //                  и с каким уже разобранным механизмом это совпадает?
    //                  (подсказка-напоминание: «оператор спрашивает
    //                  переменную, метод спрашивает объект» — куда в этой
    //                  формуле попадает метод расширения и почему?) Потому что метод выбирает по объявленному типу переменной.
    // Метод расширения спрашивает переменную.
    // ПРЕДСКАЗАНИЕ (4) 7.Describe() — какой из двух методов вызовется
    //                  и что при этом произойдёт со значением 7? вызовется метод Describe(this object o). Вот что с 7 произойдет я не уверен.
    // Изначально у 7 тип int. По идее с ней ничего не произойдет. Расширение спрашивает, какой у 7 тип, что бы вызвать нужный метод.
    // Исправление. 7 будет упакована в коробку (boxing) и передана в метод Describe(this object o).
    // Внутри метода будет доступ к объекту, который содержит значение 7.
    private static void Task3()
    {
        Console.WriteLine("--- Задача 3 ---");

        Console.WriteLine($"\"абв\".Describe() : {"абв".Describe()}");
        
        object asObj = "абв";
        Console.WriteLine($"asObj.Describe()  : {asObj.Describe()}");

        Console.WriteLine($"7.Describe()      : {7.Describe()}");
    }

}

// Твой класс для задачи 1.
public static class MyExtensions
{
    public static string Truncate(this string s, int max)
    {
        if (s == null)
        {
            // Решение: если строка null, возвращаем null.
            // Это позволяет избежать NullReferenceException при вызове метода на null.
            return null!;
        }
        return s.Length > max ? s[..max] : s;
    }

    public static string Repeat(this int n, string text)
    {
        var sb = new StringBuilder();

        for (int i = 0; i < n; i++)
        {
            sb.Append(text);
        }

        return sb.ToString();
    }
}

// Материал для задач 2 и 3 — трогать не нужно.
public static class DemoExtensions
{
    public static string Name(this ExtensionMethodsBox b) => "EXTENSION";

    public static string Tag(this string s) => "EXT:" + (s ?? "<null>");

    public static string Describe(this string s) => "string-ext";

    public static string Describe(this object o) => "object-ext";
}

// Тип для задачи 2: у него есть СВОЙ метод Name(), и рядом, в
// DemoExtensions, объявлено расширение Name() с той же сигнатурой.
public class ExtensionMethodsBox
{
    public string Name() => "INSTANCE";
}
