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
using MySql.Data.MySqlClient;
using System.Data;

namespace BDD_VeloMax
{
    /// <summary>
    /// Logique d'interaction pour Assemblage.xaml
    /// </summary>
    public partial class Assemblage : Window
    {
        MySqlConnection maConnexion;
        string instance;
        public Assemblage()
        {
            InitializeComponent();
            //VisibiliteHidden();
            instance = "";



            this.maConnexion = null;
            try
            {
                string connexionString = "SERVER=localhost;PORT=3306;" +
                "DATABASE=velomax;" +
                "UID=root;PASSWORD=Cocobis882022.";



                this.maConnexion = new MySqlConnection(connexionString);
                //maConnexion.Open();
            }
            catch (MySqlException e)
            {
                Console.WriteLine(" ErreurConnexion : " + e.ToString());
                return;
            }
        }

        public void BindDataGrid()
        {
            //VisibiliteHidden();
            MySqlCommand commande = maConnexion.CreateCommand();
            commande.CommandText = "Select * from " + instance + " WHERE n°_liste=" + ComboBoxAssemblage.SelectedItem + ";";



            //MySqlCommand commande = new MySqlCommand("select * from fournisseur;", maConnexion);



            maConnexion.Open();
            DataTable datatab = new DataTable(instance);
            datatab.Load(commande.ExecuteReader());
            maConnexion.Close();
            tab.ItemsSource = datatab.DefaultView;
        }

        private void ComboBoxAssemblage_Loaded(object sender, RoutedEventArgs e)
        {
            maConnexion.Open();
            this.instance = "listeassemblage";
            MySqlCommand commande2 = new MySqlCommand("Select n°_liste from ListeAssemblage;", maConnexion);
            MySqlDataReader reader2 = commande2.ExecuteReader();
            int cpt = 0;
            List<string> liste_string = new List<string>();
            while (reader2.Read())
            {
                if (reader2.GetValue(cpt) != null)
                {
                    string result = reader2.GetValue(cpt).ToString();
                    liste_string.Add(result);
                }
            }
            var combo = sender as ComboBox;
            combo.ItemsSource = liste_string;
            reader2.Close();
            maConnexion.Close();
        }

        private void ButtonValider_Click(object sender, RoutedEventArgs e)
        {
            if (ComboBoxAssemblage.SelectedItem != null)
            {
                BindDataGrid();
            }
        }

        private void ButtonImpression_Click(object sender, RoutedEventArgs e)
        {
            PrintDialog printDialog = new PrintDialog();
            if (printDialog.ShowDialog() == true)
            {
                printDialog.PrintVisual(tab, "My First Print Job");
            }
        }
    }
}
