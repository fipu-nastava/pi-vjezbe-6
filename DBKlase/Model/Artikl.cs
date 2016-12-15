using System;
namespace DBKlase
{
	public class Artikl
	{
		/* long? znači da atribut može poprimiti i NULL vrijednost, 
		 * a ne da je defaultno 0 što ne želimo jer se radi o ID-u
		 */
		public long? Id { get; set; }

		public String Naziv { get; set; }

		public double? Cijena { get; set; }
	}
}
