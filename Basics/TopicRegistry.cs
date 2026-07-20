using Basics.Block1;

namespace Basics;

// Реестр тем. Каждую новую тему добавляем сюда одной строкой.
public static class TopicRegistry
{
    public static readonly (string Id, string Title, Action Run)[] All =
    [
        // Блок 1: Основы C#
        ("1.1", "Value types vs reference types", ValueVsReference.Run),
    ];
}
