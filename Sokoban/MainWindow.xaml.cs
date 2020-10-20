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

namespace Sokoban
{
    /// <summary>
    /// Logique d'interaction pour MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        Jeu jeu;

        public MainWindow()
        {
            InitializeComponent();
            jeu = new Jeu();

            this.KeyDown += MainWindow_KeyDown;

            Dessiner();
        }

        private void MainWindow_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.Key.Equals(Key.Right) || e.Key.Equals(Key.Left) || e.Key.Equals(Key.Down) || e.Key.Equals(Key.Up))
            {
                jeu.ToucheAppuyer(e.Key);
                Redessinner();

                if (jeu.fini())
                {
                    MessageBoxResult msg = MessageBox.Show("bravo, vous avez gangé en " + jeu.NbDeplacement + " mouvements.", "reconmmencer ? ", MessageBoxButton.YesNo, MessageBoxImage.Information);
                    if (msg == MessageBoxResult.Yes)
                    {
                        jeu.Restart();
                        Redessinner();
                    }
                }
            }
        }

        void Redessinner()
        {
            cnvMobiles.Children.Clear();
            DessinerCaisses();
            DessinerPersonnage();
            AfficherNbDeplacement();
        }

        private void AfficherNbDeplacement()
        {
            txtNbDeplacement.Text = jeu.NbDeplacement.ToString();
        }

        private void Dessiner()
        {
            DessinerCarte();
            Redessinner();
            
        }


        private void DessinerPersonnage()
        {
            Rectangle r = new Rectangle();
            r.Width = 30;
            r.Height = 30;
            r.Margin = new Thickness(jeu.Personnage.y * 50 + 10, jeu.Personnage.x * 50 + 10, 0, 0);
            r.Fill = new ImageBrush(new BitmapImage(new Uri("images/superhero.png", UriKind.Relative)));

            cnvMobiles.Children.Add(r);
        }


        private void DessinerCaisses()
        {
            foreach(Position pose in jeu.Caisses)
            {
                Rectangle r = new Rectangle();
                r.Width = 42;
                r.Height = 42;
                r.Margin = new Thickness(pose.y * 50 +4, pose.x * 50 +4, 0, 0);
                r.Fill = new ImageBrush(new BitmapImage(new Uri("images/box.png", UriKind.Relative)));

                cnvMobiles.Children.Add(r);
            }
        }


        private void DessinerCarte()
        {
            for (int ligne = 0; ligne < 10; ligne++)
            {
                for (int colonne = 0; colonne < 10; colonne++)
                {
                    Rectangle r = new Rectangle();
                    r.Width = 50;
                    r.Height = 50;
                    r.Margin = new Thickness(colonne * 50, ligne * 50, 0, 0);

                    switch(jeu.Case(ligne, colonne))
                    {
                        case Jeu.Etat.Vide:
                            r.Fill = new ImageBrush(new BitmapImage(new Uri("images/parquet.png", UriKind.Relative)));
                            break;
                        case Jeu.Etat.Mur:
                            r.Fill = new ImageBrush(new BitmapImage(new Uri("images/wall.png", UriKind.Relative)));
                            break;
                        case Jeu.Etat.Cible:
                            r.Fill = new ImageBrush(new BitmapImage(new Uri("images/target.png", UriKind.Relative)));
                            break;
                    }


                    cnvGrille.Children.Add(r);
                }
            }
        }

        private void btnRecommencer_click(object send, System.Windows.RoutedEventArgs e)
        {
            jeu.Restart();
            Redessinner();
        }

    }
}
