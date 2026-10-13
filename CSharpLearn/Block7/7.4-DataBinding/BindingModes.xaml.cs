using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;

namespace CSharpLearn.Block7
{
    /// <summary>Сотрудник. Сообщать об изменениях своих свойств НЕ умеет — это тема 7.6.</summary>
    public class Employee
    {
        /// <summary>Имя.</summary>
        public string Name { get; set; } = "Анна";

        /// <summary>Возраст.</summary>
        public int Age { get; set; } = 30;
    }

    // Код-подложка. Механизм событий — Блок 3; здесь он используется как
    // чёрный ящик: нажатие кнопки вызывает метод. Разбирать файл не нужно,
    // кроме строк, помеченных ВАЖНО.
    public partial class BindingModes : Window
    {
        private readonly Employee _employee = new();
        private readonly (TextBox Box, BindingBase Binding)[] _modeBindings;

        public BindingModes()
        {
            InitializeComponent();

            // Задача 2: запоминаем биндинги из разметки, чтобы «Сброс» мог
            // подключить их заново, как при открытии окна.
            _modeBindings = new[] { OneWayBox, TwoWayBox, OneTimeBox, ToSourceBox }
                .Select(box => (box, (BindingBase)BindingOperations.GetBinding(box, TextBox.TextProperty)))
                .ToArray();

            LevelTo20Button.Click += (_, _) => LevelTo20();
            Reset2Button.Click += (_, _) => Reset2();
            Report2("шаг 0: окно открыто");

            // Задача 3
            // ВАЖНО: DataContext вкладки — обычный объект, без уведомлений.
            Root3.DataContext = _employee;
            ShowButton.Click += (_, _) => ShowEmployee();
            RenameButton.Click += (_, _) => RenameInCode();
        }

        // ── задача 2 ─────────────────────────────────────────────────────

        private void LevelTo20()
        {
            // ВАЖНО: меняется сам ползунок — источник всех четырёх биндингов.
            Level.Value = 20;
            Report2("кнопка «Ползунок → 20»");
        }

        private void Reset2()
        {
            Level.Value = 10;
            foreach (var (box, binding) in _modeBindings)
            {
                BindingOperations.ClearBinding(box, TextBox.TextProperty);
                box.Text = "";
                box.SetBinding(TextBox.TextProperty, binding);
            }
            Report2("сброс: ползунок 10, биндинги подключены заново");
        }

        private void Report2(string what) =>
            Status2.Text = $"{what}. Ползунок сейчас: {Level.Value}";

        // ── задача 3 ─────────────────────────────────────────────────────

        private void ShowEmployee() =>
            // ВАЖНО: читается сам объект, а не поля на экране.
            Status3.Text = $"В объекте сейчас: Name = «{_employee.Name}», Age = {_employee.Age}";

        private void RenameInCode()
        {
            // ВАЖНО: присваивание в коде, мимо биндингов.
            _employee.Name = "Вера";
            Status3.Text = "В коде выполнено employee.Name = \"Вера\".";
        }
    }
}
