using System;
using System.Linq;
using System.Reflection;
using System.Windows;
using System.Windows.Controls;

class P
{
    static void Report(Type t)
    {
        var fields = t.GetFields(BindingFlags.Public | BindingFlags.Static | BindingFlags.FlattenHierarchy)
                      .Where(f => f.FieldType == typeof(DependencyProperty));

        int total = 0;
        var twoWay = new System.Collections.Generic.List<string>();
        var trigger = new System.Collections.Generic.List<string>();

        foreach (var f in fields)
        {
            var dp = (DependencyProperty)f.GetValue(null)!;
            total++;
            if (dp.GetMetadata(t) is FrameworkPropertyMetadata m)
            {
                if (m.BindsTwoWayByDefault) twoWay.Add(dp.Name);
                if (m.DefaultUpdateSourceTrigger != UpdateSourceTrigger.PropertyChanged)
                    trigger.Add($"{dp.Name}={m.DefaultUpdateSourceTrigger}");
            }
        }

        Console.WriteLine($"{t.Name}: всего dependency property = {total}");
        Console.WriteLine($"   TwoWay по умолчанию ({twoWay.Count}): {(twoWay.Count == 0 ? "—" : string.Join(", ", twoWay.OrderBy(x => x)))}");
        Console.WriteLine($"   UpdateSourceTrigger не PropertyChanged: {(trigger.Count == 0 ? "—" : string.Join(", ", trigger))}");
        Console.WriteLine();
    }

    [STAThread]
    static void Main()
    {
        Console.OutputEncoding = System.Text.Encoding.UTF8;
        Report(typeof(TextBox));
        Report(typeof(Slider));
        Report(typeof(CheckBox));
        Report(typeof(TextBlock));
    }
}
