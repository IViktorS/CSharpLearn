namespace Basics.Block1;

// ТЕМА 1.8 — Модификаторы доступа (все 6)
//
// Доступ — это в основном compile-time. Поэтому часть проверок здесь —
// закомментированные строки: раскомментируй → увидишь ошибку компиляции,
// потом верни обратно. Предсказания пиши и про вывод, и про «скомпилируется/нет».
//
// Запуск:  dotnet run --project Basics -- 1.8

public static class AccessModifiers
{
    // База с полями разной видимости.
    public class BankAccount
    {
        private decimal _pin = 1234;     // только внутри BankAccount
        protected decimal Balance;        // BankAccount + наследники
        public string Owner = "—";        // все

        public void Deposit(decimal amount) => Balance += amount;

        // Приватное поле доступно ВНУТРИ своего класса — это законно.
        public bool CheckPin(decimal pin) => pin == _pin;
    }

    // Наследник в той же сборке.
    public class SavingsAccount : BankAccount
    {
        // protected базы доступен наследнику:
        public void ShowBalance() => Console.WriteLine($"Balance (protected): {Balance}");

        // private базы наследнику НЕ доступен — раскомментируй и увидишь CS0122:
        // public void ShowPin() => Console.WriteLine(_pin);
    }

    // Класс БЕЗ модификатора — по умолчанию internal (виден в своей сборке Basics).
    class Config
    {
        // Поле без модификатора — по умолчанию private.
        int _value = 42;
        public int GetValue() => _value;
    }

    public static void Run()
    {
        Task1();
        Task2();
        Task3();
    }

    // ── ЗАДАЧА 1: private vs protected через наследование ─────────────────
    // 1) Создай SavingsAccount, вызови Deposit(100), потом ShowBalance().
    // 2) Проверь CheckPin: выведи acc.CheckPin(1234) и acc.CheckPin(1).
    // 3) В комментарии ответь: почему ShowBalance() (protected Balance) компилируется,
    //    а обращение к _pin из SavingsAccount — нет?
    // Ответ. ShowBalance() компилируется, так как мы обращаемся к полю Balance с модификатором protected, что позволяет наследникам образаться к данному полю.
    // Класс SavingsAccount как раз наследуется от BankAccount, где живет Balance.
    //
    // ПРЕДСКАЗАНИЕ (ShowBalance после Deposit(100)): 100
    // ПРЕДСКАЗАНИЕ (CheckPin(1234) / CheckPin(1)):    CheckPin(1234) - true, CheckPin(1) - false
    private static void Task1()
    {
        Console.WriteLine("--- Задача 1 ---");

        var savingsAccount = new SavingsAccount();
        savingsAccount.Deposit(100);
        savingsAccount.ShowBalance();

        savingsAccount.CheckPin(1234);
        savingsAccount.CheckPin(1);

    }

    // ── ЗАДАЧА 2: private доступен только внутри класса ───────────────────
    // 1) Создай BankAccount, выведи acc.Owner (public — доступно).
    // 2) Попробуй обратиться к acc.Balance или acc._pin ИЗВНЕ (здесь, в статич. классе).
    //    Не пиши рабочий код — просто в комментарии ответь: какие из них
    //    доступны отсюда, а какие нет, и почему.
    //    (можешь раскомментировать строку ниже и посмотреть на ошибку)
    //    // Console.WriteLine(acc._pin);
    //
    // ПРЕДСКАЗАНИЕ (acc.Owner):                     Выведет —
    // ПРЕДСКАЗАНИЕ (доступны ли отсюда Balance/_pin): Balance недоступен из Task2, так как он доступен только в классе наследнике, а  AccessModifiers им не является + он статический
    // _pin недоступен, так как он приватный.
    private static void Task2()
    {
        Console.WriteLine("--- Задача 2 ---");

        var bankAccount = new BankAccount();
        Console.WriteLine($"Owner: {bankAccount.Owner}");
    }

    // ── ЗАДАЧА 3: модификаторы по умолчанию ───────────────────────────────
    // 1) Создай Config, выведи GetValue().
    // 2) В комментарии ответь на два вопроса:
    //    а) какой РЕАЛЬНЫЙ модификатор у класса Config и у поля _value (оба без слова)?
    //    б) был бы класс Config виден из проекта CSharpLearn (другая сборка)? Почему?
    // Ответ. а. У класса Config модфикатор Internal. У поля _value модификатор private.
    // б.Нет, не был бы виден из-за модификатора internal.
    //
    // ПРЕДСКАЗАНИЕ (GetValue()): 42
    private static void Task3()
    {
        Console.WriteLine("--- Задача 3 ---");

        var config = new Config();
        Console.WriteLine(config.GetValue());
    }
}
