using System;
using System.Linq;
using System.Windows;

namespace CSharpLearn
{
    // Раннер WPF-трека, устроен как Basics/Program.cs.
    //
    //   dotnet run --project CSharpLearn -- 7.1     — открыть тему
    //   dotnet run --project CSharpLearn -- list    — список тем
    //   dotnet run --project CSharpLearn            — последняя тема
    public partial class App : Application
    {
        protected override void OnStartup(StartupEventArgs e)
        {
            base.OnStartup(e);

            var arg = e.Args.FirstOrDefault();

            if (string.Equals(arg, "list", StringComparison.OrdinalIgnoreCase))
            {
                new MainWindow().Show();
                return;
            }

            var topic = arg is null
                ? TopicWindows.All.LastOrDefault()
                : TopicWindows.All.FirstOrDefault(t => t.Id == arg);

            if (topic.Create is null)
            {
                MessageBox.Show(
                    $"Тема \"{arg}\" не найдена. Запусти с аргументом list, чтобы увидеть список.",
                    "CSharpLearn");
                new MainWindow().Show();
                return;
            }

            var window = topic.Create();
            window.Title = $"{topic.Id} — {topic.Title}";
            window.Show();
        }
    }
}
