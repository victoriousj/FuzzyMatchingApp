using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Text;
using System.Text.RegularExpressions;

namespace FuzzyMatchingApp.Helpers
{
    public class ResultOrderHelper
    {
        /// <summary>
        /// Phonetic string matching algorithm, based on:  <a href="https://en.wikipedia.org/wiki/Soundex">SoundEx</a>
        /// </summary>
        public static string Soundex(string term)
        {
            StringBuilder result = new StringBuilder();

            if (term == null || !(term.Length > 0)) return string.Empty;

            string previousCode;
            string currentCode;

            result.Append(Char.ToUpper(term[0]));
            previousCode = string.Empty;

            for (int i = 1; i < term.Length; i++)
            {
                currentCode = EncodeChar(term[i]);

                if (currentCode != previousCode)
                    result.Append(currentCode);

                if (result.Length == 4) break;

                if (!currentCode.Equals(string.Empty))
                    previousCode = currentCode;
            }

            if (result.Length < 4)
                result.Append(new String('0', 4 - result.Length));

            return result.ToString();
        }

        public static string EncodeChar(char c)
        {

            switch (Char.ToLower(c))
            {
                case 'b':
                case 'f':
                case 'p':
                case 'v':
                    return "1";
                case 'c':
                case 'g':
                case 'j':
                case 'k':
                case 'q':
                case 's':
                case 'x':
                case 'z':
                    return "2";
                case 'd':
                case 't':
                    return "3";
                case 'l':
                    return "4";
                case 'm':
                case 'n':
                    return "5";
                case 'r':
                    return "6";
                default:
                    return string.Empty;
            }
        }

        // This is an implementation of the DIFFERENCe function in SQL SERVER
        /// <summary>
        /// Compares the equality between two string. 4 being a perfect match, while 0 being the worst.
        /// </summary>
        public static int Difference(string term1, string term2)
        {
            int result = 0;

            if (term1.Equals(string.Empty) || term2.Equals(string.Empty))
                return 0;

            string soundex1 = Soundex(term1),
            soundex2 = Soundex(term2);

            if (soundex1.Equals(soundex2))
                result = 4;

            else
            {
                if (soundex1[0] == soundex2[0])
                    result = 1;

                string sub1 = soundex1.Substring(1, 3);
                if (soundex2.IndexOf(sub1) > -1)
                {
                    result += 3;
                    return result;
                }

                string sub2 = soundex1.Substring(2, 2);
                if (soundex2.IndexOf(sub2) > -1)
                {
                    result += 2;
                    return result;
                }

                string sub3 = soundex1.Substring(1, 2);
                if (soundex2.IndexOf(sub3) > -1)
                {
                    result += 2;
                    return result;
                }

                char sub4 = soundex1[1];
                if (soundex2.IndexOf(sub4) > -1) result++;

                char sub5 = soundex1[2];
                if (soundex2.IndexOf(sub5) > -1) result++;

                char sub6 = soundex1[3];
                if (soundex2.IndexOf(sub6) > -1) result++;
            }

            return result;
        }

        // We use this inversion of the original as we are adding the aggregate between Difference
        // and the Levenshtein methods which wants a lower score to indicate a match while the 
        // original Difference implementation goes the opposite direction
        /// <summary>
        /// Returns a score to indicate the equality between two strings. 0 being a perfect match, 4 being the worst match.
        /// </summary>
        public static int DifferenceInverted(string term1, string term2)
        {
            int result = 4;

            if (term1.Equals(string.Empty) || term2.Equals(string.Empty))
                return 4;

            string soundex1 = Soundex(term1),
            soundex2 = Soundex(term2);

            if (soundex1.Equals(soundex2))
                result = 0;

            else
            {
                if (soundex1[0] == soundex2[0])
                    result = 3;

                string sub1 = soundex1.Substring(1, 3);
                if (soundex2.IndexOf(sub1) > -1)
                {
                    result -= 3;
                    return result;
                }

                string sub2 = soundex1.Substring(2, 2);
                if (soundex2.IndexOf(sub2) > -1)
                {
                    result -= 2;
                    return result;
                }

                string sub3 = soundex1.Substring(1, 2);
                if (soundex2.IndexOf(sub3) > -1)
                {
                    result -= 2;
                    return result;
                }

                char sub4 = soundex1[1];
                if (soundex2.IndexOf(sub4) > -1) result--;

                char sub5 = soundex1[2];
                if (soundex2.IndexOf(sub5) > -1) result--;

                char sub6 = soundex1[3];
                if (soundex2.IndexOf(sub6) > -1) result--;
            }

            return result;
        }

        /// <summary>
        /// <para>Levenshtein's algorithm is a string equality/distance algorithm which return an int who values indicates the amount of insertions/deletions/alterations required to make the two strings the same. </para>
        /// <para>E.g., 'Micheal' and 'Michael' would take two adjustments to make equal, so the score is two.</para>
        /// </summary>
        // https://people.cs.pitt.edu/~kirk/cs1501/Pruhs/Spring2006/assignments/editdistance/Levenshtein%20Distance.htm
        public static int Levenshtein(string term1, string term2)
        {
            int n = term1.Length,
                m = term2.Length;
            int[,] d = new int[n + 1, m + 1];

            if (n == 0) return m;
            if (m == 0) return n;

            for (int i = 0; i <= n; d[i, 0] = i++) { }

            for (int j = 0; j <= m; d[0, j] = j++) { }

            for (int i = 1; i <= n; i++)
            {
                for (int j = 1; j <= m; j++)
                {
                    int cost = (term2[j - 1] == term1[i - 1]) ? 0 : 1;

                    d[i, j] = Math.Min(
                        Math.Min(d[i - 1, j] + 1, d[i, j - 1] + 1),
                        d[i - 1, j - 1] + cost);
                }
            }
            return d[n, m];
        }

        /// <summary>
        /// Returns a phonetic representation of a string that can be used by a string distance method to compare equality. Based on Lawrence Phillips original algorithm <see cref="http://aspell.net/metaphone/metaphone.basic"> Found here </see>
        /// </summary>
        public static string MetaPhone(string term)
        {

            Regex nonLetters = new Regex("[^A-Z]", RegexOptions.Compiled),
                    vowels = new Regex("[AEIOU]", RegexOptions.Compiled),
                    frontv = new Regex("[EIY]", RegexOptions.Compiled),
                    varson = new Regex("[CSPTG]", RegexOptions.Compiled);

            string key = string.Empty;
            StringBuilder name2 = new StringBuilder(nonLetters.Replace(term.ToUpper(), ""));
            StringBuilder key2 = new StringBuilder();

            // Exception cases
            string first2chars = string.Empty;
            if (name2.Length >= 2)
                name2.ToString().Substring(0, 2);

            switch (first2chars)
            {
                case "KN":
                case "GN":
                case "PN":
                case "AE":
                case "WR":
                    name2.Remove(0, 1);
                    break;

                // Begin = WH, transform to W
                case "WH":
                    name2.Remove(1, 1);
                    break;
            }

            // Begin = X, transform to S
            if (name2[0] == 'X')
                name2[0] = 'S';

            int length = name2.Length;

            for (int i = 0; i < length; i++)
            {
                char c = name2[i];

                if (c != 'C' && i > 0 && name2[i - 1] == c)
                {
                    continue;
                }

                if (vowels.IsMatch(c.ToString()) && i == 0)
                {
                    key2.Append(c);
                    continue;
                }

                switch (c)
                {
                    case 'B':
                        if (i == length - 1 && name2[length - 2] == 'M') continue;
                        key2.Append(c);
                        break;

                    case 'C':
                        if (!(i > 0 && name2[i - 1] == 'S' && i + 1 < length && frontv.IsMatch(name2[i + 1].ToString())))
                        {
                            if (i + 2 < length && name2.ToString().Substring(i, 3) == "CIA")
                                key2.Append("X");

                            else if (i + 1 < length && frontv.IsMatch(name2[i + 1].ToString()))
                                key2.Append("S");

                            else if (i > 0 && i + 1 < length && name2[i - 1] == 'S' && name2[i + 1] == 'H')
                                key2.Append("K");

                            else if (i + 1 < length && name2[i + 1] == 'H')
                            {
                                if (i == 0 && i + 2 < length && !vowels.IsMatch(name2[i + 2].ToString()))
                                    key2.Append("K");

                                else
                                    key2.Append("X");
                            }

                            else
                                key2.Append("K");
                        }
                        break;

                    case 'D':
                        if (i + 2 < length && name2[i + 1] == 'G' && frontv.IsMatch(name2[i + 2].ToString()))
                            key2.Append("J");

                        else
                            key2.Append("T");

                        break;

                    case 'G':
                        if (i + 2 < length && name2[i + 1] == 'H' && !vowels.IsMatch(name2[i + 2].ToString()))
                            continue;

                        if (i + 1 < length && name2[i + 1] == 'N' ||
                            (i + 3 < length && name2[i + 1] == 'N' && name2[i + 2] == 'E' && name2[i + 3] == 'D'))
                            continue;

                        if (i > 0 && i + 1 < length && name2[i - 1] == 'D' && frontv.IsMatch(name2[i + 1].ToString()))
                            continue;

                        if (i > 0 && name2[i - 1] == 'G')
                            continue;

                        if (i + 1 < length && frontv.IsMatch(name2[i + 1].ToString()))
                            key2.Append("J");

                        else
                            key2.Append("K");

                        break;

                    case 'H':
                        if (!(i + 1 == length || (i > 0 && varson.IsMatch(name2[i - 1].ToString()))))
                        {
                            if (vowels.IsMatch(name2[i + 1].ToString()))
                                key2.Append("H");
                        }

                        break;

                    case 'F':
                    case 'J':
                    case 'L':
                    case 'M':
                    case 'N':
                    case 'R':
                        key2.Append(c);
                        break;

                    case 'K':
                        if (i > 0 && name2[i - 1] != 'C')
                            key2.Append("K");
                        else if (i == 0)
                            key2.Append("K");
                        break;

                    case 'P':
                        if (i + 1 < length && name2[i + 1] == 'H')
                            key2.Append("F");
                        else
                            key2.Append("P");
                        break;

                    case 'Q':
                        key2.Append("K");
                        break;

                    case 'S':
                        if (i > 0 && i + 2 < length && name2[i + 1] == 'I' && (name2[i + 2] == 'O' || name2[i + 2] == 'A'))
                            key2.Append("X");

                        else if (i + 1 < length && name2[i + 1] == 'H')
                            key2.Append("X");
                        else
                            key2.Append("S");

                        break;

                    case 'T':
                        if (i > 0 && i + 2 < length && name2[i + 1] == 'I' && (name2[i + 2] == 'O' && name2[i + 2] == 'A'))
                            key2.Append("X");

                        else if (i + 1 < length && name2[i + 1] == 'H')
                        {
                            if (!(i > 0 && name2[i - 1] == 'T'))
                                key2.Append("0");
                        }
                        else if (i + 2 < length && name2[i + 1] == 'C' && name2[i + 2] == 'H')
                            continue;

                        else
                            key2.Append("T");

                        break;

                    case 'V':
                        key2.Append("F");
                        break;

                    case 'W':
                    case 'Y':
                        if (i + 1 < length && vowels.IsMatch(name2[i + 1].ToString()))
                            key2.Append(c);

                        break;

                    case 'X':
                        key2.Append("KS");
                        break;

                    case 'Z':
                        key2.Append("S");
                        break;
                }
            }

            key = key2.ToString();
            return key;
        }


        // These following methods give you a score for comparing strings. The first compares two arrays and returns the best match between them
        // The second compares one string against an array and returns the best score between them
        // The third just compares two string.
        #region LevenshteinsDifference
        /// <summary>
        /// Find the phonetic similarity of multiple strings against multiple other string. The lower the score, the more similar they are.
        /// </summary>
        /// <returns>An Integer value, with zero being a perfect match</returns>
        public static int LevenshteinsDifference(string[] compareArray, string[] compareAgainstArray)
        {
            if (compareArray == null || compareAgainstArray == null) return default(int);

            int lowestMatch = 0;
            foreach (var compareTo in compareAgainstArray)
            {
                int rank = int.MaxValue;
                foreach (var compare in compareArray)
                {
                    int subRank = LevenshteinsDifference(compare, compareTo);
                    if (subRank < rank) rank = subRank;
                }
                lowestMatch += rank;
            }
            return lowestMatch;
        }

        /// <summary>
        /// Find the phonetic similarity of multiple strings against another strings. Returns the score for the closest match.
        /// </summary>
        /// <returns>An Integer value, with zero being a perfect match</returns>
        public static int LevenshteinsDifference(string compare, string[] compareAgainstArray)
        {
            if (compare == null || compareAgainstArray == null) return default(int);

            int lowestMatch = 0;
            foreach (var compareTo in compareAgainstArray)
            {
                int rank = LevenshteinsDifference(compare, compareTo);
                if (rank > lowestMatch) lowestMatch = rank;
            }
            return lowestMatch;
        }

        /// <summary>
        /// Combine the Metaphone and SoundEx Phonetic matching algorithm to find the similarity between two strings.
        /// </summary>
        public static int LevenshteinsDifference(string compare, string compareTo)
        {
            string comapreMetaPhone = MetaPhone(compare),
                    compareToMetaPhone = MetaPhone(compareTo);

            int differenceScore = DifferenceInverted(compare, compareTo),
                levenshteinScore = Levenshtein(comapreMetaPhone, compareToMetaPhone),
                levenshteinDifferenceScore = differenceScore + levenshteinScore;

            return levenshteinDifferenceScore;
        }
        #endregion


        /// <summary>
        /// Compare two words and find which is closest to the 'term' parameter
        /// </summary>
        /// <param name="term">The comparing string we compare the other two strings against</param>
        public static string FindBetterMatch(string term, string firstString, string secondString)
        {
            if (term == null || firstString == null || secondString == null) return string.Empty;

            int firstMatch = LevenshteinsDifference(term, firstString),
                secondMatch = LevenshteinsDifference(term, secondString);

            return firstMatch > secondMatch ? secondString : firstString;
        }
    }
}