using Basics.Block1;

namespace Basics;

// Реестр тем. Каждую новую тему добавляем сюда одной строкой.
public static class TopicRegistry
{
    public static readonly (string Id, string Title, Action Run)[] All =
    [
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
    ];
}
