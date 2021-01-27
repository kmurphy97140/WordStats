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

			SortedSet<string> fourLetterWordList = new SortedSet<string>();

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
					fourLetterWordList.Add(word);
					for (int position = 0; position < 4; ++position)
					{
						char letter = chars[position];
						positionHistograms[position][letter]++;
					}
				}
			}

			ulong allLettersCount = 0;
			ulong fourLetterWordLetterCount = 0;
			ulong[] positionalCount = new ulong[4];
			Console.WriteLine($"Number of all words: {allWordCount}  four letter words: {fourLetterWordCount}");
			Console.WriteLine();
			Console.WriteLine("Letter histogram:");
			Console.WriteLine("Letter    AllWords  4LetterWords   Pos 1    Pos 2    Pos 3    Pos 4");
			Console.WriteLine("------    --------  ------------ -------- -------- -------- --------");
			foreach (var letter in Enumerable.Range('a', 26).Select(c => (char)c))
			{
				Console.WriteLine($"{letter}:        {allLettersHistogram[letter],8}      {fourLettersHistogram[letter],8} {positionHistograms[0][letter],8} {positionHistograms[1][letter],8} {positionHistograms[2][letter],8} {positionHistograms[3][letter],8}");
				allLettersCount += allLettersHistogram[letter];
				fourLetterWordLetterCount += fourLettersHistogram[letter];
				positionalCount[0] += positionHistograms[0][letter];
				positionalCount[1] += positionHistograms[1][letter];
				positionalCount[2] += positionHistograms[2][letter];
				positionalCount[3] += positionHistograms[3][letter];
			}

			using (FileStream fs = new FileStream("WordStats.html", FileMode.Create))
			using (StreamWriter sw = new StreamWriter(fs))
			{
				sw.WriteLine("<html>");
				sw.WriteLine("<head><title>Four Letter Word Statistics</title></head>");
				sw.WriteLine("<body>");
				sw.WriteLine(" <h2>Overall stats:</h2>");
				sw.WriteLine("   <table border='1' cellspacing='0' cellpadding='2'>");
				sw.WriteLine($"     <tr><td>Total number of words</td>							<td align='right'>{allWordCount}</td></tr>");
				sw.WriteLine($"     <tr><td>Total number of four letter words</td>				<td align='right'>{fourLetterWordCount}</td></tr>");
				sw.WriteLine($"     <tr><td>Total number of letters in all words</td>			<td align='right'>{allLettersCount}</td></tr>");
				sw.WriteLine($"     <tr><td>Total number of letters in four letter words</td>	<td align='right'>{fourLetterWordLetterCount}</td></tr>");
				sw.WriteLine($"     <tr><td>Total number of letters in position 1</td>			<td align='right'>{positionalCount[0]}</td></tr>");
				sw.WriteLine($"     <tr><td>Total number of letters in position 2</td>			<td align='right'>{positionalCount[1]}</td></tr>");
				sw.WriteLine($"     <tr><td>Total number of letters in position 3</td>			<td align='right'>{positionalCount[2]}</td></tr>");
				sw.WriteLine($"     <tr><td>Total number of letters in position 4</td>			<td align='right'>{positionalCount[3]}</td></tr>");
				sw.WriteLine("   </table>");

				sw.WriteLine(" <h2>Frequency and weighted probability of letter occurences:</h2>");
				sw.WriteLine("   <table border='1' cellspacing='0' cellpadding='2'>");
				sw.WriteLine("      <tr><th>Letter</th><th colspan='2'>Equally weighted</th><th colspan='2'>All Words</th><th colspan='2'>Four Letter Words</th><th colspan='2'>4LW Pos 1</th><th colspan='2'>4LW Pos 2</th><th colspan='2'>4LW Pos 3</th><th colspan='2'>4LW Pos 4</th></tr>");
				foreach (var letter in Enumerable.Range('a', 26).Select(c => (char)c))
				{
					sw.Write($"      <tr>");
					sw.Write($"<td>{letter}</td>");
					sw.Write($"<td>1 / 26</td>");
					sw.Write($"<td>{1.0 / 26.0:0.000%}</td>");
					sw.Write($"<td>{allLettersHistogram[letter]}</td>");
					sw.Write($"<td>{(double)allLettersHistogram[letter] / (double)allLettersCount:0.000%}</td>");
					sw.Write($"<td>{fourLettersHistogram[letter]}</td>");
					sw.Write($"<td>{(double)fourLettersHistogram[letter] / (double)fourLetterWordLetterCount:0.000%}</td>");
					sw.Write($"<td>{positionHistograms[0][letter]}</td>");
					sw.Write($"<td>{(double)positionHistograms[0][letter] / (double)positionalCount[0]:0.000%}  </td>");
					sw.Write($"<td>{positionHistograms[1][letter]}</td>");
					sw.Write($"<td>{(double)positionHistograms[1][letter] / (double)positionalCount[1]:0.000%}  </td>");
					sw.Write($"<td>{positionHistograms[2][letter]}</td>");
					sw.Write($"<td>{(double)positionHistograms[2][letter] / (double)positionalCount[2]:0.000%}  </td>");
					sw.Write($"<td>{positionHistograms[3][letter]}</td>");
					sw.Write($"<td>{(double)positionHistograms[3][letter] / (double)positionalCount[3]:0.000%}  </td>");

					sw.WriteLine("</tr>");
				}
				sw.WriteLine("   </table>");

				double equalWeight = 1.0 / 26.0;  // Each letter weighted equally
				double wordEqualWeight = equalWeight * equalWeight * equalWeight * equalWeight;

				double totalEqualWeightProbability = 0.0;
				double totalAllWordsWeightProbability = 0.0;
				double totalFourLettersWeightProbability = 0.0;
				double totalPositionalWeightProbability = 0.0;

				char previousLetter = '!'; // We're going to break the table up by letter, since it's kinda large

				sw.WriteLine(" <h2>Four letter words probability:</h2>");
				foreach (var word in fourLetterWordList)
				{
					if (word.First() != previousLetter)
					{
						if (previousLetter != '!')
							sw.WriteLine("   </table>");
						sw.WriteLine("   <table border='1' cellspacing='0' cellpadding='2'>");
						sw.WriteLine("     <tr><th>word</th><th>Equal Weight</th><th>All words weighted probability</th><th>Four letter words weighted probability</th><th>Four letter words positionally weighted probability</th></tr>");
						previousLetter = word.First();
					}
					double allWordsProbability = 1.0;
					double fourLetterWordsProbability = 1.0;
					double positionalProbability = 1.0;

					var chars = word.ToCharArray();

					// Probability for each word: the product of probabilities (weighted or not) that each letter will be picked
					for (int position = 0; position < 4; ++position)
					{
						var letter = chars[position];
						allWordsProbability = allWordsProbability * ((double)allLettersHistogram[letter] / (double)allLettersCount);
						fourLetterWordsProbability = fourLetterWordsProbability * ((double)fourLettersHistogram[letter] / (double)fourLetterWordLetterCount);
						positionalProbability = positionalProbability * ((double)positionHistograms[position][letter] / (double)positionalCount[position]);
					}

					// Probability of any word on the four letter list being drawn: the probability of each word added together.
					// (first word or second word or third word....)
					totalEqualWeightProbability += wordEqualWeight;
					totalAllWordsWeightProbability += allWordsProbability;
					totalFourLettersWeightProbability += fourLetterWordsProbability;
					totalPositionalWeightProbability += positionalProbability;

					sw.Write($"     <tr><td>{word}</td>");
					sw.Write($"<td>{wordEqualWeight:E3}</td>");
					sw.Write($"<td>{allWordsProbability:E3}</td>");
					sw.Write($"<td>{fourLetterWordsProbability:E3}</td>");
					sw.Write($"<td>{positionalProbability:E3}</td>");
					sw.WriteLine("</tr>");
				}

				sw.Write($"     <tr><td><b>Total probability: </b></td>");
				sw.Write($"<td>{totalEqualWeightProbability:0.000%}</td>");
				sw.Write($"<td>{totalAllWordsWeightProbability:0.000%}</td>");
				sw.Write($"<td>{totalFourLettersWeightProbability:0.000%}</td>");
				sw.Write($"<td>{totalPositionalWeightProbability:0.000%}</td>");
				sw.WriteLine("</tr>");
				sw.WriteLine("   </table>");


				sw.WriteLine("</body>");
				sw.WriteLine("</html>");
			}
		}
	}
}
