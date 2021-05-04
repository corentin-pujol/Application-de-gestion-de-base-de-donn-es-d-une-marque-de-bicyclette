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
    /// Logique d'interaction pour WindowGestion.xaml
    /// </summary>
    public partial class WindowGestion : Window
    {
        MySqlConnection maConnexion;
        string instance;
        string[] sauvegarde;
        public WindowGestion()
        {



            InitializeComponent();
            VisibiliteHidden();
            instance = "";
            sauvegarde = null;



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
            VisibiliteHidden();
            MySqlCommand commande = maConnexion.CreateCommand();
            commande.CommandText = "Select * from " + instance + ";";



            //MySqlCommand commande = new MySqlCommand("select * from fournisseur;", maConnexion);



            maConnexion.Open();
            DataTable datatab = new DataTable(instance);
            datatab.Load(commande.ExecuteReader());
            maConnexion.Close();
            tab.ItemsSource = datatab.DefaultView;
        }
        public void ExecuterRequete(string requete)
        {
            maConnexion.Open();
            MySqlCommand commandeAjout = maConnexion.CreateCommand();
            commandeAjout.CommandText = requete;
            try
            {
                commandeAjout.ExecuteNonQuery();
            }
            catch (MySqlException i)
            {
                MessageBox.Show(" ErreurConnexion : " + i.ToString());
                return;
            }



            commandeAjout.Dispose();



            maConnexion.Close();



            VisibiliteHidden();
            BindDataGrid();
        }

        #region Suppression
        private void ClickSupprimer(object sender, RoutedEventArgs e)
        {
            VisibiliteHidden();



            DataRowView row = tab.SelectedItem as DataRowView;
            string selection = row.Row.ItemArray[0].ToString();



            string requete = "DELETE FROM " + instance + " WHERE ";



            if (instance == "fournisseur")
            {
                requete += "n°_siret =" + selection;
            }



            if (instance == "commande")
            {
                requete += "n°_commande =" + selection;
            }



            if (instance == "individu")
            {
                requete += "nom_indiv =" + selection;
            }



            if (instance == "boutique")
            {
                requete += "nom_boutique =" + selection;
            }



            MessageBox.Show("Suppression réussie");
            ExecuterRequete(requete);

        }
        #endregion

        #region Modification
        private void ClickModifier(object sender, RoutedEventArgs e)
        {
            VisibiliteHidden();



            if (instance == "fournisseur")
            {
                blocFournisseur.Visibility = Visibility.Visible;
                DataRowView row = tab.SelectedItem as DataRowView;



                sauvegarde = new string[5];
                sauvegarde[0] = row.Row.ItemArray[0].ToString();
                sauvegarde[1] = row.Row.ItemArray[1].ToString();
                sauvegarde[2] = row.Row.ItemArray[2].ToString();
                sauvegarde[3] = row.Row.ItemArray[3].ToString();
                sauvegarde[4] = row.Row.ItemArray[4].ToString();
            }



            if (instance == "commande")
            {
                blocCommande.Visibility = Visibility.Visible;
                DataRowView row = tab.SelectedItem as DataRowView;



                sauvegarde = new string[6];
                sauvegarde[0] = row.Row.ItemArray[0].ToString();
                sauvegarde[1] = row.Row.ItemArray[1].ToString();
                sauvegarde[2] = row.Row.ItemArray[2].ToString();
                sauvegarde[3] = row.Row.ItemArray[3].ToString();
                sauvegarde[4] = row.Row.ItemArray[4].ToString();
                sauvegarde[5] = row.Row.ItemArray[5].ToString();
            }



            if (instance == "individu")
            {
                blocIndividu.Visibility = Visibility.Visible;
                DataRowView row = tab.SelectedItem as DataRowView;



                sauvegarde = new string[6];
                sauvegarde[0] = row.Row.ItemArray[0].ToString();
                sauvegarde[1] = row.Row.ItemArray[1].ToString();
                sauvegarde[2] = row.Row.ItemArray[2].ToString();
                sauvegarde[3] = row.Row.ItemArray[3].ToString();
                sauvegarde[4] = row.Row.ItemArray[4].ToString();
                sauvegarde[5] = row.Row.ItemArray[5].ToString();
            }



            if (instance == "boutique")
            {
                blocEntreprise.Visibility = Visibility.Visible;
                DataRowView row = tab.SelectedItem as DataRowView;



                sauvegarde = new string[6];
                sauvegarde[0] = row.Row.ItemArray[0].ToString();
                sauvegarde[1] = row.Row.ItemArray[1].ToString();
                sauvegarde[2] = row.Row.ItemArray[2].ToString();
                sauvegarde[3] = row.Row.ItemArray[3].ToString();
                sauvegarde[4] = row.Row.ItemArray[4].ToString();
                sauvegarde[5] = row.Row.ItemArray[5].ToString();
            }



            string select = sauvegarde[0];
            for (int i = 1; i < sauvegarde.Length; i++)
            {
                if (sauvegarde[i] == "") sauvegarde[i] = "NULL";
                select += ";" + sauvegarde[i];
            }



            blocSelection.Text = select;
            blocSelection.Visibility = Visibility.Visible;
            buttonOKModif.Visibility = Visibility.Visible;



        }

        private void ModifierBlockSelect(object sender, RoutedEventArgs e)
        {
            string bloc = blocSelection.Text;
            string[] items = bloc.Split(';');
            bool changementAvant = false;
            bool requetePossible = true;
            char g = '"';
            // UPDATE `membres` SET `email`= "jacques@monfai.fr", `pseudo`= "Jacques" WHERE `id`= 1;




            string requete = "UPDATE " + instance + " SET ";



            if (instance == "fournisseur")
            {
                if (sauvegarde[1] != items[1])
                {
                    if (changementAvant == true) requete += ", ";
                    requete += "nom_entreprise = " + g + items[1] + g;
                    changementAvant = true;
                }
                if (sauvegarde[2] != items[2])
                {
                    if (changementAvant == true) requete += ", ";
                    requete += "contact_fournisseur = " + g + items[2] + g;
                    changementAvant = true;
                }
                if (sauvegarde[3] != items[3])
                {
                    if (changementAvant == true) requete += ", ";
                    requete += "adresse_fournisseur = " + g + items[3] + g;
                    changementAvant = true;
                }
                if (sauvegarde[4] != items[4])
                {
                    if (changementAvant == true) requete += ", ";
                    requete += "libelle_fournisseur = " + items[4];
                    changementAvant = true;
                }



                requete += " WHERE n°_siret =" + sauvegarde[0];



                if (sauvegarde[0] != items[0])
                {
                    MessageBox.Show("Impossible : on ne peut modifier la clé primaire n°_siret.");
                    requetePossible = false;
                }
            }



            if (instance == "commande")
            {
                if (sauvegarde[1] != items[1])
                {
                    if (changementAvant == true) requete += ", ";
                    requete += "date_commande = " + g + items[1] + g;
                    changementAvant = true;
                }
                if (sauvegarde[2] != items[2])
                {
                    if (changementAvant == true) requete += ", ";
                    requete += "adresse_livraison = " + g + items[2] + g;
                    changementAvant = true;
                }
                if (sauvegarde[3] != items[3])
                {
                    if (changementAvant == true) requete += ", ";
                    requete += "date_livraison = " + g + items[3] + g;
                    changementAvant = true;
                }
                if (sauvegarde[4] != items[4])
                {
                    //Clé étrangère = nom_indiv donc on teste si le nom que l'on souhaite mdofier existe
                    maConnexion.Open();
                    MySqlCommand command1 = new MySqlCommand("Select * from individu where nom_indiv=" + '"' + items[4] + '"' + ";", maConnexion);
                    if (command1.ExecuteReader().HasRows == true || items[4] == "NULL")
                    {
                        if (changementAvant == true) requete += ", ";
                        requete += "nom_indiv = " + g + items[4] + g;
                        changementAvant = true;
                    }



                    else
                    {
                        requetePossible = false;
                        MessageBox.Show("Impossible : Ce nom de client n'est pas enregistré dans notre base de donnée. Ajoutez-le dans Gestion des clients/entreprises puis réessayez.");
                    }
                    maConnexion.Close();
                }
                if (sauvegarde[5] != items[5])
                {
                    maConnexion.Open();
                    MySqlCommand command1 = new MySqlCommand("Select * from boutique where nom_boutique=" + '"' + items[5] + '"' + ";", maConnexion);
                    if (command1.ExecuteReader().HasRows == true || items[5] == "NULL")
                    {
                        if (changementAvant == true) requete += ", ";
                        requete += "nom_boutique = " + g + items[5] + g;
                        changementAvant = true;
                    }
                    else
                    {
                        requetePossible = false;
                        MessageBox.Show("Impossible : Ce nom de boutique n'est pas enregistré dans notre base de donnée. Ajoutez-la dans Gestion des clients/entreprises puis réessayez.");
                    }
                    maConnexion.Close();



                }




                requete += " WHERE n°_commande =" + sauvegarde[0];



                if (sauvegarde[0] != items[0])
                {
                    MessageBox.Show("Impossible : on ne peut modifier la clé primaire n°_commande.");
                    requetePossible = false;
                }





            }



            if (instance == "individu")
            {
                if (sauvegarde[1] != items[1])
                {
                    if (changementAvant == true) requete += ", ";
                    requete += "prenom_indiv = " + g + items[1] + g;
                    changementAvant = true;
                }
                if (sauvegarde[2] != items[2])
                {
                    if (changementAvant == true) requete += ", ";
                    requete += "adresse_indiv = " + g + items[2] + g;
                    changementAvant = true;
                }
                if (sauvegarde[3] != items[3])
                {
                    if (changementAvant == true) requete += ", ";
                    requete += "telephone_indiv = " + g + items[3] + g;
                    changementAvant = true; 
                }
                if (sauvegarde[4] != items[4])
                {
                    if (changementAvant == true) requete += ", ";
                    requete += "courriel_indiv = " + g + items[4] + g;
                    changementAvant = true; 
                }
                if (sauvegarde[5] != items[5])
                {
                    //Clé étrangère = nom_indiv donc on teste si le nom que l'on souhaite mdofier existe
                    maConnexion.Open();
                    MySqlCommand command1 = new MySqlCommand("Select * from programme where n°_programme=" +  items[5] + ";", maConnexion);
                    if (command1.ExecuteReader().HasRows == true || items[5] == "NULL")
                    {
                        if (changementAvant == true) requete += ", ";
                        requete += "n°_programme = " + items[5];
                        changementAvant = true;
                    }



                    else
                    {
                        requetePossible = false;
                        MessageBox.Show("Impossible : Ce n° de programme n'est pas enregistré dans notre base de donnée. Il doit être compris entre 1 et 4.");
                    }
                    maConnexion.Close();
                }


                requete += " WHERE nom_indiv =" + g + sauvegarde[0] + g;

                
                if (sauvegarde[0] != items[0])
                {
                    MessageBox.Show("Impossible : on ne peut modifier la clé primaire nom_indiv.");
                    requetePossible = false;
                }

            }



            if (instance == "boutique")
            {
                if (sauvegarde[1] != items[1])
                {
                    if (changementAvant == true) requete += ", ";
                    requete += "adresse_boutique = " + g + items[1] + g;
                    changementAvant = true;
                }
                if (sauvegarde[2] != items[2])
                {
                    if (changementAvant == true) requete += ", ";
                    requete += "telephone_boutique = " + g + items[2] + g;
                    changementAvant = true;
                }
                if (sauvegarde[3] != items[3])
                {
                    if (changementAvant == true) requete += ", ";
                    requete += "courriel_boutique = " + g + items[3] + g;
                    changementAvant = true;
                }
                if (sauvegarde[4] != items[4])
                {
                    if (changementAvant == true) requete += ", ";
                    requete += "nom_contact_boutique = " + g + items[4] + g;
                    changementAvant = true;
                }
                if (sauvegarde[5] != items[5])
                {
                    if (changementAvant == true) requete += ", ";
                    requete += "volume_achat = " + items[5];
                    changementAvant = true;
                }



                requete += " WHERE nom_boutique =" + g + sauvegarde[0] + g;



                if (sauvegarde[0] != items[0])
                {
                    MessageBox.Show("Impossible : on ne peut modifier la clé primaire nom_boutique.");
                    requetePossible = false;
                }
            }



            if (requetePossible == true)
            {
                MessageBox.Show("Modification du membre réussie.");
                ExecuterRequete(requete);
                VisibiliteHidden();
            }



        }



        #endregion

        #region Ajout
        private void ClickAjouter(object sender, RoutedEventArgs e)
        {
            VisibiliteHidden();



            if (instance == "fournisseur")
            {
                blocFournisseur.Visibility = Visibility.Visible;
            }



            if (instance == "commande")
            {
                blocCommande.Visibility = Visibility.Visible;
            }



            if (instance == "individu")
            {
                blocIndividu.Visibility = Visibility.Visible;
            }



            if (instance == "boutique")
            {
                blocEntreprise.Visibility = Visibility.Visible;
            }



            blocSelection.Text = "";
            blocSelection.Visibility = Visibility.Visible;
            buttonOKajout.Visibility = Visibility.Visible;
        }



        private void AjouterBlockSelect(object sender, RoutedEventArgs e)
        {
            string bloc = blocSelection.Text;
            string[] items = bloc.Split(';');
            bool requetepossible = true;
            string requete = "INSERT INTO " + instance + " VALUES(";
            char g = '"';



            if (instance == "fournisseur")
            {
                requete += items[0] + ',' + g + items[1] + g + ',' + g + items[2] + g + ',' + g + items[3] + g + ',' + items[4] + ");";
            }



            if (instance == "commande")
            {
                requete += items[0] + ',' + g + items[1] + g + ',' + g + items[2] + g + ',' + g + items[3] + g;
                if (items[4] == "NULL") requete += ",NULL";
                if (items[4] != "NULL")
                {
                    //Clé étrangère = nom_indiv donc on teste si le nom que l'on souhaite mdofier existe
                    maConnexion.Open();
                    MySqlCommand commande = new MySqlCommand("Select * from individu where nom_indiv=" + '"' + items[4] + '"' + ";", maConnexion);
                    if (commande.ExecuteReader().HasRows == true) requete += "," + g + items[4] + g;
                    else
                    {
                        MessageBox.Show("Impossible : Ce nom d'idividu n'est pas enregistré dans notre base de donnée. Ajoutez-le dans Gestion des clients/entreprises puis réessayez.");
                        requetepossible = false;
                    }
                    maConnexion.Close();
                }
                if (items[5] == "NULL") requete += ",NULL);";
                if (items[5] != "NULL")
                {
                    //Clé étrangère =
                    maConnexion.Open();
                    MySqlCommand commande = new MySqlCommand("Select * from boutique where nom_boutique=" + '"' + items[5] + '"' + ";", maConnexion);
                    if (commande.ExecuteReader().HasRows == true) requete += "," + g + items[5] + g + " );";
                    else
                    {
                        MessageBox.Show("Impossible : Ce nom de boutique n'est pas enregistré dans notre base de donnée. Ajoutez-la dans Gestion des clients/entreprises puis réessayez.");
                        requetepossible = false;
                    }
                    maConnexion.Close();
                }
            }



            if (instance == "individu")
            {
                requete += g + items[0] + g + ',' + g + items[1] + g + ',' + g + items[2] + g + ',' + g + items[3] + g + ',' + g + items[4] + g;
                if (items[5] == "NULL") requete += ",NULL);";
                if (items[5] != "NULL")
                {
                    //Clé étrangère =
                    maConnexion.Open();
                    MySqlCommand commande = new MySqlCommand("Select * from programme where n°_programme=" + items[5] + ";", maConnexion);
                    if (commande.ExecuteReader().HasRows == true) requete += "," + items[5] + " );";
                    else
                    {
                        MessageBox.Show("Impossible : Ce n° de programme n'est pas enregistré dans notre base de donnée. Il doit être entre 1 et 4.");
                        requetepossible = false;
                    }
                    maConnexion.Close();
                }
            }



            if (instance == "boutique")
            {
                requete += g + items[0] + g  + ',' + g + items[1] + g + ',' + g + items[2] + g + ',' + g + items[3] + g + ',' + g + items[4] + g + ',' + items[5] + ");";
            }



            if (requetepossible == true)
            {
                MessageBox.Show("Ajout réussi du membre.");
                ExecuterRequete(requete);
                VisibiliteHidden();
            }
        }



        #endregion

        #region Onglets Click Gestion
        private void clickFournisseurs(object sender, RoutedEventArgs e)
        {
            instance = "fournisseur";
            BindDataGrid();
        }



        private void ClickCommande(object sender, RoutedEventArgs e)
        {
            instance = "commande";
            BindDataGrid();
        }



        private void ClickIndividu(object sender, RoutedEventArgs e)
        {
            instance = "individu";
            BindDataGrid();
        }

        private void ClickEntreprise(object sender, RoutedEventArgs e)
        {
            instance = "boutique";
            BindDataGrid();
        }
        #endregion

        public void VisibiliteHidden()
        {
            blocSelection.Visibility = Visibility.Hidden;
            buttonOKajout.Visibility = Visibility.Hidden;
            buttonOKModif.Visibility = Visibility.Hidden;

            blocFournisseur.Visibility = Visibility.Hidden;
            blocCommande.Visibility = Visibility.Hidden;
            blocIndividu.Visibility = Visibility.Hidden;
            blocEntreprise.Visibility = Visibility.Hidden;
        }

    }
}
