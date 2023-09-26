using cc_bot.clases;


class MainClass
{
	static void Main()
	{
		Bot bot = new Bot();
		bool nSalir = false;

		while (true)
		{
			Console.WriteLine("**** ¡Candy Crush! ****");
			Console.WriteLine("Menu: \n\nIniciar \t[I] \nPuntaje \t[P] \nOpciones \t[O] \nSalir    \t[S] \n");
			string a = Console.ReadLine() ?? "";
			switch (a.ToLower())
			{
				case "i":
					Console.Clear();
					Console.WriteLine("**** Iniciar ****");

					break;
				case "p":
					Console.Clear();
					Console.WriteLine("**** Tabla de puntajes ****");
					break;
				case "o":
					Console.Clear();
					Console.WriteLine("**** Opciones ****");
					break;
				case "s":
					Console.Clear();
					Console.WriteLine("**** GG Ezz ****");
					Console.ReadLine();
					nSalir = true;
					break;
				default:
					Console.WriteLine("[Opcion no valida, intente nuevamente]\n[Enter] para continuar");
					Console.ReadLine();
					break;
			}
			if(nSalir){break;}
		}
		Console.Clear();
	}
}