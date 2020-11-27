using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LinqToSQL
{
    class Program
    {
        public static void Main(string[] args)
        {
            //Esercizi.SelectMovies();
            //Esercizi.FilterMovieByGenere();
            //Esercizi.InsertMovie();
            //Esercizi.DeleteMovie();
            Esercizi.UpdateMovieByTitolo();
           

            Console.ReadKey(); //ReadKey e non ReadLine così posso chiudere con qualsiasi tasto e non solo con invio
        }

    }
}
