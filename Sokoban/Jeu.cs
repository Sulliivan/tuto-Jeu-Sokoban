using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;


namespace Sokoban
{
    class Jeu
    {
        public enum Etat
        {
            Vide,
            Mur,
            Cible
        }

        public Jeu()
        {
            grille = new Etat[10, 10];
            InitCarte();
            nbDeplacement = 0;
        }


        private Etat[,] grille;

        private List<Position> caisses;
        public List<Position> Caisses
        {
            get { return caisses; }
        }

        private int nbDeplacement;
        public int NbDeplacement
        {
            get { return nbDeplacement; }
        }


        private Position personnage;
        public Position Personnage
        {
            get { return personnage; }
        }

        static string grilletTxt = "..XXXXXX..XXX.oo.XXXX..o..o..XX........XXXX....XXX..XX.CXX...XXXC.XXX..X.CP.C.X..X......X..XXXXXXXX.";


      
        private void InitCarte()
        {
            // creer une liste vide de caisses
            caisses = new List<Position>();

            // Pour chaque case, initialise la bonne valeur
            // ajoute les caisses si besoin
            // Determine la position de depart du personnage
            for (int ligne = 0; ligne < 10; ligne++)
            {
                for (int colonne = 0; colonne < 10; colonne++)
                {
                    switch (grilletTxt[ligne * 10 + colonne])
                    {
                        case '.':
                            grille[ligne, colonne] = Etat.Vide;
                            break;
                        case 'X':
                            grille[ligne, colonne] = Etat.Mur;
                            break;
                        case 'o':
                            grille[ligne, colonne] = Etat.Cible;
                            break;
                        case 'C':
                            Caisses.Add(new Position(ligne, colonne));
                            grille[ligne, colonne] = Etat.Vide;
                            break;
                        case 'P':
                            personnage = new Position(ligne, colonne);
                            grille[ligne, colonne] = Etat.Vide;
                            break;

                    }
                }
            }
        }

        internal bool fini()
        {
            foreach(Position caisse in caisses)
            {
                if (grille[caisse.x, caisse.y] != Etat.Cible)
                {
                    return false;
                }
            }
            return true;
        }

        public Etat Case(int ligne, int colonne)
        {
            return grille[ligne, colonne];
        }

        public void ToucheAppuyer(Key key)
        {
            Position newPos = new Position(personnage.x, personnage.y);

            NewMethod(newPos, key);

            if(CaseOk(newPos, key))
            {
                personnage = newPos;
                nbDeplacement++;
            }

        }

        private void NewMethod(Position newPos,Key key)
        {
            switch (key)
            {
                case Key.Down:
                    newPos.x++;
                    break;
                case Key.Up:
                    newPos.x--;
                    break;
                case Key.Left:
                    newPos.y--;
                    break;
                case Key.Right:
                    newPos.y++;
                    break;
            }

            if (CaseOk(newPos, key))
            {
                personnage = newPos;
            }
        }

        private bool CaseOk(Position newPos, Key key)
        {
            //presence d'un mur
            if (grille[newPos.x, newPos.y] == Etat.Mur)
            {
                return false;
            }
            // presence de caisse
            Position caisse = CaisseInPos(newPos);
            if (caisse != null)
            {
                // deplacement de la caisse
                Position newPosCaisse = new Position(caisse.x, caisse.y);
                NewMethod(newPosCaisse, key);

                if (grille[newPosCaisse.x, newPosCaisse.y] == Etat.Mur)
                {
                    return false;
                }
                else if (CaisseInPos(newPosCaisse) != null)
                {
                    return false;
                }
                else
                {
                    caisse.x = newPosCaisse.x;
                    caisse.y = newPosCaisse.y;
                    return true;
                }
            }

            //pas d'obstacle
            return true;

        }

        public void Restart()
        {
            InitCarte();
            nbDeplacement = 0;
        }

        private Position CaisseInPos(Position newPos)
        {
            foreach(Position caisse in caisses)
            {
                if (caisse.x == newPos.x && caisse.y == newPos.y)
                {
                    return caisse;
                }
            }
            return null;
        }
    }

}
