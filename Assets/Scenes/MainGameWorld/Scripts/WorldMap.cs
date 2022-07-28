using System.Collections.Generic;

namespace Scenes.MainGameWorld.Scripts
{
    public class WorldMap
    {
        public List<CityBlock> CityBlocks { get; set; } = new List<CityBlock>();
        
        public int WorldDimension { get; set; } = 8;
        
        public int BlockDimension { get; set; } = 8;
    }
}