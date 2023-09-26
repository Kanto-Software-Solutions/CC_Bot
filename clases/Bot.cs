namespace cc_bot.Clases;

class Bot
{
	//Funciones para Pruebas
	public void PrintTablero()
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
	public static int[,] GenerarTableroAleatorio()
	{
		int[,] matriz = new int[81, 2];

		Random rand = new Random();

		for (int fila = 0; fila < 81; fila++)
		{
			int val1 = rand.Next(1, 6);
			int val2;
			if (val1 == 7)
			{
				val2 = 7;
			}
			else
			{
				val2 = rand.Next(1, 4);
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
	private Dulce GetDulcedeTablero(int x, int y)
	{
		return tablero[x, y];
	}
	public int[] DecidirMovimiento()
	{
		//[xOrigen,yOrigen,xDestino,yDestino,puntaje]
		Dulce dPrueba;

		int puntaje = 0;

		int xOrigen = 0;
		int yOrigen = 0;

		int xDestino = 0;
		int yDestino = 0;

		for (int i = 1; i < 10; i++)
		{
			for (int j = 1; j < 10; j++)
			{
				dPrueba = GetDulcedeTablero(i, j);
				//Verificar Movimientos Premium
				if (i % 2 == 1 && j % 2 == 1 || i % 2 == 0 && j % 2 == 0)
				{
					int[] premium = VerMovPremium(dPrueba, i, j);
					if (puntaje < premium[4])
					{
						xOrigen = premium[0];
						yOrigen = premium[1];
						xDestino = premium[2];
						yDestino = premium[3];
						puntaje = premium[4];
					}
					if (puntaje == 13)
					{
						return new int[] { xOrigen, yOrigen, xDestino, yDestino, puntaje };
					}
				}
				//Verificar Movimientos Normales
				if (puntaje <= 8)
				{
					
				}
			}
		}
		return new int[] { xOrigen, yOrigen, xDestino, yDestino, puntaje };
	}
	private int[] VerMovPremium(Dulce d_actual, int x_d, int y_d)
	{
		Dulce d_prueba;
		int puntaje = 0;
		int nPuntaje;

		int xOrigen = 0;
		int yOrigen = 0;

		int xDestino = 0;
		int yDestino = 0;
		//Superior
		d_prueba = GetDulcedeTablero(x_d, y_d - 1);
		nPuntaje = ValidarPuntajePremium(d_actual, d_prueba);
		if (puntaje < nPuntaje)
		{
			xOrigen = x_d;
			yOrigen = y_d;
			xDestino = x_d;
			yDestino = y_d - 1;
			puntaje = nPuntaje;
		}
		//Inferior
		d_prueba = GetDulcedeTablero(x_d, y_d + 1);
		nPuntaje = ValidarPuntajePremium(d_actual, d_prueba);
		if (puntaje < nPuntaje)
		{
			xOrigen = x_d;
			yOrigen = y_d;
			xDestino = x_d;
			yDestino = y_d + 1;
			puntaje = nPuntaje;
		}
		//Izq
		d_prueba = GetDulcedeTablero(x_d - 1, y_d);
		nPuntaje = ValidarPuntajePremium(d_actual, d_prueba);
		if (puntaje < nPuntaje)
		{
			xOrigen = x_d;
			yOrigen = y_d;
			xDestino = x_d - 1;
			yDestino = y_d;
			puntaje = nPuntaje;
		}
		//Derecho
		d_prueba = GetDulcedeTablero(x_d + 1, y_d);
		nPuntaje = ValidarPuntajePremium(d_actual, d_prueba);
		if (puntaje < nPuntaje)
		{
			xOrigen = x_d;
			yOrigen = y_d;
			xDestino = x_d + 1;
			yDestino = y_d;
			puntaje = nPuntaje;
		}
		return new int[] { xOrigen, yOrigen, xDestino, yDestino, puntaje };
	}
	private int ValidarPuntajePremium(Dulce actual, Dulce prueba)
	{
		if (actual.GetTipo() == Dulce.BOMBA && prueba.GetTipo() == Dulce.BOMBA)
		{
			return 13;
		}
		else if (actual.GetTipo() == Dulce.BOMBA && prueba.GetTipo() == Dulce.RAYAS || actual.GetTipo() == Dulce.RAYAS && prueba.GetTipo() == Dulce.BOMBA)
		{
			return 12;
		}
		else if (actual.GetTipo() == Dulce.BOMBA && prueba.GetTipo() == Dulce.PAQUETE || actual.GetTipo() == Dulce.PAQUETE && prueba.GetTipo() == Dulce.BOMBA)
		{
			return 11;
		}
		else if (actual.GetTipo() == Dulce.PAQUETE && prueba.GetTipo() == Dulce.RAYAS || actual.GetTipo() == Dulce.RAYAS && prueba.GetTipo() == Dulce.PAQUETE)
		{
			return 10;
		}
		else if (actual.GetTipo() == Dulce.PAQUETE && prueba.GetTipo() == Dulce.PAQUETE)
		{
			return 9;
		}
		else if (actual.GetTipo() == Dulce.RAYAS && prueba.GetTipo() == Dulce.RAYAS)
		{
			return 8;
		}
		return 0;
	}
	//Sensor

	//Actuador

	public Bot()
	{
		tablero = new Dulce[11, 11];
		InicializarTablero();
		int[,] nuevosDulces = GenerarTableroAleatorio();
		ModificarTablero(nuevosDulces);
		PrintTablero();
		DecidirMovimiento();
	}
}