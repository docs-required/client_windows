using System.Runtime.ConstrainedExecution;
using System.Security.Cryptography;
using System.Text;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using System.Windows.Xps;

namespace Task_Manager
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            InitializeComponent();
            securityPanel.Visibility = Visibility.Visible;
        }
        int a = 1;
        string s = "Новый проект";
        Brush? fff = new BrushConverter().ConvertFromString("#FFFFFFFF") as Brush;
        Brush? ff0 = new BrushConverter().ConvertFromString("#FF0096D5") as Brush;
        Brush? black = new BrushConverter().ConvertFromString("#FF000000") as Brush;

        private void newProjectButton_Click(object sender, RoutedEventArgs e)
        {
            Border btn = new Border();
            Brush? clr = ff0;
            btn.MouseLeftButtonUp += projectChoose;
            btn.Width = 210;
            btn.Height = double.NaN;

            TextBlock txtb = new TextBlock();
            s += " a as qwe " + s;
            
            txtb.Text = s;
            txtb.TextWrapping = TextWrapping.Wrap;
            txtb.Margin = (Thickness)new ThicknessConverter().ConvertFromString("7 4");
            txtb.Foreground = fff;
            btn.Child = txtb;
            btn.CornerRadius = new CornerRadius(3);
            btn.BorderThickness = new Thickness(2);
            
            //btn.chil
            btn.Margin = (Thickness) new ThicknessConverter().ConvertFromString("0 3 0 0");
            //Height = "34" Width = "170" Content = "Создать новый проект" FontSize = "12" Background = "#FF0096D5" BorderBrush = "#FF0096D5" Click = "newProjectButton_Click"
            btn.BorderBrush = clr;
            btn.Background =  clr;
            
            projectsPanel.Children.Add(btn);
        }

        private void projectChoose (object Sender, RoutedEventArgs e)
        {
            
            foreach (UIElement x in projectsPanel.Children)
            {
                if (x is Border)
                {
                    ((Border) x).Background = ff0;
                    ((TextBlock) ((Border)x).Child).Foreground = fff;
                }
            }
            ((Border) Sender).Background = fff;
            ((TextBlock)((Border) Sender).Child).Foreground = black;
        }

        private void loginButton_Click(object sender, RoutedEventArgs e)
        {
            if (loginText.Text == "admin" && passwordText.Password == "admin")
            {
                securityPanel.Visibility = Visibility.Hidden;
                leftColumn.IsEnabled = true;
                centerColumn.IsEnabled = true;
                rightColumn.IsEnabled = true;
            } else
            {
                wrongText.Visibility = Visibility.Visible;
            }
        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            ;
        }
    }
}