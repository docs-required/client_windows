using System.Net.Sockets;
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
        Project testProj = new();

        public MainWindow()
        {
            InitializeComponent();
            securityPanel.Visibility = Visibility.Visible;


            projectChangePanel.Height = 0;
            projectChangePanel.Margin = (Thickness)new ThicknessConverter().ConvertFromString("0");


            testProj.projectTitle = "Дизайн сайта для продажи мерча и тд (ТЗ нет, но надо чота придумать)";
            testProj.projectTasks = new List<Task>();
            Task tsk = new();
            tsk.title = "Шапка";
            tsk.status = 1;
            tsk.descripton = "Шапка крч надо сделать дизайн шапки, много всего (докбары там... хлебные крошки...)";
            tsk.roles = new List<string> {"Дизайнер"};
            testProj.projectTasks.Add(tsk);
            tsk.title = "Шапка (но уже делаем)";
            tsk.status = 0;
            tsk.descripton = "Шапка крч надо сделать саму шапку, всё, что дал дизайнер";
            tsk.roles = new List<string> { "Программист" };
            testProj.projectTasks.Add(tsk);
            tsk.title = "Подвал (дизайн)";
            tsk.status = 4;
            tsk.descripton = "Подвал, задизайнить";
            tsk.roles = new List<string> { "Дизайнер" };
            testProj.projectTasks.Add(tsk);
            tsk.title = "Подвал (но уже делаем)";
            tsk.status = 0;
            tsk.descripton = "Подвал крч надо сделать саму подвал, всё, что дал дизайнер";
            tsk.roles = new List<string> { "Программист" };
            testProj.projectTasks.Add(tsk);
            listOfProjects.Add(testProj);
            projectsReveal(listOfProjects);
            tasksReveal(listOfProjects[0]);
        }

        
       

        private static string connect(string req = "client_get_teams:{1234}")
        {
            string answer = string.Empty;
            try
            {
                TcpClient client = new TcpClient("141.105.64.173", 8787);

                NetworkStream stream = client.GetStream();

                byte[] bytesWrite = Encoding.UTF8.GetBytes(req);
                stream.Write(bytesWrite, 0, bytesWrite.Length);


                byte[] bytesRead = new byte[256];
                int length = stream.Read(bytesRead, 0, bytesRead.Length);
                answer = Encoding.UTF8.GetString(bytesRead, 0, length);

                stream.Write(Encoding.UTF8.GetBytes("disconnect"), 0, Encoding.UTF8.GetBytes("disconnect").Length);

                client.Close();
            }
            catch (Exception e)
            {
                MessageBox.Show(e.Message);
            }
            return answer.Trim();
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

        private string hash_id = string.Empty;
        private void loginButton_Click(object sender, RoutedEventArgs e)
        {
            if (secureLabel.Text == "Авторизация")
                if (connect("client_login:{" + hash(loginText.Text) +";" + hash(passwordText.Password + " ") + "}") == "true")
                {
                    securityPanel.Visibility = Visibility.Hidden;
                    hash_id = hash(loginText.Text);
                    leftColumn.IsEnabled = true;
                    centerColumn.IsEnabled = true;
                    rightColumn.IsEnabled = true;
                    profileName.Text = connect("client_get_name:{" + hash_id + "}");
                } else
                {
                    wrongText.Text = "Неправильный логин и/или пароль";
                    wrongText.Visibility = Visibility.Visible;
                }
            else
            {
                if (connect("client_register:{" + hash(loginText.Text) + $";{((nameText.Text.Trim() != "") ? nameText.Text.Trim() : "Новый пользователь")};{((descriptionText.Text.Trim() != "") ? descriptionText.Text.Trim() : "Новый пользователь")};" + hash(passwordText.Password + " ") + "}") == "true")
                {
                    securityPanel.Visibility = Visibility.Hidden;
                    hash_id = hash(loginText.Text);
                    leftColumn.IsEnabled = true;
                    centerColumn.IsEnabled = true;
                    rightColumn.IsEnabled = true;
                    profileName.Text = connect("client_get_name:{" + hash_id + "}");
                }
                else
                {
                    wrongText.Text = "Данный пользователь уже зарегистрирован";
                    wrongText.Visibility = Visibility.Visible;
                }
            }
        }
        short i = 0;
        private void Button_Click(object sender, RoutedEventArgs e)
        {

            connect();
            if (projectChangePanel.Height == 0)
            {
                projectChangePanel.Margin = (Thickness)new ThicknessConverter().ConvertFromString("0 20 0 0");
                projectChangePanel.Height = 200;
            }
            else
            {
                projectChangePanel.Height = 0;
                projectChangePanel.Margin = (Thickness)new ThicknessConverter().ConvertFromString("0");
            }

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

        public static String hash(String value)
        {
            StringBuilder Sb = new StringBuilder();
            using (SHA256 hash = SHA256.Create())
            {
                Encoding enc = Encoding.UTF8;
                Byte[] result = hash.ComputeHash(enc.GetBytes(value));
                foreach (Byte b in result)
                    Sb.Append(b.ToString("x2"));
            }
            return Sb.ToString();
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

        private void regButton_Click(object sender, RoutedEventArgs e)
        {
            secureLabel.Text = (string)regButton.Content;
            if ((string) regButton.Content == "Регистрация")
            {
                regButton.Content = "Авторизация";
                nameText.IsEnabled = true;
                descriptionText.IsEnabled = true;
                nameText.Height = 40;
                descriptionText.Height = 40;
                nameText.Margin = (Thickness)new ThicknessConverter().ConvertFromString("0 0 0 20");
                descriptionText.Margin = (Thickness)new ThicknessConverter().ConvertFromString("0 0 0 20");
            } else
            {
                regButton.Content = "Регистрация";
                nameText.IsEnabled = false;
                descriptionText.IsEnabled = false;
                nameText.Height = 0;
                descriptionText.Height = 0;
                nameText.Margin = (Thickness)new ThicknessConverter().ConvertFromString("0");
                descriptionText.Margin = (Thickness)new ThicknessConverter().ConvertFromString("0");
            } 
        }

        private void Button_Click_1(object sender, RoutedEventArgs e)
        {
            MessageBox.Show(hash(taskChooser.Text));
        }
    }
}