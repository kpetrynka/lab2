

namespace ConsoleApp3
{
    public class MapGeneratorOptions
    {
        public int Width { get; set; } = 1;

        public int Height { get; set; } = 1;

        public MapType Type { get; set; } = MapType.Maze; 

        public float Noise { get; set; }

        public int Seed { get; set; } = 100;
        
        public bool AddTraffic { get; set; }
        
        public int TrafficSeed { get; set; }
    }
    
    
    public enum MapType
    {
        Maze,
        Grid
    }
}