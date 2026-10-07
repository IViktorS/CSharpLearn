using System.Reflection;
using System.Text;

namespace Basics.Checkpoints;

// 🔁 КОНТРОЛЬНАЯ ТОЧКА 2 — смешанная задача по темам 1.14–1.20
//
// Формат как в контрольной точке 1:
//   • в задачах 1 и 2 код уже написан — твоё дело ПРЕДСКАЗАТЬ результат
//     и назвать, ИЗ КАКОЙ ТЕМЫ каждое поведение;
//   • в задаче 3 код надо написать самому.
//
// В конспекты не подглядывать — в этом весь смысл. Список тем
// (названия и номера в PROGRESS.md) смотреть можно.
//
// Всё, на что опирается задача, разбиралось в темах 1.4, 1.10 и 1.14–1.20.
// Если какой-то пункт требует того, чего мы не проходили, — скажи:
// по правилу он уходит «вне зачёта».
//
// Ориентир по времени: около 1,5 часа на задачи 1–2 и около 30 минут на
// задачу 3. Если задача 1 одна заняла больше часа — остановись и скажи:
// это дефект задачи, а не твой. Не вспоминается механизм — пиши лучшую
// догадку и отмечай её в вопросе про неуверенность, не раскапывай часами.
//
// Порядок: сначала все ПРЕДСКАЗАНИЯ в задачах 1 и 2, потом запуск,
// потом задача 3.
//
// Запуск:  dotnet run --project Basics -- R2

public static class Checkpoint2
{
    public static void Run()
    {
        Task1();
        Task2();
        Task3();
    }

    // ── ЗАДАЧА 1: универсальный Dump ──────────────────────────────────────
    //
    // Ниже, в классе DumpExtensions, — метод расширения Dump для ЛЮБОГО
    // объекта. Он рекурсивно печатает объект со всеми свойствами, берёт
    // их рефлексией, а к числу дописывает единицу измерения, если у
    // свойства есть атрибут [Suffix]. Классы Pump и Valve — внизу файла.
    //
    // Прочитай Dump и DumpObject внимательно: каждая строка там важна.
    //
    // Для (1), (4), (5), (6) предскажи ТОЧНУЮ строку вывода. Для (2) и (3)
    // можно сокращать: повторяющиеся части пиши как «…как в (1)…», подробно —
    // только то, что отличается; главное — что стоит на месте Backup на
    // каждом уровне вложенности. К каждому вызову назови номера
    // тем, механизмы которых определили результат.
    //
    // ПРЕДСКАЗАНИЕ (1) new Pump().Dump():
    //                  темы:
    //
    // ПРЕДСКАЗАНИЕ (2) насос с резервным насосом (pump.Backup = new Pump { Name = "насос-2" }):
    //                  темы:
    //
    // ПРЕДСКАЗАНИЕ (3) насос, который сам себе резерв (pump.Backup = pump):
    //                  темы:
    //
    // ПРЕДСКАЗАНИЕ (4) Pump? missing = null;  missing.Dump():
    //                  темы:
    //
    // ПРЕДСКАЗАНИЕ (5) new Valve().Dump():
    //                  темы:
    //
    // ПРЕДСКАЗАНИЕ (6) object v = new Valve();  v.Dump():
    //                  темы:
    //
    // ПРЕДСКАЗАНИЕ (7) НЕ ЗАПУСКАЯ: что случилось бы в вызове (3), если бы
    //                  в Dump не было ветки  _ when depth >= 2 => "…"  ?
    //                  Можно ли было бы перехватить это try/catch?
    //
    // ПРЕДСКАЗАНИЕ (8) НЕ ЗАПУСКАЯ: в Dump переставили ветку  long n => ...
    //                  в самый конец, ПОСЛЕ  _ => DumpObject(...).
    //                  Что произойдёт и на каком этапе?
    //
    // ПРЕДСКАЗАНИЕ (9) в каком из предсказаний (1)–(8) уверенность ниже всего? Номером.
    //
    private static void Task1()
    {
        Console.WriteLine("--- Задача 1 ---");

        Console.WriteLine($"(1) {new Pump().Dump()}");

        var withBackup = new Pump { Backup = new Pump { Name = "насос-2" } };
        Console.WriteLine($"(2) {withBackup.Dump()}");

        var loop = new Pump();
        loop.Backup = loop;
        Console.WriteLine($"(3) {loop.Dump()}");

        Pump? missing = null;
        Console.WriteLine($"(4) {missing.Dump()}");

        Console.WriteLine($"(5) {new Valve().Dump()}");

        object v = new Valve();
        Console.WriteLine($"(6) {v.Dump()}");
    }

    // ── ЗАДАЧА 2: прочитать свойство нужного типа ─────────────────────────
    //
    // Метод Read<T> ниже достаёт значение свойства по имени и приводит его к T.
    //
    // ПРЕДСКАЗАНИЕ (1) Read<int>(pump, "Pressure"):
    // ПРЕДСКАЗАНИЕ (2) Read<long>(pump, "Hours"):
    // ПРЕДСКАЗАНИЕ (3) Read<long>(pump, "Pressure") — Pressure объявлено как int:
    //                  скомпилируется ли, и что будет при выполнении?
    // ПРЕДСКАЗАНИЕ (4) строка А ниже закомментирована: Read<int?>(pump, "Pressure").
    //                  Что произойдёт и на каком этапе?
    // ПРЕДСКАЗАНИЕ (5) нужно ли методу Read ограничение where T : struct?
    //                  Что станет можно и что перестанет компилироваться,
    //                  если его убрать? Пример: Read<string>(pump, "Name").
    //
    // ПРЕДСКАЗАНИЕ (6) в каком из (1)–(5) уверенность ниже всего? Номером.
    //
    private static void Task2()
    {
        Console.WriteLine("--- Задача 2 ---");

        var pump = new Pump();
        Console.WriteLine($"(1) {Try(() => Read<int>(pump, "Pressure"))}");
        Console.WriteLine($"(2) {Try(() => Read<long>(pump, "Hours"))}");
        Console.WriteLine($"(3) {Try(() => Read<long>(pump, "Pressure"))}");

        // Строка А — раскомментируй ПОСЛЕ предсказания, посмотри, верни комментарий:
        // Console.WriteLine($"(4) {Try(() => Read<int?>(pump, "Pressure"))}");
    }

    private static T Read<T>(object obj, string name) where T : struct
        => (T)obj.GetType().GetProperty(name)!.GetValue(obj)!;

    // Выполняет действие и печатает результат или имя исключения. Чёрный ящик.
    private static string Try<T>(Func<T> action)
    {
        try { return $"{action()}"; }
        catch (Exception ex) { return ex.GetType().Name; }
    }

    // ── ЗАДАЧА 3: свой Dump (написать) ────────────────────────────────────
    //
    // В выводе задачи 1 есть две вещи, которые для отчёта о насосе лишние:
    //   • свойство Installed — это состояние ТИПА, а не конкретного насоса;
    //   • иногда нужно скрыть отдельное свойство целиком.
    //
    // Напиши РЯДОМ (Dump не трогай — его вывод остаётся эталоном) метод
    // расширения DumpClean, который:
    //   • не печатает статические свойства;
    //   • пропускает свойства, помеченные новым атрибутом [Hide]
    //     (атрибут объяви сам);
    //   • в остальном ведёт себя как Dump.
    //
    // Пометь [Hide] свойство Hours у Pump и выведи new Pump().DumpClean().
    //
    // ПРЕДСКАЗАНИЕ (1) какой одной правкой убираются статические свойства:
    // ПРЕДСКАЗАНИЕ (2) что выведет new Pump().DumpClean():
    // ПРЕДСКАЗАНИЕ (3) изменится ли от пометки [Hide] вывод старого Dump? Почему?
    //
    private static void Task3()
    {
        Console.WriteLine("--- Задача 3 ---");

        // твой код
    }
}

// ── материал к задачам ────────────────────────────────────────────────────

/// <summary>Единица измерения, которую Dump дописывает к значению.</summary>
[AttributeUsage(AttributeTargets.Property)]
public class SuffixAttribute : Attribute
{
    /// <summary>Обозначение единицы.</summary>
    public string Symbol { get; }

    /// <summary>Создаёт атрибут с обозначением единицы.</summary>
    public SuffixAttribute(string symbol) => Symbol = symbol;
}

/// <summary>Насос.</summary>
public class Pump
{
    /// <summary>Имя.</summary>
    public string Name { get; set; } = "насос-1";

    /// <summary>Давление — int.</summary>
    [Suffix("бар")]
    public int Pressure { get; set; } = 4;

    /// <summary>Наработка — long.</summary>
    [Suffix("ч")]
    public long Hours { get; set; } = 1200;

    /// <summary>Резервный насос.</summary>
    public Pump? Backup { get; set; }

    /// <summary>Сколько насосов установлено — общее для всех.</summary>
    public static int Installed { get; set; } = 3;
}

/// <summary>Клапан со своим методом Dump.</summary>
public class Valve
{
    /// <summary>Собственный Dump клапана.</summary>
    public string Dump() => "Valve: собственный Dump";
}

/// <summary>Универсальный Dump для любого объекта.</summary>
public static class DumpExtensions
{
    /// <summary>Печатает значение; объект — со всеми свойствами, рекурсивно.</summary>
    public static string Dump(this object? obj, int depth = 0) => obj switch
    {
        null => "null",
        string s => $"\"{s}\"",
        long n => $"{n}L",
        int n => $"{n}",
        _ when depth >= 2 => "…",
        _ => DumpObject(obj, depth),
    };

    /// <summary>Печатает объект в виде «Тип { Свойство = значение, ... }».</summary>
    private static string DumpObject(object obj, int depth)
    {
        var sb = new StringBuilder();
        sb.Append(obj.GetType().Name).Append(" { ");

        bool first = true;
        foreach (var p in obj.GetType().GetProperties())
        {
            if (!first) sb.Append(", ");
            first = false;

            object? value = p.GetValue(obj);
            string? suffix = p.GetCustomAttribute<SuffixAttribute>()?.Symbol;

            sb.Append(p.Name).Append(" = ").Append(value.Dump(depth + 1));
            if (suffix is not null) sb.Append(' ').Append(suffix);
        }

        return sb.Append(" }").ToString();
    }
}
