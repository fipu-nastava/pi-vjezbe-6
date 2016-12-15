using System;
using System.Collections.Generic;
using Mono.Data.Sqlite;

namespace DBKlase
{
	public static class DBArtikl
	{
		static DBArtikl()
		{
			// instanciranje SQL komande
			SqliteCommand com = DB.con.CreateCommand();

			// SQL naredba koju želimo izvršiti
			com.CommandText = @"
				
				DROP TABLE IF EXISTS Artikl; 
			
				CREATE TABLE Artikl (
					id integer primary key autoincrement,
					naziv nvarchar(32),
					cijena double)";

			// izvršavanje SQL naredbe koja ne vraća rezultate (nije upit)
			com.ExecuteNonQuery();

			// otpuštanje resursa
			com.Dispose();	
		}

		public static List<Artikl> DohvatiSve()
		{
			var lista = new List<Artikl>();

			SqliteCommand c = DB.con.CreateCommand();

			c.CommandText = String.Format(@"SELECT id, naziv, cijena FROM Artikl");

			SqliteDataReader reader = c.ExecuteReader();
			while (reader.Read())
			{
				Artikl a = new Artikl();
				a.Naziv = (string) reader["naziv"];
				a.Cijena = (double) reader["cijena"];
				a.Id = (long)reader["id"];

				lista.Add(a);
			}

			c.Dispose();

			return lista;
		}

		public static List<Artikl> DohvatiPoCijeni(int min, int max)
		{
			var lista = new List<Artikl>();

			SqliteCommand c = DB.con.CreateCommand();

			c.CommandText = String.Format(@"SELECT id, naziv, cijena FROM Artikl WHERE cijena > {0} AND cijena < {1}", min, max);

			SqliteDataReader reader = c.ExecuteReader();
			while (reader.Read())
			{
				Artikl a = new Artikl();
				a.Naziv = (string)reader["naziv"];
				a.Cijena = (double)reader["cijena"];
				a.Id = (long)reader["id"];

				lista.Add(a);
			}

			c.Dispose();

			return lista;
		}

		public static void DodajSve(List<Artikl> artikli)
		{
			foreach (var a in artikli)
			{
				Dodaj(a);
			}
		}

		public static void Dodaj(Artikl a)
		{
			SqliteCommand c = DB.con.CreateCommand();

			c.CommandText = String.Format(@"INSERT INTO Artikl (naziv, cijena)
				VALUES ('{0}', {1})", a.Naziv, a.Cijena);

			c.ExecuteNonQuery();
			c.Dispose();
		}

	}
}
