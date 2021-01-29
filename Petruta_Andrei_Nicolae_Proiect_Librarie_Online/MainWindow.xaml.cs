using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
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
using LibrarieModel;

namespace Petruta_Andrei_Nicolae_Proiect_Librarie_Online
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>

    enum ActionState
    {
        New,
        Edit,
        Delete,
        Nothing
    }

    public partial class MainWindow : Window
    {
        // using LibrarieModel
        ActionState action = ActionState.Nothing;
        LibrarieEntitiesModel ctx = new LibrarieEntitiesModel();
        CollectionViewSource clientiViewSource;
        public MainWindow()
        {
            InitializeComponent();
            DataContext = this;
        }

        private void Window_Loaded(object sender, RoutedEventArgs e)
        {

            //using System.Data.Entity
            clientiViewSource = ((System.Windows.Data.CollectionViewSource)(this.FindResource("clientiViewSource")));
            clientiViewSource.Source = ctx.Clientis.Local;
            ctx.Clientis.Load();
            System.Windows.Data.CollectionViewSource inventarViewSource = ((System.Windows.Data.CollectionViewSource)(this.FindResource("inventarViewSource")));
            // Load data by setting the CollectionViewSource.Source property:
            // inventarViewSource.Source = [generic data source]
        }


        // tab-ul Clienti
        private void btnUrmatorul_Click(object sender, RoutedEventArgs e)
        {
            clientiViewSource.View.MoveCurrentToNext();
        }

        private void btnPrecedentul_Click(object sender, RoutedEventArgs e)
        {
            clientiViewSource.View.MoveCurrentToPrevious();
        }

        private void btnNou_Click(object sender, RoutedEventArgs e)
        {
            action = ActionState.New;
            btnNou.IsEnabled = false;
            btnEditare.IsEnabled = false;
            btnStergere.IsEnabled = false;

            btnSalvare.IsEnabled = true;
            btnAnulare.IsEnabled = true;

            btnPrecedentul.IsEnabled = false;
            btnUrmatorul.IsEnabled = false;

            numeTextBox.IsEnabled = true;
            prenumeTextBox.IsEnabled = true;
            telefonTextBox.IsEnabled = true;

            BindingOperations.ClearBinding(numeTextBox, TextBox.TextProperty);
            BindingOperations.ClearBinding(prenumeTextBox, TextBox.TextProperty);
            BindingOperations.ClearBinding(telefonTextBox, TextBox.TextProperty);

            numeTextBox.Text = "";
            prenumeTextBox.Text = "";
            telefonTextBox.Text = "";
            Keyboard.Focus(numeTextBox);
        }

        private void btnEditare_Click(object sender, RoutedEventArgs e)
        {
            action = ActionState.Edit;
            string tempFirstName = numeTextBox.Text.ToString();
            string tempLastName = prenumeTextBox.Text.ToString();
            string tempTelephone = telefonTextBox.Text.ToString();

            btnNou.IsEnabled = false;
            btnEditare.IsEnabled = false;
            btnStergere.IsEnabled = false;

            btnSalvare.IsEnabled = true;
            btnAnulare.IsEnabled = true;

            btnPrecedentul.IsEnabled = false;
            btnUrmatorul.IsEnabled = false;

            numeTextBox.IsEnabled = true;
            prenumeTextBox.IsEnabled = true;
            telefonTextBox.IsEnabled = true;

            BindingOperations.ClearBinding(numeTextBox, TextBox.TextProperty);
            BindingOperations.ClearBinding(prenumeTextBox, TextBox.TextProperty);
            BindingOperations.ClearBinding(telefonTextBox, TextBox.TextProperty);

            numeTextBox.Text = tempFirstName;
            prenumeTextBox.Text = tempLastName;
            telefonTextBox.Text = tempTelephone;
            Keyboard.Focus(numeTextBox);
        }

        private void btnStergere_Click(object sender, RoutedEventArgs e)
        {
            action = ActionState.Delete;
            string tempFirstName = numeTextBox.Text.ToString();
            string tempLastName = prenumeTextBox.Text.ToString();
            string tempTelephone = telefonTextBox.Text.ToString();

            btnNou.IsEnabled = false;
            btnEditare.IsEnabled = false;
            btnStergere.IsEnabled = false;

            btnSalvare.IsEnabled = true;
            btnAnulare.IsEnabled = true;

            btnPrecedentul.IsEnabled = false;
            btnUrmatorul.IsEnabled = false;

            BindingOperations.ClearBinding(numeTextBox, TextBox.TextProperty);
            BindingOperations.ClearBinding(prenumeTextBox, TextBox.TextProperty);
            BindingOperations.ClearBinding(telefonTextBox, TextBox.TextProperty);

            numeTextBox.Text = tempFirstName;
            prenumeTextBox.Text = tempLastName;
            telefonTextBox.Text = tempTelephone;
        }

        private void btnAnulare_Click(object sender, RoutedEventArgs e)
        {
            action = ActionState.Nothing;

            btnNou.IsEnabled = true;
            btnEditare.IsEnabled = true;
            btnStergere.IsEnabled = true;

            btnSalvare.IsEnabled = false;
            btnAnulare.IsEnabled = false;

            btnPrecedentul.IsEnabled = true;
            btnUrmatorul.IsEnabled = true;


            numeTextBox.IsEnabled = false;
            prenumeTextBox.IsEnabled = false;
            telefonTextBox.IsEnabled = false;
        }

        private void btnSalvare_Click(object sender, RoutedEventArgs e)
        {
            Clienti customer = null;
            if (action == ActionState.New)
            {
                try
                {
                    //instantiem Customer entity
                    customer = new Clienti()
                    {
                        Nume = numeTextBox.Text.Trim(),
                        Prenume = prenumeTextBox.Text.Trim(),
                        Telefon = telefonTextBox.Text.Trim(),
                    };
                    //adaugam entitatea nou creata in context
                    ctx.Clientis.Add(customer);
                    clientiViewSource.View.Refresh();
                    //salvam modificarile
                    ctx.SaveChanges();
                }
                //using System.Data;
                catch (DataException ex)
                {
                    MessageBox.Show(ex.Message);
                }

                btnNou.IsEnabled = true;
                btnEditare.IsEnabled = true;
                btnSalvare.IsEnabled = false;
                btnAnulare.IsEnabled = false;
                btnStergere.IsEnabled = true;
                btnPrecedentul.IsEnabled = true;
                btnUrmatorul.IsEnabled = true;
                numeTextBox.IsEnabled = false;
                prenumeTextBox.IsEnabled = false;
                telefonTextBox.IsEnabled = false;
            }
            else if (action == ActionState.Edit)
            {
                try
                {
                    customer = (Clienti)clientiDataGrid.SelectedItem;
                    customer.Nume = numeTextBox.Text.Trim();
                    customer.Prenume = prenumeTextBox.Text.Trim();
                    customer.Telefon = telefonTextBox.Text.Trim();
                    //salvam modificarile
                    ctx.SaveChanges();
                }
                catch (DataException ex)
                {
                    MessageBox.Show(ex.Message);
                }

                // pozitionarea pe item-ul curent
                clientiViewSource.View.Refresh();
                clientiViewSource.View.MoveCurrentTo(customer);
                btnNou.IsEnabled = true;
                btnEditare.IsEnabled = true;
                btnSalvare.IsEnabled = false;
                btnAnulare.IsEnabled = false;
                btnStergere.IsEnabled = true;
                btnPrecedentul.IsEnabled = true;
                btnUrmatorul.IsEnabled = true;
                numeTextBox.IsEnabled = false;
                prenumeTextBox.IsEnabled = false;
                telefonTextBox.IsEnabled = false;
            }
            else if (action == ActionState.Delete)
            {
                try
                {
                    customer = (Clienti)clientiDataGrid.SelectedItem;
                    ctx.Clientis.Remove(customer);
                    ctx.SaveChanges();
                }
                catch (DataException ex)
                {
                    MessageBox.Show(ex.Message);
                }
                clientiViewSource.View.Refresh();
                btnNou.IsEnabled = true;
                btnEditare.IsEnabled = true;
                btnSalvare.IsEnabled = false;
                btnAnulare.IsEnabled = false;
                btnPrecedentul.IsEnabled = true;
                btnUrmatorul.IsEnabled = true;
                numeTextBox.IsEnabled = false;
                prenumeTextBox.IsEnabled = false;
                telefonTextBox.IsEnabled = false;
            }
        }
    }
    
}