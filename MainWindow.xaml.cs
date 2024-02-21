using System.Runtime.ConstrainedExecution;
using System.Runtime.InteropServices;
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
using static System.Net.Mime.MediaTypeNames;

namespace Task_Manager
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        struct Task
        {
            //string title, short status, string description, List<string> roles = null
            public string title;
            public short status;
            public string descripton;
            public List<string> roles;
            
        }

        struct Project
        {
            public string projectTitle;
            public List<Task> projectTasks;
        }



        List<Project> listOfProjects = new List<Project>();

        public MainWindow()
        {
            InitializeComponent();
            securityPanel.Visibility = Visibility.Visible;
            
        }


        int a = 1;
        string s = "Новый проект";
        readonly Brush? fff = new BrushConverter().ConvertFromString("#FFFFFFFF") as Brush;
        readonly Brush? ff0 = new BrushConverter().ConvertFromString("#FF0096D5") as Brush;
        readonly Brush? black = new BrushConverter().ConvertFromString("#FF000000") as Brush;

        private void projectsReveal (List<Project> lst)
        {
            projectsPanel.Children.Clear();
            
            foreach(Project prj in lst)
            {
                projectCreator(prj.projectTitle);
            }
        }

        private void tasksReveal (Project proj)
        {
            tasksPanel.Children.Clear();
            projectTitle.Text = proj.projectTitle;
            foreach(Task task in proj.projectTasks)
            {
                taskCreator(task.title, task.status, task.descripton, task.roles);
            }
        }

        private void newProjectButton_Click(object sender, RoutedEventArgs e)
        {
            projectCreator(s);
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
        short i = 0;
        private void Button_Click(object sender, RoutedEventArgs e)
        {

            taskCreator("Hello", i, "Hello everyone you need to hack some servers :) Let's talk about it later!", new List<string>() {"None"});
            i++;
            i %= 5;
        }

        private void projectCreator(string text)
        {
            Border btn = new Border();
            Brush? clr = ff0;
            btn.MouseLeftButtonUp += projectChoose;
            btn.Width = 210;
            btn.Height = double.NaN;

            TextBlock txtb = new TextBlock();

            txtb.Text = text;
            txtb.TextWrapping = TextWrapping.Wrap;
            txtb.Margin = (Thickness)new ThicknessConverter().ConvertFromString("7 4");
            txtb.Foreground = fff;
            btn.Child = txtb;
            btn.CornerRadius = new CornerRadius(3);
            btn.BorderThickness = new Thickness(2);

            //btn.chil
            btn.Margin = (Thickness)new ThicknessConverter().ConvertFromString("0 3 0 0");
            //Height = "34" Width = "170" Content = "Создать новый проект" FontSize = "12" Background = "#FF0096D5" BorderBrush = "#FF0096D5" Click = "newProjectButton_Click"
            btn.BorderBrush = clr;
            btn.Background = clr;

            projectsPanel.Children.Add(btn);
        }

        private void taskCreator(string title, short status, string description, List<string> roles = null)
        {
            //< Border Background = "White" CornerRadius = "10" BorderThickness = "2" BorderBrush = "#FF0096D5" Padding = "10" Margin = "0 0 0 15" >
            Border brd = new Border();
            brd.Background = fff;
            brd.CornerRadius = (CornerRadius) new CornerRadiusConverter().ConvertFromString("10");
            brd.BorderThickness = (Thickness)new ThicknessConverter().ConvertFromString("2");
            brd.BorderBrush = ff0;
            brd.Padding = (Thickness)new ThicknessConverter().ConvertFromString("10"); 
            brd.Margin = (Thickness)new ThicknessConverter().ConvertFromString("0 0 0 15");

            StackPanel stk = new StackPanel();

            /*< TextBlock Text = "Название таска 1" FontSize = "14" Margin = "0 0 0 5" />
            < TextBlock Text = "Описание жесть lorem ipsum" FontSize = "12" Foreground = "DarkSlateGray" Margin = "0 0 0 5" />
            < TextBlock Text = "Роли" FontSize = "12" Foreground = "DarkSlateGray" />*/

            TextBlock txtb = new TextBlock();
            txtb.Text = title;
            txtb.FontSize = 14;
            txtb.TextWrapping = TextWrapping.Wrap;
            txtb.Margin = (Thickness)new ThicknessConverter().ConvertFromString("0 0 0 5");
            stk.Children.Add(txtb);

            TextBlock txtb_status = new TextBlock();
            switch (status)
            {
                case 0:
                    txtb_status.Text = "Не начато";
                    txtb_status.Foreground = (Brush)new BrushConverter().ConvertFromString("Gray");
                    break;
                case 1:
                    txtb_status.Text = "В процессе";
                    txtb_status.Foreground = (Brush)new BrushConverter().ConvertFromString("Purple");
                    break;
                case 2:
                    txtb_status.Text = "Приостановлено";
                    txtb_status.Foreground = (Brush)new BrushConverter().ConvertFromString("Orange");
                    break;
                case 3:
                    txtb_status.Text = "Брошено";
                    txtb_status.Foreground = (Brush)new BrushConverter().ConvertFromString("Red");
                    break;
                case 4:
                    txtb_status.Text = "Завершено";
                    txtb_status.Foreground = (Brush)new BrushConverter().ConvertFromString("Green");
                    break;
            }
            txtb_status.FontWeight = (FontWeight) new FontWeightConverter().ConvertFromString("Bold");
            txtb_status.FontSize = 14;
            txtb_status.TextWrapping = TextWrapping.Wrap;
            
            txtb_status.Margin = (Thickness)new ThicknessConverter().ConvertFromString("0 0 0 5");
            stk.Children.Add(txtb_status);

            TextBlock txtb2 = new TextBlock();
            txtb2.Text = description;
            txtb2.FontSize = 12;
            txtb2.TextWrapping = TextWrapping.Wrap;
            txtb2.Foreground = (Brush)new BrushConverter().ConvertFromString("DarkSlateGray");
            txtb2.Margin = (Thickness)new ThicknessConverter().ConvertFromString("0 0 0 5");
            stk.Children.Add(txtb2);

            TextBlock txtb3 = new TextBlock();
            txtb3.Text = String.Join(", ", roles.ToArray());
            txtb3.TextWrapping = TextWrapping.Wrap;
            txtb3.Foreground = (Brush)new BrushConverter().ConvertFromString("DarkSlateGray");
            txtb3.FontSize = 12;
            stk.Children.Add(txtb3);

            brd.Child = stk;
            tasksPanel.Children.Add(brd);
        }

    }
}