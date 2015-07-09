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
using System.Net;
using System.Net.Sockets;

namespace ClientLivretA
{
    /// <summary>
    /// Logique d'interaction pour MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            InitializeComponent();
        }

        private void btnQuitter_Click(object sender, RoutedEventArgs e)
        {
            Application.Current.Shutdown();
        }

        private void btnValider_Click(object sender, RoutedEventArgs e)
        {
            // Adresse du serveur à joindre
            IPAddress ip_addressServer = IPAddress.Parse("127.0.0.1");
            int i_userAnnee;
            double d_userSommeInit;
            byte[] b_userAnnee, b_userSommeInit, b_serverSomFinalCalc = new byte[8];
            Socket sock = null;

            try
            {
                // Check userData

                i_userAnnee = int.Parse(nbAnnees.Text);
                d_userSommeInit = double.Parse(sommeInitiale.Text);
                b_userAnnee = BitConverter.GetBytes(i_userAnnee);
                b_userSommeInit = BitConverter.GetBytes(d_userSommeInit);

                // Création de la socket
                sock = new Socket(AddressFamily.InterNetwork, SocketType.Stream, ProtocolType.Tcp);

                // Connexion au serveur
                sock.Connect(ip_addressServer, 1212);

                //MessageBox.Show("Connexion établie au serveur " + ip_addressServer);

                //Envoie des données
                sock.Send(b_userAnnee);
                sock.Send(b_userSommeInit);
                sock.Receive(b_serverSomFinalCalc);
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
                return;
            }
            finally
            {
                // Fermeture de la connexion
                sock.Shutdown(SocketShutdown.Both);
                sock.Close();
            }

            
            // Affiche du résultat
            sommeFinale.Text = BitConverter.ToDouble(b_serverSomFinalCalc, 0).ToString("F") ;
        }

        private void data_TextChanged(object sender, TextChangedEventArgs e)
        {
            if (!(String.IsNullOrWhiteSpace(nbAnnees.Text)) && !(String.IsNullOrWhiteSpace(sommeInitiale.Text)))
            {
                btnValider.IsEnabled = true;
            }
        }
    }
}
