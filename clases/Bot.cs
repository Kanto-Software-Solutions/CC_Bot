using cc_bot.Clases;
using System;
using System.Windows.Forms;


class Bot
{
	//Variables
	private Dulce[,] tablero;

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
			int val1 = rand.Next(1, 8);
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
	private Dulce GetDulcedeTablero(int fila, int columna)
	{
		return tablero[fila, columna];
	}
	private Dulce[] GetDulcesColidan(int fila, int columna)
	{
		Dulce d1, d2, d3, d4, d5, d6, d7, d8;
		Dulce nulo = new Dulce(Dulce.OCULTO, Dulce.OCULTO);

		//Inmediatos
		d1 = GetDulcedeTablero(fila - 1, columna);
		d3 = GetDulcedeTablero(fila + 1, columna);
		d5 = GetDulcedeTablero(fila, columna - 1);
		d7 = GetDulcedeTablero(fila, columna + 1);

		//Validacion de nulos
		//Arriba
		if (d1.GetTipo() == Dulce.OCULTO)
		{
			//Nulo si el de abajo es nulo
			d2 = nulo;
		}
		else
		{
			//Valor real
			d2 = GetDulcedeTablero(fila - 2, columna);
		}
		//Abajo
		if (d3.GetTipo() == Dulce.OCULTO)
		{
			//Nulo si el de abajo es nulo
			d4 = nulo;
		}
		else
		{
			//Valor real
			d4 = GetDulcedeTablero(fila + 2, columna);
		}
		//Izquierda
		if (d5.GetTipo() == Dulce.OCULTO)
		{
			//Nulo si el de abajo es nulo
			d6 = nulo;
		}
		else
		{
			//Valor real
			d6 = GetDulcedeTablero(fila, columna - 2);
		}
		//Derecha
		if (d7.GetTipo() == Dulce.OCULTO)
		{
			//Nulo si el de abajo es nulo
			d8 = nulo;
		}
		else
		{
			//Valor real
			d8 = GetDulcedeTablero(fila, columna + 2);
		}
		return new Dulce[] { d1, d2, d3, d4, d5, d6, d7, d8 };
	}
	public int[] DecidirMovimiento()
	{
		//[xOrigen,yOrigen,xDestino,yDestino,puntaje]

		int puntaje = 0;

		int xOrigen = 0;
		int yOrigen = 0;

		int xDestino = 0;
		int yDestino = 0;

		//Verificar Movimientos Premium
		for (int i = 1; i < 10; i++)
		{
			for (int j = 1; j < 10; j++)
			{
				if (i % 2 == 1 && j % 2 == 1 || i % 2 == 0 && j % 2 == 0)
				{
					int[] premium = VerMovPremium(GetDulcedeTablero(i, j), i, j);
					if (puntaje < premium[4])
					{
						xOrigen = premium[0];
						yOrigen = premium[1];
						xDestino = premium[2];
						yDestino = premium[3];
						puntaje = premium[4];
					}
					if (puntaje >= 11)
					{
						return new int[] { xOrigen, yOrigen, xDestino, yDestino, puntaje };
					}
				}
			}
		}

		//Si hay un movimiento premium ejecutarlo
		if (puntaje != 0)
		{
			return new int[] { xOrigen, yOrigen, xDestino, yDestino, puntaje };
		}

		//Verificar Movimientos Normales
		for (int i = 1; i < 10; i++)
		{
			for (int j = 1; j < 10; j++)
			{
				int[] normal = VerMovNormal(GetDulcedeTablero(i, j), i, j);
				if (puntaje < normal[4])
				{
					xOrigen = normal[0];
					yOrigen = normal[1];
					xDestino = normal[2];
					yDestino = normal[3];
					puntaje = normal[4];
				}
			}
		}

		return new int[] { xOrigen, yOrigen, xDestino, yDestino, puntaje };
	}
	private int[] VerMovPremium(Dulce d_actual, int f_d, int c_d)
	{
		Dulce d_prueba;
		int puntaje = 0;
		int nPuntaje;

		int fOrigen = 0;
		int cOrigen = 0;

		int fDestino = 0;
		int cDestino = 0;
		//Izq
		d_prueba = GetDulcedeTablero(f_d, c_d - 1);
		nPuntaje = ValidarPuntajePremium(d_actual, d_prueba);
		if (puntaje < nPuntaje)
		{
			fOrigen = f_d;
			cOrigen = c_d;
			fDestino = f_d;
			cDestino = c_d - 1;
			puntaje = nPuntaje;
		}
		//Der
		d_prueba = GetDulcedeTablero(f_d, c_d + 1);
		nPuntaje = ValidarPuntajePremium(d_actual, d_prueba);
		if (puntaje < nPuntaje)
		{
			fOrigen = f_d;
			cOrigen = c_d;
			fDestino = f_d;
			cDestino = c_d + 1;
			puntaje = nPuntaje;
		}
		//Arriba
		d_prueba = GetDulcedeTablero(f_d - 1, c_d);
		nPuntaje = ValidarPuntajePremium(d_actual, d_prueba);
		if (puntaje < nPuntaje)
		{
			fOrigen = f_d;
			cOrigen = c_d;
			fDestino = f_d - 1;
			cDestino = c_d;
			puntaje = nPuntaje;
		}
		//Abajo
		d_prueba = GetDulcedeTablero(f_d + 1, c_d);
		nPuntaje = ValidarPuntajePremium(d_actual, d_prueba);
		if (puntaje < nPuntaje)
		{
			fOrigen = f_d;
			cOrigen = c_d;
			fDestino = f_d + 1;
			cDestino = c_d;
			puntaje = nPuntaje;
		}
		return new int[] { fOrigen, cOrigen, fDestino, cDestino, puntaje };
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

	private int[] VerMovNormal(Dulce d_actual, int fila, int columna)
	{
		int[] da, db, di, dd;
		Dulce d1, d2, d3, d4, d5, d6, d7, d8;

		int puntaje = 0;
		int nPuntaje;

		int fOrigen = 0;
		int cOrigen = 0;
		int fDestino = 0;
		int cDestino = 0;

		//Posciciones a verificar
		da = new int[] { fila - 1, columna };
		db = new int[] { fila + 1, columna };
		di = new int[] { fila, columna - 1 };
		dd = new int[] { fila, columna + 1 };

		//Arriba
		if (GetDulcedeTablero(da[0], da[1]).GetTipo() != Dulce.OCULTO)
		{
			Dulce[] colidan = GetDulcesColidan(da[0], da[1]);
			d1 = colidan[0];
			d2 = colidan[1];
			d3 = colidan[2];
			d4 = colidan[3];
			d5 = colidan[4];
			d6 = colidan[5];
			d7 = colidan[6];
			d8 = colidan[7];

			nPuntaje = ValidarPuntajeNormal(d_actual, new Dulce[] { d6, d5, d7, d8, d1, d2 });
			if (puntaje < nPuntaje)
			{
				fOrigen = fila;
				cOrigen = columna;
				fDestino = da[0];
				cDestino = da[1];
				puntaje = nPuntaje;
			}
		}
		//Abajo
		if (GetDulcedeTablero(db[0], db[1]).GetTipo() != Dulce.OCULTO)
		{
			Dulce[] colidan = GetDulcesColidan(db[0], db[1]);
			d1 = colidan[0];
			d2 = colidan[1];
			d3 = colidan[2];
			d4 = colidan[3];
			d5 = colidan[4];
			d6 = colidan[5];
			d7 = colidan[6];
			d8 = colidan[7];

			nPuntaje = ValidarPuntajeNormal(d_actual, new Dulce[] { d8, d7, d5, d6, d3, d4 });
			if (puntaje < nPuntaje)
			{
				fOrigen = fila;
				cOrigen = columna;
				fDestino = db[0];
				cDestino = db[1];
				puntaje = nPuntaje;
			}
		}

		//Izquierda
		if (GetDulcedeTablero(di[0], di[1]).GetTipo() != Dulce.OCULTO)
		{
			Dulce[] colidan = GetDulcesColidan(di[0], di[1]);
			d1 = colidan[0];
			d2 = colidan[1];
			d3 = colidan[2];
			d4 = colidan[3];
			d5 = colidan[4];
			d6 = colidan[5];
			d7 = colidan[6];
			d8 = colidan[7];

			nPuntaje = ValidarPuntajeNormal(d_actual, new Dulce[] { d4, d3, d1, d2, d5, d6 });
			if (puntaje < nPuntaje)
			{
				fOrigen = fila;
				cOrigen = columna;
				fDestino = di[0];
				cDestino = di[1];
				puntaje = nPuntaje;
			}
		}

		//Derecha
		if (GetDulcedeTablero(dd[0], dd[1]).GetTipo() != Dulce.OCULTO)
		{
			Dulce[] colidan = GetDulcesColidan(dd[0], dd[1]);
			d1 = colidan[0];
			d2 = colidan[1];
			d3 = colidan[2];
			d4 = colidan[3];
			d5 = colidan[4];
			d6 = colidan[5];
			d7 = colidan[6];
			d8 = colidan[7];

			nPuntaje = ValidarPuntajeNormal(d_actual, new Dulce[] { d2, d1, d3, d4, d7, d8 });
			if (puntaje < nPuntaje)
			{
				fOrigen = fila;
				cOrigen = columna;
				fDestino = dd[0];
				cDestino = dd[1];
				puntaje = nPuntaje;
			}
		}
		return new int[] { fOrigen, cOrigen, fDestino, cDestino, puntaje };
	}
	private int ValidarPuntajeNormal(Dulce d_prueba, Dulce[] dulces)
	{
		//Descartar bombas
		if (d_prueba.GetTipo() == Dulce.BOMBA ||
			dulces[0].GetTipo() == Dulce.BOMBA ||
			dulces[1].GetTipo() == Dulce.BOMBA ||
			dulces[2].GetTipo() == Dulce.BOMBA ||
			dulces[3].GetTipo() == Dulce.BOMBA ||
			dulces[4].GetTipo() == Dulce.BOMBA ||
			dulces[5].GetTipo() == Dulce.BOMBA)
		{
			return 1;
		}

		//Validar 5 en linea
		if (dulces[0].GetColor() == d_prueba.GetColor() &&
			dulces[1].GetColor() == d_prueba.GetColor() &&
			dulces[2].GetColor() == d_prueba.GetColor() &&
			dulces[3].GetColor() == d_prueba.GetColor())
		{
			return 7;
		}
		//Validar T
		if (dulces[0].GetColor() == d_prueba.GetColor() &&
			dulces[1].GetColor() == d_prueba.GetColor() &&
			dulces[4].GetColor() == d_prueba.GetColor() &&
			dulces[5].GetColor() == d_prueba.GetColor())
		{
			return 6;
		}
		if (dulces[2].GetColor() == d_prueba.GetColor() &&
			dulces[3].GetColor() == d_prueba.GetColor() &&
			dulces[4].GetColor() == d_prueba.GetColor() &&
			dulces[5].GetColor() == d_prueba.GetColor())
		{
			return 6;
		}
		//Validar 4 en linea
		if (dulces[0].GetColor() == d_prueba.GetColor() &&
			dulces[1].GetColor() == d_prueba.GetColor() &&
			dulces[2].GetColor() == d_prueba.GetColor())
		{
			return 5;
		}
		if (dulces[1].GetColor() == d_prueba.GetColor() &&
			dulces[2].GetColor() == d_prueba.GetColor() &&
			dulces[3].GetColor() == d_prueba.GetColor())
		{
			return 5;
		}
		//Validar 3 en linea
		if (dulces[0].GetColor() == d_prueba.GetColor() &&
			dulces[1].GetColor() == d_prueba.GetColor())
		{
			if (dulces[0].GetTipo() != Dulce.NORMAL || dulces[1].GetTipo() != Dulce.NORMAL || d_prueba.GetTipo() != Dulce.NORMAL)
			{
				return 2;
			}
			return 4;
		}
		if (dulces[1].GetColor() == d_prueba.GetColor() &&
			dulces[2].GetColor() == d_prueba.GetColor())
		{
			if (dulces[1].GetTipo() != Dulce.NORMAL || dulces[2].GetTipo() != Dulce.NORMAL || d_prueba.GetTipo() != Dulce.NORMAL)
			{
				return 2;
			}
			return 4;
		}
		if (dulces[2].GetColor() == d_prueba.GetColor() &&
			dulces[3].GetColor() == d_prueba.GetColor())
		{
			if (dulces[2].GetTipo() != Dulce.NORMAL || dulces[3].GetTipo() != Dulce.NORMAL || d_prueba.GetTipo() != Dulce.NORMAL)
			{
				return 2;
			}
			return 4;
		}
		if (dulces[4].GetColor() == d_prueba.GetColor() &&
			dulces[5].GetColor() == d_prueba.GetColor())
		{
			if (dulces[4].GetTipo() != Dulce.NORMAL || dulces[5].GetTipo() != Dulce.NORMAL || d_prueba.GetTipo() != Dulce.NORMAL)
			{
				return 2;
			}
			return 4;
		}
		return 0;
	}
	//Sensor
	public bool Jugar()
	{
		int[,] nuevosDulces = CandyColorMapper.GetBoard();
		ModificarTablero(nuevosDulces);
		PrintTablero();
		int[] a = DecidirMovimiento();
		Console.WriteLine($"[{a[0]} {a[1]}]  [{a[2]} {a[3]}] PUNTAJE: {a[4]}");

		return true;
	}
	//Actuador
	static void MoverMouse(int x, int y)
	{
		Cursor.Position = new System.Drawing.Point(x, y);
	}
	//Constructor
	public Bot()
	{
		tablero = new Dulce[11, 11];
		InicializarTablero();
		//PrintTablero();

		int[,] nuevosDulces = CandyColorMapper.GetBoard();

		ModificarTablero(nuevosDulces);
		PrintTablero();
		int[] a = DecidirMovimiento();
		Console.WriteLine($"[{a[0]} {a[1]}]  [{a[2]} {a[3]}] PUNTAJE: {a[4]}");
		Console.ReadLine();

	}
}