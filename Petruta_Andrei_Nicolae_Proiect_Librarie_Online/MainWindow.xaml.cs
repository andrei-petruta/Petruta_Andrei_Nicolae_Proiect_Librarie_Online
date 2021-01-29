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

        ActionState actionClienti = ActionState.Nothing;
        ActionState actionInventar = ActionState.Nothing;
        ActionState actionComenzi = ActionState.Nothing;

        LibrarieEntitiesModel ctx = new LibrarieEntitiesModel();
        CollectionViewSource clientiViewSource;
        CollectionViewSource inventarViewSource;
        CollectionViewSource clientiComenzisViewSource;


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
            //using System.Data.Entity
            inventarViewSource = ((System.Windows.Data.CollectionViewSource)(this.FindResource("inventarViewSource")));
            inventarViewSource.Source = ctx.Inventars.Local;
            ctx.Inventars.Load();
            //using System.Data.Entity
            clientiComenzisViewSource = ((System.Windows.Data.CollectionViewSource)(this.FindResource("clientiComenzisViewSource")));
            clientiComenzisViewSource.Source = ctx.Comenzis.Local;
            ctx.Comenzis.Load();

            cmbClienti.ItemsSource = ctx.Clientis.Local;
            cmbClienti.DisplayMemberPath = "Prenume";
            cmbClienti.SelectedValuePath = "ClientID";
            cmbInventar.ItemsSource = ctx.Inventars.Local;
            cmbInventar.DisplayMemberPath = "Titlu";
            cmbInventar.SelectedValuePath = "CarteID";

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
            actionClienti = ActionState.New;
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
            actionClienti = ActionState.Edit;
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
            actionClienti = ActionState.Delete;
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
            actionClienti = ActionState.Nothing;

            btnNou.IsEnabled = true;
            btnEditare.IsEnabled = true;
            btnStergere.IsEnabled = true;

            btnSalvare.IsEnabled = false;
            btnAnulare.IsEnabled = false;

            btnPrecedentul.IsEnabled = true;
            btnUrmatorul.IsEnabled = true;

            numeTextBox.Text = "";
            prenumeTextBox.Text = "";
            telefonTextBox.Text = "";


            numeTextBox.IsEnabled = true;
            prenumeTextBox.IsEnabled = true;
            telefonTextBox.IsEnabled = true;

        }

        private void btnSalvare_Click(object sender, RoutedEventArgs e)
        {
            Clienti customer = null;
            if (actionClienti == ActionState.New)
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

                numeTextBox.Text = "";
                prenumeTextBox.Text = "";
                telefonTextBox.Text = "";

                numeTextBox.IsEnabled = true;
                prenumeTextBox.IsEnabled = true;
                telefonTextBox.IsEnabled = true;
            }
            else if (actionClienti == ActionState.Edit)
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

                numeTextBox.Text = "";
                prenumeTextBox.Text = "";
                telefonTextBox.Text = "";

                numeTextBox.IsEnabled = true;
                prenumeTextBox.IsEnabled = true;
                telefonTextBox.IsEnabled = true;
            }
            else if (actionClienti == ActionState.Delete)
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

                numeTextBox.Text = "";
                prenumeTextBox.Text = "";
                telefonTextBox.Text = "";

                numeTextBox.IsEnabled = true;
                prenumeTextBox.IsEnabled = true;
                telefonTextBox.IsEnabled = true;
            }
        }













        // tab-ul Clienti
        private void btnUrmatorulI_Click(object sender, RoutedEventArgs e)
        {
            inventarViewSource.View.MoveCurrentToNext();
        }

        private void btnPrecedentulI_Click(object sender, RoutedEventArgs e)
        {
            inventarViewSource.View.MoveCurrentToPrevious();
        }

        private void btnNouI_Click(object sender, RoutedEventArgs e)
        {
            actionInventar = ActionState.New;
            btnNouI.IsEnabled = false;
            btnEditareI.IsEnabled = false;
            btnStergereI.IsEnabled = false;

            btnSalvareI.IsEnabled = true;
            btnAnulareI.IsEnabled = true;

            btnPrecedentulI.IsEnabled = false;
            btnUrmatorulI.IsEnabled = false;

            titluTextBox.IsEnabled = true;
            autorTextBox.IsEnabled = true;
            pretTextBox.IsEnabled = true;

            BindingOperations.ClearBinding(titluTextBox, TextBox.TextProperty);
            BindingOperations.ClearBinding(autorTextBox, TextBox.TextProperty);
            BindingOperations.ClearBinding(pretTextBox, TextBox.TextProperty);

            titluTextBox.Text = "";
            autorTextBox.Text = "";
            pretTextBox.Text = "";
            Keyboard.Focus(titluTextBox);
        }

        private void btnEditareI_Click(object sender, RoutedEventArgs e)
        {
            actionInventar = ActionState.Edit;
            string tempTitle = titluTextBox.Text.ToString();
            string tempAuthor = autorTextBox.Text.ToString();
            string tempPrice = pretTextBox.Text.ToString();

            btnNouI.IsEnabled = false;
            btnEditareI.IsEnabled = false;
            btnStergereI.IsEnabled = false;

            btnSalvareI.IsEnabled = true;
            btnAnulareI.IsEnabled = true;

            btnPrecedentulI.IsEnabled = false;
            btnUrmatorulI.IsEnabled = false;

            titluTextBox.IsEnabled = true;
            autorTextBox.IsEnabled = true;
            pretTextBox.IsEnabled = true;

            BindingOperations.ClearBinding(titluTextBox, TextBox.TextProperty);
            BindingOperations.ClearBinding(autorTextBox, TextBox.TextProperty);
            BindingOperations.ClearBinding(pretTextBox, TextBox.TextProperty);

            titluTextBox.Text = tempTitle;
            autorTextBox.Text = tempAuthor;
            pretTextBox.Text = tempPrice;
            Keyboard.Focus(titluTextBox);
        }

        private void btnStergereI_Click(object sender, RoutedEventArgs e)
        {
            actionInventar = ActionState.Delete;
            string tempTitle = titluTextBox.Text.ToString();
            string tempAuthor = autorTextBox.Text.ToString();
            string tempPrice = pretTextBox.Text.ToString();

            btnNouI.IsEnabled = false;
            btnEditareI.IsEnabled = false;
            btnStergereI.IsEnabled = false;

            btnSalvareI.IsEnabled = true;
            btnAnulareI.IsEnabled = true;

            btnPrecedentulI.IsEnabled = false;
            btnUrmatorulI.IsEnabled = false;

            BindingOperations.ClearBinding(titluTextBox, TextBox.TextProperty);
            BindingOperations.ClearBinding(autorTextBox, TextBox.TextProperty);
            BindingOperations.ClearBinding(pretTextBox, TextBox.TextProperty);

            titluTextBox.Text = tempTitle;
            autorTextBox.Text = tempAuthor;
            pretTextBox.Text = tempPrice;
        }

        private void btnAnulareI_Click(object sender, RoutedEventArgs e)
        {
            actionInventar = ActionState.Nothing;

            btnNouI.IsEnabled = true;
            btnEditareI.IsEnabled = true;
            btnStergereI.IsEnabled = true;

            btnSalvareI.IsEnabled = false;
            btnAnulareI.IsEnabled = false;

            btnPrecedentulI.IsEnabled = true;
            btnUrmatorulI.IsEnabled = true;

            titluTextBox.Text = "";
            autorTextBox.Text = "";
            pretTextBox.Text = "";


            titluTextBox.IsEnabled = true;
            autorTextBox.IsEnabled = true;
            pretTextBox.IsEnabled = true;

        }

        private void btnSalvareI_Click(object sender, RoutedEventArgs e)
        {
            Inventar book = null;
            if (actionInventar == ActionState.New)
            {
                try
                {
                    //instantiem Inventar entity
                    book = new Inventar()
                    {
                        Titlu = titluTextBox.Text.Trim(),
                        Autor = autorTextBox.Text.Trim(),
                        Pret = int.Parse(pretTextBox.Text),
                    };
                    //adaugam entitatea nou creata in context
                    ctx.Inventars.Add(book);
                    inventarViewSource.View.Refresh();
                    //salvam modificarile
                    ctx.SaveChanges();
                }
                //using System.Data;
                catch (DataException ex)
                {
                    MessageBox.Show(ex.Message);
                }

                btnNouI.IsEnabled = true;
                btnEditareI.IsEnabled = true;
                btnSalvareI.IsEnabled = false;
                btnAnulareI.IsEnabled = false;
                btnStergereI.IsEnabled = true;
                btnPrecedentulI.IsEnabled = true;
                btnUrmatorulI.IsEnabled = true;

                titluTextBox.Text = "";
                autorTextBox.Text = "";
                pretTextBox.Text = "";

                titluTextBox.IsEnabled = true;
                autorTextBox.IsEnabled = true;
                pretTextBox.IsEnabled = true;
            }
            else if (actionInventar == ActionState.Edit)
            {
                try
                {
                    book = (Inventar)inventarDataGrid.SelectedItem;
                    book.Titlu = titluTextBox.Text.Trim();
                    book.Autor = autorTextBox.Text.Trim();
                    book.Pret = int.Parse(pretTextBox.Text);
                    //salvam modificarile
                    ctx.SaveChanges();
                }
                catch (DataException ex)
                {
                    MessageBox.Show(ex.Message);
                }

                // pozitionarea pe item-ul curent
                inventarViewSource.View.Refresh();
                inventarViewSource.View.MoveCurrentTo(book);
                btnNouI.IsEnabled = true;
                btnEditareI.IsEnabled = true;
                btnSalvareI.IsEnabled = false;
                btnAnulareI.IsEnabled = false;
                btnStergereI.IsEnabled = true;
                btnPrecedentulI.IsEnabled = true;
                btnUrmatorulI.IsEnabled = true;

                titluTextBox.Text = "";
                autorTextBox.Text = "";
                pretTextBox.Text = "";

                titluTextBox.IsEnabled = true;
                autorTextBox.IsEnabled = true;
                pretTextBox.IsEnabled = true;
            }
            else if (actionInventar == ActionState.Delete)
            {
                try
                {
                    book = (Inventar)inventarDataGrid.SelectedItem;
                    ctx.Inventars.Remove(book);
                    ctx.SaveChanges();
                }
                catch (DataException ex)
                {
                    MessageBox.Show(ex.Message);
                }
                inventarViewSource.View.Refresh();
                btnNouI.IsEnabled = true;
                btnEditareI.IsEnabled = true;
                btnSalvareI.IsEnabled = false;
                btnAnulareI.IsEnabled = false;
                btnPrecedentulI.IsEnabled = true;
                btnUrmatorulI.IsEnabled = true;

                titluTextBox.Text = "";
                autorTextBox.Text = "";
                pretTextBox.Text = "";

                titluTextBox.IsEnabled = true;
                autorTextBox.IsEnabled = true;
                pretTextBox.IsEnabled = true;
            }
        }

        private void ComboBox_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {

        }











        // tab-ul Comenzi
        private void btnUrmatorulC_Click(object sender, RoutedEventArgs e)
        {
            clientiComenzisViewSource.View.MoveCurrentToNext();
        }

        private void btnPrecedentulC_Click(object sender, RoutedEventArgs e)
        {
            clientiComenzisViewSource.View.MoveCurrentToPrevious();
        }

        private void btnNouC_Click(object sender, RoutedEventArgs e)
        {
            actionComenzi = ActionState.New;
            btnNouC.IsEnabled = false;
            btnEditareC.IsEnabled = false;
            btnStergereC.IsEnabled = false;

            btnSalvareC.IsEnabled = true;
            btnAnulareC.IsEnabled = true;

            btnPrecedentulC.IsEnabled = false;
            btnUrmatorulC.IsEnabled = false;
        }

        private void btnEditareC_Click(object sender, RoutedEventArgs e)
        {
            actionComenzi = ActionState.Edit;

            btnNouC.IsEnabled = false;
            btnEditareC.IsEnabled = false;
            btnStergereC.IsEnabled = false;

            btnSalvareC.IsEnabled = true;
            btnAnulareC.IsEnabled = true;

            btnPrecedentulC.IsEnabled = false;
            btnUrmatorulC.IsEnabled = false;
        }

        private void btnStergereC_Click(object sender, RoutedEventArgs e)
        {
            actionComenzi = ActionState.Delete;

            btnNouC.IsEnabled = false;
            btnEditareC.IsEnabled = false;
            btnStergereC.IsEnabled = false;

            btnSalvareC.IsEnabled = true;
            btnAnulareC.IsEnabled = true;

            btnPrecedentulC.IsEnabled = false;
            btnUrmatorulC.IsEnabled = false;
        }

        private void btnAnulareC_Click(object sender, RoutedEventArgs e)
        {
            actionComenzi = ActionState.Nothing;

            btnNouC.IsEnabled = true;
            btnEditareC.IsEnabled = true;
            btnStergereC.IsEnabled = true;

            btnSalvareC.IsEnabled = false;
            btnAnulareC.IsEnabled = false;

            btnPrecedentulC.IsEnabled = true;
            btnUrmatorulC.IsEnabled = true;
        }

        private void btnSalvareC_Click(object sender, RoutedEventArgs e)
        {
            Comenzi order = null;
            Comenzi selectedOrder = null;
            if (actionComenzi == ActionState.New)
            {
                try
                {
                    //instantiem Comenzi entity
                    Clienti customer = (Clienti)cmbClienti.SelectedItem;
                    Inventar book = (Inventar)cmbInventar.SelectedItem;

                    order = new Comenzi()
                    {
                        ClientID = customer.ClientId,
                        CarteID = book.CarteId,
                    };
                    //adaugam entitatea nou creata in context
                    ctx.Comenzis.Add(order);
                    clientiComenzisViewSource.View.Refresh();
                    //salvam modificarile
                    ctx.SaveChanges();
                }
                //using System.Data;
                catch (DataException ex)
                {
                    MessageBox.Show(ex.Message);
                }
                
                btnNouC.IsEnabled = true;
                btnEditareC.IsEnabled = true;
                btnSalvareC.IsEnabled = false;
                btnAnulareC.IsEnabled = false;
                btnStergereC.IsEnabled = true;
                btnPrecedentulC.IsEnabled = true;
                btnUrmatorulC.IsEnabled = true;
            }
            else if (actionComenzi == ActionState.Edit)
            {
                selectedOrder = (Comenzi)comenzisDataGrid.SelectedItem;


                //customer = (Clienti)clientiDataGrid.SelectedItem;
                //customer.Nume = numeTextBox.Text.Trim();
                //customer.Prenume = prenumeTextBox.Text.Trim();
                //customer.Telefon = telefonTextBox.Text.Trim();
                try
                {

                    int curr_id = selectedOrder.ComandaId;
                    var editedOrder = ctx.Comenzis.FirstOrDefault(s => s.ComandaId == curr_id);

                    if (editedOrder != null)
                    {
                        Clienti customer = (Clienti)cmbClienti.SelectedItem;
                        Inventar book = (Inventar)cmbInventar.SelectedItem;
                        editedOrder.ClientID = customer.ClientId;// int.Parse(cmbClienti.SelectedValue.ToString());
                        editedOrder.CarteID = book.CarteId;// int.Parse(cmbInventar.SelectedValue.ToString());
                        ctx.SaveChanges();
                    }
                    //Clienti customer = (Clienti)cmbClienti.SelectedItem;
                    //Inventar book = (Inventar)cmbInventar.SelectedItem;
                    //order = (Comenzi)comenzisDataGrid.SelectedItem;
                    //order.ClientID = customer;
                    //order.Autor = autorTextBox.Text.Trim();
                    //order.Pret = int.Parse(pretTextBox.Text);
                    //salvam modificarile
                    
                }
                catch (DataException ex)
                {
                    MessageBox.Show(ex.Message);
                }

                // pozitionarea pe item-ul curent
                clientiComenzisViewSource.View.Refresh();
                clientiComenzisViewSource.View.MoveCurrentTo(selectedOrder);
                
                btnNouC.IsEnabled = true;
                btnEditareC.IsEnabled = true;
                btnSalvareC.IsEnabled = false;
                btnAnulareC.IsEnabled = false;
                btnStergereC.IsEnabled = true;
                btnPrecedentulC.IsEnabled = true;
                btnUrmatorulC.IsEnabled = true;
            }
            else if (actionComenzi == ActionState.Delete)
            {
                try
                {
                    selectedOrder = (Comenzi)comenzisDataGrid.SelectedItem;
                    int curr_id = selectedOrder.ComandaId;
                    var deletedOrder = ctx.Comenzis.FirstOrDefault(s => s.ComandaId == curr_id);

                    if (deletedOrder != null)
                    {
                        ctx.Comenzis.Remove(deletedOrder);
                        ctx.SaveChanges();
                        MessageBox.Show("Comanda stearsa cu succes!", "Message");
                    }
                }
                catch (DataException ex)
                {
                    MessageBox.Show(ex.Message);
                }
                clientiComenzisViewSource.View.Refresh();
                btnNouC.IsEnabled = true;
                btnEditareC.IsEnabled = true;
                btnSalvareC.IsEnabled = false;
                btnAnulareC.IsEnabled = false;
                btnPrecedentulC.IsEnabled = true;
                btnUrmatorulC.IsEnabled = true;
            }
        }
    }
    
}