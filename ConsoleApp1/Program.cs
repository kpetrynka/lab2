using System.Data;
using ConsoleApp1;

DataTable Results = new DataTable();
var i = 0;

while (i > 30)
{
    var generator = new MapGenerator(new MapGeneratorOptions()
    {
        Height = 35,
        Width = 90,
    });

    var map = generator.Generate();
    var start = new Point(43, 12);
    var target = new Point(26, 27);

    // var dots = new List<Point> {start, target};

    var open = new List<Point>();
    var closed = new List<Point>();
    var distance = new Dictionary<Point, double> { { start, 0 } };

    var shortestPath = GetShortestPath(map, start, target);
    new MapPrinter().Print(map, shortestPath);
    Console.WriteLine($"Opened: {closed.Count}, Distance {distance[target]}");

    List<Point> GetShortestPath(string[,] maze, Point begin, Point goal)
    {
        var origin = new Dictionary<Point, Point>();

        open.Add(begin);
        while (open.Count > 0)
        {
            // Sort the open list by f-cost and select the first (i.e., lowest f-cost) point
            var current = open[0];

            if (current.Equals(goal))
            {
                distance[current] = distance[origin[current]] + 1;
                Console.WriteLine("Path found!");
                break;
            }

            // Move the current point from the open list to the closed list
            open.Remove(current);
            closed.Add(current);

            var neighbours = GetNeighbours(current.Column, current.Row, map);
            foreach (var neighbour in neighbours)
            {
                if (!closed.Contains(neighbour))
                {
                    distance[neighbour] = distance[current] + 1;
                    origin[neighbour] = closed[^1];
                    open.Add(neighbour);
                }
            }
        }

        var path = new List<Point> { goal }; // change the start and goal
        var last = origin[goal];
        while (true)
        {
            if (origin.ContainsKey(last))
            {
                path.Add(last);
                last = origin[last];
            }
            else
            {
                break;
            }
        }

        path.Add(begin);
        return path;
    }


    List<Point> GetNeighbours(int column, int row, string[,] mazeMap)
    {
        var neighbours = new List<Point>();

        bool IsTraversable(Point point) => CheckPosition(point, mazeMap) != "";


        var topNeighbour = new Point(column, row - 1);
        if (IsTraversable(topNeighbour))
        {
            neighbours.Add(topNeighbour);
        }

        var bottomNeighbour = new Point(column, row + 1);
        if (IsTraversable(bottomNeighbour))
        {
            neighbours.Add(bottomNeighbour);
        }

        var leftNeighbour = new Point(column - 1, row);
        if (IsTraversable(leftNeighbour))
        {
            neighbours.Add(leftNeighbour);
        }

        var rightNeighbour = new Point(column + 1, row);
        if (IsTraversable(rightNeighbour))
        {
            neighbours.Add(rightNeighbour);
        }

        return neighbours;
    }


    string CheckPosition(Point point, string[,] mazeMap)
    {
        var leftBorder = point.Column < 0;
        var rightBorder = point.Column >= mazeMap.GetLength(0);
        var topBorder = point.Row < 0;
        var bottomBorder = point.Row >= mazeMap.GetLength(1);

        // TODO: catch exception
        if (leftBorder || rightBorder || topBorder || bottomBorder ||
            mazeMap[point.Column, point.Row] == "█") return "";

        return mazeMap[point.Column, point.Row];
    }
}