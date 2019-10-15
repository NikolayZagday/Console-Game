using System.Collections.Generic;
using System.Diagnostics;
using System.Windows.Input;

namespace ConsoleGame
{
    class TypingControl : GameEntity
    {
        public TypingControl()
        {
            TypingSW.Start();
            CursorBlinkingSW.Start();
        }

        /// <summary>
        /// The number of unique keystrokes per second
        /// </summary>
        public double UniqueKSPS { get; set; } = 40;

        /// <summary>
        /// The number of repetitive keystrokes per second
        /// </summary>
        public double RepetitiveKSPS { get; set; } = 3;

        public int MaxLength { get; set; } = 255;

        public double BackspacesPerSecond { get; set; } = 7;

        public double CursorBlinksPerSecond { get; set; } = 1;

        public bool CursorVisible { get; set; } = true;

        private Stopwatch TypingSW { get; set; } = new Stopwatch();

        private Stopwatch CursorBlinkingSW { get; set; } = new Stopwatch();

        private List<GameMessage> Letters { get; set; } = new List<GameMessage>();

        public string LetterSequence { get; private set; } = "";

        private bool CanType(char letter)
        {
            if (LetterSequence.Length >= MaxLength)
                return false;

            if (LetterSequence.Length == 0)
            {
                if (TypingSW.Elapsed.TotalSeconds > (1 / UniqueKSPS))
                {
                    TypingSW = Stopwatch.StartNew();
                    TypingSW.Start();
                    return true;
                }
            }

            if (letter == LetterSequence[LetterSequence.Length - 1])
            {
                //the keystroke is repetitive
                if (TypingSW.Elapsed.TotalSeconds > (1 / RepetitiveKSPS))
                {
                    TypingSW = Stopwatch.StartNew();
                    TypingSW.Start();
                    return true;
                }
            }
            else
            {
                //the keystroke is unique 
                if (TypingSW.Elapsed.TotalSeconds > (1 / UniqueKSPS))
                {
                    TypingSW = Stopwatch.StartNew();
                    TypingSW.Start();
                    return true;
                }
            }

            return false;
        }

        private bool CanDelete()
        {
            if (TypingSW.Elapsed.TotalSeconds > (1 / BackspacesPerSecond))
            {
                TypingSW = Stopwatch.StartNew();
                TypingSW.Start();
                return true;
            }

            return false;
        }

        public override void RecalculatePoints()
        {
            SkinCoordinates.Clear();

            foreach (var letter in Letters)
            {
                var en = letter.SkinCoordinates.GetEnumerator();

                while (en.MoveNext())
                {
                    var keyPoint = en.Entry.Key;
                    var charValue = en.Entry.Value;

                    if (!SkinCoordinates.ContainsKey(keyPoint))
                        SkinCoordinates.Add(keyPoint, charValue);
                }
            }
        }

        public void AddSpace()
        {
            if (!CanType(' '))
                return;

            int xOffset = 0;
            Letters.ForEach(x => { xOffset += x.Width; });

            var gm = new GameMessage()
            {
                Skin = "   ",
                PreciseX = this.PreciseX + xOffset,
                PreciseY = this.PreciseY
            };

            Letters.Add(gm);
            LetterSequence += " ";
            RecalculatePoints();
        }

        public void AddLetter(Key key)
        {
            if (!CanType(key.ToString().ToLower()[0]))
                return;

            int xOffset = 0;
            Letters.ForEach(x => { xOffset += x.Width; });

            var gm = new GameMessage()
            {
                Skin = GetASCII(key),
                PreciseX = this.PreciseX + xOffset,
                PreciseY = this.PreciseY
            };

            Letters.Add(gm);
            LetterSequence += key.ToString().ToLower();
            RecalculatePoints();
        }

        public void DeleteLastLetter()
        {
            if (!CanDelete())
                return;

            if (Letters.Count != 0)
                Letters.RemoveAt(Letters.Count - 1);

            if (LetterSequence.Length != 0)
                LetterSequence = LetterSequence.Substring(0, LetterSequence.Length - 1);

            RecalculatePoints();
        }

        private string GetASCII(Key key)
        {
            switch (key)
            {
                case Key.A:
                    return
                        "\n" +
                        "\n" +
                        "  __ _ \n" +
                        " / _` |\n" +
                        "| (_| |\n" +
                        " \\__,_|\n" +
                        " \n" +
                        " \n";
                case Key.B:
                    return
                        " _ \n" +
                        "| |\n" +
                        "| |__\n" +
                        "| '_ \\\n" +
                        "| |_) |\n" +
                        "|_.__/\n";
                case Key.C:
                    return
                        "\n" +
                        "\n" +
                        "  ___ \n" +
                        " / __|\n" +
                        "| (__ \n" +
                        " \\___|\n";
                case Key.D:
                    return
                        "     _ \n" +
                        "    | |\n" +
                        "  __| |\n" +
                        " / _` |\n" +
                        "| (_| |\n" +
                        " \\__,_|\n";
                case Key.E:
                    return
                        "\n" +
                        "\n" +
                        "  ___ \n" +
                        " / _ \\\n" +
                        "| __ /\n" +
                        " \\___|\n";
                case Key.F:
                    return
                        "  __ \n" +
                        " / _|\n" +
                        "| |_\n" +
                        "|  _|\n" +
                        "| |\n" +
                        "|_|\n";
                case Key.G:
                    return
                        " \n" +
                        " \n" +
                        "  __ _ \n" +
                        " / _` |\n" +
                        "| (_| |\n" +
                        " \\__, |\n" +
                        " __ / |\n" +
                        "| ___ /\n";
                case Key.H:
                    return
                        " _     \n" +
                        "| |    \n" +
                        "| |__  \n" +
                        "| '_ \\ \n" +
                        "| | | |\n" +
                        "|_| |_|\n";
                case Key.I:
                    return
                        " _ \n" +
                        "(_)\n" +
                        " _ \n" +
                        "| |\n" +
                        "| |\n" +
                        "|_|\n";
                case Key.J:
                    return
                        "   _ \n" +
                        "  (_)\n" +
                        "   _ \n" +
                        "  | |\n" +
                        "  | |\n" +
                        "  | |\n" +
                        " _/ |\n" +
                        "|__/ \n";
                case Key.K:
                    return
                        " _    \n" +
                        "| |   \n" +
                        "| | __\n" +
                        "| |/ /\n" +
                        "|   < \n" +
                        "|_|\\_\\\n";
                case Key.L:
                    return
                        " _ \n" +
                        "| |\n" +
                        "| |\n" +
                        "| |\n" +
                        "| |\n" +
                        "|_|\n";
                case Key.M:
                    return
                        "\n" +
                        "\n" +
                        " _ __ ___  \n" +
                        "| '_ ` _ \\ \n" +
                        "| | | | | |\n" +
                        "|_| |_| |_|\n";
                case Key.N:
                    return
                        "\n" +
                        "\n" +
                        " _ __  \n" +
                        "| '_ \\ \n" +
                        "| | | |\n" +
                        "|_| |_|\n";
                case Key.O:
                    return
                        "\n" +
                        "\n" +
                        "  ___  \n" +
                        " / _ \\ \n" +
                        "| (_) |\n" +
                        " \\___/ \n";
                case Key.P:
                    return
                        " _ __  \n" +
                        "| '_ \\ \n" +
                        "| |_) |\n" +
                        "| .__/ \n" +
                        "| |    \n" +
                        "|_|    \n";
                case Key.Q:
                    return
                        "  __ _ \n" +
                        " / _` |\n" +
                        "| (_| |\n" +
                        " \\__, |\n" +
                        "    | |\n" +
                        "    |_|\n";
                case Key.R:
                    return
                        "\n" +
                        "\n" +
                        " _ __ \n" +
                        "| '__|\n" +
                        "| |   \n" +
                        "|_|   \n";
                case Key.S:
                    return
                        "\n" +
                        "\n" +
                        " ___ \n" +
                        "/ __|\n" +
                        "\\__ \\\n" +
                        "|___/\n";
                case Key.T:
                    return
                        " _   \n" +
                        "| |  \n" +
                        "| |_ \n" +
                        "| __|\n" +
                        "| |_ \n" +
                        " \\__|\n";
                case Key.U:
                    return
                        "\n" +
                        "\n" +
                        " _   _ \n" +
                        "| | | |\n" +
                        "| |_| |\n" +
                        " \\__,_|\n";
                case Key.V:
                    return
                        "\n" +
                        "\n" +
                        "__   __\n" +
                        "\\ \\ / /\n" +
                        " \\ V / \n" +
                        "  \\_/  \n";
                case Key.W:
                    return
                        " \n" +
                        " \n" +
                        "__      __\n" +
                        "\\ \\ /\\ / /\n" +
                        " \\ V  V / \n" +
                        "  \\_/\\_/  \n";
                case Key.X:
                    return
                        "\n" +
                        "\n" +
                        "__  __\n" +
                        "\\ \\/ /\n" +
                        " >  < \n" +
                        "/_/\\_\\\n";
                case Key.Y:
                    return
                        "\n" +
                        "\n" +
                        " _   _ \n" +
                        "| | | |\n" +
                        "| |_| |\n" +
                        " \\__, |\n" +
                        "  __/ |\n" +
                        " |___/ \n";
                case Key.Z:
                    return
                        "\n" +
                        "\n" +
                        " ____\n" +
                        "|_  /\n" +
                        " / / \n" +
                        "/___|\n";
                default:
                    break;
            }
            return null;
        }
    }
}
