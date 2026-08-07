namespace Basics.Block1;

// ТЕМА 1.3 — Stack vs Heap
//
// 3 задачи. Реши САМ. Перед запуском впиши ПРЕДСКАЗАНИЕ.
// Когда закончишь — скажи, проверю код и предсказания.
//
// Запуск:  dotnet run --project Basics -- 1.3

public static class StackVsHeap
{
    // Значимый тип...
    public struct Counter
    {
        public int Value;
    }

    // ...и класс, который держит его ПОЛЕМ (значит, поле живёт в куче внутри Box).
    public class Box
    {
        public Counter Inner;
    }

    // Ссылочный аналог Counter — для замера нагрузки на GC в Задаче 2.
    public class CounterClass
    {
        public int Value;
    }

    public static void Run()
    {
        Task1();
        Task2();
        Task3();
    }

    // ── ЗАДАЧА 1: где живёт struct-поле и когда получается копия ───────────
    // 1) Создай box1 = new Box(); box1.Inner.Value = 1;
    // 2) Присвой box2 = box1; (копируется ССЫЛКА).
    //    Сделай box2.Inner.Value = 99; выведи box1.Inner.Value.
    // 3) Теперь прочитай поле в ЛОКАЛЬНУЮ переменную:
    //    var local = box1.Inner; local.Value = 7; выведи box1.Inner.Value.
    //
    // Идея: struct-поле лежит в куче ВНУТРИ объекта Box. Через вторую ссылку
    // ты меняешь тот же объект. А чтение в локальную переменную делает копию.
    //
    // ПРЕДСКАЗАНИЕ (шаг 2, box1.Inner.Value): box1.Inner.Value будет равен 99, так как в box2 хранится ссылка на тот же самый экземпляр Box,
    // что и в box1.
    // ПРЕДСКАЗАНИЕ (шаг 3, box1.Inner.Value): box1.Inner.Value будет равно 99, так как в local мы передаем копию значения box1.Inner.
    private static void Task1()
    {
        Console.WriteLine("--- Задача 1 ---");

        var box1 = new Box();
        box1.Inner.Value = 1;

        var box2 = box1;
        box2.Inner.Value = 99;

        Console.WriteLine(box1.Inner.Value);

        var local = box1.Inner;
        local.Value = 7;

        Console.WriteLine(box1.Inner.Value);
    }

    // ── ЗАДАЧА 2: нагрузка на GC — объекты в куче vs массив структур ───────
    // Замерим число сборок мусора поколения 0 в двух сценариях.
    //
    // 1) Считай GC.CollectionCount(0) в начале (before).
    // 2) Сценарий А (классы): в цикле на N = 5_000_000 создавай new CounterClass()
    //    и клади в переменную (чтобы JIT не выкинул) — например, суммируй .Value.
    //    Замерь CollectionCount(0) после и выведи разницу.
    // 3) Сценарий Б (структуры): один раз создай Counter[] на N элементов,
    //    в цикле заполни каждый элемент. Замерь разницу CollectionCount(0).
    //
    // Класс CounterClass с полем Value уже объявлен выше — используй его для
    // сценария А. Для сценария Б бери структуру Counter.
    //
    // ПРЕДСКАЗАНИЕ: В сценарии А сборок мусора будет заметно больше, так как экземпляры CounterClass создаются в разных местах кучи(памяти) и это именно отдельные объекты, за которыми нужно следить.
    // А counters это единый объект-массив в куче и для сборщика это также единый объект, который нужно уничтожить 1 раз. 
    private static void Task2()
    {
        Console.WriteLine("--- Задача 2 ---");

        var before = GC.CollectionCount(0);
        Console.WriteLine(before);

        const int N = 5000000;

        long sum = 0;
        for (int i = 0; i < N; i++)
        {
            var counter = new CounterClass
            {
                Value = i
            };
            sum += counter.Value;
        }
        Console.WriteLine($"CounterClass sum: {sum}");

        var after = GC.CollectionCount(0);
        Console.WriteLine($"CounterClass GC collections: {after - before}");

        before = GC.CollectionCount(0);
        Console.WriteLine(before);

        sum = 0;
        var counters = new Counter[N];
        for (int i = 0; i < N; i++)
        {
            counters[i].Value = i;
            sum += counters[i].Value;
        }
        Console.WriteLine($"Counter[] sum: {sum}");

        after = GC.CollectionCount(0);
        Console.WriteLine($"Counter[] GC collections: {after - before}");
    }

    // ── ЗАДАЧА 3: время жизни и возврат ───────────────────────────────────
    // 1) Напиши метод private static Counter MakeCounter(int v),
    //    который создаёт ЛОКАЛЬНЫЙ Counter { Value = v } и возвращает его.
    // 2) В Task3 получи c = MakeCounter(10); измени c.Value = 42;
    //    Выведи c.Value. Поясни в комментарии: это тот же экземпляр, что внутри
    //    метода, или копия? Почему это безопасно, хотя локальная переменная
    //    метода уже "умерла"?
    // Ответ: Локальная переменная умерла, но нам вернулась копия и уже в Value этой копии мы присваиваем новое значение.
    //
    // Письменный вопрос (ответь в комментарии):
    //    Почему компилятор запретил бы вернуть ССЫЛКУ на локальную переменную
    //    (ref int GetRef() { int x = 5; return ref x; })? Свяжи с кадром стека.
    //    Ответ: Потому что после выхода их метода кадр из стека удаляется безвозвратно и мы не можем ника получить его значение. Это привело бы к ошибке.
    //
    // ПРЕДСКАЗАНИЕ (c.Value после изменения): c.Value = 42, так как мы изменяем значение копии.
    private static void Task3()
    {
        Console.WriteLine("--- Задача 3 ---");

        var c = MakeCounter(10);
        c.Value = 42;
        Console.WriteLine(c.Value);
    }

    private static Counter MakeCounter(int v)
    {
        return new Counter { Value = v };
    }
}
