using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CheeseNibblerII
{
    class Program
    {
        static void Main(string[] args)
        {
            //creates instance of game
            CheeseNibbler game = new CheeseNibbler();
            game.PlayGame();
            //keeps console open
            Console.ReadKey();
        }
    }

    /// <summary>
    /// Represents the position of an object on the grid
    /// </summary>
    public class Point
    {
        public enum PointStatus
        {
            Empty, Cheese, Mouse, Cat, CatAndCheese
        }
        
        //Declaring point properties
        public int X { get; set; }
        public int Y { get; set; }
        public PointStatus Status { get; set; }

        public Point(int x, int y)
        {
            this.X = x;
            this.Y = y;
            this.Status = PointStatus.Empty;
        }

    }

    public class Mouse
    {
        public Point Position { get; set; }
        public int Energy { get; set; }
        public bool HasBeenPouncedOn { get; set; }

        public Mouse()
        {
            this.Energy = 50;
            this.HasBeenPouncedOn = false;
        }
    }

    public class Cat
    {
        public Point Position { get; set; }

        public Cat()
        {
        
        }
    }

    public class CheeseNibbler
    {
        public Random rng { get; set; }
        public Point[,] Grid { get; set; }//Grid itself
        public Mouse Mouse { get; set; }//Our Mouse
        public Point Cheese { get; set; }//Sought after cheese
        public int CheeseCount { get; set; }//Pieces of cheese nibbled  
        public List<Cat> Cats { get; set; }//List of Cats to chase Mouse
        public bool MoveIsCat = false;

        public CheeseNibbler()
        {
            this.Mouse = new Mouse();
            
            this.rng = new Random(); 

            //initialize the grid
            this.Grid = new Point[10, 10];

            //loop through every x, y value in grid
            for (int y = 0; y < 10; y++)
            {
                for (int x = 0; x < 10; x++)
                {
                    //setting a new Point, into each
                    //coordinate of the grid
                    this.Grid[x, y] = new Point(x, y);
                }
            }

            //Initializes list of Cats  
            this.Cats = new List<Cat>();

            //Places the cheese
            PlaceCheese();
           
            // place a mouse
            do
            {
                this.Mouse.Position = this.Grid[rng.Next(this.Grid.GetLength(0)), rng.Next(this.Grid.GetLength(1))];
            } while (this.Mouse.Position.Status != Point.PointStatus.Empty);
            this.Mouse.Position.Status = Point.PointStatus.Mouse;
        }

        public void DrawGrid()
        {
            //Clear the console
            Console.Clear();

            for(int y = 0; y < this.Grid.GetLength(1); y++)
            {
                for(int x = 0; x < this.Grid.GetLength(0); x++)
                {
                    //check the type of each point
                    switch (Grid[x, y].Status)
                    {
                        case Point.PointStatus.Mouse:
                            Console.Write("[M]");
                            break;
                        case Point.PointStatus.Cheese:
                            Console.Write("[C]");
                            break;
                        case Point.PointStatus.Cat:
                            Console.Write("[X]");
                            break;
                        case Point.PointStatus.CatAndCheese:
                            Console.Write("[X]");
                            break;
                        default:
                            Console.Write("[ ]");
                            break;
                    }
                }
                Console.WriteLine();
            }
            Console.WriteLine("Use the numpad to move the mouse!");
            Console.WriteLine("\nPieces of Cheese: {0}", CheeseCount);
            Console.WriteLine("Mouse Energy: {0}", Mouse.Energy);
        }

        public ConsoleKey GetUserMove()
        {
            //putting a single key stroke into a variable
            //(true) keeps the character from being written to the console
            ConsoleKey playerInput = Console.ReadKey(true).Key;

            bool validInput = false;
            while (!validInput)
            {
                //checks for valid input
                switch (playerInput)
                {
                    case ConsoleKey.NumPad4://left
                    case ConsoleKey.NumPad6://right
                    case ConsoleKey.NumPad8://up
                    case ConsoleKey.NumPad2://down
                    case ConsoleKey.NumPad7://upper left
                    case ConsoleKey.NumPad9://upper right
                    case ConsoleKey.NumPad1://lower left
                    case ConsoleKey.NumPad3://lower right
                        validInput = true;
                        return playerInput;
                    default:
                        //invalid
                        Console.WriteLine("\nNot a valid move");
                        Console.WriteLine("Use the numpad to move the mouse");
                        playerInput = Console.ReadKey(true).Key;
                        return playerInput;
                }
            }
            return playerInput;
        }

        public bool ValidMove(ConsoleKey playerInput)
        {
            //look at each type of keypress
            switch (playerInput)
            {
                //checks if the move is on the grid
                case ConsoleKey.NumPad4:
                    return this.Mouse.Position.X > 0;//left
                case ConsoleKey.NumPad6:
                    return this.Mouse.Position.X < 9;//right
                case ConsoleKey.NumPad2:
                    return this.Mouse.Position.Y< 9;//down
                case ConsoleKey.NumPad8:
                    return this.Mouse.Position.Y > 0;//up
                case ConsoleKey.NumPad7:
                    return this.Mouse.Position.Y > 0 && this.Mouse.Position.X > 0;//upper left
                case ConsoleKey.NumPad9:
                    return this.Mouse.Position.Y > 0 && this.Mouse.Position.X < 9;//upper right
                case ConsoleKey.NumPad1:
                    return this.Mouse.Position.Y < 9 && this.Mouse.Position.X > 0;//down left
                case ConsoleKey.NumPad3:
                    return this.Mouse.Position.Y < 9 && this.Mouse.Position.X < 9;//down right
                default:
                    break;
            }
            return false;
        }

        public bool MoveMouse(ConsoleKey playerInput)
        {
            int nextX = this.Mouse.Position.X;
            int nextY = this.Mouse.Position.Y;

            if (ValidMove(playerInput))
            {
                switch (playerInput)
                {
                    case ConsoleKey.NumPad4://left
                        nextX -= 1;
                        break;
                    case ConsoleKey.NumPad6://right
                        nextX += 1;
                        break;
                    case ConsoleKey.NumPad2://down
                        nextY += 1;
                        break;
                    case ConsoleKey.NumPad8://up
                        nextY -= 1;
                        break;
                    case ConsoleKey.NumPad7://upper left
                        nextX -= 1;
                        nextY -= 1;
                        break;
                    case ConsoleKey.NumPad9://upper right
                        nextX += 1;
                        nextY -= 1;
                        break;
                    case ConsoleKey.NumPad1://down left
                        nextX -= 1;
                        nextY += 1;
                        break;
                    case ConsoleKey.NumPad3://down right
                        nextX += 1;
                        nextY += 1;
                        break;
                    default:
                        break;
                }
                Mouse.Energy--;
            }

            Point nextMouseMove = this.Grid[nextX, nextY];

            if (nextMouseMove.Status == Point.PointStatus.Cheese)
            {
                this.Grid[Mouse.Position.X, Mouse.Position.Y].Status = Point.PointStatus.Empty;
                this.Grid[nextX, nextY].Status = Point.PointStatus.Mouse;
                this.Mouse.Position = this.Grid[nextX, nextY];
                return true;
            }
            else if (nextMouseMove.Status == Point.PointStatus.Cat || nextMouseMove.Status == Point.PointStatus.CatAndCheese)
            {
                Mouse.HasBeenPouncedOn = true;
                // mabye move mouse here
                return false;
            }
            else
            {
                this.Grid[Mouse.Position.X, Mouse.Position.Y].Status = Point.PointStatus.Empty;
                this.Grid[nextX, nextY].Status = Point.PointStatus.Mouse;
                this.Mouse.Position = this.Grid[nextX, nextY];
                return false;
            }
        }

        public void PlaceCheese()
        {
            do
            {

                this.Cheese = Grid[rng.Next(0, 10), rng.Next(0, 10)];

            } while (this.Cheese.Status != Point.PointStatus.Empty);

            //adds cheese to grid with random coordinates
            this.Cheese.Status = Point.PointStatus.Cheese;
        }

        public void AddCat()
        {
            Cat cat = new Cat();
            PlaceCat(cat);
            this.Cats.Add(cat);
        }

        public void PlaceCat(Cat cat)
        {

            do{
                cat.Position = this.Grid[rng.Next(this.Grid.GetLength(0)), rng.Next(this.Grid.GetLength(1))];
                
            } while(cat.Position.Status != Point.PointStatus.Empty);
            cat.Position.Status = Point.PointStatus.Cat;
        }

        public void MoveCat(Cat cat)
        {
            if (rng.Next(0, 6) < 4)
            {
                int xDiff = Mouse.Position.X - cat.Position.X;
                int yDiff = Mouse.Position.Y - cat.Position.Y;
                bool tryLeft = xDiff < 0;
                bool tryRight = xDiff > 0;
                bool tryUp = yDiff < 0;
                bool tryDown = yDiff > 0;
                bool validMove = false;

                Point targetPosition = cat.Position;

                while (!validMove && (tryLeft || tryRight || tryUp || tryDown))
                {
                    int catX = cat.Position.X;
                    int catY = cat.Position.Y;

                    if (tryLeft)
                    {
                        targetPosition = Grid[--catX, catY];
                        tryLeft = false;
                    }
                    else if (tryRight)
                    {
                        targetPosition = Grid[++catX, catY];
                        tryRight = false;
                    }
                    else if (tryUp)
                    {
                        targetPosition = Grid[catX, --catY];
                        tryUp = false;
                    }
                    else if (tryDown)
                    {
                        targetPosition = Grid[catX, ++catY];
                        tryDown = false;
                    }
                    validMove = ValidCatMoveOnBoard(targetPosition);

                }

                // Look at the next move and change the status
                switch (targetPosition.Status)
                {
                    case Point.PointStatus.Cheese:
                        targetPosition.Status = Point.PointStatus.CatAndCheese;
                        break;
                    default:
                        targetPosition.Status = Point.PointStatus.Cat;
                        break;
                }
                // handle previous position status before executing move
                if (cat.Position.Status == Point.PointStatus.CatAndCheese)
                {
                    cat.Position.Status = Point.PointStatus.Cheese;
                }
                else
                {
                    cat.Position.Status = Point.PointStatus.Empty;
                }
                // execute the move
                cat.Position = targetPosition;
            }

        }

        public bool ValidCatMoveOnBoard(Point targetLocation)
        {
            return targetLocation.Status == Point.PointStatus.Empty || targetLocation.Status == Point.PointStatus.Mouse || targetLocation.Status == Point.PointStatus.Cheese;
        }

        public bool CatsTurnToMove(Point targetPosition)
        {
            return targetPosition.Status == Point.PointStatus.Cat || targetPosition.Status == Point.PointStatus.CatAndCheese;
        }

        public void PlayGame()
        {
            bool hasFoundCheese = false;
            this.CheeseCount = 0;
            while(Mouse.Energy > 0)
            {
                //Draw the grid
                DrawGrid();
                //Get valid user input
                ConsoleKey playerMove = GetUserMove();


                //move the mouse and determine
                //if the game has ended
                hasFoundCheese = MoveMouse(playerMove);

                if (this.Mouse.HasBeenPouncedOn)
                {
                    this.Mouse.Energy = 0;
                }

                foreach(Cat cat in Cats)
                {
                    this.MoveCat(cat);
                }

                if (hasFoundCheese)
                {
                    PlaceCheese();
                    this.Mouse.Energy += 10;
                    CheeseCount++;
                    if (CheeseCount % 2 == 0)
                    {
                        AddCat();
                    }
                }
            }
            // Update last status on death
            DrawGrid();
            // determine cause of death
            if(this.Mouse.Energy == 0)
            {
                Console.WriteLine("You ran out of energy and died");
            }
            else
            {
                Console.WriteLine("You were eaten by the cat!");
            }
            
            Console.WriteLine("Would you like to play again?");
        }
    }
}
