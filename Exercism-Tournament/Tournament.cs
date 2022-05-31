using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;


namespace Exercism_Tournament
{

    public static class Tournament
    {
        public static string resultString => "{0,-30} | {1,2} | {2,2} | {3,2} | {4,2} | {5,2}";
        public static void Tally(Stream inStream, Stream outStream)
        {
            var teamsList = new Dictionary<string, Team>();
            using (var sr = new StreamReader(inStream))
            {
                while (!sr.EndOfStream)
                {
                    var match = sr.ReadLine().Split(';');
                    teamsList.TryAdd(match[0], new Team());
                    teamsList.TryAdd(match[1], new Team());
                    switch (match[2])
                    {
                        case "win":
                            teamsList[match[0]].Wins++;
                            teamsList[match[1]].Losses++;
                            break;
                        case "loss":
                            teamsList[match[0]].Losses++;
                            teamsList[match[1]].Wins++;
                            break;
                        case "draw":
                            teamsList[match[0]].Draws++;
                            teamsList[match[1]].Draws++;
                            break;
                    }
                }
            }
            using (var sw = new StreamWriter(outStream))
            {
                sw.Write(string.Format(resultString, "Team", "MP", "W", "D", "L", "P"));
                teamsList.OrderByDescending(x => x.Value.Points).ThenBy(x => x.Key)
                    .ToList()
                    .ForEach(x =>
                    {
                        sw.Write("\n");
                        sw.Write(x.Value.Results(x.Key, resultString));
                    });
            }
        }
    }
    public class Team
    {
        public int Wins { get; set; }
        public int Losses { set; get; }
        public int Draws { set; get; }
        public int Matches => Wins + Losses + Draws;
        public int Points => Wins * 3 + Draws;
        public string Results(string name, string resultsFormat) => string.Format(resultsFormat, name, Matches, Wins, Draws, Losses, Points);
    }

}
