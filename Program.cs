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
			Console.WriteLine("Menu: \n\nIniciar \t[I]	 \nSalir    \t[S] \n");
			string a = Console.ReadLine() ?? "";
			switch (a.ToLower())
			{
				case "i":
					Console.Clear();
					Console.WriteLine("**** Iniciar ****");
					bot = new Bot();
					Bot.AbrirJuego();
					Thread.Sleep(3000);
					while (true)
					{
						if(!bot.Jugar()){break;}
					}
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