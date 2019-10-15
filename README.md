# Console Game

A small command prompt project that I've coded just for fun.

Every parameter of the enties (skin/health/speed/bullet speed/bullet damage) is configurable.

Check out the code and download binaries to play around.
Use arrows to move the spacecraft and spacebar to shoot. 
![](ConsoleGame/console_game_demo.gif)

## The Structure of the Code

The Game class contains all of the game logic. So the main function contains only one line of code.

```csharp

class Program
{
    static void Main(string[] args)
    {
        new Game().Run();
    }
}

```

The Run method creates a new thread with a classic game loop like so

```csharp 

public void Run()
{
    Running = true;
    Thread t = new Thread(() =>
    {
        // disables blinking cursor 
        Console.CursorVisible = false;
        
        // counts down the time between frames to make relevant changes in a game state
        Stopwatch sw = new Stopwatch();
        // responsible for an enemy apperaing timing
        EnemyAppearingSW.Start();

        // changing this property tells the game what to render 
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
        }
    });
    t.SetApartmentState(ApartmentState.STA);
    t.Start();
}

```

#### Rendering

The game class contains a collection of renderable objects (that implements IRenderable interface) .

```csharp

List<IRenderable> Creatures { get; set; } // the collection

interface IRenderable
{
    string Skin { get; set; }
    
    // contains key value pairs where key is a point and value is a skin character 
    Hashtable SkinCoordinates { get; set; }

    // exact position of an object
    double PreciseX { get; set; }
    double PreciseY { get; set; } 

    // approximation of the exact position
    int Y { get; }
    int X { get; }
    
    int Height { get; }
    int Width { get; }

    char GetSkinChar(int x, int y);
    bool IsEntityInBounds(int x, int y);
}

private void Render()
{
    switch (State)
    {
        case GameState.OpenedMainMenu:
            RenderScreen(MainMenu);
            break;
        case GameState.Playing:
            RenderScreen(Creatures); //the method that renders the game
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

```

The render method goes through the entity collection. 
And then it replaces a space character in a StringBuilder object with a skin character of an entity.

```csharp

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

```



