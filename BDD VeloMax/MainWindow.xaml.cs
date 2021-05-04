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
using System.Windows.Navigation;
using System.Windows.Shapes;
using MySql.Data.MySqlClient;
using System.Data;
using System.Web;
using System.Net.Mail;
using System.Security.Cryptography;

namespace BDD_VeloMax
{
    /// <summary>
    /// Logique d'interaction pour MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        MySqlConnection maConnexion;
        string identifiant;
        string hash_string;
        public MainWindow()
        {
            InitializeComponent();

            this.maConnexion = null;
            this.identifiant = "";
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



        private void ConfirmeEntree(object sender, RoutedEventArgs e)
        {
            if(blocIdtxt.Text=="")
            {
                MessageBox.Show("Veuillez entrer votre identifiant (adresse mail).");
            }
            else
            {
                this.identifiant = blocIdtxt.Text;

                //On vérifie que l'identifiant existe bien dans sla base de données
                maConnexion.Open();
                MySqlCommand command1 = new MySqlCommand("Select * from identifiant where id=" + '"' + identifiant + '"' + ";", maConnexion);
                MySqlDataReader reader1 = command1.ExecuteReader();
                if (reader1.HasRows == true)
                {
                    reader1.Close();
                    if (blocPassewordtxt.Text == "")
                    {
                        MessageBox.Show("Veuillez entrer votre mot de passe.");
                        maConnexion.Close();
                    }
                    else
                    {
                        string passeword = blocPassewordtxt.Text;
                        byte[] textAsByte = Encoding.Default.GetBytes(passeword);
                        SHA512 sha512 = SHA512.Create();  
                        byte[] hash = sha512.ComputeHash(textAsByte);

                        this.hash_string = Convert.ToBase64String(hash);
                        MySqlCommand commande2 = new MySqlCommand("Select hash from identifiant where id=" + '"' + identifiant + '"' + ";",maConnexion);
                        MySqlDataReader reader2 = commande2.ExecuteReader();
                        if(reader2.Read() && reader2.GetValue(0)!=null)
                        {
                            string result = reader2.GetValue(0).ToString();
                            if (this.hash_string != result)
                            {
                                MessageBox.Show("Mot de passe incorrect, veuillez réessayer.");
                            }
                            else
                            {
                                WindowGestion wGestion = new WindowGestion();
                                MessageBox.Show("Identification réussie.Bienvenue.");
                                wGestion.Show();
                            }
                        }
                        else
                        {
                            MessageBox.Show("Erreur requête SQL");
                        }
                        reader2.Close();
                        maConnexion.Close();
                    }
                }
                else
                {
                    MessageBox.Show("L'identifiant n'existe pas");
                    maConnexion.Close();
                }
            }
        }

        private void NewPasseword(object sender, RoutedEventArgs e)
        {
            ConfirmerEntree.Visibility = Visibility.Hidden;
            ButtonCreation.Visibility = Visibility.Hidden;
            if (blocIdtxt.Text == "")
            {
                MessageBox.Show("Veuillez entrer votre identifiant (adresse mail).");
            }
            else
            {
                this.identifiant = blocIdtxt.Text;

                //On vérifie que l'identifiant existe bien dans sla base de données
                maConnexion.Open();
                MySqlCommand command1 = new MySqlCommand("Select * from identifiant where id=" + '"' + identifiant + '"' + ";", maConnexion);
                MySqlDataReader reader1 = command1.ExecuteReader();
                if (reader1.HasRows == true)
                {
                    reader1.Close();
                    if (blocPassewordtxt.Text == "")
                    {
                        MessageBox.Show("Veuillez entrer votre nouveau mot de passe.");
                        maConnexion.Close();
                    }
                    else
                    {
                        string passeword = blocPassewordtxt.Text;
                        byte[] textAsByte = Encoding.Default.GetBytes(passeword);
                        SHA512 sha512 = SHA512.Create();
                        byte[] hash = sha512.ComputeHash(textAsByte);

                        this.hash_string = Convert.ToBase64String(hash);
                        MySqlCommand commande2 = new MySqlCommand("UPDATE identifiant SET hash=" + '"' + this.hash_string + '"' + "WHERE id=" + '"' + this.identifiant + '"' + ";", maConnexion);
                        try
                        {
                            commande2.ExecuteNonQuery();
                        }
                        catch (MySqlException i)
                        {
                            MessageBox.Show(" ErreurConnexion : " + i.ToString());
                            return;
                        }
                        MessageBox.Show("Modification du mot de passe réussie.");
                        ConfirmerEntree.Visibility = Visibility.Visible;
                        ButtonCreation.Visibility = Visibility.Visible;
                        maConnexion.Close();
                    }
                }
                else
                {
                    MessageBox.Show("L'identifiant n'existe pas");
                    maConnexion.Close();
                }
            }
        }

        private void PageCreationID(object sender, RoutedEventArgs e)
        {
            ConfirmerEntree.Visibility = Visibility.Hidden;
            ButtonOublie.Visibility = Visibility.Hidden;
            if (blocIdtxt.Text == "")
            {
                MessageBox.Show("Veuillez entrer votre identifiant (adresse mail).");
            }
            else
            {
                this.identifiant = blocIdtxt.Text;

                //On vérifie que l'identifiant n'existe pas déjà dans la base de données
                maConnexion.Open();
                MySqlCommand command1 = new MySqlCommand("Select * from identifiant where id=" + '"' + identifiant + '"' + ";", maConnexion);
                MySqlDataReader reader1 = command1.ExecuteReader();
                if (reader1.HasRows != true)
                {
                    reader1.Close();
                    if (blocPassewordtxt.Text == "")
                    {
                        MessageBox.Show("Veuillez entrer votre nouveau mot de passe.");
                        maConnexion.Close();
                    }
                    else
                    {
                        string passeword = blocPassewordtxt.Text;
                        byte[] textAsByte = Encoding.Default.GetBytes(passeword);
                        SHA512 sha512 = SHA512.Create();
                        byte[] hash = sha512.ComputeHash(textAsByte);

                        this.hash_string = Convert.ToBase64String(hash);
                        MySqlCommand commande2 = new MySqlCommand("INSERT INTO identifiant VALUES(" + '"' + identifiant + '"' + ',' + '"' + hash_string + '"' + ")",maConnexion);
                        try
                        {
                            commande2.ExecuteNonQuery();
                        }
                        catch (MySqlException i)
                        {
                            MessageBox.Show(" ErreurConnexion : " + i.ToString());
                            return;
                        }
                        MessageBox.Show("Création du membre réussie, vous pouvez dès maintenant vous connecter.");
                        ConfirmerEntree.Visibility = Visibility.Visible;
                        ButtonOublie.Visibility = Visibility.Visible;
                        maConnexion.Close();
                    }
                }
                else
                {
                    MessageBox.Show("L'identifiant existe déjà, veuillez en choisir un autre ou alors, veuillez vous connecter en cliquant sur confirmer.");
                    ConfirmerEntree.Visibility = Visibility.Visible;
                    ButtonOublie.Visibility = Visibility.Visible;
                    maConnexion.Close();
                }
            }
        }
    }
}
