using System;
using System.ComponentModel.DataAnnotations;
using System.Text;
using Microsoft.EntityFrameworkCore;
using System.Linq;
using System.IO;

namespace Lab11_5
{
    class Film
    {
        [Key]
        public int film_id { get; set; }
        public string title { get; set; }
        public string description { get; set; }
        public string release_year { get; set; }
        public byte language_id { get; set; }
        public byte? original_language_id { get; set; }
        public byte rental_duration { get; set; }
        public decimal rental_rate { get; set; }
        public Int16? length { get; set; }
        public decimal replacement_cost { get; set; }
        public string rating { get; set; }
        public string special_features { get; set; }
        public DateTime last_update { get; set; }

        public Film (string title, string description, string release_year, 
            byte rental_duration, Decimal rental_rate, Int16? length, Decimal replacement_cost, string rating)
        {
            this.title = title;
            this.description = description;
            this.release_year = release_year;
            this.rental_duration = rental_duration;
            this.rental_rate = rental_rate;
            this.length = length;
            this.replacement_cost = replacement_cost;
            this.rating = rating;

            special_features = "Trailers";
            last_update = DateTime.Now;
            language_id = 1;
            original_language_id = 1;
        }
    }

    class SakilaContext : DbContext
    {
        public DbSet<Film> Film { get; set; }
        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            optionsBuilder.UseSqlServer(@"Server=localhost\sqlexpress;Database=sakila;Trusted_Connection=True;");

        }
    }

    class Program
    {
        static void Main(string[] args)
        {
            SakilaContext sakila = new SakilaContext();

            Film film1917 = new Film("1917", "War Drama by Director Sam Mendes", "2019", 3, 5.99m, 179, 19.99m, "R");
            Film joker = new Film("Joker", "Oscar Nominated SuperHero Drama", "2019", 3, 6.99m, 182, 23.99m, "R");
            Film skywalker = new Film("Star Wars: The Rise of Skywalker", "Trash Disney Fan-fic", "2019", 3, 4.99m, 202, 21.99m, "PG-13");

            sakila.Film.Add(film1917);
            sakila.Film.Add(joker);
            sakila.Film.Add(skywalker);
        
            //sakila.SaveChanges();

            Film[] allfilms = sakila.Film.ToArray();
           
            Film[] allFilms = (from db in sakila.Film select new Film(db.title, db.description, db.release_year, db.rental_duration, db.rental_rate, db.length, db.replacement_cost, db.rating)).ToArray();

            var newFilms = allFilms.Where(x => x.release_year == "2019");


            StringBuilder htmlBuilder = new StringBuilder();

            string openingHtml = "<html> \n <head> \n <title>New Sakila Films</title> \n  </head> \n <body> \n " +
                "<h1>New Films Coming Soon to a Theater Near You!</h1> \n" +
                "<ul>";

            htmlBuilder.Append(openingHtml);

            foreach (var film in newFilms)
            {
                htmlBuilder.Append("<li>" + film.title + " " + film.description + "</li>");
            }

            string closingHtml = "<ul> \n" +
                "</body>";

            htmlBuilder.Append(closingHtml);

            string filename = "D:\\output\\newfilms.html";
            File.WriteAllText(filename, htmlBuilder.ToString());
        }
    }
}
