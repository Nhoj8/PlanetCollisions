using System;
using System.Collections;
using System.Collections.Generic;
using System.Text;
using System.CodeDom.Compiler;
using Microsoft.VisualBasic;
using System.Windows.Forms;
using System.Diagnostics.Eventing.Reader;
using System.Windows.Forms.VisualStyles;
using System.Data.OleDb;
using Microsoft.Xna.Framework;

namespace CellGame
{
    class WorldGen
    {

  

        public int[,] world;
        long seedint = 0;
        public (int[,], string) Start(int width, int height, string seed, double randomFillPercent, int smoothIndex)
        {
            GenerateWorld(width, height, seed, randomFillPercent, smoothIndex);
            return (world, seedint.ToString());
        }
        void GenerateWorld(int width, int height, string seed, double randomFillPercent, int smoothIndex)
        {
            world = new int[width, height];
            RandomFillWorld(width, height, seed, randomFillPercent);

            for (int i = 0; i < smoothIndex; i++)
            {
                Smoothworld(width, height);
            }
           // world[width / 2, height / 2] = 2; //MIDDLE BOLCK
        }

        void RandomFillWorld(int width, int height, string seed, double randomFillPercent)
        {


            if (seed == null || seed == "")
            {
                //seedint = "".GetHashCode();
                
                seedint = DateAndTime.TimeString.GetHashCode();
            }
            else
            {
                if (!long.TryParse(seed, out seedint))
                {
                    char[] char_array = seed.ToCharArray();
                    long oldseedint = 0;

                    for (int i = 0; i < char_array.Length; i++)
                    {
                        int index;
                        index = (char_array[i]);
                        long power = Convert.ToInt32(Math.Pow(2, i));
                        seedint += index * power;
                        if (seedint > int.MaxValue)
                        {
                            seedint = oldseedint;
                            break;
                        }
                        oldseedint = seedint;
                    }
                }
            }




            //MessageBox.Show(seedint.ToString());   
            System.Random rnd = new System.Random(Convert.ToInt32(seedint));

            //MessageBox.Show(rnd.Next(0 ,100).ToString());
            for (int x = 0; x < width; x++)
            {
                for (int y = 0; y < height; y++)
                {
                    if ((x > width/2 -10 && x < width/2 + 10) && (y > height/2 - 10 && y < height/2 + 10))
                    {
                        world[x, y] = 0;
                    }
                    else if (x == 0 || x == width - 1 || y == 0 || y == height - 1)
                    {
                        world[x, y] = 3;
                    }
                    else
                        world[x, y] = rnd.Next(0, 100) < randomFillPercent ? rnd.Next(1, 2) : 0 ;

                }
            }
            //world[width / 2, height / 2] = 3; MIDDLE BOLCK Actually no its not cuz smooth

        }
        void Smoothworld(int width, int height)
        {
            for (int x = 0; x < width; x++)
            {
                for (int y = 0; y < height; y++)
                {
                    int adjacentblocks = Getadjacentblockcount(x, y, width, height);
                    if (adjacentblocks > 4)
                        world[x, y] = 1;
                    else if (adjacentblocks < 4)
                      world[x, y] = 0;

                }
            }
        }

        int Getadjacentblockcount(int gridX, int gridY, int width, int height)
        {
            int wallcount = 0;
            for (int neighborX = gridX - 1; neighborX <= gridX + 1; neighborX++)
            {
                for (int neighborY = gridY - 1; neighborY <= gridY + 1; neighborY++)
                {
                    if (neighborX >= 0 && neighborX < width && neighborY >= 0 && neighborY < height)
                    {
                        if (neighborX != gridX || neighborY != gridY)
                        {
                            if (world[neighborX, neighborY] != 0)
                            {
                                wallcount += 1;
                            }



                        }
                    }
                    else
                        wallcount += 2;

                }
            }
            return wallcount;
        }


    }


}


