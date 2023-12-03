namespace AdventOfCode2023
{
    internal class D02 : Day
    {
        private IEnumerable<Game> _games;

        internal override void SolvePart1()
        {
            var lines = FileHelper.ReadLines(FileName);
            _games = GetGames(lines);

            var sum = 0;
            foreach (var game in _games)
            {
                game.Limit = new Color { R = 12, G = 13, B = 14 };
                if (game.IsPossible()) sum += game.Id;
            }

            Console.WriteLine(sum);
        }

        internal override void SolvePart2()
        {
            var sum = 0;
            foreach (var game in _games)
            {
                sum += game.Power();
            }

            Console.WriteLine(sum);
        }

        private static IEnumerable<Game> GetGames(IEnumerable<string> lines)
        {
            var games = new List<Game>();

            foreach (var line in lines)
            {
                var data = line.Split(':');

                var l = new List<Color>();
                var revealed = data[1].Split(';');
                foreach (var reveal in revealed)
                {
                    var colors = reveal.Split(",");
                    var c = new Color();
                    foreach (var color in colors)
                    {
                        var d = color.Split(" ", StringSplitOptions.RemoveEmptyEntries);
                        if (d[1][0] == 'b') c.B = int.Parse(d[0]);
                        if (d[1][0] == 'g') c.G = int.Parse(d[0]);
                        if (d[1][0] == 'r') c.R = int.Parse(d[0]);
                    }
                    l.Add(c);
                }

                var game = new Game
                {
                    Id = int.Parse(data[0].Split(' ')[1]),
                    Colors = l
                };

                games.Add(game);
            }

            return games;
        }

        internal class Game
        {
            internal required int Id { get; set; }

            internal required IEnumerable<Color> Colors { get; set; }

            internal Color Limit { get; set; }

            internal bool IsPossible()
            {
                foreach (var color in Colors)
                {
                    if (color.R > Limit.R || color.G > Limit.G || color.B > Limit.B) return false;
                }

                return true;
            }

            internal int Power()
            {
                return Colors.Select(c => c.R).Max() *
                       Colors.Select(c => c.G).Max() *
                       Colors.Select(c => c.B).Max();
            }
        }

        internal struct Color
        {
            internal int R { get; set; }

            internal int G { get; set; }

            internal int B { get; set; }
        }
    }
}
