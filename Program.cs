using System;
using System.IO;
using System.Linq;

namespace WordStats
{
	// Gathers some stats based on the word list from https://github.com/dwyl/english-words.  
	// Note: words_alpha.txt (included in this solution) is not mine; it comes from the link above.

	class Program
	{
		static void Main(string[] args)
		{
			var words = File.ReadAllLines("words_alpha.txt");
			int allWordCount = words.Length;
			int fourLetterWordCount = 0;

			fourLetterWordCount = words.Count(word => word.Length == 4);

			Console.WriteLine($"Number of all words: {allWordCount}  four letter words: {fourLetterWordCount}");
		}
	}
}
