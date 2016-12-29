using System;
using System.Collections.Generic;
using ServiceStack.DataAnnotations;

namespace ORM
{
	public class Artikl
	{
		[AutoIncrement]
		public long Id { get; set; }

		public String Naziv { get; set; }

		public Double Cijena { get; set; }

		[Reference]
		public Normativ Normativ { get; set; }

		public DateTime DatumProizvodnje { get; set;}

		public Artikl(String n, Double c)
		{
			Naziv = n;
			Cijena = c;
			DatumProizvodnje = DateTime.Now;
		}

		public override string ToString()
		{
			return string.Format("[Artikl: Id={0}, Naziv={1}, Cijena={2}, Datum={3}]", 
			                     Id, Naziv, Cijena, DatumProizvodnje.ToString("dd.MM.yyyy HH:mm"));
		}
	}
}