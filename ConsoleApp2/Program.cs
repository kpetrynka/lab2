using ConsoleApp3;


var generator = new MapGenerator(new MapGeneratorOptions()
{
    Height = 35,
    Width = 90,
});

var map = generator.Generate();
var start = new Point(43, 12); 
var target = new Point(26, 27);

// origins the way of getting to the point
// distance - the distance
new MapPrinter().Print(map, GetShortestPath(map, start, target ));
 

List<Point> GetShortestPath(string[,] maze, Point starter, Point goal)
{
    var path = new List<Point> {starter, goal}; 
    var open = new List<Point>();
    var closed = new List<Point>();
    
    open.Add(starter);
    
    while (open.Count > 0)
    {
        // Sort the open list by f-cost and select the first (i.e., lowest f-cost) point
    	// The whole algorithm not the Dijkstra's 
        var current = open.MinBy(p => FCost(p, start, target));
        Console.WriteLine(FCost(current, start, target));
        
        
        if (current.Equals(goal))
        {
            Console.WriteLine("Path found!");
            break;
        }
        
        // Move the current point from the open list to the closed list
        open.Remove(current);
        closed.Add(current);

        var neighbours = GetNeighbours(current.Column, current.Row, maze);
        Console.WriteLine(neighbours.Count);
        foreach (var neighbour in neighbours)
        {
            if (!closed.Contains(neighbour))
            {
                open.Add(neighbour);
                maze[neighbour.Column, neighbour.Row] = ".";
            }
        }
    }
    return path;
}


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

// Check if cell is walkable
bool IsTraversable(Point point, string[,] map)
{
    if (map[point.Row, point.Column] != MapGenerator.Wall)
    {
        return true;
    }

    return false;
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
