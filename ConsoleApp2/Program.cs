using ConsoleApp2;
using ConsoleApp3;


var generator = new MapGenerator(new MapGeneratorOptions()
{
    Height = 35,
    Width = 90,
    Noise = .1f,
    AddTraffic = true,
    TrafficSeed = 1234
});

var map = generator.Generate();
var start = new Point(43, 12);
var target = new Point(26, 27);

var dots = new List<Point> {start, target};

var open = new List<Point>();
var closed = new List<Point>();

new MapPrinter().Print(map, dots);


List<Point> GetShortestPath(string[,] maze, Point begin, Point goal)
{
    var origin = new Dictionary<Point, Point>();
    open.Add(begin);
    while (open.Count > 0)
    {
        // Sort the open list by f-cost and select the first (i.e., lowest f-cost) point
        var current = open.MinBy(p => FCost(p, begin, goal));
        
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
                origin.Add(neighbour,current);
                open.Add(neighbour);
            }
        }
    }
    var path = new List<Point> { goal };
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

new MapPrinter().Print(map, GetShortestPath(map, start, target));

List<Point> GetNeighbours(int column, int row, string[,] map)
{
    var neighbours = new List<Point>();
        
    bool IsTraversable(Point point) => CheckPosition(point, map) == " ";
        
        
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


string CheckPosition(Point point, string[,] map)
{
    var leftBorder = point.Column < 0;
    var rightBorder = point.Column >= map.GetLength(0);
    var topBorder = point.Row < 0;
    var bottomBorder = point.Row >= map.GetLength(1);
        
    // TODO: catch exception
    if (leftBorder || rightBorder || topBorder || bottomBorder) return "";

    return map[point.Column, point.Row];
}


double HCost(Point current, Point target)
{
    // The distance from the current point to the target one (B)
    int dx = target.Column - current.Column;
    int dy = target.Row - current.Row;
    double distance = Math.Sqrt(dx*dx + dy*dy);
   
    return distance;

}

double GCost(Point start, Point current)
{
    
    // The distance from the starting point (A) to the current one
    int dx = current.Column - start.Column;
    int dy = current.Row - start.Row;
    double distance = Math.Sqrt(dx*dx + dy*dy);

    return distance;
}

double FCost(Point current, Point start, Point target)
{
    double gCost = GCost(start, current);
    double hCost = HCost(current, target);
    double fCost = gCost + hCost;

    return fCost;
}
