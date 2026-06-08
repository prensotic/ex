using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Windows;
using System.Windows.Controls;

namespace Module_4
{
    public partial class DashboardWindow : Window
    {
        public DashboardWindow()
        {
            InitializeComponent();

            LoadUsers();
        }

        private void LoadUsers()
        {
            List<UserViewModel> users =
                new List<UserViewModel>();

            using (SqlConnection connection =
                DataBaseHelper.GetConnection())
            {
                connection.Open();

                string query =
                    "SELECT id, login, Role, isActive FROM Users";

                SqlCommand command =
                    new SqlCommand(query, connection);

                SqlDataReader reader =
                    command.ExecuteReader();

                while (reader.Read())
                {
                    users.Add(new UserViewModel
                    {
                        id = Convert.ToInt32(reader["id"]),
                        login = reader["login"].ToString(),
                        Role = reader["Role"].ToString(),
                        isActive = Convert.ToBoolean(reader["isActive"])
                    });
                }
            }

            dgUsers.ItemsSource = users;
        }

        private void AddUser_Click(object sender,
            RoutedEventArgs e)
        {


            string login = tbLogin.Text;
            string password = pbPassword.Password;


            string role =
                (cbRole.SelectedItem as ComboBoxItem)?
                .Content.ToString();

            if (string.IsNullOrWhiteSpace(login))
            {
                MessageBox.Show("Введите логин");
                return;
            }

            if (string.IsNullOrWhiteSpace(password))
            {
                MessageBox.Show("Введите пароль");
                return;
            }

            if (string.IsNullOrWhiteSpace(role))
            {
                MessageBox.Show("Выберите роль");
                return;
            }

            using (SqlConnection connection =
                DataBaseHelper.GetConnection())
            {
                connection.Open();

                string checkQuery =
                    "SELECT COUNT(*) FROM Users WHERE login=@login";

                SqlCommand checkCommand =
                    new SqlCommand(checkQuery, connection);

                checkCommand.Parameters.AddWithValue(
                    "@login",
                    login);

                int count =
                    (int)checkCommand.ExecuteScalar();

                if (count > 0)
                {
                    MessageBox.Show(
                        "Пользователь уже существует");

                    return;
                }

                string insertQuery =
                    @"INSERT INTO Users
                    (login,password,isActive,Role)
                    VALUES
                    (@login,@password,1,@role)";

                SqlCommand insertCommand =
                    new SqlCommand(insertQuery,
                    connection);

                insertCommand.Parameters.AddWithValue(
                    "@login",
                    login);

                insertCommand.Parameters.AddWithValue(
                    "@password",
                    password);

                insertCommand.Parameters.AddWithValue(
                    "@role",
                    role);

                insertCommand.ExecuteNonQuery();
            }

            MessageBox.Show(
                "Пользователь добавлен");

            LoadUsers();
        }

        private void DeleteUser_Click(object sender,
    RoutedEventArgs e)
        {
            UserViewModel selected =
                dgUsers.SelectedItem as UserViewModel;

            if (selected == null)
            {
                MessageBox.Show(
                    "Выберите пользователя");

                return;
            }

            MessageBoxResult result =
                MessageBox.Show(
                    $"Удалить пользователя {selected.login}?",
                    "Подтверждение",
                    MessageBoxButton.YesNo,
                    MessageBoxImage.Question);

            if (result != MessageBoxResult.Yes)
                return;

            using (SqlConnection connection =
                DataBaseHelper.GetConnection())
            {
                connection.Open();

                string query =
                    "DELETE FROM Users WHERE id=@id";

                SqlCommand command =
                    new SqlCommand(query, connection);

                command.Parameters.AddWithValue(
                    "@id",
                    selected.id);

                command.ExecuteNonQuery();
            }

            MessageBox.Show(
                "Пользователь удален");

            LoadUsers();
        }

        private void UnblockUser_Click(object sender,
            RoutedEventArgs e)
        {
            dynamic selected =
                dgUsers.SelectedItem;

            if (selected == null)
            {
                MessageBox.Show(
                    "Выберите пользователя");

                return;
            }

            using (SqlConnection connection =
                DataBaseHelper.GetConnection())
            {
                connection.Open();

                string query =
                    @"UPDATE Users
                      SET isActive = 1
                      WHERE id = @id";

                SqlCommand command =
                    new SqlCommand(query,
                    connection);

                command.Parameters.AddWithValue(
                    "@id",
                    selected.id);

                command.ExecuteNonQuery();
            }

            MessageBox.Show(
                "Пользователь разблокирован");

            LoadUsers();
        }
    }
}

public class UserViewModel
{
    public int id { get; set; }

    public string login { get; set; }

    public string Role { get; set; }

    public bool isActive { get; set; }
}