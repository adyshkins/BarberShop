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
using System.Windows.Shapes;
using BarberShop.ClassHelper;

namespace BarberShop.Windows
{
    /// <summary>
    /// Логика взаимодействия для AddEmployeeWindow.xaml
    /// </summary>
    public partial class AddEmployeeWindow : Window
    {
        public AddEmployeeWindow()
        {
            InitializeComponent();
            cmbSpec.ItemsSource = ClassHelper.AppData.context.Specialization.ToList();
            cmbSpec.DisplayMemberPath = "NameSpecialization";
            cmbSpec.SelectedIndex = 0;
        }

        private void btnAddEmpl_Click(object sender, RoutedEventArgs e)
        {
            // Проверка на пустоту

            if (string.IsNullOrWhiteSpace(txtLastName.Text))
            {
                MessageBox.Show("Поле Фамилия не должно быть пустым", "Ошибка", MessageBoxButton.OK, MessageBoxImage.Error);
                return;
            }

            if (string.IsNullOrWhiteSpace(txtFirsttName.Text))
            {
                MessageBox.Show("Поле Имя не должно быть пустым", "Ошибка", MessageBoxButton.OK, MessageBoxImage.Error);
                return;
            }

            if (string.IsNullOrWhiteSpace(txtPhone.Text))
            {
                MessageBox.Show("Поле Телефон не должно быть пустым", "Ошибка", MessageBoxButton.OK, MessageBoxImage.Error);
                return;
            }

            if (string.IsNullOrWhiteSpace(txtLogin.Text))
            {
                MessageBox.Show("Поле Логин не должно быть пустым", "Ошибка", MessageBoxButton.OK, MessageBoxImage.Error);
                return;
            }

            if (string.IsNullOrWhiteSpace(txtPassword.Password))
            {
                MessageBox.Show("Поле Пароль не должно быть пустым", "Ошибка", MessageBoxButton.OK, MessageBoxImage.Error);
                return;
            }

            // Проверка пароля на совпадение

            if (txtPassword.Password != txtRepeatPassword.Password)
            {
                MessageBox.Show("Пароли не совпадают", "Ошибка", MessageBoxButton.OK, MessageBoxImage.Error);
                return;
            }

            // Проверка на уникальность телефона

            var userAuth = AppData.context.Employee.ToList().
                Where(i => i.Phone == txtPhone.Text).
                FirstOrDefault();

            if (userAuth != null)
            {
                MessageBox.Show("Данный номер телефона уже есть в системе", "Ошибка", MessageBoxButton.OK, MessageBoxImage.Error);
                return;
            }

            // Проверка на уникальность Логина

            var userAuthLogin = AppData.context.Employee.ToList().
                Where(i => i.Login == txtLogin.Text).
                FirstOrDefault();

            if (userAuth != null)
            {
                MessageBox.Show("Данный Логин зянят", "Ошибка", MessageBoxButton.OK, MessageBoxImage.Error);
                return;
            }


            var resClick = MessageBox.Show("Вы уверены?", "Подтверждение", MessageBoxButton.YesNo, MessageBoxImage.Question);


            try
            {
                if (resClick == MessageBoxResult.Yes)
                {

                    EF.Employee addEployee = new EF.Employee();
                    addEployee.FirstName = txtFirsttName.Text;
                    addEployee.LastName = txtLastName.Text;
                    addEployee.IdSpecialization = cmbSpec.SelectedIndex + 1;
                    addEployee.Phone = txtPhone.Text;
                    addEployee.Login = txtLogin.Text;
                    addEployee.Password = txtPassword.Password;

                    ClassHelper.AppData.context.Employee.Add(addEployee);
                    ClassHelper.AppData.context.SaveChanges();

                    MessageBox.Show("Пользователь успещно добавлен", "Успех", MessageBoxButton.OK, MessageBoxImage.Information);
                    this.Close();
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message.ToString());
            }

         
        }
    }
}
