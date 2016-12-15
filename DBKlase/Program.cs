using System;
using System.Collections.Generic;

namespace DBKlase
{
	class MainClass
	{
		public static void Main(string[] args)
		{
			DB.OtvoriKonekciju();

			// unesimo nekoliko artikala
			var artikli = new List<Artikl>();
			artikli.Add(new Artikl() { Naziv = "Mlijeko", Cijena = 3.90 });
			artikli.Add(new Artikl() { Naziv = "Kruh", Cijena = 6.00 });
			artikli.Add(new Artikl() { Naziv = "Jogurt", Cijena = 2.40 });
			artikli.Add(new Artikl() { Naziv = "Slanac", Cijena = 1.50 });

			// primjetimo da ispisani artikli nemaju Id
			foreach (var a in artikli)
			{
				Console.WriteLine("ID: {0}, Naziv: {1}, Cijena: {2}", a.Id, a.Naziv, a.Cijena);
			}

			// spremanje artikala u bazu
			DBArtikl.DodajSve(artikli);

			Console.WriteLine("---------- artikli iz baze ----------------");

			// dohvat artikala iz baze, sada imaju Id jer im je baza dodijelila
			var artikliIzBaze = DBArtikl.DohvatiSve();
			foreach (var a in artikliIzBaze)
			{
				Console.WriteLine("ID: {0}, Naziv: {1}, Cijena: {2}", a.Id, a.Naziv, a.Cijena);
			}

			Console.WriteLine("---------- artikli od 2 do 4 kn ----------------");

			var artikliPretraga = DBArtikl.DohvatiPoCijeni(2, 4);
			foreach (var a in artikliPretraga)
			{
				Console.WriteLine("ID: {0}, Naziv: {1}, Cijena: {2}", a.Id, a.Naziv, a.Cijena);
			}

			DB.ZatvoriKonekciju();
		}
	}
}
