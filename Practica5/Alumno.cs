/*
* PRÁCTICA.............: Práctica 5
* NOMBRE y APELLIDOS...: Sara Blanco Muñoz
* CURSO y GRUPO........: 2º DAM
* TÍTULO de la PRÁCTICA: Estructuras de Datos Internas y Manejo de Ficheros
* FECHA de ENTREGA.....: 12 de Diciembre de 2017
*/

using System;
using System.Linq;

namespace Practica5
{
    [Serializable]

    class Alumno : IComparable
    {
        #region Atributos

        private static uint contMat = 0;
        private uint nMat;
        private string nombre;
        private float[] notas;

        #endregion

        public Alumno(string nombre, float[] notas)
        {
            nMat = ++contMat;
            this.nombre = nombre;
            this.notas = new float[notas.Length];
            Array.Copy(notas, this.notas, notas.Length);
        }

        #region Propiedades

        public uint ContMat
        {
            set => contMat = value;
        }

        public uint NMat
        {
            get => nMat;
        }

        public string Nombre
        {
            get => nombre;
        }

        public float[] Notas
        {
            get => notas;
        }

        #endregion

        #region Métodos
        
        public void imprimeAlumno()
        {
            Console.WriteLine("\n" + this.NMat + "\t" + this.Nombre);
        }

        public float mediaAlumno()
        {
            float media = 0;

            foreach (float n in notas)
            {
                media += n;
            }

            return (float) Math.Round((media / notas.Length), 1);
        }

        public byte numSusAlumno()
        {
            int c = 0;

            foreach (int n in notas)
            {
                if (n < 5)
                    c++;
            }

            return Convert.ToByte(c);
        }

        #endregion

        public int CompareTo(object o)
        {
            Alumno b = (Alumno)o;

            return this.Nombre.CompareTo(b.Nombre);
        }
    }
}
