using System.Reflection;

namespace Basics.Block1;

// ТЕМА 1.19 — Атрибуты и partial
//
// Конспект: 1.19-Attributes.md в этой же папке.
// Запуск:   dotnet run --project Basics -- 1.19
//
// Все три задачи — предсказания. Сразу знаешь ответ — скажи, заменю.

public static class Attributes
{
    public static void Run()
    {
        Task1();
        Task2();
        Task3();
    }

    // ── ЗАДАЧА 1: свой атрибут ────────────────────────────────────────────
    //
    // Ниже объявлен атрибут UnitAttribute (единица измерения) и класс
    // Sensor, у которого два свойства помечены им, а одно — нет.
    //
    // Напиши код, который для КАЖДОГО из трёх свойств Sensor печатает
    // имя свойства и единицу измерения, а если пометки нет — прочерк.
    // Читать атрибут: prop.GetCustomAttribute<UnitAttribute>().
    //
    // ПРЕДСКАЗАНИЕ (1) что вернёт GetCustomAttribute для непомеченного
    //                  свойства — null, исключение или пустой объект? Вернется null.
    // ПРЕДСКАЗАНИЕ (2) что напечатает твой код для всех трёх свойств: Temperature: °C, Rpm: об/мин, Model: -. 
    // ПРЕДСКАЗАНИЕ (3) если убрать [Unit] со всех свойств и НЕ менять
    //                  больше ничего — что изменится в поведении класса
    //                  Sensor? (не в твоём выводе, а у самого класса) - Ничего не изменится.
    // ПРЕДСКАЗАНИЕ (4) строка А ниже закомментирована. Раскомментируй,
    //                  посмотри код ошибки. Почему так, если значение
    //                  вроде бы известно заранее? Потому что RuntimeUnit будет вычисляться во временя выполнения.
    //                  Атрибуты принимают только окнстанты времени компиляции.
    private static void Task1()
    {
        Console.WriteLine("--- Задача 1 ---");

        var props = typeof(Sensor).GetProperties();
        foreach (var prop in props)
        {
            var attr = prop.GetCustomAttribute<UnitAttribute>();
            Console.WriteLine($"{prop.Name}: {attr?.Symbol ?? "-"}");
        }
    }

    // ── ЗАДАЧА 2: [Obsolete] ──────────────────────────────────────────────
    //
    // Ниже два устаревших класса: OldSlow и OldBroken. Отличаются одним
    // аргументом атрибута.
    //
    // ПРЕДСКАЗАНИЕ (1) строка Б (new OldSlow) — предупреждение или ошибка? Предупреждение.
    // ПРЕДСКАЗАНИЕ (2) строка В (new OldBroken) — предупреждение или ошибка? Ошибка.
    // ПРЕДСКАЗАНИЕ (3) кто именно читает [Obsolete] и в какой момент?
    //                  Ответ должен объяснять, почему для него не нужна
    //                  ни рефлексия, ни запуск программы. Потому что его читает компилятор на этапе компиляции.
    private static void Task2()
    {
        Console.WriteLine("--- Задача 2 ---");

        // Строка Б — раскомментируй, посмотри, оставь как получится:
         var a = new OldSlow();

        // Строка В — раскомментируй, посмотри, верни комментарий:
        // var b = new OldBroken();
    }

    // ── ЗАДАЧА 3 (ПРОВОКАЦИОННАЯ): partial-метод ──────────────────────────
    //
    // Класс Pipeline объявлен partial и разбит на две части (обе в этом
    // файле, ниже). В первой части объявлен partial-метод OnStep, во
    // второй его реализации НЕТ.
    //
    // Метод Run3 вызывает OnStep(NextId()), где NextId() увеличивает
    // счётчик Counter и возвращает его.
    //
    // Здесь правдоподобный ответ неверен.
    //
    // ПРЕДСКАЗАНИЕ (1) сколько будет Counter после трёх вызовов Run3()? 0, так как OnStep не реализован и компилятор удаляет вызов и аргументы. Соответственно ничего вычисляться не будет.
    // ПРЕДСКАЗАНИЕ (2) объясни ответ: что именно делает компилятор
    //                  с вызовом OnStep, у которого нет реализации? Я так понял, что вырежет всю строку public void Run3() => OnStep(NextId()); и все что с ней связано.
    // ПРЕДСКАЗАНИЕ (3) практическое правило: что НЕЛЬЗЯ писать в
    //                  аргументах partial-метода и почему? Ничего кроме вычислений, которые не имеют побочных эффектов, так как если метод не реализован, то вызов будет удален и вычисления не произойдут.
    // ПРЕДСКАЗАНИЕ (4) раскомментируй реализацию OnStep (строка Г внизу
    //                  файла) и предскажи Counter ЗАРАНЕЕ, до запуска: 3
    private static void Task3()
    {
        Console.WriteLine("--- Задача 3 ---");

        var p = new Pipeline();
        p.Run3();
        p.Run3();
        p.Run3();

        Console.WriteLine($"Counter = {Pipeline.Counter}");
    }
}

// ── материал задачи 1 ─────────────────────────────────────────────────────

[AttributeUsage(AttributeTargets.Property)]
public class UnitAttribute : Attribute
{
    public string Symbol { get; }
    public string Note { get; set; } = "";

    public UnitAttribute(string symbol) => Symbol = symbol;
}

public class Sensor
{
    private const string Celsius = "°C";

    [Unit(Celsius, Note = "шпиндель")]
    public double Temperature { get; set; }

    [Unit("об/мин")]
    public int Rpm { get; set; }

    public string Model { get; set; } = "S-100";

    // Строка А — раскомментируй, посмотри ошибку, верни комментарий.
    // Подсказка к предсказанию (4): чем это отличается от Celsius выше?
    //private static readonly string RuntimeUnit = DateTime.Now.ToString();
    //[Unit(RuntimeUnit)]
    //public double Pressure { get; set; }
}

// ── материал задачи 2 ─────────────────────────────────────────────────────

[Obsolete("медленный, используй NewFast")]
public class OldSlow { }

[Obsolete("сломан, использовать нельзя", true)]
public class OldBroken { }

// ── материал задачи 3 ─────────────────────────────────────────────────────

public partial class Pipeline
{
    public static int Counter;

    private static int NextId()
    {
        Counter++;
        return Counter;
    }

    // объявление partial-метода
    partial void OnStep(int id);

    public void Run3() => OnStep(NextId());
}

public partial class Pipeline
{
    // Строка Г — реализация partial-метода. Раскомментируй в пункте (4).
     partial void OnStep(int id) => Console.WriteLine($"  шаг {id}");
}
