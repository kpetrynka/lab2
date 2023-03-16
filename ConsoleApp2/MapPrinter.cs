using ConsoleApp3;

namespace ConsoleApp2;

public class MapPrinter
{
    public void Print(string[,] maze, List<Point> path)
    {
        PrintTopLine();
            
        var startPoint = path[0];
        var endPoint = path[^1];
            
        for (var row = 0; row < maze.GetLength(1); row++)
        {
            Console.Write($"{row}\t");
            for (var column = 0; column < maze.GetLength(0); column++)
            {
                    
                var currentPoint = new Point(column, row);
                if (currentPoint.Equals(startPoint))
                {
                    Console.Write("A"); //check if it is wall or note
                }
                    
                else if (currentPoint.Equals(endPoint))
                {
                    Console.Write("B");
                }
                else if (path.Contains(currentPoint))
                {
                    Console.Write(".");
                }
                else 
                {
                    Console.Write(maze[column, row]);  
                }
                    
            }
            Console.WriteLine();
        }


        void PrintTopLine()
        {
            Console.Write($" \t");
            for (int i = 0; i < maze.GetLength(0); i++)
            {
                Console.Write(i % 10 == 0? i / 10 : " ");
            }
    
            Console.Write($"\n \t");
            for (int i = 0; i < maze.GetLength(0); i++)
            {
                Console.Write(i % 10);
            }
    
            Console.WriteLine("\n");
        }


    }
}