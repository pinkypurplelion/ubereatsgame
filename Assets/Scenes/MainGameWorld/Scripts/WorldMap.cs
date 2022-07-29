using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using Random = System.Random;

namespace Scenes.MainGameWorld.Scripts
{
    public class WorldMap
    {
        public List<CityBlock> CityBlocks { get; set; } = new List<CityBlock>();
        public List<int[]> CityBlockCoords { get; set; } = new List<int[]>();
        public int WorldDimension { get; set; } = 8;
        
        public int BlockDimension { get; set; } = 8;


        private int CurrentWidth = 0;

        public void GenerateWorld()
        {
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
            
            RecursiveGeneration(originBlock, 0);
            
            // for (int i = WorldDimension/-2; i < WorldDimension/2; i++)
            // {
            //     for (int j = WorldDimension/-2; j < WorldDimension/2; j++)
            //     {
            //         if (!(i == 0 && j == 0))
            //         {
            //             BlockGenerator(i, j);
            //         }
            //     }
            // }
        }

        void RecursiveGeneration(CityBlock block, int width)
        {
            if (width < WorldDimension)
            {
                CityBlock leftBlock, rightBlock, upBlock, downBlock;
                try
                {
                    leftBlock = CityBlocks.First(b => b.BlockX == block.BlockX - 1 && b.BlockY == block.BlockY);
                } catch (InvalidOperationException e)
                {
                    RecursiveGeneration(BlockGenerator(block.BlockX - 1, block.BlockY), width + 1);
                }
                
                try
                {
                    rightBlock = CityBlocks.First(b => b.BlockX == block.BlockX + 1 && b.BlockY == block.BlockY);
                } catch (InvalidOperationException e)
                {
                    RecursiveGeneration(BlockGenerator(block.BlockX + 1, block.BlockY), width + 1);
                }

                try 
                {
                    upBlock = CityBlocks.First(b => b.BlockX == block.BlockX && b.BlockY == block.BlockY + 1);
                } catch (InvalidOperationException e)
                {
                    RecursiveGeneration(BlockGenerator(block.BlockX, block.BlockY + 1), width + 1);
                }
                
                try 
                {
                    downBlock = CityBlocks.First(b => b.BlockX == block.BlockX && b.BlockY == block.BlockY - 1);
                } catch (InvalidOperationException e)
                {
                    RecursiveGeneration(BlockGenerator(block.BlockX, block.BlockY - 1), width + 1);
                }
            }

            
            
            
               
            
        }

        CityBlock BlockGenerator(int x, int y)
        {
            Debug.Log($"Generating block at {x}, {y}");
            Debug.Log(CityBlockCoords.Count);
            Random random = new Random();

            int leftConn, rightConn, upConn, downConn;

            try
            {
                leftConn = CityBlocks.First(b => b.BlockX == x - 1 && b.BlockY == y).ConnectionDirections["right"];
            } catch (InvalidOperationException e)
            {
                leftConn = random.Next(1, BlockDimension - 1);
            }
            
            try 
            {
                rightConn = CityBlocks.First(b => b.BlockX == x + 1 && b.BlockY == y).ConnectionDirections["left"];
            } catch (InvalidOperationException e)
            {
                rightConn = random.Next(1, BlockDimension - 1);
            }
            
            try 
            {
                upConn = CityBlocks.First(b => b.BlockX == x && b.BlockY == y + 1).ConnectionDirections["bottom"];
            } catch (InvalidOperationException e)
            {
                upConn = random.Next(1, BlockDimension - 1);
            }
            
            try 
            {
                downConn = CityBlocks.First(b => b.BlockX == x && b.BlockY == y - 1).ConnectionDirections["top"];
            } catch (InvalidOperationException e)
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

            return block;
        }
    }
}