using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;

namespace CSharpLearn.Block7
{
    // Код-подложка для задачи 2. Механизм событий — Блок 3; здесь он
    // используется как чёрный ящик: нажатие кнопки вызывает метод.
    // Разбирать этот файл не нужно, кроме трёх строк, помеченных ВАЖНО.
    public partial class Resources : Window
    {
        public Resources()
        {
            InitializeComponent();

            ReplaceButton.Click += (_, _) => Replace();
            MutateButton.Click += (_, _) => Mutate();
            ResetButton.Click += (_, _) => Reset();
        }

        // Словарь, в котором лежит DemoBrush, — это Resources той панели,
        // где он объявлен. Достаём её через имя кнопки: кнопка и панель
        // в одном дереве, поэтому родительский DockPanel находится вверх по нему.
        private ResourceDictionary DemoScope =>
            ((FrameworkElement)ReplaceButton.Parent).Parent is FrameworkElement panel
                ? panel.Resources
                : Resources;

        private void Replace()
        {
            // ВАЖНО: в словарь кладётся НОВЫЙ объект по тому же ключу.
            DemoScope["DemoBrush"] = new SolidColorBrush(Colors.SeaGreen);
            StatusText.Text = "Заменили ресурс: в словарь положен НОВЫЙ объект по ключу DemoBrush.";
        }

        private void Mutate()
        {
            // ВАЖНО: объект из словаря не заменяется — меняется его свойство.
            if (DemoScope["DemoBrush"] is SolidColorBrush brush && !brush.IsFrozen)
            {
                brush.Color = Colors.RoyalBlue;
                StatusText.Text = "Изменили кисть: тот же объект, у него поменяли Color.";
            }
            else
            {
                StatusText.Text = "Кисть заморожена — изменить нельзя (см. граблю 6 в конспекте).";
            }
        }

        private void Reset()
        {
            // ВАЖНО: возвращаем исходное состояние — снова НОВЫЙ объект.
            DemoScope["DemoBrush"] = new SolidColorBrush(Colors.IndianRed);
            StatusText.Text = "Сброс. Учти: сброс — это тоже ЗАМЕНА, а не правка.";
        }
    }
}
