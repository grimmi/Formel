using Formel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace FormelGui
{
    /// <summary>
    /// Interaktionslogik für MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        public IResolver Resolver { get; }
        public IEnumerable<MenuItem> ResolverVariables =>
            Resolver.ProvidedVariables.Select(v => new MenuItem
            {
                Header = $"{Resolver.GetDescription(v)} ({v})",
                Command = new ReplaceSpanCommand(v),
                CommandParameter = this,
                ToolTip = Resolver.GetDescription(v),
                Icon = new Image { Source = new BitmapImage(new Uri("images/variable.png", UriKind.Relative)) }
            });

        public Span CurrentClickedSpan { get; set; }
        public Run CurrentClickedRun { get; set; }

        public MainWindow()
        {
            Resolver = new DictionaryResolver(new Dictionary<string, decimal>
            {
                ["${abc}"] = 1m,
                ["${factor}"] = 10m,
                ["${multiplier}"] = 0.5m
            }, new Dictionary<string, string>
            {
                ["${abc}"] = "the abc factor",
                ["${factor}"] = "very important",
                ["${multiplier}"] = "the thing that's to be multiplied with"
            });
            InitializeComponent();
            var contextMenu = FindResource("spanMenu") as ContextMenu;
            contextMenu.DataContext = this;
            entryBox.Focus();
        }

        private void ContextMenu_Closed(object sender, RoutedEventArgs e)
        {
            if (CurrentClickedSpan != null)
            {
                CurrentClickedSpan.Background = (SolidColorBrush)(new BrushConverter().ConvertFrom("#FFA1DDFF"));
                CurrentClickedSpan = null;
            }
            if (CurrentClickedRun != null)
            {
                CurrentClickedRun.Background = (SolidColorBrush)(new BrushConverter().ConvertFrom("#FFA1DDFF"));
                CurrentClickedRun = null;
            }
        }

        private void Span_MouseDown(object sender, MouseButtonEventArgs e)
        {
            if (e.RightButton == MouseButtonState.Pressed)
            {
                CurrentClickedSpan = (sender as Span);
                CurrentClickedSpan.Background = Brushes.LightCoral;
            }
        }

        private void RichTextBox_TextChanged(object sender, TextChangedEventArgs e)
        {
        }

        private void Run_MouseDown(object sender, MouseButtonEventArgs e)
        {
            if (e.RightButton == MouseButtonState.Pressed)
            {
                CurrentClickedRun = (sender as Run);
                CurrentClickedRun.Background = Brushes.LightCoral;
            }
        }
    }

    public class ReplaceSpanCommand : ICommand
    {
        public event EventHandler CanExecuteChanged;

        public string Text { get; }

        public ReplaceSpanCommand(string withText)
        {
            Text = withText;
        }

        public bool CanExecute(object parameter)
        {
            return true;
        }

        public void Execute(object parameter)
        {
            var mainWindow = parameter as MainWindow;
            var clickedSpan = mainWindow.CurrentClickedSpan;
            if (clickedSpan != null)
            {
                var firstInline = clickedSpan.Inlines.FirstInline as Run;
                firstInline.Text = Text;
                clickedSpan.Background = (SolidColorBrush)(new BrushConverter().ConvertFrom("#FFA1DDFF"));
            }
            var clickedRun = mainWindow.CurrentClickedRun;
            if (clickedRun != null)
            {
                clickedRun.Text = Text;
                clickedRun.Background = (SolidColorBrush)(new BrushConverter().ConvertFrom("#FFA1DDFF"));
            }
        }
    }
}
