using System;
using System.Collections.Generic;
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
			int fourLetterWordCount = words.Count(word => word.Length == 4);

			var allLettersHistogram = new Dictionary<char, ulong>();
			var fourLettersHistogram = new Dictionary<char, ulong>();
			var positionHistograms = new Dictionary<char, ulong>[4] // letter occurrence by position in four letter words
			{
				new Dictionary<char, ulong>(),
				new Dictionary<char, ulong>(),
				new Dictionary<char, ulong>(),
				new Dictionary<char, ulong>(),
			};

			// Initialize counts for all 'a' to 'z' to 0. 
			foreach (var letter in Enumerable.Range('a',26).Select(c => (char)c))
			{
				allLettersHistogram.Add(letter, 0);
				fourLettersHistogram.Add(letter, 0);
				for (int position = 0; position < 4; ++position)
				{
					positionHistograms[position].Add(letter, 0);
				}
			}

			// Start counting!
			foreach (var word in words)
			{
				bool isFourLetterWord = word.Length == 4;
				var chars = word.ToCharArray();
				foreach (var c in chars)
				{
					allLettersHistogram[c]++;
					if (isFourLetterWord)
						fourLettersHistogram[c]++;
				}
				if (isFourLetterWord)
				{
					for (int position = 0; position < 4; ++position)
					{
						positionHistograms[position][chars[position]]++;
					}
				}
			}

			Console.WriteLine($"Number of all words: {allWordCount}  four letter words: {fourLetterWordCount}");
			Console.WriteLine();
			Console.WriteLine("Letter histogram:");
			Console.WriteLine("Letter    AllWords  4LetterWords   Pos 1    Pos 2    Pos 3    Pos 4");
			Console.WriteLine("------    --------  ------------ -------- -------- -------- --------");
			foreach (var letter in Enumerable.Range('a', 26).Select(c => (char)c))
			{
				Console.WriteLine($"{letter}:        {allLettersHistogram[letter],8}      {fourLettersHistogram[letter],8} {positionHistograms[0][letter],8} {positionHistograms[1][letter],8} {positionHistograms[2][letter],8} {positionHistograms[3][letter],8}");
			}
		}
	}
}
