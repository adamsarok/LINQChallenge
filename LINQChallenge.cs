using System.Collections.Generic;

namespace LINQChallenge {
	public class Challenge3 {
		//Solutions for LINQ Challenges by Mark Heath: https://markheath.net/post/2018/7/13/linq-challenge-3

		/// <summary>
		/// Problem 1 - Longest Sequence
		/// The following string contains number of sales made per day in a month:
		/// "1,2,1,1,0,3,1,0,0,2,4,1,0,0,0,0,2,1,0,3,1,0,0,0,6,1,3,0,0,0"
		/// How long is the longest sequence of days without a sale? (in this example it's 4)
		[Fact]
		public void Problem1() {
			string input = "1,2,1,1,0,3,1,0,0,2,4,1,0,0,0,0,2,1,0,3,1,0,0,0,6,1,3,0,0,0";
			int result = input.Split(",")
				.Select(x => x == "0" ? "0" : "1")
				.Aggregate("", (acc, next) => acc + next)
				.Split("1", StringSplitOptions.RemoveEmptyEntries)
				.Max(x => x.Length);
			Assert.Equal(4, result);
		}

		/// <summary>
		/// Problem 2 - Full House
		/// poker a hand is a "full house" if it contains three cards of one value and two of another value. 
		/// The following string defines five poker hands, separated by a semicolon:
		/// "4♣ 5♦ 6♦ 7♠ 10♥;10♣ Q♥ 10♠ Q♠ 10♦;6♣ 6♥ 6♠ A♠ 6♦;2♣ 3♥ 3♠ 2♠ 2♦;2♣ 3♣ 4♣ 5♠ 6♠".
		/// Write a LINQ expression that returns an sequence containing only the "full house" hands.
		/// </summary>
		[Fact]
		public void Problem2() {
			string input = "4♣ 5♦ 6♦ 7♠ 10♥;10♣ Q♥ 10♠ Q♠ 10♦;6♣ 6♥ 6♠ A♠ 6♦;2♣ 3♥ 3♠ 2♠ 2♦;2♣ 3♣ 4♣ 5♠ 6♠";
			var full = new[] { 3, 2 };
			var result = input.Split(";")
				.Where(x =>
					x.Split(" ")
					.Select(x => x[..^1])
					.GroupBy(x => x)
					.All(x => full.Contains(x.Count()))
					);
			Assert.Equal(["10♣ Q♥ 10♠ Q♠ 10♦", "2♣ 3♥ 3♠ 2♠ 2♦"], result);
		}

		/// <summary>
		/// Problem 3 - Christmas Days
		/// What day of the week is Christmas day (25th December) on for the next 10 years (starting with 2018)? 
		/// The answer should be a string (or sequence of strings) starting: Tuesday,Wednesday,Friday,...
		/// </summary>
		[Fact]
		public void Problem3() {
			var years = Enumerable.Range(2018, 10);
			var result = years.Select(x => new DateTime(x, 12, 25).DayOfWeek.ToString()).Take(3);
			Assert.Equal(["Tuesday", "Wednesday", "Friday"], result);
		}

		/// <summary>
		/// Problem 4 - Anagrams
		/// From the following dictionary of words,
		/// "parts,traps,arts,rats,starts,tarts,rat,art,tar,tars,stars,stray"
		/// return all words that are an anagram of star(no leftover letters allowed).
		/// </summary>
		[Fact]
		public void Problem4() {
			string input = "parts,traps,arts,rats,starts,tarts,rat,art,tar,tars,stars,stray";
			var star = new[] { 's', 't', 'a', 'r' };
			var result = input.Split(",")
				.Where(x => x.Length == 4 && !x.ToCharArray().Except(star).Any());
			Assert.Equal(["arts", "rats", "tars"], result);
		}
	}
}
