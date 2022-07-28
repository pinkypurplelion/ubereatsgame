using System;
using System.Collections.Generic;

namespace Scenes.MainGameWorld.Scripts
{
    public class WorldMap
    {
        public List<CityBlock> CityBlocks { get; set; } = new List<CityBlock>();
        
        public int WorldDimension { get; set; } = 8;
        
        public int BlockDimension { get; set; } = 8;


        public void GenerateWorld()
        {
            Random random = new Random();
            CityBlock originBlock = new CityBlock
            {
                BlockDimension = BlockDimension,
                Connections = new[]
                {
                    random.Next(1, BlockDimension - 1), 
                    random.Next(1, BlockDimension - 1), 
                    random.Next(1, BlockDimension - 1), 
                    random.Next(1, BlockDimension - 1)
                }
            };
            originBlock.CreateMap();
            
            CityBlocks.Add(originBlock);
        }
        
    }
}