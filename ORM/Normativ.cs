using System;
using System.Collections.Generic;
using ServiceStack.DataAnnotations;

namespace ORM
{
	public enum JMJ
	{
		KOM,
		L,
		KG
	}
	public class Normativ
	{
		public long ArtiklId { get; set; }

		public List<StavkaNormativa> Stavke { get; set; }

		public Normativ()
		{
			Stavke = new List<StavkaNormativa>();
		}
	}

	public class StavkaNormativa
	{
		public long ArtiklId { get; set; }

		[Reference]
		public Artikl Artikl { get; set; }

		public JMJ Jmj { get; set;}
		public Double Kolicina { get; set; }
	}
}
