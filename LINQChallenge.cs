using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Text.RegularExpressions;

namespace LINQChallenge {
	public class Challenge3 {
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

		/// <summary>
		/// Problem 5 - Initial Letters
		/// From the following list of names
		/// "Santi Cazorla, Per Mertesacker, Alan Smith, Thierry Henry, Alex Song, Paul Merson, Alexis Sánchez, Robert Pires, Dennis Bergkamp, Sol Campbell"
		/// find any groups of people who share the same initials as each other.
		/// </summary>
		[Fact]
		public void Problem5() {
			string input = "Santi Cazorla, Per Mertesacker, Alan Smith, Thierry Henry, Alex Song, Paul Merson, Alexis Sánchez, Robert Pires, Dennis Bergkamp, Sol Campbell";
			var result = input.Split(",")
				.GroupBy(x => x.Trim().Split(" ")[0][0] + x.Trim().Split(" ")[1][0])
				.Where(x => x.Count() > 1)
				.Select(x => x.Select(x => x.Trim()))
				.ToArray();
			Assert.Equal(["Santi Cazorla", "Sol Campbell"], result[0]);
			Assert.Equal(["Per Mertesacker", "Paul Merson"], result[1]);
			Assert.Equal(["Alan Smith", "Alex Song", "Alexis Sánchez"], result[2]);
		}

		/// <summary>
		/// Problem 6 - Video Editing
		/// A video is two hours long exactly, and we want to make some edits, cutting out the following time ranges(expressed in H:MM:SS):
		/// "0:00:00-0:00:05;0:55:12-1:05:02;1:37:47-1:37:51".
		/// (You can assume that the input ranges are in order and contain no overlapping portions)
		/// We would like to turn this into a sequence of time-ranges to keep.So in this example, the output should be:
		/// "0:00:05-0:55:12;1:05:02-1:37:47;1:37:51-2:00:00"
		/// </summary>
		[Fact]
		public void Problem6() {
			string input = "0:00:00-0:00:05;0:55:12-1:05:02;1:37:47-1:37:51";
			var result = input.Split([';', '-'])[(input.StartsWith("0:00:00") ? 1 : 0)..^(input.EndsWith("2:00:00") ? 1 : 0)]
				.Prepend(input.StartsWith("0:00:00") ? null : "0:00:00")
				.Append(input.EndsWith("2:00:00") ? null : "2:00:00")
				.Where(x => !string.IsNullOrEmpty(x))
				.Select((value, index) => new { value, index })
				.Aggregate("", (acc, next) => acc + next.value + (next.index % 2 == 0 ? "-" : ";"))[..^1];
			Assert.Equal("0:00:05-0:55:12;1:05:02-1:37:47;1:37:51-2:00:00", result);
		}
	}
}
