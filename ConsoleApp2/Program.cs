using System.ComponentModel.DataAnnotations.Schema;
using System.Threading.Tasks.Sources;
using ConsoleApp2;



static void AlgoGenerator(int i, List<Point> open, List<Point> closed, Dictionary<Point, double> distance)
{
    
    var generator = new MapGenerator(new MapGeneratorOptions()
    {
        Height = 35,
        Width = 90,
        Noise = .1f,
        AddTraffic = true,
        TrafficSeed = 1234,
        Seed = i
    });

    var map = generator.Generate();
    var start = new Point(43, 12);
    var target = new Point(26, 27);

    // var dots = new List<Point> {start, target};
    
    var shortestPath = GetShortestPath(map, start, target);
    new MapPrinter().Print(map, shortestPath);
    Console.WriteLine("REPRESENTATION OF A* ALGORITHM");
    Console.WriteLine($"Time with traffic: {TrafficTime(shortestPath, map)}");
    Console.WriteLine($"Total count of cells: {Pathsum(shortestPath, map)}");


    List<Point> GetShortestPath(string[,] maze, Point begin, Point goal)
    {
        var origin = new Dictionary<Point, Point>();

        open.Add(begin);
        while (open.Count > 0)
        {
            // Sort the open list by f-cost and select the first (i.e., lowest f-cost) point
            var current = open.MinBy(p => FCost(p, begin, goal));
            distance[current] = FCost(current, begin, goal);

            if (current.Equals(goal))
            {
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

        // path.Add(begin);
        return path;
    }
    
    

    double TrafficTime(List<Point> path, string[,] maze)
    {
        double score = 0;
        foreach (var point in path)
        {
            if (maze[point.Column, point.Row] != "█")
            {
                var n = int.Parse(maze[point.Column, point.Row]);
                var dist = distance.ContainsKey(point) ? distance[point] : 0;
                score += dist / (60 - (n - 1) * 6);
            }
            else
            {
                score += 0;
            }
        }

        return score;
    }

    
    

    // Function that gets all the point from the path, returns its sum

    int Pathsum(List<Point> path, string[,] maze)
    {
        int sumi = 0;
        foreach (var point in path)
        {
            if (maze[point.Column, point.Row] != "█")
            {
                sumi += 1;
            }
        }

        return sumi;
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
    

    
    


    double HCost(Point current, Point final)
    {
        // The distance from the current point to the target one (B)
        int dx = final.Column - current.Column;
        int dy = final.Row - current.Row;
        double cost = Math.Sqrt(dx * dx + dy * dy);

        return cost;

    }

    double GCost(Point begin, Point current)
    {

        // The distance from the starting point (A) to the current one
        int dx = current.Column - begin.Column;
        int dy = current.Row - begin.Row;
        double cost = Math.Sqrt(dx * dx + dy * dy);

        return cost;
    }

    double FCost(Point current, Point begin, Point final)
    {
        double gCost = GCost(begin, current);
        double hCost = HCost(current, final);
        double fCost = gCost + hCost;

        return fCost;
    }
    
    
}

var open = new List<Point>();
var closed = new List<Point>();
var distance = new Dictionary<Point, double>();

for (int i = 0; i < 100; i++)
{
    // make an expeption for the algorithm if in given i there is an error
    
    try
    {
        open.Clear();
        closed.Clear();
        distance.Clear();
        AlgoGenerator(i, open, closed, distance);
    }
    catch (Exception e)
    {
        i = i + 1;
    }
}

