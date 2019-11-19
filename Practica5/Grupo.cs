/*
* PRÁCTICA.............: Práctica 5
* NOMBRE y APELLIDOS...: Sara Blanco Muñoz
* CURSO y GRUPO........: 2º DAM
* TÍTULO de la PRÁCTICA: Estructuras de Datos Internas y Manejo de Ficheros
* FECHA de ENTREGA.....: 12 de Diciembre de 2017
*/

using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.Serialization.Formatters.Binary;

namespace Practica5
{
    [Serializable]
    class Grupo
    {
        #region Atributos

        private string nombreGrupo;
        private byte nAsig;
        private string[] codAsignaturas;
        private List<Alumno> alumnos;

        #endregion

        public Grupo(string nombre, byte nAsig, string[] codAsig)
        {
            this.nombreGrupo = nombre;
            this.nAsig = nAsig;

            this.codAsignaturas = new string[codAsig.Length];
            Array.Copy(codAsig, this.codAsignaturas, codAsig.Length);
            
            this.alumnos = new List<Alumno>();
        }

        #region Propiedades

        public string NombreGrupo
        {
            get => nombreGrupo;
        }

        public byte NAsig
        {
            get => nAsig;
        }

        public string[] CodAsignaturas
        {
            get => codAsignaturas;
        }

        public List<Alumno> Alumnos
        {
            get => alumnos;
        }

        #endregion

        #region Métodos

        public void aniadirAlumno(Alumno a)
        {
            alumnos.Add(a);
            alumnos.Sort();
        }

        private int localizaAlumno(uint nMat)
        {
            return alumnos.FindIndex(a => a.NMat == nMat);
        }

        public Alumno buscaAlumno(uint nMat)
        {
            int ind = localizaAlumno(nMat);
            
            return (ind < 0) ? null : alumnos.ElementAt(ind);
        }

        public bool borraAlumno(Alumno a)
        {
            bool borrado = false, sino;
            string opcion;

            a.imprimeAlumno();

            do
            {
                Auxiliar.imprimirVerde("\n¿Está seguro de que desea borrarlo? S/N ");
                opcion = Auxiliar.leerCadena("").ToUpper();
                sino = (opcion.Equals("S") || opcion.Equals("N"));

                if (!sino)
                    Auxiliar.imprimirError("\nERROR. Debe contestar con el carácter 'S' o 'N'.\n");
            }
            while (!sino);

            if (opcion.Equals("S") && a != null)
            {
                alumnos.Remove(a);
                borrado = true;
            }

            return borrado;
        }

        public float mediaAsignatura (string asignatura)
        {
            float media = 0;
            int ind = Array.IndexOf(CodAsignaturas, asignatura);

            foreach (Alumno a in alumnos)
            {
                media += a.Notas[ind];
            }

            return (float) Math.Round(media/alumnos.Count, 1);
        }
        
        public void actaGrupo()
        {
            Console.WriteLine("\nACTA DEL GRUPO " + this.NombreGrupo + "\n");

            int[] contador = { 0, 0, 0, 0 };
            float[] medias = new float[this.codAsignaturas.Length];
            byte nSus = 0;
            string[] columnas = { "MATRÍCULA", "NOMBRE".PadRight(20, ' '), "  " + string.Join("  ", this.CodAsignaturas), "MEDIA", "Nº SUS"};
            
            Array.Clear(medias, 0, medias.Length);

            int vert = Console.CursorTop, hztlUp = 0, hztl;

            foreach (string s in columnas)
            {
                Auxiliar.imprimirAzul(s);
                
                hztl = hztlUp;
                hztlUp = Console.CursorLeft + 1;

                Console.SetCursorPosition(hztl, Console.CursorTop + 1);
                Auxiliar.imprimirAzul(new string('-', s.Length) + "\n");

                foreach (Alumno a in alumnos)
                {
                    Console.SetCursorPosition(hztl, Console.CursorTop);

                    if (s.Equals("MATRÍCULA"))
                        Console.WriteLine(a.NMat);
                    else if (s.Contains("NOMBRE"))
                        Console.WriteLine(a.Nombre);
                    else if (s.Equals("MEDIA"))
                        Console.WriteLine(a.mediaAlumno().ToString().PadLeft(s.Length, ' '));
                    else if (s.Equals("Nº SUS"))
                    {
                        nSus = a.numSusAlumno();
                        Console.WriteLine(nSus.ToString().PadLeft(s.Length, ' '));

                        if (nSus > 3)
                            nSus = 3;

                        contador[nSus]++;
                    }
                    else
                    {
                        for (int i = 0; i < a.Notas.Length; i++)
                        {
                            Console.Write(a.Notas[i].ToString().PadLeft(5, ' '));
                            medias[i] += a.Notas[i];
                        }
                        Console.WriteLine();
                    }                    
                }
                
                if (!s.Equals(columnas.ElementAt(columnas.Length-1)))
                    Console.SetCursorPosition(hztlUp, vert);
            }

            Auxiliar.imprimirAzul("\nMEDIA".PadRight(columnas.ElementAt(0).Length + columnas.ElementAt(1).Length + 3));

            foreach (string s in CodAsignaturas)
            {
                Console.Write(mediaAsignatura(s).ToString().PadLeft(5));
            }

            //Recuento de suspensos

            Auxiliar.imprimirAzul("\n\nRECUENTO DE SUSPENSOS\n");

            for (int i = 0; i < contador.Length; i++)
            {
                if (i == contador.Length-1)
                    Console.WriteLine(i + " o más suspensos --> " + contador[i]);
                else
                    Console.WriteLine(i + " suspensos --> " + contador[i]);
            }
        }

        public void creaFichero()
        {
            FileStream fs = new FileStream(@"..\..\" + this.NombreGrupo + ".dat", FileMode.Create);
            BinaryFormatter bf = new BinaryFormatter();
            bf.Serialize(fs, this);
            fs.Close();
        }

        public uint maxMat()
        {
            uint nMatMax = 0;

            if (alumnos != null)
            {
                foreach (Alumno a in Alumnos)
                {
                    nMatMax = (a.NMat > nMatMax) ? a.NMat : nMatMax;
                }
            }

            return nMatMax;
        }

        #endregion
    }
}
