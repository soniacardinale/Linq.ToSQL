using System;
using System.Collections.Generic;
using System.Data.Linq;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LinqToSQL
{
    public class Esercizi
    {
        const string conn = @"Persist Security Info=False; Integrated Security=True; Initial Catalog=CinemaDB; Server=WINAPHDFXGCXX6X\SQLEXPRESS";

        //Selezionare film
        public static void SelectMovies()
        {
            CinemaDataContext db = new CinemaDataContext(conn); //mi assicuro che punti al DB giusto

            Console.WriteLine("Film disponibili: ");
            foreach (var movie in db.Movies)
            {
                Console.WriteLine("({0}) {1}-{2}", movie.ID, movie.Titolo, movie.Genere);
            }
        }
        
        //Filtrare Film
        public static void FilterMovieByGenere()
        {
            CinemaDataContext db = new CinemaDataContext(conn);


            Console.WriteLine("Film disponibili: ");
            foreach (var movie in db.Movies)
            {
                Console.WriteLine("({0}) {1}-{2}", movie.ID, movie.Titolo, movie.Genere);
            }

            //QuerySyntax
            Console.WriteLine("Genere: ");
            string Genere = Console.ReadLine();

            IQueryable<Movy> moviesFiltered = //posso mettere anche var ma meglio accertarsi che sia IQuerable
                from m in db.Movies
                where m.Genere == Genere
                select m;

            Console.WriteLine("Film disponibili per il genere {0}: ", Genere);
            foreach (var movie in moviesFiltered)
            {
                Console.WriteLine("({0}){1}-{2}", movie.ID, movie.Titolo, movie.Genere);
            }

        }

        //Inserire Record
        public static void InsertMovie()
        {
            CinemaDataContext db = new CinemaDataContext(conn);
            SelectMovies();

            var movieToInsert = new Movy();
            movieToInsert.Titolo = "LaLaLand";
            movieToInsert.Genere = "Romantico";
            movieToInsert.Durata = 120;

            db.Movies.InsertOnSubmit(movieToInsert); //inserimento nella tabella in locale (che però ha una corrispondenza nel database)

            try
            {
                db.SubmitChanges();  //inserimento nel databse
            }
            catch (Exception e)
            {
                Console.WriteLine(e);
            }
            Console.WriteLine("-----AGGIORNAMENTO FILM-----");
            SelectMovies();

        }

        //Eliminare Record
        public static void DeleteMovie()
        {
            CinemaDataContext db = new CinemaDataContext(conn);
            SelectMovies();
            var deleteMovie = db.Movies.SingleOrDefault(m => m.ID == 10);
            if (deleteMovie != null)
            {
                db.Movies.DeleteOnSubmit(deleteMovie);
            }
            try
            {
                db.SubmitChanges();  //inserimento nel databse
            }
            catch (Exception e)
            {
                Console.WriteLine(e);
            }
            Console.WriteLine("-----AGGIORNAMENTO FILM-----");
            SelectMovies();
        }

        //Update Movie
        public static void UpdateMovieByTitolo()
        {
            CinemaDataContext db = new CinemaDataContext(conn);

            Console.WriteLine("Dimmi il Titolo del Film da aggiornare: ");
            string Titolo = Console.ReadLine();

            IQueryable<Movy> filmByTitolo =
                from film in db.Movies
                where film.Titolo == Titolo
                select film;

            Console.WriteLine("I Film trovati sono {0}: ", filmByTitolo.Count());
            if (filmByTitolo.Count() == 0)
            {
                return; //esco dalla funzione
            }

            SelectMovies();
            Console.WriteLine("Scrivere i valori aggiornati: ");
            Console.WriteLine("Titolo: ");
            string nuovoTitolo = Console.ReadLine();
            Console.WriteLine("Genere: ");
            string nuovoGenere = Console.ReadLine();
            Console.WriteLine("Durata: ");
            int nuovaDurata = Convert.ToInt32(Console.ReadLine());

            foreach (var item in filmByTitolo)
            {
                item.Titolo = nuovoTitolo;
                item.Genere = nuovoGenere;
                item.Durata = nuovaDurata;
            }

            //dobbiamo fare l'update e controllare un'eventuale concorrenza
            try
            {
                Console.WriteLine("Premi un tasto per mandare modifiche al DB");
                Console.ReadKey();
                db.SubmitChanges(ConflictMode.FailOnFirstConflict); //ConflictMode=Come gestire tutte le eccezioni
            }
            catch (ChangeConflictException e )
            {

                Console.WriteLine("Concurrency Error");
                Console.WriteLine(e);
                Console.ReadKey();

                db.ChangeConflicts.ResolveAll(RefreshMode.OverwriteCurrentValues);
                db.SubmitChanges();
            }
            
        }
        
    }
}
