/*
* PRÁCTICA.............: Práctica 5
* NOMBRE y APELLIDOS...: Sara Blanco Muñoz
* CURSO y GRUPO........: 2º DAM
* TÍTULO de la PRÁCTICA: Estructuras de Datos Internas y Manejo de Ficheros
* FECHA de ENTREGA.....: 12 de Diciembre de 2017
*/

using System;
using System.Text.RegularExpressions;

namespace Practica5
{
    class Auxiliar
    {
        #region Lectura
        public static string leerCadena(string mensaje)
        {
            imprimirVerde(mensaje);
            mensaje = Console.ReadLine();

            if (mensaje.Length == 0)
            {
                imprimirError("\nERROR. Campo vacío.\n");
                esperaCorta();
            }

            return mensaje;
        }

        public static string leerNombre(string mensaje)
        {
            imprimirVerde(mensaje);
            mensaje = Console.ReadLine();

            if (mensaje.Length == 0)
            {
                imprimirError("\nERROR. Campo vacío.\n");
                esperaCorta();
            }
            else if (mensaje.Length > 20)
            {
                imprimirError("\nERROR. El nombre no debe ser más largo de 20 caracteres.\n");
                esperaCorta();
            }
            else
            {
                Regex letras = new Regex("^[a-zA-z ñÑÀ-ÿ]+$");

                if (!letras.Match(mensaje).Success)
                {
                    mensaje = "";
                    imprimirError("\nERROR. Un nombre no debe tener números.\n");
                    esperaCorta();
                }
            }

            return mensaje;
        }

        public static string leerCodAsig(string mensaje)
        {
            string cod = leerCadena(mensaje);

            if (!cod.Equals("") && cod.Length != 3)
            {
                imprimirError("\nERROR. El código de la asignatura debe tener 3 caracteres.\n");
                esperaCorta();
                cod = "";
            }

            return cod;
        }

        public static byte leerByte(string mensaje)
        {
            byte n = 0;
            
            imprimirVerde(mensaje);
            mensaje = Console.ReadLine();

            try
            {
                n = byte.Parse(mensaje);

                //if (n == 0)
                //{
                //    throw new OverflowException();
                //}
            }
            catch (ArgumentNullException)
            {
                imprimirError("\nERROR. Campo vacío.\n");
                esperaCorta();
            }
            catch (FormatException)
            {
                if (mensaje.Length == 0)
                    imprimirError("\nERROR. Campo vacío.\n");
                else
                    imprimirError("\nERROR. Valor no númerico.\n");
                esperaCorta();
            }
            catch (OverflowException)
            {
                imprimirError("\nERROR. Fuera de rango (0 - 255).\n");
                esperaCorta();
            }

            return n;
        }

        public static byte leerNAsig(string mensaje)
        {
            byte n = leerByte(mensaje);

            if (n < 4 || n > 7)
            {
                imprimirError("\nERROR. Fuera de rango (4 ~ 7).\n");
                esperaCorta();
                n = 0;
            }

            return n;
        }

        public static float leerNota(string mensaje)
        {
            float n = 11;

            imprimirVerde(mensaje);
            mensaje = Console.ReadLine();
            mensaje = mensaje.Replace('.', ',');

            try
            {
                n = float.Parse(mensaje);

                if (n < 0 || n > 10)
                {
                    imprimirError("\nERROR. Fuera de rango (0 ~ 10).\n");
                    esperaCorta();
                    n = 11;
                }
            }
            catch (ArgumentNullException)
            {
                imprimirError("\nERROR. Campo vacío.\n");
                esperaCorta();
            }
            catch (FormatException)
            {
                if (mensaje.Length == 0)
                    imprimirError("\nERROR. Campo vacío.\n");
                else
                    imprimirError("\nERROR. Valor no númerico.\n");
                esperaCorta();
            }
            catch (OverflowException)
            {
                imprimirError("\nERROR. Fuera de rango (0 - 255).\n");
                esperaCorta();
            }

            return (float) Math.Round(n, 1);
        }

        public static uint leerUInt(string mensaje)
        {
            imprimirVerde(mensaje);
            mensaje = Console.ReadLine();

            uint n = 0;

            try
            {
                if (mensaje.Length == 0)
                {
                    throw new ArgumentNullException();
                }

                n = UInt16.Parse(mensaje);

                //if (n == 0)
                //{
                //    throw new OverflowException();
                //}
            }
            catch (ArgumentNullException)
            {
                imprimirError("\nERROR. Campo vacío.\n");
                esperaCorta();
            }
            catch (FormatException)
            {
                imprimirError("\nERROR. Valor no númerico.\n");
                esperaCorta();
            }
            catch (OverflowException)
            {
                imprimirError("\nERROR. Fuera de rango (0 - 65.535).\n");
                esperaCorta();
            }

            return n;
        }
        #endregion

        #region Imprimir
        public static void imprimirError(string s)
        {
            Console.ForegroundColor = ConsoleColor.Red;
            Console.Write(s);
            Console.ResetColor();
        }

        public static void imprimirAzul(string s)
        {
            Console.ForegroundColor = ConsoleColor.Cyan;
            Console.Write(s);
            Console.ResetColor();
        }

        public static void imprimirVerde(string s)
        {
            Console.ForegroundColor = ConsoleColor.Green;
            Console.Write(s);
            Console.ResetColor();
        }
        #endregion

        public static void esperaCorta()
        {
            System.Threading.Thread.Sleep(800);
        }

        public static void esperaLarga()
        {
            System.Threading.Thread.Sleep(1000);
        }

        public static void pulsarContinuar()
        {
            Console.Write("\n\nPulse cualquier tecla para continuar.");
            Console.ReadKey();
        }
    }
}
