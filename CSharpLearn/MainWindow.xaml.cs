using System;
using System.Linq;
using System.Windows;

namespace CSharpLearn
{
    // Окно-заглушка со списком тем. Список берётся из TopicWindows,
    // чтобы не дублировать реестр в разметке.
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            InitializeComponent();

            TopicList.Text = string.Join(
                Environment.NewLine,
                TopicWindows.All.Select(t => $"{t.Id}   {t.Title}"));
        }
    }
}
