namespace Basics.Block1;

// ТЕМА 1.6 — Свойства (properties)
//
// 3 задачи. Реши САМ. Перед запуском впиши ПРЕДСКАЗАНИЕ.
// Когда закончишь — скажи, проверю код и предсказания.
//
// Запуск:  dotnet run --project Basics -- 1.6

public static class Properties
{
    // Для задачи 1: свойство с валидацией в сеттере.
    public class Person
    {
        private int _age;
        public int Age
        {
            get => _age;
            set => _age = value < 0
                ? throw new ArgumentOutOfRangeException(nameof(value), "Возраст не может быть отрицательным")
                : value;
        }
    }

    // Для задачи 2: разные виды авто-свойств + вычисляемое.
    public class Employee
    {
        public string FirstName { get; set; } = "";
        public string LastName { get; set; } = "";

        // Вычисляемое: значение не хранится, считается при каждом обращении.
        public string FullName => $"{FirstName} {LastName}";

        // init-only: задаётся ТОЛЬКО при создании, потом только чтение.
        public int Id { get; init; }
    }

    // Для задачи 3: инкапсуляция через private set.
    public class Counter
    {
        public int Count { get; private set; }
        public void Increment() => Count++;
    }

    public static void Run()
    {
        Task1();
        Task2();
        Task3();
    }

    // ── ЗАДАЧА 1: валидация в сеттере ─────────────────────────────────────
    // 1) Создай Person, присвой Age = 25, выведи Age.
    // 2) В try/catch попробуй присвоить Age = -5, поймай исключение,
    //    выведи его тип (ex.GetType().Name).
    //
    // ПРЕДСКАЗАНИЕ (Age после = 25):        Age будкт равно 25
    // ПРЕДСКАЗАНИЕ (тип исключения при -5): ArgumentOutOfRangeException
    private static void Task1()
    {
        Console.WriteLine("--- Задача 1 ---");

        var person = new Person() { Age = 25};
        Console.WriteLine($"Age: {person.Age}");

        try
        {
            person.Age = -5;
        }
        catch (Exception ex)
        {
            Console.WriteLine($"Тип исключения: {ex.GetType().Name}");
        }
    }

    // ── ЗАДАЧА 2: init, вычисляемое свойство ──────────────────────────────
    // 1) Создай Employee через объектный инициализатор:
    //    new Employee { FirstName = "Иван", LastName = "Петров", Id = 7 }
    //    Выведи FullName и Id.
    // 2) Поменяй emp.FirstName = "Пётр"; снова выведи FullName.
    // 3) В комментарии ответь: почему строку emp.Id = 99; компилятор бы НЕ пропустил?
    //    (можешь попробовать раскомментировать и увидеть ошибку — потом верни обратно)
    // Ответ. emp.Id = 99 компилятор не пропустит, так как сеттер у свойства id установлен как init, то присвоить что то в это свойство можно только при создании экземпляра.
    //
    // ПРЕДСКАЗАНИЕ (FullName до смены имени):    Иван Петров
    // ПРЕДСКАЗАНИЕ (FullName после смены имени): Петр Петров
    private static void Task2()
    {
        Console.WriteLine("--- Задача 2 ---");

        var employee = new Employee { FirstName = "Иван", LastName = "Петров", Id = 7 };
        Console.WriteLine($"FullName: {employee.FullName}");
        Console.WriteLine($"Id: {employee.Id}");

        employee.FirstName = "Петр";
        Console.WriteLine($"FirstName: {employee.FullName}");

        // emp.Id = 99;   // ← попробуй раскомментировать: что скажет компилятор?
    }

    // ── ЗАДАЧА 3: private set (инкапсуляция) ──────────────────────────────
    // 1) Создай Counter, выведи Count.
    // 2) Вызови Increment() дважды, выведи Count.
    // 3) В комментарии ответь: почему извне нельзя написать counter.Count = 100;,
    //    а Increment() менять Count может?
    // Ответ. Извне нельзя написать counter.Count = 100; а Increment() менять Count может так как сеттер приватный, что означает,
    // что его можно изменить только внутри родительского класса, что метод Increment() и делает.
    //
    // ПРЕДСКАЗАНИЕ (Count в начале):           0
    // ПРЕДСКАЗАНИЕ (Count после 2× Increment): 2
    private static void Task3()
    {
        Console.WriteLine("--- Задача 3 ---");

        var counter = new Counter();
        Console.WriteLine($"Count: {counter.Count}");

        for (int i = 0; i < 2; i++)
            counter.Increment();

        Console.WriteLine($"Count: {counter.Count}");

        // counter.Count = 100;   // ← почему это не скомпилируется?
    }
}
