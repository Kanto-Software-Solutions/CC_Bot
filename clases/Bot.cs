namespace cc_bot.Clases;

class Bot
{
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
		for (int i = 1; i < 10; i++)
		{
			for (int j = 1; j < 10; j++)
			{
				tablero[i,j].SetColor(1);
				tablero[i,j].SetTipo(1);
			}
		}
	}
	private void PrintTablero()
	{
		Console.Clear();
		for (int fila = 0; fila < tablero.GetLength(0); fila++)
		{
			for (int columna = 0; columna < tablero.GetLength(1); columna++)
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
	}
}