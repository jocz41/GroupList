/*
* PRÁCTICA.............: Práctica 5
* NOMBRE y APELLIDOS...: Sara Blanco Muñoz
* CURSO y GRUPO........: 2º DAM
* TÍTULO de la PRÁCTICA: Estructuras de Datos Internas y Manejo de Ficheros
* FECHA de ENTREGA.....: 12 de Diciembre de 2017
*/

using System;
using System.IO;
using System.Runtime.Serialization.Formatters.Binary;

namespace Practica5
{
    class Aplicacion
    {
        #region Deshabilitar botón cerrar
        //Fragmento de código que deshabilita el botón de cerrar la ventana
        private const int _MF_BYCOMMAND = 0x00000000;   //Número que indica un uso por defecto
        public const int SC_CLOSE = 0xF060;            //Número que identifica al botón de cerrar ventana

        //Necesitamos importar las user32.dll para poder manejar la ventana de consola
        [System.Runtime.InteropServices.DllImport("user32.dll")]
        public static extern int DeleteMenu(IntPtr hMenu, int nPosition, int wFlags);

        [System.Runtime.InteropServices.DllImport("user32.dll")]
        private static extern IntPtr GetSystemMenu(IntPtr hWnd, bool bRevert);

        [System.Runtime.InteropServices.DllImport("kernel32.dll", ExactSpelling = true)]
        private static extern IntPtr GetConsoleWindow();
        #endregion

        static void Main(string[] args)
        {
            /*DeleteMenu elimina la funcionalidad del ítem pasado como segundo parámetro
             * según dicte el tercer parámetro de la ventana indicada por el primer parámetro */

            DeleteMenu(GetSystemMenu(GetConsoleWindow(), false), SC_CLOSE, _MF_BYCOMMAND);

            Console.SetWindowSize(Console.LargestWindowWidth * 2 / 3, Console.LargestWindowHeight * 2 / 3);

            Aplicacion app = new Aplicacion();
                      
            byte opcion;

            do
            {
                opcion = app.menuInicial();

                if (opcion != 0)
                    app.seleccionInicial(opcion);
            }
            while (opcion != 4);
        }

        #region Menús

        private byte menuInicial()
        {
            Console.Clear();
            string cad1 = "┌────── Programa de Gestión de un Grupo ──────┐";

            Auxiliar.imprimirAzul("\n┌──────");
            Console.Write(" Programa de Gestión de un Grupo ");
            Auxiliar.imprimirAzul("──────┐\n");

            imprimirLineaMenu(cad1.Length, "");
            imprimirLineaMenu(cad1.Length, "¿Qué desea hacer?");
            imprimirLineaMenu(cad1.Length, "");
            imprimirLineaMenu(cad1.Length, "1.     Mostrar Listado de Grupos");
            imprimirLineaMenu(cad1.Length, "2.     Crear un Grupo");
            imprimirLineaMenu(cad1.Length, "3.     Eliminar un Grupo");
            imprimirLineaMenu(cad1.Length, "4.     Salir");

            Auxiliar.imprimirAzul("└");
            Auxiliar.imprimirAzul(new string('─', cad1.Length - 2) + "┘\n");

            return Auxiliar.leerByte("\nTeclee opción: ");
        }

        private void mostrarMenuOpciones(Grupo g)
        {
            byte opcion;

            do
            {
                opcion = menuOpciones(g.NombreGrupo);

                if (opcion != 0)
                    seleccionOpcion(opcion, g);
            }
            while (opcion != 5);
        }

        private byte menuOpciones(string s)
        {
            Console.Clear();
            string cad1 = "┌────── GESTIÓN DEL GRUPO " + s + " ──────┐";

            Auxiliar.imprimirAzul("\n┌──────");
            Console.Write(" GESTIÓN DEL GRUPO " + s + " ");
            Auxiliar.imprimirAzul("──────┐\n");

            imprimirLineaMenu(cad1.Length, "");
            imprimirLineaMenu(cad1.Length, "¿Qué desea hacer?");
            imprimirLineaMenu(cad1.Length, "");
            imprimirLineaMenu(cad1.Length, "1.     Añadir alumno");
            imprimirLineaMenu(cad1.Length, "2.     Borrar alumno");
            imprimirLineaMenu(cad1.Length, "3.     Consultar alumno");
            imprimirLineaMenu(cad1.Length, "4.     Acta del Grupo");
            imprimirLineaMenu(cad1.Length, "5.     Atrás");
            imprimirLineaMenu(cad1.Length, "6.     Salir");

            Auxiliar.imprimirAzul("└");
            Auxiliar.imprimirAzul(new string('─', cad1.Length-2) + "┘\n");

            return Auxiliar.leerByte("\nTeclee opción: ");
        }

        private void imprimirLineaMenu(int n, string s)
        {
            Auxiliar.imprimirAzul("| ");
            Console.Write(s);
            string sp = new string (' ', n - s.Length - 3);
            Auxiliar.imprimirAzul(sp + "|\n");
        }

        private void seleccionInicial(byte n)
        {
            Grupo g;

            switch (n)
            {
                case 1:

                    Console.Clear();
                    g = listarGrupos();

                    if (g != null)
                    {
                        mostrarGrupo(g);
                        mostrarMenuOpciones(g);
                    }

                    break;

                case 2:

                    Console.Clear();
                    g = buscarGrupo();

                    mostrarGrupo(g);
                    mostrarMenuOpciones(g);

                    break;

                case 3:

                    Console.Clear();
                    g = listarGrupos();

                    if (g != null)
                    {
                        bool sino;
                        string opcion;
                        
                        do
                        {
                            Auxiliar.imprimirVerde("\n¿Está seguro de que desea borrarlo? S/N ");
                            opcion = Auxiliar.leerCadena("").ToUpper();
                            sino = (opcion.Equals("S") || opcion.Equals("N"));

                            if (!sino)
                                Auxiliar.imprimirError("\nERROR. Debe contestar con el carácter 'S' o 'N'.\n");
                        }
                        while (!sino);

                        if (opcion.Equals("S"))
                        {
                            File.Delete(@"..\..\" + g.NombreGrupo + ".dat");
                        }                        
                    }

                    break;

                case 4:
                    
                    Environment.Exit(0);
                    break;

                default:
                    Auxiliar.imprimirError("\nERROR. Opción no válida.\n");
                    Auxiliar.esperaCorta();
                    break;                
            }
        }

        private void seleccionOpcion(byte n, Grupo g)
        {
            if (n > 1 && n < 5 && g.Alumnos.Count == 0)
            {
                Auxiliar.imprimirError("\nERROR. La lista de alumnos está vacía.\n");
                Auxiliar.esperaLarga();
            }
            else
            {
                uint nMat;
                Alumno a = null;

                switch (n)
                {
                    case 1:

                        Console.Clear();
                        a = creaAlumno(g);
                        Auxiliar.imprimirAzul("\nAlumno creado:\n");
                        a.imprimeAlumno();
                        g.aniadirAlumno(a);

                        Auxiliar.pulsarContinuar();
                        break;

                    case 2:

                        Console.Clear();
                        do
                        {
                            nMat = Auxiliar.leerUInt("\nIntroduzca el nº de matrícula: ");

                            if (nMat == 0)
                            {
                                Auxiliar.imprimirError("\nERROR. Nº de matrúcla no válido.\n");
                            }
                            else if (g.buscaAlumno(nMat) == null)
                            {
                                nMat = 0;
                                Auxiliar.imprimirError("\nERROR. El nº de matrícula no existe.\n");
                            }
                        }
                        while (nMat == 0);
                        
                        if (g.borraAlumno(g.buscaAlumno(nMat)))
                            Auxiliar.imprimirAzul("\nAlumno eliminado.");

                        Auxiliar.pulsarContinuar();

                        break;

                    case 3:

                        Console.Clear();

                        do
                        {
                            nMat = Auxiliar.leerUInt("\nIntroduzca el nº de matrícula: ");

                            if (nMat == 0)
                            {
                                Auxiliar.imprimirError("\nERROR. Nº de matrúcla no válido.\n");
                            }
                            else
                            {
                                Auxiliar.imprimirError("\nERROR. Nº de matrícula inexistente.\n");
                                a = g.buscaAlumno(nMat);
                            }
                        }
                        while (nMat == 0 || a == null);

                        if (a != null)
                        {
                            a.imprimeAlumno();
                        }

                        Auxiliar.pulsarContinuar();
                        break;

                    case 4:

                        Console.Clear();
                        g.actaGrupo();
                        Auxiliar.pulsarContinuar();
                        break;

                    case 5:
                        
                        guardarCambios(g);                        
                        break;

                    case 6:
                        
                        guardarCambios(g);
                        Environment.Exit(0);
                        break;

                    default:
                        Auxiliar.imprimirError("\nERROR. Opción no válida.\n");
                        Auxiliar.esperaCorta();
                        break;
                }
            }
        }

        #endregion

        #region Crear
        public static Grupo creaGrupo(string nombre)
        {
            Grupo g = null;

            while (nombre == null) 
            {
                nombre = Auxiliar.leerCadena("\nIntroduzca el nombre del grupo: ");
            } 

            nombre = nombre.ToUpper();

            //Comprobar si existe fichero con el nombre indicado

            byte nAsig;

            do
            {
                nAsig = Auxiliar.leerNAsig("\nIntroduzca el número de asignaturas: ");
            } while (nAsig == 0);

            string[] asig = new string[nAsig];

            Console.Clear();
            Console.WriteLine("\nIntroduzca los códigos de las asignaturas.");

            for (int i = 0; i < nAsig; i++)
            {
                asig[i] = Auxiliar.leerCodAsig("\nAsignatura " + (i+1) + ": ");
                asig[i] = asig[i].ToUpper();
                
                if (asig[i].Equals(""))
                    i--;
                else if (Array.IndexOf(asig, asig[i]) < i)
                {
                    Auxiliar.imprimirError("\nERROR. El código ya existe.\n");
                    i--;
                }
            }

            g = new Grupo(nombre, nAsig, asig);

            return g;
        }

        public Alumno creaAlumno(Grupo g)
        {
            Alumno a = null;

            string nombre;

            do
            {
                nombre = Auxiliar.leerNombre("\nIntroduzca el nombre del alumno: ");
            } while (nombre.Equals(""));

            nombre = nombre.ToUpper();

            float[] notas = new float[g.NAsig];

            Console.WriteLine("\nIntroduzca las notas del alumno.");

            for (int i = 0; i < g.NAsig; i++)
            {
                notas[i] = Auxiliar.leerNota("\n" + g.CodAsignaturas[i] + ": ");

                if (notas[i] == 11)
                    i--;
            }

            a = new Alumno(nombre, notas);

            return a;
        }
        #endregion

        public void mostrarGrupo(Grupo g)
        {
            Console.Clear();

            Auxiliar.imprimirAzul("\nGRUPO CREADO: ");
            Console.WriteLine(g.NombreGrupo);
            Auxiliar.imprimirAzul("\nASIGNATURAS: ");
            Console.WriteLine(string.Join(", ", g.CodAsignaturas));

            Auxiliar.pulsarContinuar();
        }

        private Grupo listarGrupos()
        {
            DirectoryInfo d = new DirectoryInfo(@"..\..\");
            FileInfo[] grupos = d.GetFiles("*.dat");
            Grupo g = null;
            string archivoGrupo = null;
            int opcion;

            do
            {
                opcion = Convert.ToInt16(mostrarListaGrupos(grupos));

                if (opcion != 0 && opcion < grupos.Length+1)
                    archivoGrupo = grupos[opcion-1].Name;
            }
            while (opcion < 1 || opcion > grupos.Length+1);

            if (archivoGrupo != null)
            {
                g = leerFichero(@"..\..\" + archivoGrupo);
            }

            return g;
        }

        private byte mostrarListaGrupos(FileInfo[] grupos)
        {
            int i = 0;

            Auxiliar.imprimirAzul("\n\tLISTADO DE GRUPOS\n\n");

            for (i = 0; i < grupos.Length; i++)
            {
                Console.WriteLine("\t" + (i + 1) + ".\t" + grupos[i].Name);
            }

            Console.WriteLine("\t" + (++i) + ".\tAtrás");

            return Auxiliar.leerByte("\nTeclee opción: ");
        }
 
        public Grupo buscarGrupo()
        {
            Grupo g = null;
            string nombre, ruta;

            do
            {
                nombre = Auxiliar.leerCadena("\nIntroduzca el nombre del grupo: ");
            } while (nombre.Equals(""));

            nombre = nombre.ToUpper();
            ruta = @"..\..\" + nombre + ".dat";

            if (File.Exists(ruta))
            {
                string opcion;
                bool sino;

                do
                {
                    Auxiliar.imprimirVerde("\nYa existe un grupo con este nombre. ¿Desea abrirlo? S/N ");
                    opcion = Auxiliar.leerCadena("").ToUpper();
                    sino = (opcion.Equals("S") || opcion.Equals("N"));

                    if (!sino)
                        Auxiliar.imprimirError("\nERROR. Debe contestar con el carácter 'S' o 'N'.\n");
                }
                while (!sino);

                if (opcion.Equals("S"))
                {
                    g = leerFichero(ruta);
                }
                else
                {
                    g = creaGrupo(nombre);
                }
            }
            else
            {
                g = creaGrupo(nombre);
            }

            return g;
        }

        private Grupo leerFichero(string ruta)
        {
            FileStream fs = new FileStream(ruta, FileMode.Open);
            BinaryFormatter bf = new BinaryFormatter();
            Grupo g = bf.Deserialize(fs) as Grupo;
            fs.Close();

            if (g.Alumnos != null && g.Alumnos.Count > 0)
            {
                Alumno a;
                uint i = 1;

                while ((a = g.buscaAlumno(i)) == null)
                {
                    i++;
                }
                
                a.ContMat = g.maxMat();
            }

            return g;
        }

        private void guardarCambios(Grupo g)
        {
            bool sino;
            string opcion;
            
            do
            {
                Auxiliar.imprimirVerde("\n¿Desea guardar los cambios realizados a " + g.NombreGrupo + "? S/N ");
                opcion = Auxiliar.leerCadena("").ToUpper();
                sino = (opcion.Equals("S") || opcion.Equals("N"));

                if (!sino)
                    Auxiliar.imprimirError("\nERROR. Debe contestar con el carácter 'S' o 'N'.\n");
            }
            while (!sino);

            if (opcion.Equals("S"))
            {
                g.creaFichero();
            }
        }
    }
}
