using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Input;

namespace ConsoleGame
{
    class Game
    {
        private bool Running { get; set; }

        private List<Creature> Enemies { get; set; } = new List<Creature>();
        private List<Bullet> EnemyBullets { get; set; } = new List<Bullet>();
        private List<IRenderable> Creatures { get; set; } = new List<IRenderable>();
        private double EnemiesPerSecond { get; set; } = 0.5;

        private Hero mCharacter = new Hero(bulletSpeed: 50) { PreciseX = 10, PreciseY = 10, Health = 20 };
        private Hero Character
        {
            get
            {
                return mCharacter;
            }
            set
            {
                Creatures.Remove(mCharacter);
                Creatures.Add(value);
                mCharacter = value;
            }
        } 

        private int RenderHeight { get; set; }
        private int RenderWidth { get; set; }
        private TimeSpan FrameTime { get; set; } = new TimeSpan(100);
        private Stopwatch EnemyAppearingSW { get; set; } = new Stopwatch();
        private Random Rand { get; set; } = new Random();
        private int Kills { get; set; }
        private GameState State { get; set; }
        private Menu MainMenu { get; set; }
        private Menu PauseMenu { get; set; }

        private TypingControl Typewriter { get; set; } = new TypingControl()
        {
            MaxLength = 3,
            UniqueKSPS = 30,
            RepetitiveKSPS = 3,
            BackspacesPerSecond = 10,
            PreciseX = 10,
            PreciseY = 10
        };

        private GameMessage GameOverMessage { get; set; } = new GameMessage
        {
            Skin = 
                "  _____ \n" +
                " / ____|\n" +
                "| |  __  __ _ _ __ ___   ___    _____   _____ _ __ \n" +
                "| | |_ |/ _` | '_ ` _ \\ / _ \\  / _ \\ \\ / / _ \\ '__|\n" +
                "| |__| | (_| | | | | | |  __/ | (_) \\ V /  __/ |   \n" +
                " \\_____|\\__,_|_| |_| |_|\\___|  \\___/ \\_/ \\___|_|\n" +
                "\n" +
                "     press escape to go back to main menu\n",
            PreciseX = 32,
            PreciseY = 10
        };

        public Game()
        {
            RenderHeight = Console.WindowHeight - 1;
            RenderWidth = Console.WindowWidth - 2;
        }

        public void Run()
        {
            Running = true;
            Thread t = new Thread(() =>
            {
                Console.CursorVisible = false;
                
                Stopwatch sw = new Stopwatch();
                EnemyAppearingSW.Start();

                State = GameState.Playing;

                InitMenus();

                Creatures.Add(Character);

                while (Running)
                {
                    sw = Stopwatch.StartNew();
                    sw.Start();

                    ProcessInput();
                    Update();
                    Render();

                    sw.Stop();
                    FrameTime = sw.Elapsed;

                    //if (FrameTime.TotalMilliseconds < 33)
                    //{
                    //    sw.Start();
                    //    Thread.Sleep(TimeSpan.FromMilliseconds(33) - FrameTime);
                    //    sw.Stop();
                    //    FrameTime = sw.Elapsed;
                    //}
                }
            });
            t.SetApartmentState(ApartmentState.STA);
            t.Start();
        }

        private void InitMenus()
        {
            MainMenu = new Menu(new List<IMenuItem>() {
                    new MenuItem() { Title = "Play", Action = () => 
                        {
                            Kills = 0;
                            Character = new Hero(bulletSpeed:50) { PreciseX = 10, PreciseY = 5, Health = 20 };
                            State = GameState.Playing;
                        } 
                    },
                    new MenuItem() { Title = "Hall of Fame", Action = () => { }},
                    new MultiItem("MultiItem", new List<MenuSubItem>()
                    {
                        new MenuSubItem() { Title = "Test item 1" },
                        new MenuSubItem() { Title = "Test item 2" },
                        new MenuSubItem() { Title = "Test item 3" },
                        new MenuSubItem() { Title = "Test item 4" }
                    }),
                    new MenuItem() { Title = "Exit", Action = () => { State = GameState.Exiting; } }
                });
            MainMenu.PreciseX = 52;
            MainMenu.PreciseY = 12;

            PauseMenu = new Menu(new List<IMenuItem>() {
                    new MenuItem() { Title = "Resume", Action = () => { State = GameState.Playing; } },
                    new MenuItem() { Title = "Back to main menu", Action = () => { State = GameState.OpenedMainMenu; }},
                    new MenuItem() { Title = "Exit", Action = () => { State = GameState.Exiting; } }
                });
            PauseMenu.PreciseX = 52;
            PauseMenu.PreciseY = 12;
        }

        #region Input Processing

        private void ProcessInput()
        {
            if (State == GameState.Typing)
            {
                ProcessTypingInput();
                return;
            }

            if (Keyboard.IsKeyDown(Key.Escape) && State == GameState.Playing)
                State = GameState.OpenedPauseMenu;

            if (Keyboard.IsKeyDown(Key.Escape) && State != GameState.Playing && State != GameState.OpenedPauseMenu)
                State = GameState.OpenedMainMenu;

            if (State == GameState.OpenedMainMenu || State == GameState.OpenedPauseMenu)
            {
                ProcessMenuInput();
                return;
            }

            if (State == GameState.ShowingDeathMessage)
                return;

            if (Keyboard.IsKeyDown(Key.Right))
            {
                if (Character.PreciseX < RenderWidth)
                    Character.PreciseX += Character.Speed * FrameTime.TotalSeconds;
            }

            if (Keyboard.IsKeyDown(Key.Left))
            {
                if (Character.PreciseX > 0)
                    Character.PreciseX -= Character.Speed * FrameTime.TotalSeconds;
            }

            if (Keyboard.IsKeyDown(Key.Up))
            {
                if (Character.PreciseY > 0)
                    Character.PreciseY -= Character.Speed * FrameTime.TotalSeconds;
            }

            if (Keyboard.IsKeyDown(Key.Down))
            {
                if (Character.PreciseY < RenderHeight - Character.Height)
                    Character.PreciseY += Character.Speed * FrameTime.TotalSeconds;
            }

            if (Keyboard.IsKeyDown(Key.Space))
            {
                var bullet = Character.Fire();
                if (bullet != null)
                    Creatures.Add(bullet);
            }
        }

        private void ProcessTypingInput()
        {
            //iterate through the alphabet 
            for (int i = 44; i <= 69; i++)
                if (Keyboard.IsKeyDown((Key)i))
                    Typewriter.AddLetter((Key)i);

            if (Keyboard.IsKeyDown(Key.Back))
                Typewriter.DeleteLastLetter();

            if (Keyboard.IsKeyDown(Key.Space))
                Typewriter.AddSpace();
        }

        private void ProcessMenuInput()
        {
            if (Keyboard.IsKeyDown(Key.Up))
            {
                if (State == GameState.OpenedMainMenu)
                    MainMenu.SwitchToUpperItem();
                else if (State == GameState.OpenedPauseMenu)
                    PauseMenu.SwitchToUpperItem();
            }

            if (Keyboard.IsKeyDown(Key.Down))
            {
                if (State == GameState.OpenedMainMenu)
                    MainMenu.SwitchToLowerItem();
                else if (State == GameState.OpenedPauseMenu)
                    PauseMenu.SwitchToLowerItem();
            }

            if (Keyboard.IsKeyDown(Key.Enter))
            {
                if (State == GameState.OpenedMainMenu)
                    MainMenu.ExecuteAction();
                else if (State == GameState.OpenedPauseMenu)
                    PauseMenu.ExecuteAction();
            }

            if (Keyboard.IsKeyDown(Key.Left))
            {
                if (State == GameState.OpenedMainMenu)
                    MainMenu.PreviousSubItem();
            }

            if (Keyboard.IsKeyDown(Key.Right))
            {
                if (State == GameState.OpenedMainMenu)
                    MainMenu.NextSubItem();
            }
        }

        #endregion

        private void Update()
        {
            if (State == GameState.Exiting)
            {
                Environment.Exit(0);
                return;
            }

            if (State == GameState.ShowingDeathMessage)
                return;

            if (State == GameState.Playing)
            {
                UpdatePlayingState();
            }
        }

        private void KillEnemy(Enemy enemy)
        {
            Kills++;
            Creatures.Remove(enemy);
            Enemies.Remove(enemy);
        }

        private void UpdatePlayingState()
        {
            UpdateCharacter();
            UpdateEnemies();
            SpawnEnemies();
        }

        private void UpdateCharacter()
        {
            if (Character.Health <= 0)
            {
                State = GameState.ShowingDeathMessage;
                Creatures.Clear();
                Enemies.Clear();

                return;
            }

            for (int i = 0; i < Character.Bullets.Count; i++)
            {
                var bullet = Character.Bullets[i];
                if (bullet.X >= RenderWidth)
                    Character.Bullets.Remove(bullet);
                else
                    bullet.PreciseX += bullet.Speed * FrameTime.TotalSeconds;

                for (int j = 0; j < Enemies.Count; j++)
                {
                    var enemy = Enemies[j];
                    if (enemy.IsEntityInBounds(bullet.X, bullet.Y))
                    {
                        enemy.Health -= bullet.Damage;
                        Character.Bullets.Remove(bullet);
                        Creatures.Remove(bullet);

                        if (enemy.Health <= 0)
                            KillEnemy((Enemy)enemy);
                    }
                }
            }
        }

        private void SpawnEnemies()
        {
            if (EnemyAppearingSW.Elapsed.TotalSeconds >= 1 / EnemiesPerSecond)
            {
                var enemy = new Enemy(RenderWidth - 1, Rand.Next(0, RenderHeight - 1)) { Health = 25 };
                Creatures.Add(enemy);
                Enemies.Add(enemy);
                EnemyAppearingSW = Stopwatch.StartNew();
                EnemyAppearingSW.Start();
            }
        }

        private void UpdateEnemies()
        {
            foreach (var enemy in Enemies)
            {
                enemy.PreciseX += enemy.Speed * FrameTime.TotalSeconds;
                var eBullet = ((Enemy)enemy).Fire();
                if (eBullet != null)
                {
                    EnemyBullets.Add(eBullet);
                    Creatures.Add(eBullet);
                }
            }

            UpdateEnemyBullets();
        }

        private void UpdateEnemyBullets()
        {
            for (int bulletIndex = 0; bulletIndex < EnemyBullets.Count; bulletIndex++)
            {
                var enemyBullet = EnemyBullets[bulletIndex];

                if (enemyBullet.X <= 0)
                {
                    Creatures.Remove(enemyBullet);
                    EnemyBullets.Remove(enemyBullet);
                    continue;
                }

                enemyBullet.PreciseX += enemyBullet.Speed * FrameTime.TotalSeconds;

                if (Character.IsEntityInBounds(enemyBullet.X, enemyBullet.Y))
                {
                    Character.Health -= enemyBullet.Damage;
                    EnemyBullets.Remove(enemyBullet);
                    Creatures.Remove(enemyBullet);
                }
            }
        }

        #region Render Related Methods

        private void Render()
        {
            switch (State)
            {
                case GameState.OpenedMainMenu:
                    RenderScreen(MainMenu);
                    break;
                case GameState.Playing:
                    RenderScreen(Creatures);
                    break;
                case GameState.OpenedPauseMenu:
                    RenderScreen(PauseMenu);
                    break;
                case GameState.ShowingDeathMessage:
                    RenderScreen(GameOverMessage);
                    break;
                case GameState.Exiting:
                    break;
                case GameState.Typing:
                    RenderScreen(Typewriter);
                    break;
                default:
                    break;
            }
        }

        private void RenderScreen(IRenderable entity)
        {
            StringBuilder sb = GetBlankScreen();

            foreach (var point in entity.SkinCoordinates.Keys)
            {
                var x = ((Point)point).X;
                var y = ((Point)point).Y;

                if (x < RenderWidth && y < RenderHeight)
                {
                    int strIndex = (RenderWidth + 1) * ((int)y - 1) + (int)x;
                    var charToDraw = entity.GetSkinChar((int)x, (int)y);
                    sb.Replace(' ', charToDraw, strIndex, 1);
                }
            }

            Console.SetCursorPosition(0, 0);
            Console.Write(sb);
        }

        private void RenderScreen(IEnumerable<IRenderable> entities)
        {
            StringBuilder sb = GetBlankScreen();

            foreach (var entity in entities)
            {
                foreach (var point in entity.SkinCoordinates.Keys)
                {
                    var x = ((Point)point).X;
                    var y = ((Point)point).Y;

                    if (x < RenderWidth && x > 0 && y < RenderHeight && y > 0)
                    {
                        int strIndex = (RenderWidth + 1) * ((int)y - 1) + (int)x;
                        var charToDraw = entity.GetSkinChar((int)x, (int)y);
                        sb.Replace(' ', charToDraw, strIndex, 1);
                    }
                }
            }

            Console.SetCursorPosition(0, 0);
            Console.Write(sb);
        }      

        private StringBuilder GetBlankScreen()
        {
            var sb = new StringBuilder();
            sb.AppendLine(GetGameStats());
            var blankLine = new string(' ', RenderWidth) + '\n';

            for (int y = 0; y < RenderHeight - 1; y++)
            {
                sb.Append(blankLine);
            }

            return sb;
        }

        private string GetGameStats()
        {
            double fps = 1 / FrameTime.TotalSeconds;

#if DEBUG
            var metadata = string.Format($"X:{Character.X}; Y:{Character.Y}; " +
               $"bullets:{Character.Bullets.Count}; enemies:{Enemies.Count}; " +
               $"kills {Kills}; health: {Character.Health}; frame time: {FrameTime.TotalMilliseconds} " +
               $"fps: {fps.ToString("F0")};");
#endif

#if !DEBUG
            var metadata = string.Format(
               $"kills {Kills}; health: {Character.Health}; frame time: {FrameTime.TotalMilliseconds}");
#endif

            if (metadata.Length < RenderWidth - 1)
            {
                string s = new string(' ', RenderWidth - metadata.Length);
                metadata += s;
                return metadata;
            }
            else
            {
                return metadata.Substring(0, RenderWidth - 1);
            }
        }

#endregion

        public void Stop()
        {
            Running = false;
        }
    }
}
