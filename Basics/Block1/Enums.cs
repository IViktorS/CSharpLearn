namespace Basics.Block1;

// ТЕМА 1.7 — enum (+ [Flags])
//
// 3 задачи. Реши САМ. Перед запуском впиши ПРЕДСКАЗАНИЕ.
// Когда закончишь — скажи, проверю код и предсказания.
//
// Запуск:  dotnet run --project Basics -- 1.7

public static class Enums
{
    // Обычный enum (одно значение из списка).
    public enum Status
    {
        Active,     // 0
        Blocked,    // 1
        Deleted     // 2
    }

    // Флаговый enum (комбинация битов).
    [Flags]
    public enum Permissions
    {
        None = 0,
        Read = 1,      // 0001
        Write = 2,     // 0010
        Execute = 4    // 0100
    }

    public static void Run()
    {
        Task1();
        Task2();
        Task3();
    }

    // ── ЗАДАЧА 1: enum как число + «неизвестное» значение ──────────────────
    // 1) Выведи (int)Status.Deleted.
    // 2) Выведи Status.Blocked (просто в WriteLine — что напечатается, число или имя?).
    // 3) Заведи Status weird = (Status)99; выведи weird
    //    и выведи Enum.IsDefined(typeof(Status), weird).
    //
    // ПРЕДСКАЗАНИЕ ((int)Status.Deleted):     2
    // ПРЕДСКАЗАНИЕ (Status.Blocked):          Blocked
    // ПРЕДСКАЗАНИЕ (weird):                   99
    // ПРЕДСКАЗАНИЕ (Enum.IsDefined для weird): false
    private static void Task1()
    {
        Console.WriteLine("--- Задача 1 ---");

        Console.WriteLine($"(int)Status.Deleted: {(int)Status.Deleted}");
        Console.WriteLine($"Status.Blocked: {Status.Blocked}");

        var weird = (Status)99;
        Console.WriteLine($"weird: {weird}");

        Console.WriteLine($"Enum.IsDefined для weird: {Enum.IsDefined(typeof(Status), weird)}");
    }

    // ── ЗАДАЧА 2: [Flags] — комбинация битов ──────────────────────────────
    // 1) var p = Permissions.Read | Permissions.Write;
    //    Выведи p (что напечатается благодаря [Flags]?) и (int)p.
    // 2) Выведи p.HasFlag(Permissions.Read) и p.HasFlag(Permissions.Execute).
    // 3) Добавь флаг Execute:  p |= Permissions.Execute;  выведи p.
    // 4) Сними флаг Write:     p &= ~Permissions.Write;   выведи p.
    //
    // ПРЕДСКАЗАНИЕ (p и (int)p):                    p - Read, Write, (int)p - 3
    // ПРЕДСКАЗАНИЕ (HasFlag Read / HasFlag Execute): HasFlag Read - true, HasFlag Execute - false
    // ПРЕДСКАЗАНИЕ (p после добавления Execute):     p - Read, Write, Execute
    // ПРЕДСКАЗАНИЕ (p после снятия Write):           p - Read, Execute
    private static void Task2()
    {
        Console.WriteLine("--- Задача 2 ---");

        var p = Permissions.Read | Permissions.Write;
        Console.WriteLine($"p: {p}");
        Console.WriteLine($"(int)p: {(int)p}");
        Console.WriteLine($"p.HasFlag(Permissions.Read): {p.HasFlag(Permissions.Read)}");
        Console.WriteLine($"p.HasFlag(Permissions.Execute): {p.HasFlag(Permissions.Execute)}");

        p |= Permissions.Execute;
        Console.WriteLine($"p после добавления Execute: {p}");

        p &= ~Permissions.Write;
        Console.WriteLine($"p после снятия Write: {p}");
    }

    // ── ЗАДАЧА 3: default и разбор из строки ──────────────────────────────
    // 1) Выведи default(Status) — какое имя окажется под нулём?
    // 2) Enum.TryParse("Blocked", out Status ok); выведи результат TryParse и ok.
    // 3) Enum.TryParse("Xxx", out Status bad); выведи результат TryParse и bad.
    //    (подумай, что окажется в bad, если разбор не удался)
    //
    // ПРЕДСКАЗАНИЕ (default(Status)):        Active
    // ПРЕДСКАЗАНИЕ (TryParse "Blocked", ok): true, результат: Blocked
    // ПРЕДСКАЗАНИЕ (TryParse "Xxx", bad):    false, результат: Active
    private static void Task3()
    {
        Console.WriteLine("--- Задача 3 ---");

        Console.WriteLine($"default(Status): {default(Status)}");

        var ok = Enum.TryParse("Blocked", out Status statusOk);
        Console.WriteLine($"TryParse \"Blocked\": {ok}, результат: {statusOk}");

        var bad = Enum.TryParse("Xxx", out Status statusBad);
        Console.WriteLine($"TryParse \"Xxx\": {bad}, результат: {statusBad}");
    }
}
