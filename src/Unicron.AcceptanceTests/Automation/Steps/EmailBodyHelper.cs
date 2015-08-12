using System.Collections.Generic;
using System.Text.RegularExpressions;

namespace Starscream.AcceptanceTests.Automation.Steps
{
    public static class EmailBodyHelper
    {
        public static IEnumerable<string> GetUrls(string bodyData)
        {
            const string hRefPattern = "href\\s*=\\s*(?:[\"'](?<1>[^\"']*)[\"']|(?<1>\\S+))";
            Match m = Regex.Match(bodyData, hRefPattern, RegexOptions.IgnoreCase);
            var results = new List<string>();
            while (m.Success)
            {
                results.Add(m.Groups[0].ToString());
            }
            return results;
        }
    }
}