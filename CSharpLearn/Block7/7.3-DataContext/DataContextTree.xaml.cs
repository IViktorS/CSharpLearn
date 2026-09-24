using System.Windows;

namespace CSharpLearn.Block7
{
    /// <summary>Человек. Сообщать об изменениях своих свойств НЕ умеет — это тема 7.6.</summary>
    public class Person
    {
        /// <summary>Имя.</summary>
        public string Name { get; set; } = "";

        /// <summary>Смена (для задачи 3).</summary>
        public string Shift { get; set; } = "";

        /// <summary>Строковое представление — его покажет {Binding} без пути.</summary>
        public override string ToString() => $"Человек: {Name}";
    }

    /// <summary>Станок с дежурным оператором (для задачи 3).</summary>
    public class Machine
    {
        /// <summary>Имя станка. Совпадает по названию со свойством Person.Name — намеренно.</summary>
        public string Name { get; set; } = "";

        /// <summary>Дежурный оператор; null — смена кончилась, новый не заступил.</summary>
        public Person? Operator { get; set; }

        /// <summary>Строковое представление.</summary>
        public override string ToString() => $"Станок: {Name}";
    }

    // Код-подложка. Механизм событий — Блок 3; здесь он используется как
    // чёрный ящик: нажатие кнопки вызывает метод. Разбирать файл не нужно,
    // кроме строк, помеченных ВАЖНО.
    public partial class DataContextTree : Window
    {
        public DataContextTree()
        {
            InitializeComponent();

            // Задача 1
            // ВАЖНО: единственное место, где задаётся DataContext для задачи 1.
            Root1.DataContext = new Person { Name = "Анна" };

            // Задача 2
            ReplaceRootButton.Click += (_, _) => ReplaceRoot();
            NullFrameButton.Click   += (_, _) => NullFrame();
            ClearFrameButton.Click  += (_, _) => ClearFrame();
            RenameButton.Click      += (_, _) => Rename();
            Reset2Button.Click      += (_, _) => Reset2();
            Reset2();

            // Задача 3
            ShiftEndedButton.Click += (_, _) => ShiftEnded();
            NewShiftButton.Click   += (_, _) => NewShift();
            Root3.DataContext = new Machine
            {
                Name = "Станок-7",
                Operator = new Person { Name = "Иванов", Shift = "дневная" },
            };
        }

        // ── задача 2 ─────────────────────────────────────────────────────

        private void ReplaceRoot()
        {
            // ВАЖНО: корню кладётся НОВЫЙ объект.
            Root2.DataContext = new Person { Name = "Вера" };
            Report("1. Корню дали новый Person «Вера».");
        }

        private void NullFrame()
        {
            // ВАЖНО: рамке ПРИСВАИВАЕТСЯ null.
            Frame2.DataContext = null;
            Report("2. Рамке присвоили DataContext = null.");
        }

        private void ClearFrame()
        {
            // ВАЖНО: у рамки УБИРАЕТСЯ её собственное значение.
            Frame2.ClearValue(FrameworkElement.DataContextProperty);
            Report("3. У рамки вызвали ClearValue(DataContextProperty).");
        }

        private void Rename()
        {
            // ВАЖНО: объект НЕ заменяется — меняется его свойство.
            if (Root2.DataContext is Person p)
            {
                p.Name = "Дина";
                Report($"4. У объекта в DataContext корня поменяли Name. Сам объект теперь: {p}.");
            }
        }

        private void Reset2()
        {
            Root2.DataContext = new Person { Name = "Анна" };
            Frame2.DataContext = new Person { Name = "Борис" };
            Report("Сброс: корень «Анна», у рамки свой «Борис».");
        }

        private void Report(string what)
        {
            var source = DependencyPropertyHelper
                .GetValueSource(Frame2, FrameworkElement.DataContextProperty)
                .BaseValueSource;
            Status2.Text = $"{what}\nОткуда рамка берёт DataContext: {source}";
        }

        // ── задача 3 ─────────────────────────────────────────────────────

        private void ShiftEnded()
        {
            // ВАЖНО: корню кладётся НОВЫЙ Machine, у которого Operator = null.
            Root3.DataContext = new Machine { Name = "Станок-7", Operator = null };
        }

        private void NewShift()
        {
            // ВАЖНО: корню кладётся НОВЫЙ Machine с новым оператором.
            Root3.DataContext = new Machine
            {
                Name = "Станок-7",
                Operator = new Person { Name = "Петров", Shift = "ночная" },
            };
        }
    }
}
