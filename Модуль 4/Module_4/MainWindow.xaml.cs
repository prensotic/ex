using System;
using System.Collections.Generic;
using System.Data.SqlClient;
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

namespace Module_4
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        private int attempts = 0;

        private bool captchaPassed = false;

        private Random random = new Random();

        public MainWindow()
        {
            InitializeComponent();

            LoadCaptcha();
        }

        private void LoadCaptcha()
        {
            SetImage(img1, "Images/1.png");
            SetImage(img2, "Images/2.png");
            SetImage(img3, "Images/3.png");
            SetImage(img4, "Images/4.png");
        }

        private void SetImage(Image image, string path)
        {
            image.Source = new BitmapImage(new Uri(path, UriKind.Relative));

            int angle = random.Next(0, 4) * 90;

            RotateTransform rotate = new RotateTransform(angle);

            image.RenderTransform = rotate;

            image.RenderTransformOrigin = new Point(0.5, 0.5);
        }

        private void Image_Click(object sender, MouseButtonEventArgs e)
        {
            Image image = sender as Image;

            RotateTransform rotate =
                image.RenderTransform as RotateTransform;

            rotate.Angle += 90;

            if (CheckCaptcha())
            {
                captchaPassed = true;

                MessageBox.Show("Капча пройдена");
            }
        }

        private bool CheckCaptcha()
        {
            return
                ((RotateTransform)img1.RenderTransform).Angle % 360 == 0 &&
                ((RotateTransform)img2.RenderTransform).Angle % 360 == 0 &&
                ((RotateTransform)img3.RenderTransform).Angle % 360 == 0 &&
                ((RotateTransform)img4.RenderTransform).Angle % 360 == 0;
        }

        private void Login_Click(object sender, RoutedEventArgs e)
        {
            if (!captchaPassed)
            {
                MessageBox.Show("Сначала соберите капчу");

                return;
            }

            string username = tbUsername.Text;
            string password = pbPassword.Password;

            if (DataBaseHelper.IsBlocked(username))
            {
                MessageBox.Show("Пользователь заблокирован");

                return;
            }

            bool success = DataBaseHelper.Login(username, password);

            if (success)
            {
                attempts = 0;

                string role = "";

                using (SqlConnection connection =
                    DataBaseHelper.GetConnection())
                {
                    connection.Open();

                    string query =
                        "SELECT Role FROM Users WHERE login=@login";

                    SqlCommand command =
                        new SqlCommand(query, connection);

                    command.Parameters.AddWithValue("@login", username);

                    role = command.ExecuteScalar().ToString();
                }

                if (role == "admin")
                {
                    MessageBox.Show("Админ");
                }
                else
                {
                    MessageBox.Show("Пользователь");
                }


                this.Close();
            }
            else
            {
                attempts++;

                MessageBox.Show("Неверный логин или пароль");

                if (attempts >= 3)
                {
                    DataBaseHelper.BlockUser(username);

                    MessageBox.Show("Пользователь заблокирован");
                }
            }
        }
    }
}