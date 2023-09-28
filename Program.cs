using cc_bot.Clases;

class MainClass
{
	static void Main()
	{
		Bot bot;
		bool nSalir = false;

		while (true)
		{
			Console.Clear();
			Console.WriteLine("**** ¡Candy Crush! ****");
			Console.WriteLine("Menu: \n\nIniciar \t[I]  \nModo    \t[M]\nSalir    \t[S] \n");
			bot = new Bot();
			string a = Console.ReadLine() ?? "";
			switch (a.ToLower())
			{
				case "i":
					Console.Clear();
					Console.WriteLine("**** ¡Jugando! ****");
					Bot.AbrirJuego();
					while (true)
					{
						if (!bot.Jugar()) { break; }
					}
					break;
				case "s":
					Console.Clear();
					Console.WriteLine("**** GG Ezz ****");
					Console.ReadLine();
					nSalir = true;
					break;
				case "m":
					Console.Clear();
					Console.WriteLine("**** Modo de ejecucion ****");
					Console.WriteLine("\n\nDev \t[D]  \nPro \t[P]\n");
					string modo = Console.ReadLine() ?? "";
					switch (modo.ToLower())
					{
						case "d":
							bot.SetModo(true);
							break;
						case "p":
							bot.SetModo(false);
							break;
						default:
							Console.WriteLine("[Opcion no valida, intente nuevamente]\n[Enter] para continuar");
							Console.ReadLine();
							break;
					}
					break;
				default:
					Console.WriteLine("[Opcion no valida, intente nuevamente]\n[Enter] para continuar");
					Console.ReadLine();
					break;
			}
			if (nSalir) { break; }
		}
		Console.Clear();
	}
}