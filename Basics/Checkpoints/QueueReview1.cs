using System.Text;

namespace Basics.Checkpoints;

// 🔁 РАЗБОР ОЧЕРЕДИ ПОВТОРЕНИЯ №1
//
// Не новая тема. Здесь собраны факты, которые провалились на холодном
// входе и накопились в очереди сверх порога. Цель — закрыть их разом,
// а не растягивать на три сессии.
//
// В конспекты не подглядывать — в этом весь смысл.
//
// ПЕРЕД НАЧАЛОМ: сделай задачу 5 в теме 1.11 (она не решена). Там
// устанавливается правило, которое здесь применяется в задаче 3.
// Запуск той темы:  dotnet run --project Basics -- 1.11
//
// Запуск этой:      dotnet run --project Basics -- Q1

public static class QueueReview1
{
    public static void Run()
    {
        Task1();
        Task2();
        Task3();
    }

    private class Box<T> where T : struct
    {
        public static int Count;
    }

    // ── ЗАДАЧА 1: сколько разных типов ────────────────────────────────────
    //
    // У Box<T> статическое поле Count. Ниже три инкремента.
    //
    // ПРЕДСКАЗАНИЕ (1) Box<int>.Count после всех трёх строк: 2. Статическое поле общее для каждого типа T. int и System.Int32 это один и тот же тип.
    // ПРЕДСКАЗАНИЕ (2) Box<System.Int32>.Count: 2 как в ответе на 1 предсказание, так как это один и тот же тип.
    // ПРЕДСКАЗАНИЕ (3) Box<long>.Count: 1. Каждый тип T имеет свое статическое поле.
    // ПРЕДСКАЗАНИЕ (4) объясни числа одной фразой — сколько ЗАКРЫТЫХ типов
    //                  здесь получилось и почему именно столько: 2 закрытых типа - int и long. int и System.Int32 это один и тот же тип.
    // ПРЕДСКАЗАНИЕ (5) typeof(int) == typeof(System.Int32) — что вернёт: true.
    // ПРЕДСКАЗАНИЕ (6) строка А ниже закомментирована. Раскомментируй,
    //                  посмотри код ошибки. Почему `int?` не проходит,
    //                  хотя это значимый тип? int? это Nullable int, который может иметь значение Null. Дженерик не может работать с null без соответствующего ограничения :class, так как T может оказаться int.
    private static void Task1()
    {
        Console.WriteLine("--- Задача 1 ---");

        Box<int>.Count++;
        Box<System.Int32>.Count++;
        Box<long>.Count++;

        Console.WriteLine($"Box<int>.Count          = {Box<int>.Count}");
        Console.WriteLine($"Box<System.Int32>.Count = {Box<System.Int32>.Count}");
        Console.WriteLine($"Box<long>.Count         = {Box<long>.Count}");
        Console.WriteLine($"typeof(int) == typeof(System.Int32) -> {typeof(int) == typeof(System.Int32)}");

        // Строка А — раскомментируй, посмотри ошибку, верни комментарий:
        // Box<int?>.Count++;
    }

    // ── ЗАДАЧА 2 (ПРОВОКАЦИОННАЯ): одна строка или разные ─────────────────
    //
    // Семь переменных, у всех значение «привет». Вопрос каждый раз один:
    // это ОДИН объект в памяти или разные.
    //
    // Здесь как минимум одна строка ведёт себя не так, как кажется.
    //
    // ПРЕДСКАЗАНИЕ (1) a == b:
    // ПРЕДСКАЗАНИЕ (2) ReferenceEquals(a, b)  — два литерала:
    // ПРЕДСКАЗАНИЕ (3) ReferenceEquals(a, c)  — new string(...):
    // ПРЕДСКАЗАНИЕ (4) ReferenceEquals(a, d)  — склейка с переменной:
    // ПРЕДСКАЗАНИЕ (5) ReferenceEquals(a, e)  — склейка двух литералов:
    // ПРЕДСКАЗАНИЕ (6) ReferenceEquals(a, f)  — StringBuilder.ToString():
    // ПРЕДСКАЗАНИЕ (7) a == c:
    //
    // ПРЕДСКАЗАНИЕ (8) если (4) и (5) разошлись — объясни, чем склейка
    //                  двух литералов отличается от склейки с переменной.
    //                  В какой момент происходит каждая?
    // ПРЕДСКАЗАНИЕ (9) в каком из предсказаний уверенность ниже всего?
    private static void Task2()
    {
        Console.WriteLine("--- Задача 2 ---");

        string a = "привет";
        string b = "привет";
        string c = new string("привет".ToCharArray());
        string half = "при";
        string d = half + "вет";
        string e = "при" + "вет";
        string f = new StringBuilder("привет").ToString();

        Console.WriteLine($"a == b                -> {a == b}");
        Console.WriteLine($"ReferenceEquals(a, b) -> {ReferenceEquals(a, b)}   два литерала");
        Console.WriteLine($"ReferenceEquals(a, c) -> {ReferenceEquals(a, c)}   new string(...)");
        Console.WriteLine($"ReferenceEquals(a, d) -> {ReferenceEquals(a, d)}   склейка с переменной");
        Console.WriteLine($"ReferenceEquals(a, e) -> {ReferenceEquals(a, e)}   склейка двух литералов");
        Console.WriteLine($"ReferenceEquals(a, f) -> {ReferenceEquals(a, f)}   StringBuilder.ToString()");
        Console.WriteLine($"a == c                -> {a == c}");
    }

    // ── ЗАДАЧА 3: написать TryGet ─────────────────────────────────────────
    //
    // Напиши метод по образцу TryXxx из 1.11:
    //
    //   private static bool TryGet<T>(T[] items, int index, out T value)
    //       where T : struct
    //
    // Возвращает true и элемент, если индекс в пределах массива;
    // иначе false. Вызови его дважды — с годным индексом и с негодным,
    // выведи и bool, и value в обоих случаях.
    //
    // ПРЕДСКАЗАНИЕ (1) что будет, если на неудачном пути НЕ присвоить
    //                  value? Соберётся ли метод, и если нет — почему?
    // ПРЕДСКАЗАНИЕ (2) что придётся присвоить на неудачном пути и что
    //                  окажется в value у вызывающего после false:
    // ПРЕДСКАЗАНИЕ (3) вызывающий пишет так:
    //                      int found;
    //                      if (TryGet(numbers, 99, out found)) { ... }
    //                  обязан ли он инициализировать found ДО вызова?
    //                  А если бы параметр был ref — что изменилось бы?
    // ПРЕДСКАЗАНИЕ (4) ограничение where T : struct здесь обязательно
    //                  для работы метода — или его можно снять? Если
    //                  снять, что перестанет компилироваться и почему?
    private static void Task3()
    {
        Console.WriteLine("--- Задача 3 ---");

        // твой код здесь
    }
}
