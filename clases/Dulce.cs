namespace cc_bot.Clases;

class Dulce{
//Valores estaticos
	//Comodin
	public static int OCULTO = 0;
	public static int BOMBA = 7;
	//Colores de dulces
	public static int AMARILLO = 1;
	public static int MORADO = 2;
	public static int NARANJA = 3;
	public static int ROJO = 4;
	public static int VERDE = 5;
	public static int AZUL = 6;
	//Tipo de dulce
	public static int NORMAL = 1;
	public static int VERTICAL = 2;
	public static int HORIZONTAL = 3;
	public static int PAQUETE = 4;

	//Valores
	private int color;
	private int tipo;
	//Constructor
	public Dulce(int color, int tipo)
	{
		this.color = color;
		this.tipo = tipo;
	}
	//Getters
	public int GetColor()
	{
		return color;
	}
	public int GetTipo()
	{
		return tipo;
	}
}
