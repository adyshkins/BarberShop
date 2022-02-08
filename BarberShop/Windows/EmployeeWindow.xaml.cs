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

namespace BarberShop.Windows
{
    /// <summary>
    /// Логика взаимодействия для EmployeeWindow.xaml
    /// </summary>
    public partial class EmployeeWindow : Window
    {
        List<EF.Employee> listEmployee = new List<EF.Employee>();

        List<string> listForSort = new List<string>() 
        {
            "По умолчанию",
            "По фимилии",
            "По имени",
            "По телефону",
            "По специализации"
        };

        public EmployeeWindow()
        {
            InitializeComponent();
            cmbSort.ItemsSource = listForSort;
            cmbSort.SelectedIndex = 0;
            Filter();
        }

        private void Filter()
        {
            listEmployee = ClassHelper.AppData.context.Employee.ToList();
            listEmployee = listEmployee.
                Where(i => i.LastName.Contains(txtSearch.Text) 
                || i.FirstName.Contains(txtSearch.Text) 
                || i.Phone.Contains(txtSearch.Text)).ToList();

            switch (cmbSort.SelectedIndex)
            {
                case 0: 
                    listEmployee = listEmployee.OrderBy(i => i.Id).ToList();
                    break;

                case 1:
                    listEmployee = listEmployee.OrderBy(i => i.LastName).ToList();
                    break;

                case 2:
                    listEmployee = listEmployee.OrderBy(i => i.FirstName).ToList();
                    break;

                case 3:
                    listEmployee = listEmployee.OrderBy(i => i.Phone).ToList();
                    break;

                case 4:
                    listEmployee = listEmployee.OrderBy(i => i.IdSpecialization).ToList();
                    break;

                default:
                    listEmployee = listEmployee.OrderBy(i => i.Id).ToList();
                    break;
            }

            if (listEmployee.Count == 0)
            {
                MessageBox.Show("Записей нет");
            }
            lvEmployee.ItemsSource = listEmployee;
        }

        private void btnAddEmployee_Click(object sender, RoutedEventArgs e)
        {
            Windows.AddEmployeeWindow addEmployeeWindow = new AddEmployeeWindow();
            this.Opacity = 0.2;
            addEmployeeWindow.ShowDialog();
            this.Opacity = 1;
            Filter();
        }

        private void txtSearch_TextChanged(object sender, TextChangedEventArgs e)
        {
            Filter();
        }

        private void cmbSort_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            Filter();
        }
    }
}
