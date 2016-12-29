using System;
using Mono.Data.Sqlite;
using System.Data;
using System.Configuration;
using ServiceStack.OrmLite;

namespace ORM
{
	class MainClass
	{
		public static void Main(string[] args)
		{
			/* Primjer kako se connection string također može definirati izdvojeno
			 * pomoću konfiguracijske datoteke "App.config"
			 */
			var dbConnectionString = ConfigurationManager.AppSettings["baza"];

			var dbFactory = new OrmLiteConnectionFactory(dbConnectionString, SqliteDialect.Provider);

			// otvaranje konekcije stvara instancu koja predstavlja bazu
			var db = dbFactory.Open();

			// instanca konekcije pruža metode definirate pomoću ORM paketa
			// pogledati dokumentaciju na 
			// https://github.com/ServiceStack/ServiceStack.OrmLite
			db.DropAndCreateTable<Artikl>();
			db.DropAndCreateTable<Normativ>();

			// POJO instance modela klase Artikl
			var kava = new Artikl("Kava s mlijekom mala", 6.0);
			var mlijeko = new Artikl("Mlijeko 2.8% m.m.", 5.40);
			var secer = new Artikl("Šećer vrećica 3g", 0.50);
			var esspreso = new Artikl("Esspreso kava 1kg", 53.20);

			// spremanje u bazu pomoću "db" instance
			db.Save(mlijeko);
			db.Save(kava);
			db.Save(secer);
			db.Save(esspreso);

			// normativ kaže od čega je neki prodajni artikl sastavljen
			// primjerice kava ima: mlijeko (1 dcl), mljevenu kavu (7 grama), šećer (7g)
			var normativ = new Normativ();
			normativ.Stavke.Add(new StavkaNormativa() { ArtiklId = mlijeko.Id, Jmj = JMJ.L, Kolicina = 0.1 });
			normativ.Stavke.Add(new StavkaNormativa() { ArtiklId = secer.Id, Jmj = JMJ.KOM, Kolicina = 1 });
			normativ.Stavke.Add(new StavkaNormativa() { ArtiklId = esspreso.Id, Jmj = JMJ.KG, Kolicina = 0.007 });

			kava.Normativ = normativ;

			// spremi kavu u bazu, ali ovaj puta i spremi sve njene reference (normativ)
			db.Save(kava, references: true);


			// oblikovanje upita (pogledati ORMlite dokumentaciju)
			// https://github.com/ServiceStack/ServiceStack.OrmLite
			// primjer upita koji dohvaća sve artikle jeftinije od 10 kn
			var q = db.From<Artikl>()
					  .Where(x => x.Cijena <= 10);

			// dohvat rezultata s tima da se učitaju i reference
			var result = db.LoadSelect<Artikl>(q);

			// ispis SQL upita koji se izrvšio (ukoliko se želimo uvjeriti u ispravnost)
			// Console.WriteLine(db.GetLastSql().Normalize());

			foreach (var r in result)
			{
				// ispis rezultata
				Console.WriteLine(r);

				if (r.Normativ != null)
				{
					Console.WriteLine("== Normativ ======");
					foreach (var n in r.Normativ.Stavke)
					{
						// učitaj daljnje reference
						db.LoadReferences(n);
						//Console.WriteLine(db.GetLastSql().Normalize());
						Console.WriteLine(" * {0} {1} {2}", n.Artikl.Naziv, n.Jmj, n.Kolicina);
					}
				}
			}
		}
	}
}
