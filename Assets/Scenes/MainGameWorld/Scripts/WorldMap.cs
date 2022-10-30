using System;
using System.Collections.Generic;
using System.Linq;
using Random = System.Random;

namespace Scenes.MainGameWorld.Scripts
{
    public class WorldMap
    {
        public List<CityBlock> CityBlocks { get; } = new();
        public int WorldDimension { get; set; } = 8; // How many recursive steps to generate the world map.
        public int BlockDimension { get; set; } = 8; // The width of the city blocks

        /**
         * Generates a game world (city) by recursively generating city blocks.
         */
        public void GenerateWorld()
        {
            Random random = new Random();
            CityBlock originBlock = new CityBlock
            {
                BlockDimension = BlockDimension,
                ConnectionDirections = new()
                {
                    { "top", random.Next(1, BlockDimension - 1) },
                    { "bottom", random.Next(1, BlockDimension - 1) },
                    { "left", random.Next(1, BlockDimension - 1) },
                    { "right", random.Next(1, BlockDimension - 1) }
                }
            };
            originBlock.CreateMap();

            CityBlocks.Add(originBlock);

            RecursiveGeneration(originBlock, 0);
        }

        /**
         * Recursively generates the world map.
         */
        void RecursiveGeneration(CityBlock block, int width)
        {
            if (width < WorldDimension) // Recursive final case
            {
                // Try statements used because of the implementation of List.First in C#
                CityBlock leftBlock, rightBlock, upBlock, downBlock;
                try
                {
                    leftBlock = CityBlocks.First(b => b.BlockX == block.BlockX - 1 && b.BlockY == block.BlockY);
                }
                catch (InvalidOperationException e)
                {
                    RecursiveGeneration(BlockGenerator(block.BlockX - 1, block.BlockY), width + 1);
                }

                try
                {
                    rightBlock = CityBlocks.First(b => b.BlockX == block.BlockX + 1 && b.BlockY == block.BlockY);
                }
                catch (InvalidOperationException e)
                {
                    RecursiveGeneration(BlockGenerator(block.BlockX + 1, block.BlockY), width + 1);
                }

                try
                {
                    upBlock = CityBlocks.First(b => b.BlockX == block.BlockX && b.BlockY == block.BlockY - 1);
                }
                catch (InvalidOperationException e)
                {
                    RecursiveGeneration(BlockGenerator(block.BlockX, block.BlockY - 1), width + 1);
                }

                try
                {
                    downBlock = CityBlocks.First(b => b.BlockX == block.BlockX && b.BlockY == block.BlockY + 1);
                }
                catch (InvalidOperationException e)
                {
                    RecursiveGeneration(BlockGenerator(block.BlockX, block.BlockY + 1), width + 1);
                }
            }
        }

        /**
          * Generates a new block at the given coordinates.
          * 
          * Will pull the edge connections from existing blocks to ensure the world map is connected.
          */
        CityBlock BlockGenerator(int x, int y)
        {
            Random random = new Random();

            int leftConn, rightConn, upConn, downConn;

            // Try-Catch required due to implementation of List.First in C#
            try
            {
                leftConn = CityBlocks.First(b => b.BlockX == x - 1 && b.BlockY == y).ConnectionDirections["right"];
            }
            catch (InvalidOperationException e)
            {
                leftConn = random.Next(1, BlockDimension - 1);
            }

            try
            {
                rightConn = CityBlocks.First(b => b.BlockX == x + 1 && b.BlockY == y).ConnectionDirections["left"];
            }
            catch (InvalidOperationException e)
            {
                rightConn = random.Next(1, BlockDimension - 1);
            }

            try
            {
                upConn = CityBlocks.First(b => b.BlockX == x && b.BlockY == y - 1).ConnectionDirections["bottom"];
            }
            catch (InvalidOperationException e)
            {
                upConn = random.Next(1, BlockDimension - 1);
            }

            try
            {
                downConn = CityBlocks.First(b => b.BlockX == x && b.BlockY == y + 1).ConnectionDirections["top"];
            }
            catch (InvalidOperationException e)
            {
                downConn = random.Next(1, BlockDimension - 1);
            }

            CityBlock block = new CityBlock
            {
                BlockDimension = BlockDimension,
                BlockX = x,
                BlockY = y,
                ConnectionDirections = new()
                {
                    { "top", upConn },
                    { "bottom", downConn },
                    { "left", leftConn },
                    { "right", rightConn }
                }
            };
            block.CreateMap();

            CityBlocks.Add(block);

            return block;
        }
    }
}