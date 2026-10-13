using System;
using System.Windows;
using CSharpLearn.Block7;

namespace CSharpLearn;

// Реестр тем WPF-трека. Новая тема добавляется одной строкой,
// как в Basics/TopicRegistry.cs.
public static class TopicWindows
{
    public static readonly (string Id, string Title, Func<Window> Create)[] All =
    [
        // Блок 7: WPF — основы и вёрстка
        ("7.1", "Панели компоновки (Grid, StackPanel, DockPanel, Canvas)", () => new LayoutPanels()),
        ("7.2", "StaticResource vs DynamicResource", () => new Block7.Resources()),
        ("7.3", "DataContext — наследование по дереву", () => new DataContextTree()),
        ("7.4", "Data Binding — режимы и триггеры", () => new BindingModes()),
    ];
}
