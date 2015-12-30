using UnityEngine;
using UnityEngine.UI;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
//using System.Threading.Tasks;
using System.Diagnostics;
using System.IO;

public class Optimization : MonoBehaviour
{
    public static void optimize()
	{
		print ("optimize");
		//initialization
		int timer = 1000;
        string executable_myoccs = OptimizationController.File_path.my_occs;
        string executable_calcul_flux = OptimizationController.File_path.calcul_flux;
        string fichier_carto = OptimizationController.File_path.transfert(OptimizationController.File_path.carto);
        string fichier_cas_optim = OptimizationController.File_path.transfert(OptimizationController.File_path.cas_optim);
        string resultat = OptimizationController.File_path.temp;
        string fichier_retour_info = OptimizationController.File_path.info;
        System.Diagnostics.Process process = new Process ();
		process.StartInfo.UseShellExecute = false;
		process.StartInfo.WindowStyle = System.Diagnostics.ProcessWindowStyle.Hidden;
		process.StartInfo.CreateNoWindow = true;

		//appel calculflux chargement fichier text
		process.StartInfo.FileName = executable_calcul_flux;
		process.StartInfo.Arguments = string.Concat("load ",fichier_carto);
        process.Start();
        process.WaitForExit();
		print ("calcul flux loading done !");

        //appel my-occs chargement cas optim thermique
        process.StartInfo.FileName = executable_myoccs;
		process.StartInfo.Arguments = string.Concat("ghost ",fichier_cas_optim);
        process.Start();
		print ("optimization has just beging !");

        //lecture résultat en temps réel
		print("Press s key to stop. Press r key to get all information. Press any other key to get basic info");
		DateTime start = DateTime.Now;
		string reponse = "";
		char touche = ' ';
		bool test = true;
		while (test)
		{
			while (test && (DateTime.Now - start).TotalMilliseconds < timer)
			{
				if (process.HasExited)
				{
					test = false;
				}
				else
				{
					if (Input.anyKey)
					{
						touche = Input.inputString.First();
						switch (touche)
						{
						case 's':
							test = false;
							break;
						case 'r':
							retour_info(fichier_retour_info);
							break;
						default:
							reponse = lecture(resultat);
							print(reponse);
							break;
						}

					}
					else
					{
						System.Threading.Thread.Sleep(1);
					}
				}
			}
			start = DateTime.Now;
		}
        //arret process optim
        print("waiting for process to exit");
        process.WaitForExit();
        print("fini!");
    }
    static string lecture(string filepath)
    {
        string last_line = "eror";
        if (System.IO.File.Exists(filepath))
        {
            bool sucess = false;
            int nb_fail = 0;
            while (!sucess)
            {
                try
                {
                    StreamReader fichier = new StreamReader(filepath);
                    string ligne = fichier.ReadLine();
                    while (!fichier.EndOfStream)
                    {
                        ligne = fichier.ReadLine();
                    }
                    System.Threading.Thread.Sleep(1);
                    fichier.Close();
                    sucess = true;
                    last_line = ligne;
                }
                catch
                {
                    nb_fail++;
                    if (nb_fail == 100)
                    {
                        sucess = true;
                    }
                }
            }
        }
        else
        {
            print("fichier non trouver");
        }
        return last_line;
    }
    static void retour_info(string filepath)
    {
        if (System.IO.File.Exists(filepath))
        {
            bool sucess = false;
            int nb_fail = 0;
            while (!sucess)
            {
                try
                {
                    StreamReader fichier = new StreamReader(filepath);
                    while (!fichier.EndOfStream)
                    {
						print(fichier.ReadLine());
                    }
                    fichier.Close();
                    sucess = true;
                }
                catch
                {
                    nb_fail++;
                    if (nb_fail == 100)
                    {
                        sucess = true;
                    }
                }
            }
        }
        else
        {
			print("fichier non trouver");
        }
    }
}
