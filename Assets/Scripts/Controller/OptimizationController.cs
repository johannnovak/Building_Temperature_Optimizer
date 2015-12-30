using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using System.Collections.Generic;
using System.IO;

public class OptimizationController : MonoBehaviour {

	private string m_buildingConstraintFilePath;
	private string m_optimizationConstraintFilePath;

	public Button m_buttonGo;
	public Building m_building; // building qui contient toutes les pièces
	public SimulationController m_simulationController; 
	private List<Weather> m_weatherList; // liste des températures de l'environnement (tu me demanderas à l'occas')

	public class File_path
	{
		private static string my_location;//chemin d'acces du fichier executant ce script
		private static string my_directory;//repertoir du projet
		public static string cas_optim_name;//nom du fichier text du cas d'optimisation
		public static string cas_optim;//fichier text du cas d'optimisation
		public static string carto;//fichier text de la cartographie du batiment
		public static string info;//fichier text servant au retour détaillé des informations
		public static string temp;//fichier texte des resultats temporaire de l'optimisation (sert au retour succint)
		public static string my_occs;//chemin d'acces du programme d'optimisation My-OCCS
		public static string calcul_flux;//chemin d'acces de l'executable du modele thermique
		//determiner les valeur des variables ci dessus
		public static void set()
		{
			//recuperation des chemin d'acces relatif
			my_location = System.Reflection.Assembly.GetExecutingAssembly().Location;//on recupère l'emplacement du fichier executant les script
			//on remonte de 3 cran dans l'arborescence des fichier pour être dans le repertoire racinne du projet
			my_directory = my_location;
			int i = 3;
			while(i>0)
			{
				my_directory = System.IO.Directory.GetParent(my_directory).FullName;
				i--;
			}
			//on va dans le sous repertoire optimisation du projet ; ce sous repertoire contient tout les fichier et executable nécésaire à l'optimisation
			my_directory += "\\Optimisation";
			my_directory = "E:\\UTBM\\GMC\\GMC05\\TX\\Programme\\Building_Temperature_Optimizer-jnovak\\Optimisation";
			cas_optim_name = "casoptimTX";

			cas_optim = my_directory + "\\" + cas_optim_name + ".txt";
			carto = my_directory + "\\carto.txt";
			info = my_directory + "\\info.txt";
			temp = my_directory + "\\soft\\Resultats\\" + cas_optim_name + "\\Temp.txt";
			my_occs = my_directory + "\\soft\\my-OCCS Optim Software.exe";
			calcul_flux = my_directory + "\\Calcul Flux\\bin\\Release\\Calcul Flux.exe";
		}
		//sert à ecrire le chemin d'accès d'un fichier de manière a ce qu'il puisse être transferer comme argument à un programme sous forme d'application console
		public static string transfert(string filepath)
		{
			return "\"" + filepath.Replace('\\', '/') + "\"";
		}
	}
	// Use this for initialization
	void Start () {
		m_weatherList = m_simulationController.GetWeatherList ();
	}

	// Update is called once per frame
	void Update () {
	}

	public void CreateConfigurationFiles()
	{
		//bidouilloux de Keum

		//initialisation des chemins d'acces
		File_path.set();

		//preparation de l'optimisation a partir des variables de la classe Building
		Prepare_optimization_file(m_building);
		print ("prepared !");

		// créaation du fichier d'optimisation concernant le bâtiment
		CreateBuildingOptimizationFile ();
		print("optimization file created !");

		// creatino du fichier d'optimisation par rapport aux contraintes des actionneurs
		CreateBuildingconfiguration();
		print("Building file created !");

		//lancement de l'optimisation
		Optimization.optimize();
		print("optimization is done !");

		//remise a zero du bouton
		m_buttonGo.interactable = true;
	}

	private void CreateBuildingOptimizationFile()
	{
		string file_path = File_path.cas_optim;
		string file_path_flux = File_path.calcul_flux;
		string line;
		float temp;
		if(!System.IO.File.Exists(file_path))
		{
			System.IO.File.Create(file_path);
		}
		System.IO.StreamWriter file = new System.IO.StreamWriter(file_path);

		file.WriteLine("informations :");//balise du fichier
		file.WriteLine(File_path.cas_optim_name);//nom fichier
		file.WriteLine(File_path.cas_optim);//chemin acces fichier
		file.WriteLine("13/12/2015 12:13:13");//date de création
		file.WriteLine("13/12/2015 12:13:13");//date de derniere modification
		file.WriteLine("1");//nombre de ligne du commentaire de l'utilisateur
		file.WriteLine("Test optimisation sur modele thermique");//commentaire de l'utilisateur concernant le cas d'optimisation
		file.WriteLine("0 2 0 0 0");//configuration de la sortie des resultats

		file.WriteLine("enonce :");//balise du fichier
		file.WriteLine("PSO");//algorithme choisit
		file.WriteLine("custom");//nature de la fonction objectif
		file.WriteLine("[l0.0]+[l0.1];[x0];[x1]");//fonction objectif
		file.WriteLine("post");//type de contrainte choisit
		file.WriteLine("1");//nombre de fois que la simulation doit etre lancer

		file.WriteLine("parameters :");//balise du fichier
		line = lien_text.nb_commandable_rooms.ToString();//premier parametre est le nombre de variable (qui est dans notre cas égale au nombre de pièce commandable)
		line += " 50 50 1 1 0,4 0,9 1 10 1 0,8 2 50";//parametre 2 à 13
		/*Parametre influencant le calcul 2,3,13
                                                         * 2 : nombre iteration
                                                         * 3 : nb particule
                                                         * 13 : nombre de particule spéciale
                                                     */
		file.WriteLine(line);

		file.WriteLine("variables :");//balise du fichier
		for (int i = 0; i < lien_text.nb_commandable_rooms; i++)
		{
			file.WriteLine(" c ");
		}

		file.WriteLine("init :");//balise du fichier
		file.WriteLine("0");//nombre de solution connue acceptable permettant d'initialiser l'algorithme de calcul

		file.WriteLine("contraintes :");//balise du fichier
		for (int i = 0; i < lien_text.nb_commandable_rooms; i++)
		{
			line = lien_text.rooms_interval[i, 0].ToString();//valeur min de l'intervale de commande d'une piece
			line += " " + lien_text.rooms_interval[i, 1].ToString();//valeur max de l'intervale de commande d'une piece
			line += " 1e9";//valeur de penalité si la valeur de la variable est en dehors de l'interval
			file.WriteLine(line);
		}

		file.WriteLine("contraintes spe1 :");//balise du fichier
		file.WriteLine("fin spe");//balise du fichier (indiquant la fin des contraintes spéciale)

		file.WriteLine("contraintes spe2 :");//balise du fichier
		file.WriteLine("fin spe");//balise du fichier (indiquant la fin des contraintes spéciale)

		file.WriteLine("contraintes post1 :");//balise du fichier
		file.WriteLine("fin post");//balise du fichier (indiquant la fin des contraintes a posteriori du calcul)

		file.WriteLine("contraintes post2 :");//balise du fichier
		file.WriteLine("fin post");//balise du fichier (indiquant la fin des contraintes a posteriori du calcul)

		file.WriteLine("extra :");//balise du fichier
		for (int i = 0; i < lien_text.nb_commandable_rooms; i++)
		{
			line = lien_text.rooms_interval[i, 0].ToString();//dans le cas de la création aléatoire d'une particule, valeur min de l'interval pour cette variable
			line += " " + lien_text.rooms_interval[i, 1].ToString();//dans le cas de la création aléatoire d'une particule, valeur max de l'interval pour cette variable
			temp = (lien_text.rooms_interval[i, 1] - lien_text.rooms_interval[i, 0]) / 10;
			line += " " + (-temp).ToString();// deplacement min des particules selon cette variable
			line += " " + temp.ToString();// deplacement max des particules selon cette variable
			line += " " + (temp / 100).ToString();//precision rechercher sur cette variable (sert au critère d'arret)
			file.WriteLine(line);
		}

		file.WriteLine("lien :");//balise du fichier
		file.WriteLine("1");//nombre de lien utiliser par ce cas 'optimisation
		file.WriteLine("debut lien");//balise du fichier
		file.WriteLine("thermique");//nom donner au lien pour savoir a quoi il correspond
		file.WriteLine("ESA");//type de lien utiliser
		file.WriteLine(file_path_flux);//argument 1 : chemin d'acces du programme
		file.WriteLine();//argument 2 : NA
		file.WriteLine();//argument 3 : NA
		file.WriteLine();//argument 4 : NA
		file.WriteLine();//argument 5 : NA
		file.WriteLine("fin lien");//balise du fichier (fin du lein)

		//fermeture du fichier
		file.Close();
	}
	private void CreateBuildingconfiguration()
	{
		string file_path = File_path.carto;
		if (!System.IO.File.Exists(file_path))
		{
			System.IO.File.Create(file_path);
		}
		System.IO.StreamWriter file = new System.IO.StreamWriter(file_path);
		//nombre de piece
		file.WriteLine("[Fichier text type]");
		file.WriteLine();
		file.WriteLine("nombre de pièce :");
		file.WriteLine("!Balise 1!");
		file.WriteLine(lien_text.nb_rooms);
		file.WriteLine();

		//temperature consigne des pieces
		file.WriteLine("temperature consigne des pieces :");
		file.WriteLine("!Balise 2!");
		ecrire(file, lien_text.romms_temperature);
		file.WriteLine();

		//Lien entre les pieces
		file.WriteLine("Lien entre les pieces :");
		file.WriteLine("!Balise 3!");
		ecrire(file, lien_text.link_beetwen_romm);
		file.WriteLine();

		//Epaisseur mur
		file.WriteLine("Epaisseur mur :");
		file.WriteLine("!Balise 4!");
		ecrire(file, lien_text.walls_depth);
		file.WriteLine();

		//surface de contact entre les pieces
		file.WriteLine("surface de contact entre les pieces :");
		file.WriteLine("!Balise 5!");
		ecrire(file, lien_text.walls_surface);
		file.WriteLine();

		//Coefficient de conductivité thermique des murs
		file.WriteLine("Coefficient de conductivité thermique des murs :");
		file.WriteLine("!Balise 6!");
		ecrire(file, lien_text.walls_thermal_conductivity);
		file.WriteLine();

		//Numéro des pièces commandable
		file.WriteLine("Numéro des pièces commandable :");
		file.WriteLine("!Balise 7!");
		ecrire(file, lien_text.commandable_rooms);
		file.WriteLine();

		//fermeture fichier text
		file.Close();
	}
	//ajout Keum
	public class lien_text
	{
		public static int nb_rooms;
		public static float[] romms_temperature;
		public static int[,] link_beetwen_romm;
		public static float[,] walls_depth;
		public static float[,] walls_surface;
		public static float[,] walls_thermal_conductivity;
		public static int[] commandable_rooms;
		public static float[,] rooms_interval;
		public static int nb_commandable_rooms;
		public static void Initialize(int _nb_rooms)
		{
			nb_rooms = _nb_rooms + 1;//l'exterieur est considéré comme la piece 0
			romms_temperature = new float[nb_rooms];
			link_beetwen_romm = new int[nb_rooms, nb_rooms];
			walls_depth = new float[nb_rooms, nb_rooms];
			walls_surface = new float[nb_rooms, nb_rooms];
			walls_thermal_conductivity = new float[nb_rooms, nb_rooms];
		}
	}

	static void Prepare_optimization_file(Building batiment_A)
	{
		//creation batiment fictif
		Floor[] etages = batiment_A.GetFloors().ToArray();

		//nombre de pièce
		int nombre_pieces = 0;
		foreach (Floor f in etages)
		{
			foreach (RoomContainer rc in f.GetRoomContainers())
			{
				++nombre_pieces;
				rc.number = nombre_pieces;
			}
		}
		//dimensionnement des variables dependant du nombre de pièces
		lien_text.Initialize(nombre_pieces);
		//temperature de consigne des pièces
		foreach (Floor f in etages)
		{
			foreach (RoomContainer rc in f.GetRoomContainers())
			{
				lien_text.romms_temperature[rc.number] = rc.ObjectiveTemperature;
				//lien entre les pièces
				links(rc, nombre_pieces);
			}
		}

		//pieces commandables
		nombre_pieces = 0;
		List<int> commandable = new List<int>();
		List<float> min = new List<float>();
		List<float> max = new List<float>();
		foreach (Floor f in etages)
		{
			foreach (RoomContainer rc in f.GetRoomContainers())
			{
				if(rc.ContainsCommandableActionners())
				{
					commandable.Add(rc.number);
					min.Add(rc.MinDeliveredEnergy);
					max.Add(rc.MaxDeliveredEnergy);
					++nombre_pieces;
				}
			}
		}
		lien_text.nb_commandable_rooms = nombre_pieces;
		lien_text.commandable_rooms = commandable.ToArray();
		lien_text.rooms_interval = new float[nombre_pieces, 2];
		for(int i = 0; i < nombre_pieces;i++)
		{
			lien_text.rooms_interval[i, 0] = min[i];
			lien_text.rooms_interval[i, 1] = max[i];
		}
	}
	static void links(RoomContainer rc, int room_number)
	{
		//liste des murs de la pièce
		List<Wall> Room_walls = new List<Wall>();

		//recuperation des espaces d'une pièce
		List<Room> blocks = rc.GetRooms();
		Room[] espaces = blocks.ToArray();

		//recherche de tout les murs
		foreach(Room espace in espaces)
		{
			Room_walls.AddRange(espace.GetWalls());
		}
		Wall[] walls = Room_walls.ToArray();

		//mise en place
		int nb1;
		int nb2;
		foreach(Wall w in walls)
		{
			if (w.GetRoomContainer1 () == null)
			{
				nb1 = 0;
			}
			else
			{
				nb1 = w.GetRoomContainer1().number;
			}
			if (w.GetRoomContainer2 () == null)
			{
				nb2 = 0;
			}
			else
			{
				nb2 = w.GetRoomContainer2().number;
			}
			var type = typeof(WallMaterial);
			var memInfo = type.GetMember(w.m_wallMaterial.ToString());
			var attributes = memInfo[0].GetCustomAttributes(typeof(WallMaterialAttr),false);
			var description = ((WallMaterialAttr)attributes[0]).Conductivity;
			set_mur(w, nb1, nb2, w.Depth, w.Surface, (float)description);//erreur
		}
	}
	static void set_mur(Wall wall_e, int rc1, int rc2, float depth, float surface, float thermal_conductivity)
	{
		//modifiying index to take into account that outside is rc 0
		if(wall_e.m_room1 == null)
		{
			rc1 = 0;
		}
		if (wall_e.m_room2 == null)
		{
			rc2 = 0;
		}
		//setting properties
		if(lien_text.link_beetwen_romm[rc1, rc2] == 0)
		{
			lien_text.walls_surface[rc1, rc2] = surface;
		}
		else
		{
			lien_text.walls_surface[rc1, rc2] += surface;
		}
		lien_text.link_beetwen_romm[rc1, rc2] = 1;
		lien_text.walls_depth[rc1, rc2] = depth;
		lien_text.walls_thermal_conductivity[rc1, rc2] = thermal_conductivity;
	}
	static void ecrire(StreamWriter file, string text)
	{
		file.WriteLine ();
		file.WriteLine ("==========================");
		file.WriteLine (text);
		file.WriteLine ("==========================");
	}
	static void ecrire(StreamWriter file, float[] vecteur)
	{
		string line = "";
		for(int i = 0; i < vecteur.Length; i++)
		{
			line += vecteur[i] + " ";
		}
		line = line.Substring (0, line.Length - 1);
		file.WriteLine(line);
	}
	static void ecrire(StreamWriter file, int[] vecteur)
	{
		string line = "";
		for(int i = 0; i < vecteur.Length; i++)
		{
			line += vecteur[i] + " ";
		}
		line = line.Substring (0, line.Length - 1);
		file.WriteLine(line);
	}
	static void ecrire(StreamWriter file, float[,] matrice)
	{
		string line = "";
		for(int i = 0; i < matrice.GetLength(0); i++)
		{
			line = "";
			for(int j = 0; j < matrice.GetLength(1); j++)
			{
				line += matrice[i,j]+" ";
			}
			line = line.Substring (0, line.Length - 1);
			file.WriteLine (line);
		}
	}
	static void ecrire(StreamWriter file, int[,] matrice)
	{
		string line = "";
		for(int i = 0; i < matrice.GetLength(0); i++)
		{
			line = "";
			for(int j = 0; j < matrice.GetLength(1); j++)
			{
				line += matrice[i,j]+" ";
			}
			line = line.Substring (0, line.Length - 1);
			file.WriteLine (line);
		}
	}
}
