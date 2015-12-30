using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;

namespace Calcul_Flux
{
    class Program
    {
        //Class
        class Variable
        {
            public static int np;
            public static double[] temperature_consigne;
            public static double[] temperature_reel;
            public static double[,] link_room;
            public static double[,] flux;
            public static double[,] wall_thickness;
            public static double[,] contact_area;
            public static double[,] wall_conductivity;
            public static double[] flux_commande;
            public static double[,] coefficient_conductivite;
            public static int[] num_flux_commande;
            public static double[] valeur_flux_commande;
            public static void set_variable()
            {
                np = 6;
                temperature_consigne = new double[] { 10, 16, 20, 18, 21, 18 };
                temperature_reel = new double[] { 10, 16, 20, 20, 21, 18 };
                link_room = new double[,] {{0,1,1,0,1,1},{1,0,1,1,1,0},{1,1,0,1,0,1},{0,1,1,0,1,1},{1,1,0,1,0,1},{1,0,1,1,1,0}};            
                flux = new double[np,np];
                wall_thickness = new double[,] { { 0, 0.3, 0.3, 0, 0.3, 0.3 }, { 0.3, 0, 0.1, 0.1, 0.1, 0 }, { 0.3, 0.1, 0, 0.1, 0, 0.1 }, { 0, 0.1, 0.1, 0, 0.1, 0.1 }, { 0.3, 0.1, 0, 0.1, 0, 0.1 }, { 0.1, 0, 0.1, 0.1, 0.1, 0 } };
                contact_area = new double[,] { { 0, 180, 60, 0, 120, 80 }, { 180, 0, 40, 20, 40, 0 }, { 60, 40, 0, 60, 0, 40 }, { 0, 20, 60, 0, 60, 20 }, { 120, 40, 0, 60, 0, 20 }, { 80, 0, 40, 20, 20, 0 } };
                wall_conductivity = new double[,] { { 0, 1.75, 1.75, 0, 1.75, 1.75 }, { 1.75, 0, 1.75, 1.75, 1.75, 0 }, { 1.75, 1.75, 0, 1.75, 0, 1.75 }, { 0, 1.75, 1.75, 0, 1.75, 1.75 }, { 1.75, 1.75, 0, 1.75, 0, 1.75 }, { 1.75, 0, 1.75, 1.75, 1.75, 0 } };
                flux_commande = new double[np];
                num_flux_commande = new int[] { 2, 3, 4 };
                valeur_flux_commande = new double[] { 10000, -5000, 15000 };
                coefficient_conductivite = new double[np, np];
                for (int n = 0; n < np; n++)
                {
                    flux_commande[n] = 0;
                }
                for (int n = 0; n < num_flux_commande.Length; n++)
                {
                    flux_commande[num_flux_commande[n]] = valeur_flux_commande[n];
                }
                for (int i = 0; i < np; i++)
                {
                    for (int j = 0; j < i; j++)
                    {
                        if (link_room[i, j] == 1)
                        {
                            coefficient_conductivite[i, j] = wall_conductivity[i, j] * contact_area[i, j] / wall_thickness[i, j];
                            coefficient_conductivite[j, i] = coefficient_conductivite[i, j];
                        }
                        else
                        {
                            coefficient_conductivite[i, j] = 0;
                            coefficient_conductivite[j, i] = 0;
                        }
                    }
                }
            }
            public static void load_variable(double[] valeur_flux)
            {
                string filepath = Properties.Settings.Default.cartographie_filepath;
                if (File.Exists(filepath))
                {
                    StreamReader carto = new StreamReader(filepath);
                    try
                    {
                        //lire nombre de pièce
                        atteindre_balise(carto, 1);
                        np = Convert.ToInt32(lire_nombre(carto));

                        try
                        {
                            //lire température de consigne
                            atteindre_balise(carto, 2);
                            temperature_consigne = lire_liste_nombre(carto);
                            temperature_reel = new double[temperature_consigne.Length];
                            for (int i = 0; i < temperature_consigne.Length; i++)
                            {
                                temperature_reel[i] = temperature_consigne[i];
                            }

                            try
                            {
                                //lire lien pièce
                                atteindre_balise(carto, 3);
                                link_room = lire_matrice_carre_nombre(carto, np);
                                flux = new double[np, np];
                                //lire epaisseur mur
                                atteindre_balise(carto, 4);
                                wall_thickness = lire_matrice_carre_nombre(carto, np);
                                //lire surface de contact
                                atteindre_balise(carto, 5);
                                contact_area = lire_matrice_carre_nombre(carto, np);
                                //lire conductivité thermique mur
                                atteindre_balise(carto, 6);
                                wall_conductivity = lire_matrice_carre_nombre(carto, np);
                                flux_commande = new double[np];
                                //lire pièce commandé
                                atteindre_balise(carto, 7);
                                double[] temp = lire_liste_nombre(carto);
                                num_flux_commande = new int[temp.Length];
                                for (int i = 0; i < temp.Length; i++)
                                {
                                    num_flux_commande[i] = Convert.ToInt32(temp[i]);
                                }
                                //recupérer les valeur de flux de commande
                                valeur_flux_commande = valeur_flux;
                                coefficient_conductivite = new double[np, np];
                                for (int n = 0; n < np; n++)
                                {
                                    flux_commande[n] = 0;
                                }
                                for (int n = 0; n < num_flux_commande.Length; n++)
                                {
                                    flux_commande[num_flux_commande[n]] = valeur_flux_commande[n];
                                }
                                for (int i = 0; i < np; i++)
                                {
                                    for (int j = 0; j < i; j++)
                                    {
                                        if (link_room[i, j] == 1)
                                        {
                                            coefficient_conductivite[i, j] = wall_conductivity[i, j] * contact_area[i, j] / wall_thickness[i, j];
                                            coefficient_conductivite[j, i] = coefficient_conductivite[i, j];
                                        }
                                        else
                                        {
                                            coefficient_conductivite[i, j] = 0;
                                            coefficient_conductivite[j, i] = 0;
                                        }
                                    }
                                }
                            }
                            catch
                            {
                                Console.Write("error : part 3");
                            }
                            
                        }
                        catch
                        {
                            Console.Write("error : part 2");
                        }
                        
                    }
                    catch
                    {
                        Console.Write("error : part 1");
                    }
                    
                }
                else
                {
                    Console.WriteLine("fichier non trouver");
                }
                
            }
        }
        //Programme Principale
        static void Main(string[] args)
        {
            if (args.Length == 0)
            {
                Developpement();
            }
            else
            {
                switch(args[0])
                {
                    case"load":
                        load(args);
                        break;
                    case "return":
                        retour(args);
                        break;
                    default:
                        Optim(args);
                        break;
                }
            }
        }
        static void Developpement()
        {
            Console.WriteLine(Properties.Settings.Default.cartographie_filepath);
            //Recuperation des variables
            Variable.set_variable();
            //Calcul des flux naturel du au difference de temperature
            calcul_tout_flux_mur();
            //affichage matrice de flux 
            afficher(Variable.flux);
            //calcul des flux de commande idéaux
            calcul_tout_flux_commande();
            //affichage matrice flux
            afficher(Variable.flux);
            //calcul de la réalité
            realite();
            //affichage
            afficher(Variable.flux);
            afficher(Variable.temperature_reel);
            //Verification
            //verification();
            //resultat
            double[] resultat = new double[2];
            resultat[0] = cout();
            resultat[1] = ecart_temperature();
            Console.Write("{0} {1}", resultat[0], resultat[1]);
            //attente
            Console.Read();
        }
        static void load(string[] arguments)
        {
            if(arguments[1] != null && File.Exists(arguments[1]))
            {
                Properties.Settings.Default.cartographie_filepath = arguments[1];
                Properties.Settings.Default.Save();
            }
        }
        static void Optim(string[] arguments)
        {
            try
            {
                double[] valeur_flux = new double[arguments.Length];
                for (int i = 0; i < valeur_flux.Length; i++)
                {
                    valeur_flux[i] = Convert.ToDouble(arguments[i].Replace('.', ','));
                }
                Variable.load_variable(valeur_flux);
                calcul_tout_flux_mur();
                calcul_tout_flux_commande();
                realite();
                double[] resultat = new double[2];
                resultat[0] = cout();
                resultat[1] = ecart_temperature();
                Console.Write("{0} {1}", resultat[0], resultat[1]);
            }
            catch
            {
                Console.Write("error");
            }
        }
        static void retour(string[] arguments)
        {
            double[] valeur_flux = new double[arguments.Length-2];
            for (int i = 0; i < valeur_flux.Length-2; i++)
            {
                valeur_flux[i] = Convert.ToDouble(arguments[i+2].Replace('.', ','));
            }
            Variable.load_variable(valeur_flux);
            calcul_tout_flux_mur();
            calcul_tout_flux_commande();
            realite();
            StreamWriter file = new StreamWriter(arguments[1]);
            file.WriteLine("Cout");
            file.WriteLine(cout().ToString());
            file.WriteLine("Temperature reel");
            ecrire_vecteur(file, Variable.temperature_reel);
            file.WriteLine("Flux commande");
            ecrire_vecteur(file, Variable.flux_commande);
            file.WriteLine("Flux pieces");
            ecrire_matrice(file, Variable.flux);
            file.Close();
        }
        //Fonction
        static double conductivite_mur(double T1, double T2, double coefficient_conductivite)
        {
            double ecart = T2 - T1;
            double flux = ecart * coefficient_conductivite;
            return flux;
        }
        static void calcul_tout_flux_mur()
        {
            for (int i = 0; i < Variable.np; i++)
            {
                for (int j = 0; j <= i; j++)
                {
                    if (Variable.link_room[i, j] == 0)
                    {
                        Variable.flux[i, j] = 0;
                        Variable.flux[j, i] = 0;
                    }
                    else
                    {
                        Variable.flux[i, j] = conductivite_mur(Variable.temperature_consigne[i], Variable.temperature_consigne[j], Variable.coefficient_conductivite[i,j]);
                        Variable.flux[j, i] = -Variable.flux[i, j];
                    }
                }
            }
        }
        static void calcul_tout_flux_commande()
        {
            double somme;
            for (int i = 0; i < Variable.np; i++)
            {
                somme = 0;
                for (int j = 0; j < Variable.np; j++)
                {
                    somme += Variable.flux[i, j];
                }
                Variable.flux[i, i] = -somme;
            }
        }
        static void realite()
        {
            int nb_try = 0;
            int limite = 500;
            double erreur = erreur_flux();
            double precision = 0.1;
            int position_pire;
            double temperature;
            while(nb_try < limite && erreur > precision)
            {
                //Trouver plus gros ecart de flux
                position_pire = position_max_ecart_flux();
                //trouver temperature pour equilibrer les flux
                temperature = temperature_equilibre(position_pire);
                //modifier temperature reel
                Variable.temperature_reel[position_pire] = temperature;
                //recalculer flux
                recalculer_flux(position_pire);
                //Calculer l'écart flux
                erreur = erreur_flux();
                nb_try++;
            }
            //Console.WriteLine("Iteration n°{0,8:N2}", nb_try);
        
        }
        static int position_max_ecart_flux()
        {
            int position = 1;
            double ecart = Math.Abs(Variable.flux[1, 1] - Variable.flux_commande[1]);
            for(int i = 2; i < Variable.np; i++)
            {
                if(Math.Abs(Variable.flux[i, i] - Variable.flux_commande[i])>ecart)
                {
                    ecart = Math.Abs(Variable.flux[i, i] - Variable.flux_commande[i]);
                    position = i;
                }
            }
            return position;
        }
        static double temperature_equilibre(int room)
        {
            double temperature_equilibre;
            double flux_commande = Variable.flux_commande[room];
            double Coef_t_room = 0;
            double flux_pieces = 0;
            for(int i = 0; i < Variable.np; i++)
            {
                if(room != i)
                {
                    Coef_t_room += Variable.coefficient_conductivite[room, i];
                    flux_pieces += Variable.coefficient_conductivite[room, i] * Variable.temperature_reel[i];
                }
            }
            temperature_equilibre = (flux_pieces + flux_commande) / Coef_t_room;
            return temperature_equilibre;
        }
        static void recalculer_flux(int position)
        {
            //recalcul flux de la piece dont on as changer la température
            for(int i = 0; i < Variable.np; i++)
            {
                if(i != position)
                {
                    Variable.flux[i, position] = conductivite_mur(Variable.temperature_reel[i], Variable.temperature_reel[position], Variable.coefficient_conductivite[i, position]);
                    Variable.flux[position, i] = -Variable.flux[i, position];
                }
            }
            //recalcul tout les flux commande reel
            double somme;
            for (int i = 0; i < Variable.np; i++)
            {
                somme = 0;
                for (int j = 0; j < Variable.np; j++)
                {
                    if(i != j)
                    {
                        somme += Variable.flux[i, j];
                    }
                }
                Variable.flux[i, i] = -somme;
            }
        }
        static double erreur_flux()
        {
            double ecart = 0;
            for(int i = 1; i < Variable.np; i++)
            {
                ecart += Math.Pow(Variable.flux_commande[i] - Variable.flux[i, i],2);
            }
            ecart = Math.Sqrt(ecart / (Variable.np-1));
            return ecart;
        }
        static double cout()
        {
            double somme = 0;
            for(int i = 0; i < Variable.flux_commande.Length;i++)
            {
                somme += cout_unitaire(Variable.flux_commande[i]);
            }
            return somme;
        }
        static double ecart_temperature()
        {
            double somme = 0;
            for(int i = 1; i < Variable.np; i++)
            {
                somme += Math.Pow(Variable.temperature_reel[i] - Variable.temperature_consigne[i], 2);
            }
            somme = Math.Sqrt(somme / (Variable.np - 1));
            return somme;
        }
        static double cout_unitaire(double flux)
        {
            double prix_kWh = 0.14;
            double prix_horraire = Math.Abs(flux) / 1000 * prix_kWh;
            return prix_horraire;
        }
        static void recuperation_flux(string[] arguments)
        {
            double[] nombres = new double[arguments.Length];
            for (int i = 0; i < arguments.Length; i++)
            {
                nombres[i] = Convert.ToDouble(arguments[i].Replace(".", ","));
                Variable.flux_commande[Variable.num_flux_commande[i]] = nombres[i];
            }
        }
        static void afficher(double[,] matrice)
        {
            Console.WriteLine("===============================");
            for(int i = 0; i < matrice.GetLength(0); i++)
            {
                for(int j = 0; j < matrice.GetLength(1); j++)
                {
                    Console.Write("{0,10:N2} ",matrice[i, j]);
                }
                Console.Write("\n");
            }
            Console.WriteLine("===============================");
        }
        static void afficher(double[] vecteur)
        {
            Console.WriteLine("===============================");
            for (int j = 0; j < vecteur.Length; j++)
            {
                Console.Write("{0,10:N2} ", vecteur[j]);
            }
            Console.Write("\n");
            Console.WriteLine("===============================");
        }
        //lire fichier text
        static void atteindre_balise(StreamReader file, int nb_balise)
        {
            string balise;
            string line;
            balise = "!Balise " + nb_balise.ToString() + "!";
            line = file.ReadLine();
            while (!file.EndOfStream && line != balise)
            {
                line = file.ReadLine();
            }
        }
        static double lire_nombre(StreamReader file)
        {
            string line = file.ReadLine();
            line = line.Replace('.', ',');
            double A = Convert.ToDouble(line);
            return A;
        }
        static double[] lire_liste_nombre(StreamReader file)
        {
            string line = file.ReadLine();
            line = line.Replace('.', ',');
            char[] separators = new char[] { ' ' };
            string[] numbers = line.Split(separators);
            double[] num = new double[numbers.Length];
            for (int i = 0; i < numbers.Length; i++)
            {
                num[i] = Convert.ToDouble(numbers[i]);
            }
            return num;
        }
        static double[,] lire_matrice_carre_nombre(StreamReader file, int n)
        {
            double[,] matrice = new double[n, n];
            string line;
            char[] sep = new char[] { ' ' };
            string[] numbers;
            for (int i = 0; i < n; i++)
            {
                line = file.ReadLine();
                line = line.Replace('.', ',');
                numbers = line.Split(sep);
                for (int j = 0; j < n; j++)
                {
                    matrice[i, j] = Convert.ToDouble(numbers[j]);
                }
            }
            return matrice;
        }
        //ecrire fichier text
        static void ecrire_vecteur(StreamWriter file, double[] vecteur)
        {
            for(int i = 0; i < vecteur.Length; i++)
            {
                file.Write("{0} ",vecteur[i]);
            }
            file.Write("\r\n");
        }
        static void ecrire_matrice(StreamWriter file, double[,] matrice)
        {
            for(int i = 0; i < matrice.GetLength(0); i++)
            {
                for(int j = 0; j < matrice.GetLength(1); j++)
                {
                    file.Write("{0} ", matrice[i, j]);
                }
                file.Write("\r\n");
            }
        }
        /*
        static void verification()
        {
            Console.WriteLine("Verification");
            //calcul flux
            double[,] flux_verif = new double[Variable.np, Variable.np];
            for (int i = 0; i < Variable.np; i++)
            {
                for (int j = 0; j <= i; j++)
                {
                    if (Variable.link_room[i, j] == 0)
                    {
                        flux_verif[i, j] = 0;
                        flux_verif[j, i] = 0;
                    }
                    else
                    {
                        flux_verif[i, j] = conductivite_mur(Variable.temperature_reel[i], Variable.temperature_reel[j], Variable.coefficient_conductivite[i, j]);
                        flux_verif[j, i] = -Variable.flux[i, j];
                    }
                }
            }
            double somme;
            for (int i = 0; i < Variable.np; i++)
            {
                somme = 0;
                for (int j = 0; j < Variable.np; j++)
                {
                    if(i != j)
                    {
                        somme += flux_verif[i, j];
                    }
                }
                flux_verif[i, i] = -somme;
            }
            afficher(flux_verif);
        }
        */
    }
}
