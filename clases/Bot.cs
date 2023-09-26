namespace cc_bot.Clases;

class Bot
{
	//Funciones para Pruebas
	public static int[,] GenerarTableroAleatorio()
	{
		int[,] matriz = new int[81, 2];
		
		Random rand = new Random();

		for (int fila = 0; fila < 81; fila++)
		{
			int val1 = rand.Next(1, 8);
			int val2;
			if (val1 == 7)
			{
				val2 = 7;
			}else{
				val2 = rand.Next(1, 5);
			}
			matriz[fila, 0] = val1; // Valores entre 1 y 7 (inclusive)
			matriz[fila, 1] = val2; // Valores entre 1 y 4 (inclusive)
		}
		return matriz;
	}
	static void PrintTableroAleatorio(int[,] matriz)
	{
		int filas = matriz.GetLength(0);
		int columnas = matriz.GetLength(1);

		for (int fila = 0; fila < filas; fila++)
		{
			for (int columna = 0; columna < columnas; columna++)
			{
				Console.Write(matriz[fila, columna] + " ");
			}
			Console.WriteLine(); // Salto de línea después de cada fila
		}
	}
	//Variables
	private Dulce[,] tablero;

	//Agente
	private void InicializarTablero()
	{
		for (int i = 0; i < 11; i++)
		{
			for (int j = 0; j < 11; j++)
			{
				if (i == 0 || j == 0 || i == 10 || j == 10)
				{
					tablero[i, j] = new Dulce(Dulce.OCULTO, Dulce.OCULTO);
				}
				else
				{
					tablero[i, j] = new Dulce(Dulce.AMARILLO, Dulce.NORMAL);
				}
			}
		}
	}
	public void ModificarTablero(int[,] nuevosDulces)
	{
		int indice = 0;
		for (int i = 1; i < 10; i++)
		{
			for (int j = 1; j < 10; j++)
			{
				tablero[i, j].SetColor(nuevosDulces[indice, 0]);
				tablero[i, j].SetTipo(nuevosDulces[indice++, 1]);
			}
		}
	}
	private void PrintTablero()
	{
		for (int fila = 0; fila < 11; fila++)
		{
			for (int columna = 0; columna < 11; columna++)
			{
				int d_color = tablero[fila, columna].GetColor();
				int d_tipo = tablero[fila, columna].GetTipo();
				Console.Write($"{d_color}{d_tipo} ");
			}
			Console.WriteLine("\t "); // Salto de línea después de cada fila
		}
	}
	public int[] DecidirMovimiento()
	{
		int[] movimiento = new int[2];

		return movimiento;
	}
	//Sensor

	//Actuador

	public Bot()
	{
		Console.WriteLine("Hola mundo");
		tablero = new Dulce[11, 11];
		InicializarTablero();
		PrintTablero();
		
		int[,] nuevosDulces = GenerarTableroAleatorio();
		//PrintTableroAleatorio(nuevosDulces);
		ModificarTablero(nuevosDulces);
		PrintTablero();
	}
}