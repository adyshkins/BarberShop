using System;
using System.Collections.Generic;
using System.IO;
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
using Microsoft.Win32;

namespace BarberShop.Windows
{
    /// <summary>
    /// Логика взаимодействия для AddEmployeeWindow.xaml
    /// </summary>
    public partial class AddEmployeeWindow : Window
    {

        EF.Employee editEmployee = new EF.Employee();
        bool isEdit = true;
        private string pathPhoto = null;

        public AddEmployeeWindow()
        {
            InitializeComponent();
            cmbSpec.ItemsSource = ClassHelper.AppData.context.Specialization.ToList();
            cmbSpec.DisplayMemberPath = "NameSpecialization";
            cmbSpec.SelectedIndex = 0;

            isEdit = false;
        }

        public AddEmployeeWindow(EF.Employee employee)
        {
            InitializeComponent();

            cmbSpec.ItemsSource = ClassHelper.AppData.context.Specialization.ToList();
            cmbSpec.DisplayMemberPath = "NameSpecialization";
            cmbSpec.SelectedIndex = Convert.ToInt32(employee.IdSpecialization - 1);

            txtFirsttName.Text = employee.FirstName;
            txtLastName.Text = employee.LastName;
            txtPhone.Text = employee.Phone;
            txtLogin.Text = employee.Login;
            txtPassword.Password = employee.Password;

            tbTitle.Text = "Изменение данных работника";
            btnAddEmpl.Content = "Изменить";

            // вывод изображения из БД в Image
            if (employee.Photo != null)
            {
                using (MemoryStream stream = new MemoryStream(employee.Photo))
                {
                    BitmapImage bitmapImage = new BitmapImage();
                    bitmapImage.BeginInit();
                    bitmapImage.CacheOption = BitmapCacheOption.OnLoad;
                    bitmapImage.CreateOptions = BitmapCreateOptions.PreservePixelFormat;
                    bitmapImage.StreamSource = stream;
                    bitmapImage.EndInit();
                    photoUser.Source = bitmapImage;
                }

            }

            editEmployee = employee;
            isEdit = true;
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

            //var userAuth = AppData.context.Employee.ToList().
            //    Where(i => i.Phone == txtPhone.Text).
            //    FirstOrDefault();

            //if (userAuth != null)
            //{
            //    MessageBox.Show("Данный номер телефона уже есть в системе", "Ошибка", MessageBoxButton.OK, MessageBoxImage.Error);
            //    return;
            //}

            // Проверка на уникальность Логина

            //var userAuthLogin = AppData.context.Employee.ToList().
            //    Where(i => i.Login == txtLogin.Text).
            //    FirstOrDefault();

            //if (userAuth != null)
            //{
            //    MessageBox.Show("Данный Логин зянят", "Ошибка", MessageBoxButton.OK, MessageBoxImage.Error);
            //    return;
            //}


            var resClick = MessageBox.Show("Вы уверены?", "Подтверждение", MessageBoxButton.YesNo, MessageBoxImage.Question);

            try
            {
                if (resClick == MessageBoxResult.Yes)
                {

                    if (isEdit)
                    {
                        // изменение
                        editEmployee.FirstName = txtFirsttName.Text;
                        editEmployee.LastName = txtLastName.Text;
                        editEmployee.IdSpecialization = cmbSpec.SelectedIndex + 1;
                        editEmployee.Phone = txtPhone.Text;
                        editEmployee.Login = txtLogin.Text;
                        editEmployee.Password = txtPassword.Password;
                        if (pathPhoto != null)
                        {
                            editEmployee.Photo = File.ReadAllBytes(pathPhoto);
                        }
                       

                        ClassHelper.AppData.context.SaveChanges();

                        MessageBox.Show("Пользователь успещно изменен", "Успех", MessageBoxButton.OK, MessageBoxImage.Information);
                        this.Close();
                    }
                    else
                    {
                        // добавление
                        EF.Employee addEployee = new EF.Employee();
                        addEployee.FirstName = txtFirsttName.Text;
                        addEployee.LastName = txtLastName.Text;
                        addEployee.IdSpecialization = cmbSpec.SelectedIndex + 1;
                        addEployee.Phone = txtPhone.Text;
                        addEployee.Login = txtLogin.Text;
                        addEployee.Password = txtPassword.Password;
                        if (pathPhoto != null)
                        {
                            editEmployee.Photo = File.ReadAllBytes(pathPhoto);
                        }
                        ClassHelper.AppData.context.Employee.Add(addEployee);
                        ClassHelper.AppData.context.SaveChanges();

                        MessageBox.Show("Пользователь успещно добавлен", "Успех", MessageBoxButton.OK, MessageBoxImage.Information);
                        this.Close();
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message.ToString());
            }

         
        }

        private void btnChoosePhoto_Click(object sender, RoutedEventArgs e)
        {
            OpenFileDialog openFile = new OpenFileDialog();
            if (openFile.ShowDialog() == true)
            {
                photoUser.Source = new BitmapImage(new Uri(openFile.FileName));
                pathPhoto = openFile.FileName;
            }
        }
    }
}
