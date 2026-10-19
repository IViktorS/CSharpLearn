using Basics.Block1;
using Basics.Block2;
using Basics.Checkpoints;

namespace Basics;

// Реестр тем. Каждую новую тему добавляем сюда одной строкой.
public static class TopicRegistry
{
    public static readonly (string Id, string Title, Action Run)[] All =
    [
        // Контрольные точки — смешанные задачи на стык тем
        ("R1", "🔁 Контрольная точка 1 (темы 1.1–1.13)", Checkpoint1.Run),
        ("Q1", "🔁 Разбор очереди повторения №1", QueueReview1.Run),
        ("Q2", "🔁 Разбор очереди повторения №2", QueueReview2.Run),
        ("R2", "🔁 Контрольная точка 2 (темы 1.14–1.20)", Checkpoint2.Run),

        // Блок 1: Основы C#
        ("1.1", "Value types vs reference types", ValueVsReference.Run),
        ("1.2", "struct vs class — когда что выбирать", StructVsClass.Run),
        ("1.3", "Stack vs Heap", StackVsHeap.Run),
        ("1.4", "Boxing / Unboxing", BoxingUnboxing.Run),
        ("1.5", "null, Nullable<T>, ?., ??, ??=, NRT", NullHandling.Run),
        ("1.6", "Свойства (properties)", Properties.Run),
        ("1.7", "enum (+ [Flags])", Enums.Run),
        ("1.8", "Модификаторы доступа (все 6)", AccessModifiers.Run),
        ("1.9", "readonly vs const", ReadonlyVsConst.Run),
        ("1.10", "static (класс/метод/конструктор/поле)", StaticMembers.Run),
        ("1.11", "out / ref / in параметры", RefOutIn.Run),
        ("1.12", "StringBuilder vs string", StringBuilderVsString.Run),
        ("1.13", "Кортежи (tuples) и деконструкция", Tuples.Run),
        ("1.14", "Pattern matching", PatternMatching.Run),
        ("1.15", "Рекурсия", Recursion.Run),
        ("1.16", "Методы расширения", ExtensionMethods.Run),
        ("1.17", "Дженерики: обобщённые классы и методы", Generics.Run),
        ("1.18", "Ограничения дженериков (where T : ...)", GenericConstraints.Run),
        ("1.19", "Атрибуты и partial", Basics.Block1.Attributes.Run),
        ("1.20", "Рефлексия", ReflectionBasics.Run),
        // Блок 2: ООП
        ("2.1", "Инкапсуляция, наследование, полиморфизм", OopPrinciples.Run),

        // Последняя строка — то, что запускается БЕЗ аргумента (dotnet run --project Basics).
        // Повтор записи намеренный: сюда ставится текущая работа. Не дубль, не удалять.
        ("2.1", "Инкапсуляция, наследование, полиморфизм", OopPrinciples.Run),
    ];
}
