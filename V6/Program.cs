using System;
using Mono.Data.Sqlite;

namespace Sqlite
{
	class MainClass
	{
		public static void Main(string[] args)
		{
			// pristup bazi koja se nalazi u datoteci MojaBaza.db
			string connectionString = "URI=file:MojaBaza.db3,version=3";

			// instanciranje objekta za konekciju prema bazi
			SqliteConnection con = new SqliteConnection(connectionString);

			// otvaranje konekcije
			con.Open();

			// instanciranje SQL naredbe
			SqliteCommand dbcmd = con.CreateCommand();

			// SQL naredba koju želimo izvršiti
			dbcmd.CommandText = @"CREATE TABLE IF NOT EXISTS student (
				id integer primary key autoincrement,
				ime nvarchar(32),
				prezime nvarchar(32),
				datum_rodjenja datetime,
                zaposlen boolean)";

			// izvršavanje SQL naredbe koja ne vraća rezultate (nije upit)
			dbcmd.ExecuteNonQuery();

			// otpuštanje resursa
			dbcmd.Dispose();

			// instanciranje SQL naredbe za INSERT
			SqliteCommand dbInsertCmd = con.CreateCommand();

			// instanciranje datuma (31.12.1988)
			DateTime datum = new DateTime(1988, 12, 31);

			// postavljanje SQL komande za unos
			dbInsertCmd.CommandText = String.Format(@"INSERT INTO student (ime, prezime, datum_rodjenja, zaposlen) 
														VALUES ('Hrvoje', 'Horvat', {0}, 1)", datum.ToFileTime());

			// dobivamo povratnu informaciju o tome koliko redaka je unešeno/izmjenjeno
			int unesenoRedaka = dbInsertCmd.ExecuteNonQuery();

			Console.WriteLine("Unešeno je {0} redaka", unesenoRedaka);

			Console.WriteLine(DateTime.FromFileTime(1));

			// instanciranje objekta za provođenje SQL naredbe
			SqliteCommand dbQueryCmd = con.CreateCommand();

			// podešavanje same SQL naredbe
			dbQueryCmd.CommandText = "SELECT * FROM student";

			// izvršavanje naredbe vraća "reader"
			SqliteDataReader reader = dbQueryCmd.ExecuteReader();

			Console.WriteLine("Broj stupaca u rezultatu: {0}", reader.FieldCount);
			Console.WriteLine("Broj zapisa u tablici: {0}", pobrojiZapise(con));
			Console.WriteLine("---------------------------------------------");

			// dohvat sljedećeg retka 
			while (reader.Read())
			{
				// dohvat vrijednosti prvog stupca (i=0)
				int id = reader.GetInt32(0);
				// dohvati vrijednost drugog stupca (i=1) kao string
				string firstName = reader.GetString(1);
				// alternativni način dohvaćanja stringa uz indekser
				string lastName = (string)reader["prezime"];

				bool zaposlen = (bool)reader["zaposlen"];

				/* za datume je stvar malo kompliciranija, moramo spremljenu brojku pretvoriti u datum
				 * pomoću metode "DateTime.FromFileTime" */
				DateTime datumRodjenja = DateTime.FromFileTime(reader.GetInt64(3));

				Console.WriteLine("{0} {1} {2} {3} {4}",
								  id, firstName, lastName, datumRodjenja, zaposlen);
			}
			Console.WriteLine("---------------------------------------------");

			// clean up, uvijek je lijepo počistiti za sobom
			reader.Dispose();
			dbQueryCmd.Dispose();
			con.Close();
		}

		private static long pobrojiZapise(SqliteConnection con)
		{
			// primjer naredbe koja vraća skalarnu vrijednost

			SqliteCommand countCommand = con.CreateCommand();
			countCommand.CommandText = "SELECT COUNT(*) FROM student";

			long brojZapisa = (long) countCommand.ExecuteScalar();

			return brojZapisa;
		}
	}
}
