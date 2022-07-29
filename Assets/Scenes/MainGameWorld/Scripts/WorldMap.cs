using System;
using System.Collections.Generic;
using System.Linq;

namespace Scenes.MainGameWorld.Scripts
{
    public class WorldMap
    {
        public List<CityBlock> CityBlocks { get; set; } = new List<CityBlock>();
        public List<int[]> CityBlockCoords { get; set; } = new List<int[]>();
        public int WorldDimension { get; set; } = 4;
        
        public int BlockDimension { get; set; } = 8;


        private int CurrentWidth = 0;

        public void GenerateWorld()
        {
            Random random = new Random();
            CityBlock originBlock = new CityBlock
            {
                BlockDimension = BlockDimension,
                ConnectionDirections = new(){ 
                    { "top", 4 }, 
                    { "bottom", 2 },
                    { "left", 6 }, 
                    { "right", 5 } 
                }
            };
            originBlock.CreateMap();
            
            CityBlocks.Add(originBlock);
            CityBlockCoords.Add(new[] {0, 0});

            for (int i = WorldDimension/-2; i < WorldDimension/2; i++)
            {
                for (int j = WorldDimension/-2; j < WorldDimension/2; j++)
                {
                    if (!(i == 0 && j == 0))
                    {
                        BlockGenerator(i, j);
                    }
                }
            }
            
            
            // RecursiveGeneration(originBlock);
        }

        void RecursiveGeneration(CityBlock block)
        {
            CityBlock leftBlock, rightBlock, upBlock, downBlock;
            if (CityBlockCoords.Contains(new []{ block.BlockX - 1, block.BlockY}))
                 leftBlock = CityBlocks.First(b => b.BlockX == block.BlockX - 1 && b.BlockY == block.BlockY);
            
            if (CityBlockCoords.Contains(new []{ block.BlockX + 1, block.BlockY}))
                rightBlock = CityBlocks.First(b => b.BlockX == block.BlockX + 1 && b.BlockY == block.BlockY);
            
            if (CityBlockCoords.Contains(new []{ block.BlockX, block.BlockY - 1}))
                upBlock = CityBlocks.First(b => b.BlockX == block.BlockX && b.BlockY == block.BlockY - 1);
            
            if (CityBlockCoords.Contains(new []{ block.BlockX, block.BlockY + 1}))
                downBlock = CityBlocks.First(b => b.BlockX == block.BlockX && b.BlockY == block.BlockY + 1);
            
            
        }

        void BlockGenerator(int x, int y)
        {
            Random random = new Random();

            int leftConn, rightConn, upConn, downConn;

            if (CityBlockCoords.Contains(new[] { x - 1, y }))
            {
                var leftBlock = CityBlocks.First(b => b.BlockX == x - 1 && b.BlockY == y);
                leftConn = leftBlock.ConnectionDirections["right"];
            } else
            {
                leftConn = random.Next(1, BlockDimension - 1);
            }

            if (CityBlockCoords.Contains(new[] { x + 1, y }))
            {
                var rightBlock = CityBlocks.First(b => b.BlockX == x + 1 && b.BlockY == y);
                rightConn = rightBlock.ConnectionDirections["left"];
            } else
            {
                rightConn = random.Next(1, BlockDimension - 1);
            }

            if (CityBlockCoords.Contains(new[] { x, y - 1 }))
            {
                var upBlock = CityBlocks.First(b => b.BlockX == x && b.BlockY == y - 1);
                upConn = upBlock.ConnectionDirections["down"];
            }
            else
            {
                upConn = random.Next(1, BlockDimension - 1);
            }

            if (CityBlockCoords.Contains(new[] { x, y + 1 }))
            {
                var downBlock = CityBlocks.First(b => b.BlockX == x && b.BlockY == y + 1);
                downConn = downBlock.ConnectionDirections["up"];
            }
            else
            {
                downConn = random.Next(1, BlockDimension - 1);
            }
            
            CityBlock block = new CityBlock
            {
                BlockDimension = BlockDimension,
                BlockX = x,
                BlockY = y,
                ConnectionDirections = new(){ 
                    { "top", upConn }, 
                    { "bottom", downConn },
                    { "left", leftConn }, 
                    { "right", rightConn }
                }
            };
            block.CreateMap();
            
            CityBlocks.Add(block);
            CityBlockCoords.Add(new[] {x, y});
        }
    }
}