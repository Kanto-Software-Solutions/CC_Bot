namespace cc_bot.clases;

class Bot
{
	//Variables
	Dulce[,] tablero;

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
	private void PrintTablero()
	{
		for (int fila = 0; fila < tablero.GetLength(0); fila++)
		{
			for (int columna = 0; columna < tablero.GetLength(1); columna++)
			{
				Console.Write(tablero[fila, columna].GetColor() + " ");
			}
			Console.WriteLine(); // Salto de línea después de cada fila
		}
	}
	public int[] Movimiento()
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