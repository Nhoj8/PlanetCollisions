using System;
using System.Collections.Generic;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using SharpDX.MediaFoundation;

namespace CellGame.Content
{
    class Collisions
    {
        public bool CheckCollisionsRight(int[,] world, int width, int height, int blocksize,Rectangle[,] blockrectangle,Rectangle movingrectangle)
        {
            for (int y = ((movingrectangle.Y) / blocksize) - 1; y < ((movingrectangle.Y + movingrectangle.Size.Y) / blocksize) + 1; y++)
            {
                for (int x = (movingrectangle.X / blocksize) - 1; x < ((movingrectangle.X + movingrectangle.Size.X) / blocksize) + 1; x++)
                {
                    if (x < width && y < height && x > -1 && y > -1 && world[x, y] != 0)
                    {
                        //System.Windows.Forms.MessageBox.Show((blockrectangle[x, y].Left - CoordsX + 1600 + 640).ToString() + " < " + (movingrectangle.Right + 1).ToString());
                        if ((blockrectangle[x, y].Top) < movingrectangle.Bottom && (blockrectangle[x, y].Left) < movingrectangle.Right + 1 && blockrectangle[x, y].Right > movingrectangle.Right && (blockrectangle[x, y].Bottom) > movingrectangle.Top)
                        //960 + 360 = height/2 * blocksize + screen size/2
                        //(blockrectangle[x, y].Top - CoordsY +height/2*blocksize
                        {//movingrectangle.Right + 1 < blockrectangle[x, y].Left && 

                            return false;

                        }
                    }
                }

            }
            return true;
        }
        //for (int y = ((CoordsY + (imagesizeX - movingrectangle.Size.CoordsY)) / blocksize) - 1; y < ((CoordsY + (imagesizeY - movingrectangle.Size.CoordsY) + movingrectangle.Size.CoordsY) / blocksize) + 1; y++)
        public bool CheckCollisionsLeft(int[,] world, int width, int height, int blocksize, Rectangle[,] blockrectangle, Rectangle movingrectangle)
        {
            for (int y = ((movingrectangle.Y) / blocksize) - 1; y < ((movingrectangle.Y + movingrectangle.Size.Y) / blocksize) + 1; y++)
            {
                for (int x = (movingrectangle.X / blocksize) - 1; x < ((movingrectangle.X + movingrectangle.Size.X) / blocksize) + 1; x++)
                    if (x < width && y < height && x > -1 && y > -1 && world[x, y] != 0)
                        if ((blockrectangle[x, y].Top) < movingrectangle.Bottom && (blockrectangle[x, y].Right) > movingrectangle.Left - 1 && blockrectangle[x, y].Left < movingrectangle.Left && (blockrectangle[x, y].Bottom) > movingrectangle.Top)
                            return false; 
            }
            return true;
        }
        public bool CheckCollisionsDown(int[,] world, int width, int height, int blocksize, Rectangle[,] blockrectangle, Rectangle movingrectangle)
        {
            for (int y = ((movingrectangle.Y) / blocksize) - 1; y < ((movingrectangle.Y + movingrectangle.Size.Y) / blocksize) + 1; y++)
            {
                for (int x = (movingrectangle.X / blocksize) - 1; x < ((movingrectangle.X + movingrectangle.Size.X) / blocksize) + 1; x++)
                    if (x < width && y < height && x > -1 && y > -1 && world[x, y] != 0)
                        if ((blockrectangle[x, y].Left) < movingrectangle.Right && (blockrectangle[x, y].Top) < movingrectangle.Bottom + 1 && blockrectangle[x, y].Bottom > movingrectangle.Bottom && (blockrectangle[x, y].Right) > movingrectangle.Left)
                            return false;
            }
            return true;
        }
        public bool CheckCollisionsUp(int[,] world, int width, int height, int blocksize, Rectangle[,] blockrectangle, Rectangle movingrectangle)
        {
            for (int y = ((movingrectangle.Y) / blocksize) - 1; y < ((movingrectangle.Y + movingrectangle.Size.Y) / blocksize) + 1; y++)
            {
                for (int x = (movingrectangle.X / blocksize) - 1; x < ((movingrectangle.X + movingrectangle.Size.X) / blocksize) + 1; x++)
                    if (x < width && y < height && x > -1 && y > -1 && world[x, y] != 0)
                        if ((blockrectangle[x, y].Left) < movingrectangle.Right && (blockrectangle[x, y].Bottom) > movingrectangle.Top - 1 && blockrectangle[x, y].Top < movingrectangle.Top && (blockrectangle[x, y].Right) > movingrectangle.Left)
                            return false;
            }
            return true;
        }


        //public bool CheckCollisionsUp(int[,] world, int CoordsX, int CoordsY, int width, int height, int blocksize, Rectangle[,] blockrectangle, Rectangle movingrectangle)
        //{
        //    for (int y = ((CoordsY - movingrectangle.Size.CoordsY) / blocksize) - 1; y < ((CoordsY) / blocksize) + 1; y++)
        //    {
        //        for (int x = ((CoordsX - movingrectangle.Size.CoordsX / 2) / blocksize) - 1; x < ((CoordsX + movingrectangle.Size.CoordsX / 2) / blocksize) + 1; x++)
        //            if (x < width && y < height && world[x, y] != 0)
        //                if ((blockrectangle[x, y].Left) < movingrectangle.Right && (blockrectangle[x, y].Bottom) > movingrectangle.Top - 1 && blockrectangle[x, y].Top < movingrectangle.Top && (blockrectangle[x, y].Right) > movingrectangle.Left)
        //                    return false;
        //    }
        //    return true;
        //}






        // POLYGON/POLYGON
        public bool PolyPoly(DoubleCoordinates[] p1, DoubleCoordinates[] p2)
        {

            // go through each of the vertices, plus the next
            // vertex in the list
            int next = 0;
            for (int current = 0; current < p1.Length; current++)
            {

                // get next vertex in list
                // if we've hit the end, wrap around to 0
                next = current + 1;
                if (next == p1.Length) next = 0;

                // get the PVectors at our current position
                // this makes our if statement a little cleaner
                DoubleCoordinates vc = p1[current];    // c for "current"
                DoubleCoordinates vn = p1[next];       // n for "next"

                // now we can use these two points (a line) to compare
                // to the other polygon's vertices using polyLine()
                bool collision = PolyLine(p2, vc.X, vc.Y, vn.X, vn.Y);
                if (collision) return true;

                // optional: check if the 2nd polygon is INSIDE the first
                collision = PolyPoint(p1, p2[0].X, p2[0].Y);
                if (collision) return true;
            }

            return false;
        }


        // POLYGON/RECTANGLE
        public (bool, bool, int[], DoubleCoordinates[]) PolyRect(DoubleCoordinates[] vertices, double rx, double ry, double rw, double rh)
        {

            // go through each of the vertices, plus the next
            // vertex in the list
            int next = 0;
            int[] Totalorientation;
            DoubleCoordinates[] Totalintersectpoint;
            Totalorientation = new int[4* vertices.Length];
            Totalintersectpoint = new DoubleCoordinates[4 * vertices.Length];
            bool collision = false;
            int totalcollisions = 0;
            bool inside = false;
            for (int current = 0; current < vertices.Length; current++)
            {

                // get next vertex in list
                // if we've hit the end, wrap around to 0
                next = current + 1;
                if (next == vertices.Length) next = 0;

                // get the PVectors at our current position
                // this makes our if statement a little cleaner
                DoubleCoordinates vc = vertices[current];    // c for "current"
                DoubleCoordinates vn = vertices[next];       // n for "next"

                // check against all four sides of the rectangle
                bool thiscollision;
                int[] orientation;
                DoubleCoordinates[] intersectpoint;
                orientation = new int[4];
                intersectpoint = new DoubleCoordinates[4];
                (thiscollision, orientation, intersectpoint) = LineRect(vc.X, vc.Y, vn.X, vn.Y, rx, ry, rw, rh);
                if (thiscollision)
                {
                    collision = true;
                    for(int i = 0; i < 4; i++)
                    {
                        Totalorientation[totalcollisions*4 + i] = orientation[i];
                        Totalintersectpoint[totalcollisions * 4 + i] = intersectpoint[i];
                    }
                    totalcollisions += 1;
                }

            }
            // optional: test if the rectangle is INSIDE the polygon
            // note that this iterates all sides of the polygon
            // again, so only use this if you need to
            if (!collision && !inside)
                inside = PolyPoint(vertices, rx, ry);

            //if (inside) return true;
            System.Array.Resize(ref Totalorientation, totalcollisions * 4);
            System.Array.Resize(ref Totalintersectpoint, totalcollisions * 4);
            return (collision, inside, Totalorientation, Totalintersectpoint);
        }

        // POLYGON/LINE
        public bool PolyLine(DoubleCoordinates[] vertices, double x1, double y1, double x2, double y2)
        {

            // go through each of the vertices, plus the next
            // vertex in the list
            DoubleCoordinates[] intersectpoints;
            intersectpoints = new DoubleCoordinates[vertices.Length];
            //bool overlap
            int numberofintersects = 0;
            int next = 0;
            for (int current = 0; current < vertices.Length; current++)
            {

                // get next vertex in list
                // if we've hit the end, wrap around to 0
                next = current + 1;
                if (next == vertices.Length) next = 0;

                // get the PVectors at our current position
                // extract CoordsX/CoordsY coordinates from each
                double x3 = vertices[current].X;
                double y3 = vertices[current].Y;
                double x4 = vertices[next].X;
                double y4 = vertices[next].Y;

                // do a Line/Line comparison
                // if true, return 'true' immediately and
                // stop testing (faster)
                bool hit = false;
                //(hit, intersectpoints[numberofintersects]) = lineLine(x1, y1, x2, y2, x3, y3, x4, y4);
                if (hit)
                {
                    numberofintersects += 1;
                    return true;
                }
            }
            // never got a hit
            return false;
        }

        // POLYGON/POINT
        // used only to check if the second polygon is
        // INSIDE the first
        bool PolyPoint(DoubleCoordinates[] vertices, double px, double py)
        {
            bool collision = false;

            // go through each of the vertices, plus the next
            // vertex in the list
            int next = 0;
            for (int current = 0; current < vertices.Length; current++)
            {

                // get next vertex in list
                // if we've hit the end, wrap around to 0
                next = current + 1;
                if (next == vertices.Length) next = 0;

                // get the PVectors at our current position
                // this makes our if statement a little cleaner
                DoubleCoordinates vc = vertices[current];    // c for "current"
                DoubleCoordinates vn = vertices[next];       // n for "next"

                // compare position, flip 'collision' variable
                // back and forth
                if (((vc.Y > py && vn.Y < py) || (vc.Y < py && vn.Y > py)) &&
                     (px < (vn.X - vc.X) * (py - vc.Y) / (vn.Y - vc.Y) + vc.X))
                {
                    collision = !collision;
                }
            }
            return collision;
        }


        // LINE/RECTANGLE
        public (bool, int[], DoubleCoordinates[]) LineRect(double x1, double y1, double x2, double y2, double rx, double ry, double rw, double rh)
        {

            int[] orientation;
            DoubleCoordinates[] intersectpoint;
            orientation = new int[4];
            intersectpoint = new DoubleCoordinates[4];
            // check if the line has hit any of the rectangle's sides
            // uses the Line/Line function below
            (orientation[0], intersectpoint[0]) = LineLine(x1, y1, x2, y2, rx, ry, rx, ry + rh);//Left
            (orientation[1], intersectpoint[1]) = LineLine(x1, y1, x2, y2, rx + rw, ry, rx + rw, ry + rh);//Right
            (orientation[2], intersectpoint[2]) = LineLine(x1, y1, x2, y2, rx, ry, rx + rw, ry);//Top
            (orientation[3], intersectpoint[3]) = LineLine(x1, y1, x2, y2, rx, ry + rh, rx + rw, ry + rh);//Bottom

            // if ANY of the above are true,
            // the line has hit the rectangle
            if (orientation[0] == 1 || orientation[1] ==1 || orientation[2] == 1 || orientation[3] == 1)
            {
                for (int i = 0; i < 4; i++)
                {
                        orientation[i] = (i+1) * orientation[i];
                }
                return (true, orientation, intersectpoint);
            }
            bool inside = PointRect(x1, y2, rx, ry, rw, rh);
            //if (inside)
                //System.Windows.Forms.MessageBox.Show("line inside");
            return (false, orientation, intersectpoint);
        }

        // POINT/RECTANGLE
        bool PointRect(double px, double py, double rx, double ry, double rw, double rh)
        {

            // is the point inside the rectangle's bounds?
            if (px >= rx &&        // right of the left edge AND
                px <= rx + rw &&   // left of the right edge AND
                py >= ry &&        // below the top AND
                py <= ry + rh)
            {   // above the bottom
                return true;
            }
            return false;
        }


        // LINE/LINE
        public (int, DoubleCoordinates) LineLine(double x1, double y1, double x2, double y2, double x3, double y3, double x4, double y4)
        {

            // calculate the direction of the lines
            double uA = ((x4 - x3) * (y1 - y3) - (y4 - y3) * (x1 - x3)) / ((y4 - y3) * (x2 - x1) - (x4 - x3) * (y2 - y1));
            double uB = ((x2 - x1) * (y1 - y3) - (y2 - y1) * (x1 - x3)) / ((y4 - y3) * (x2 - x1) - (x4 - x3) * (y2 - y1));

            // if uA and uB are between 0-1, lines are colliding
            if (uA >= 0 && uA <= 1 && uB >= 0 && uB <= 1)
            {
                DoubleCoordinates intersectpoint;
                intersectpoint.X = x1 + (uA * (x2 - x1));
                intersectpoint.Y = y1 + (uA * (y2 - y1));
                return (1, intersectpoint);
            }
            return (0, new DoubleCoordinates());
        }

        public bool rectRect(double r1x, double r1y, double r1w, double r1h, double r2x, double r2y, double r2w, double r2h)
        {

            // are the sides of one rectangle touching the other?

            if (r1x + r1w > r2x &&    // r1 right edge past r2 left
                r1x < r2x + r2w &&    // r1 left edge past r2 right
                r1y + r1h > r2y &&    // r1 top edge past r2 bottom
                r1y < r2y + r2h)
            {    // r1 bottom edge past r2 top
                return true;
            }
            return false;
        }
        // POLYGON/LINE
        public bool PolyLineGood(DoubleCoordinates[] vertices, Double x1, Double y1, Double x2, Double y2)
        {

            // go through each of the vertices, plus the next
            // vertex in the list
            int next = 0;
            for (int current = 0; current < vertices.Length; current++)
            {

                // get next vertex in list
                // if we've hit the end, wrap around to 0
                next = current + 1;
                if (next == vertices.Length) next = 0;

                // get the PVectors at our current position
                // extract CoordsX/CoordsY coordinates from each
                Double x3 = vertices[current].X;
                Double y3 = vertices[current].Y;
                Double x4 = vertices[next].X;
                Double y4 = vertices[next].Y;

                // do a Line/Line comparison
                // if true, return 'true' immediately and
                // stop testing (faster)
                bool hit = LineLineGood(x1, y1, x2, y2, x3, y3, x4, y4);
                if (hit)
                {
                    return true;
                }
            }
            //return polyPointGOOD(vertices, x1, y1);
            return false;// better because ploypoint does the wierd select all glitch

        }

        // POLYGON/POINT
        // used only to check if the second polygon is
        // INSIDE the first
        bool PolyPointGOOD(DoubleCoordinates[] vertices, Double px, Double py)
        {
            bool collision = false;

            // go through each of the vertices, plus the next
            // vertex in the list
            int next = 0;
            for (int current = 0; current < vertices.Length; current++)
            {

                // get next vertex in list
                // if we've hit the end, wrap around to 0
                next = current + 1;
                if (next == vertices.Length) next = 0;

                // get the PVectors at our current position
                // this makes our if statement a little cleaner
                DoubleCoordinates vc = vertices[current];    // c for "current"
                DoubleCoordinates vn = vertices[next];       // n for "next"

                // compare position, flip 'collision' variable
                // back and forth
                if (((vc.Y > py && vn.Y < py) || (vc.Y < py && vn.Y > py)) &&
                     (px < (vn.X - vc.X) * (py - vc.Y) / (vn.Y - vc.Y) + vc.X))
                {
                    collision = !collision;
                }
            }
            return collision;
        }

        // LINE/LINE
        bool LineLineGood(Double x1, Double y1, Double x2, Double y2, Double x3, Double y3, Double x4, Double y4)
        {

            // calculate the direction of the lines
            Double uA = ((x4 - x3) * (y1 - y3) - (y4 - y3) * (x1 - x3)) / ((y4 - y3) * (x2 - x1) - (x4 - x3) * (y2 - y1));
            Double uB = ((x2 - x1) * (y1 - y3) - (y2 - y1) * (x1 - x3)) / ((y4 - y3) * (x2 - x1) - (x4 - x3) * (y2 - y1));

            // if uA and uB are between 0-1, lines are colliding
            if (uA >= 0 && uA <= 1 && uB >= 0 && uB <= 1)
            {
                return true;
            }
            return false;
        }

        //bool, bool, Point, int, bool, double, double
        public (bool, double, DoubleCoordinates, int, int) CollisionWithBlocks(DoubleCoordinates StartPixel, DoubleCoordinates EndingPixel, int[,] world, int blocksize, int width, int height)
        { // last three are removeble, ie: bool, double, double
            bool fullhit = false; // removeble
            double XTimeValue = 0; // removeble
            double YTimeValue = 0; // removeble

            bool hit = false;
            System.Random rnd = new System.Random();
            DoubleCoordinates[] TestVectricesX, TestVectricesY;
            TestVectricesX = new DoubleCoordinates[4];
            TestVectricesY = new DoubleCoordinates[4];

            //int XModifier = blocksize;
            //int YModifier = blocksize;


            //if (StartPixel.X > EndingPixel.X)
            //    XModifier = -blocksize;
            //if (StartPixel.Y > EndingPixel.Y)
            //    YModifier = -blocksize;


            //TestVectricesX[0] = new DoubleCoordinates(StartPixel.X + XModifier, StartPixel.Y + YModifier);
            //TestVectricesX[1] = new DoubleCoordinates(EndingPixel.X, EndingPixel.Y);
            //TestVectricesX[2] = new DoubleCoordinates(EndingPixel.X - XModifier, EndingPixel.Y);
            //TestVectricesX[3] = new DoubleCoordinates(StartPixel.X, StartPixel.Y + YModifier);

            //TestVectricesY[0] = new DoubleCoordinates(StartPixel.X + XModifier, StartPixel.Y + YModifier);
            //TestVectricesY[1] = new DoubleCoordinates(EndingPixel.X, EndingPixel.Y);
            //TestVectricesY[2] = new DoubleCoordinates(EndingPixel.X, EndingPixel.Y - YModifier);
            //TestVectricesY[3] = new DoubleCoordinates(StartPixel.X + XModifier, StartPixel.Y);


            //int XOffset = 0;
            //int YOffset = 0;
            //if ((StartPixel.X + XModifier) > EndingPixel.X)
            //    XOffset = blocksize;
            //if ((StartPixel.Y + YModifier) > EndingPixel.Y)
            //    YOffset = blocksize;



            int XModifier = -blocksize;
            int YModifier = -blocksize;
            int XOffset = blocksize;
            int YOffset = blocksize;

            if (StartPixel.X < EndingPixel.X)
            {
                XModifier = +blocksize;
                EndingPixel.X += blocksize;
                StartPixel.X += blocksize;
                XOffset = 0;
            }

            if (StartPixel.Y < EndingPixel.Y)
            {
                YModifier = +blocksize;
                EndingPixel.Y += blocksize;
                StartPixel.Y += blocksize;
                YOffset = 0;
            }


            TestVectricesX[0] = new DoubleCoordinates(StartPixel.X, StartPixel.Y);
            TestVectricesX[1] = new DoubleCoordinates(EndingPixel.X, EndingPixel.Y);
            TestVectricesX[2] = new DoubleCoordinates(EndingPixel.X - XModifier, EndingPixel.Y);
            TestVectricesX[3] = new DoubleCoordinates(StartPixel.X - XModifier, StartPixel.Y);

            TestVectricesY[0] = new DoubleCoordinates(StartPixel.X, StartPixel.Y);
            TestVectricesY[1] = new DoubleCoordinates(EndingPixel.X, EndingPixel.Y);
            TestVectricesY[2] = new DoubleCoordinates(EndingPixel.X, EndingPixel.Y - YModifier);
            TestVectricesY[3] = new DoubleCoordinates(StartPixel.X, StartPixel.Y - YModifier);




            //if ((StartPixel.CoordsX + XModifier) == EndingPixel.CoordsX)
            //    System.Windows.Forms.MessageBox.Show("CoordsX same");

            //if ((StartPixel.CoordsY + YModifier) == EndingPixel.CoordsY)
            //    System.Windows.Forms.MessageBox.Show("CoordsY same");


            double BiggerXThisPlanet = StartPixel.X > EndingPixel.X ? StartPixel.X : EndingPixel.X;
            double BiggerYThisPlanet = StartPixel.Y > EndingPixel.Y ? StartPixel.Y : EndingPixel.Y;
            double SmallerXThisPlanet = StartPixel.X > EndingPixel.X ? EndingPixel.X : StartPixel.X;
            double SmallerYThisPlanet = StartPixel.Y > EndingPixel.Y ? EndingPixel.Y : StartPixel.Y;

            int DebugintersectpointchosenY = 0;
            int DebugintersectpointchosenX = 0;
            bool IschosenpointX = true;

            int numberofintersectpointsX = 0;
            int numberofintersectpointsY = 0;
            Point[] DebugIntersectBlocksX, DebugIntersectBlocksY;
            DebugIntersectBlocksX = new Point[0];
            DebugIntersectBlocksY = new Point[0];
            //Rectangle DebugDetectrange = new Rectangle(Convert.ToInt32((SmallerXThisPlanet) / blocksize) - 1, Convert.ToInt32((SmallerYThisPlanet) / blocksize) - 1, Convert.ToInt32((BiggerXThisPlanet) / blocksize) - Convert.ToInt32((SmallerXThisPlanet) / blocksize) + 2, Convert.ToInt32((BiggerYThisPlanet) / blocksize) - Convert.ToInt32((SmallerYThisPlanet) / blocksize) + 2);
            for (int Y = (int)Math.Floor((SmallerYThisPlanet - blocksize + YOffset) / blocksize); Y < (int)Math.Ceiling((BiggerYThisPlanet + YOffset) / blocksize); Y++)
            {
                for (int X = (int)Math.Floor((SmallerXThisPlanet - blocksize + XOffset) / blocksize); X < (int)Math.Ceiling((BiggerXThisPlanet + XOffset) / blocksize); X++)
                {
                    if ((X > -1) && (X < width) && (Y > -1) && (Y < height))
                        if (world[X, Y] != 0)
                        {

                            //Line Along the CoordsY axis
                            if (PolyLineGood(TestVectricesY, X * blocksize + XOffset, Y * blocksize, X * blocksize + XOffset, Y * blocksize + blocksize))
                            {
                                Array.Resize(ref DebugIntersectBlocksY, numberofintersectpointsY + 1);
                                DebugIntersectBlocksY[numberofintersectpointsY] = new Point(X, Y);
                                if (((XOffset == 0 && DebugIntersectBlocksY[numberofintersectpointsY].X < DebugIntersectBlocksY[DebugintersectpointchosenY].X)) || ((XOffset != 0 && DebugIntersectBlocksY[numberofintersectpointsY].X > DebugIntersectBlocksY[DebugintersectpointchosenY].X)))
                                    //if (DebugIntersectBlocksY[numberofintersectpointsY].X == DebugIntersectBlocksY[DebugintersectpointchosenY].X)
                                    //{
                                    //    if (((YOffset == 0 && DebugIntersectBlocksY[numberofintersectpointsY].Y < DebugIntersectBlocksY[DebugintersectpointchosenY].Y)) || ((YOffset != 0 && DebugIntersectBlocksY[numberofintersectpointsY].Y > DebugIntersectBlocksY[DebugintersectpointchosenY].Y)))
                                    //        DebugintersectpointchosenY = numberofintersectpointsY;
                                    //}
                                    //else
                                    DebugintersectpointchosenY = numberofintersectpointsY;
                                numberofintersectpointsY += 1;
                                IschosenpointX = false;
                                hit = true;
                                YTimeValue = (DebugIntersectBlocksY[DebugintersectpointchosenY].X * blocksize + XOffset - StartPixel.X) / (EndingPixel.X - StartPixel.X);

                            }
                            //Line Along the CoordsX axis
                            if (PolyLineGood(TestVectricesX, X * blocksize, Y * blocksize + YOffset, X * blocksize + blocksize, Y * blocksize + YOffset))
                            {
                                Array.Resize(ref DebugIntersectBlocksX, numberofintersectpointsX + 1);
                                DebugIntersectBlocksX[numberofintersectpointsX] = new Point(X, Y);
                                if (((YOffset == 0 && DebugIntersectBlocksX[numberofintersectpointsX].Y < DebugIntersectBlocksX[DebugintersectpointchosenX].Y)) || ((YOffset != 0 && DebugIntersectBlocksX[numberofintersectpointsX].Y > DebugIntersectBlocksX[DebugintersectpointchosenX].Y)))
                                    //if (DebugIntersectBlocksX[numberofintersectpointsX].Y == DebugIntersectBlocksX[DebugintersectpointchosenX].Y)
                                    //{
                                    //    if (((XOffset == 0 && DebugIntersectBlocksX[numberofintersectpointsX].X < DebugIntersectBlocksX[DebugintersectpointchosenX].X)) || ((XOffset != 0 && DebugIntersectBlocksX[numberofintersectpointsX].X > DebugIntersectBlocksX[DebugintersectpointchosenX].X)))
                                    //        DebugintersectpointchosenX = numberofintersectpointsX;
                                    //}
                                    //else
                                    DebugintersectpointchosenX = numberofintersectpointsX;
                                numberofintersectpointsX += 1;
                                IschosenpointX = true;
                                hit = true;
                                XTimeValue = (DebugIntersectBlocksX[DebugintersectpointchosenX].Y * blocksize + YOffset - StartPixel.Y) / (EndingPixel.Y - StartPixel.Y);

                            }

                        }
                }

            }
            if (numberofintersectpointsX > 0 && numberofintersectpointsY > 0)
            {
                //int boost = 0; // removeble
                if (((DebugIntersectBlocksX[DebugintersectpointchosenX].Y * blocksize + YOffset - StartPixel.Y) / (EndingPixel.Y - StartPixel.Y)) == ((DebugIntersectBlocksY[DebugintersectpointchosenY].X * blocksize + XOffset - StartPixel.X) / (EndingPixel.X - StartPixel.X))) // removeble
                {
                    //boost = rnd.Next(0, 2) == 0 ? 1 : -1;//Idea: Test if when // removeble
                    fullhit = true; // removeble
                    IschosenpointX = true;
                    if (DebugIntersectBlocksX[DebugintersectpointchosenX].X > -1 && DebugIntersectBlocksX[DebugintersectpointchosenX].X < width && DebugIntersectBlocksX[DebugintersectpointchosenX].Y - YModifier > -1 && DebugIntersectBlocksX[DebugintersectpointchosenX].Y - YModifier < height)
                        if (world[DebugIntersectBlocksX[DebugintersectpointchosenX].X, DebugIntersectBlocksX[DebugintersectpointchosenX].Y - YModifier / blocksize] != 0)
                            IschosenpointX = false;
                } // removeble
                else
                    IschosenpointX = ((DebugIntersectBlocksX[DebugintersectpointchosenX].Y * blocksize + YOffset - StartPixel.Y) / (EndingPixel.Y - StartPixel.Y)) < ((DebugIntersectBlocksY[DebugintersectpointchosenY].X * blocksize + XOffset - StartPixel.X) / (EndingPixel.X - StartPixel.X));
                //IschosenpointX = (((DebugIntersectBlocksX[DebugintersectpointchosenX].Y * blocksize + YOffset - (StartPixel.Y + YModifier)) / (EndingPixel.Y - (StartPixel.Y + YModifier))) <= ((DebugIntersectBlocksY[DebugintersectpointchosenY].X * blocksize + XOffset - (StartPixel.X + XModifier)) / (EndingPixel.X - (StartPixel.X + XModifier)) + boost));
                //XTimeValue = (DebugIntersectBlocksX[DebugintersectpointchosenX].Y * blocksize + YOffset - StartPixel.Y) / (EndingPixel.Y - StartPixel.Y); // removeble
                //YTimeValue = (DebugIntersectBlocksY[DebugintersectpointchosenY].X * blocksize + XOffset - StartPixel.X) / (EndingPixel.X - StartPixel.X); // removeble
            }
            int HitX = 1;
            int HitY = 1;
            DoubleCoordinates FinalCoords = new DoubleCoordinates(EndingPixel.X - blocksize + XOffset, EndingPixel.Y - blocksize + YOffset);
            bool Auxhit;
            int hitcoords;
            if (hit)
                if (IschosenpointX)
                {
                    HitY = 0;
                    FinalCoords.Y = DebugIntersectBlocksX[DebugintersectpointchosenX].Y * blocksize - YModifier;
                    int Ypos = (int)FinalCoords.Y / blocksize;
                    int operatordirection = -XModifier / blocksize;
                    (Auxhit, hitcoords) = CollisionWithAuxBlocksX(DebugIntersectBlocksX[DebugintersectpointchosenX].X * blocksize, EndingPixel.X, world, blocksize, width, height, operatordirection, Ypos);//
                    if (Auxhit)
                    {
                        FinalCoords.X = (hitcoords + 1 * operatordirection) * blocksize;
                        HitX = 0;
                    }
                }
                else
                {
                    HitX = 0;
                    FinalCoords.X = DebugIntersectBlocksY[DebugintersectpointchosenY].X * blocksize - XModifier;
                    int Xpos = (int)FinalCoords.X / blocksize;
                    int operatordirection = -YModifier / blocksize;
                    (Auxhit, hitcoords) = CollisionWithAuxBlocksY(DebugIntersectBlocksY[DebugintersectpointchosenY].Y * blocksize, EndingPixel.Y, world, blocksize, width, height, operatordirection, Xpos);//
                    if (Auxhit)
                    {
                        FinalCoords.Y = (hitcoords + 1 * operatordirection) * blocksize;
                        HitY = 0;
                    }
                }


            //Point TheCollision = new Point(0, 0);//https://en.wikipedia.org/wiki/Elastic_collision
            //int NoOffset = 0;
            //if (hit)
            //    if (IschosenpointX)
            //    {   //System.Windows.Forms.MessageBox.Show("x");
            //        TheCollision = DebugIntersectBlocksX[DebugintersectpointchosenX];
            //        //NoOffset = XOffset == 0 ? -1 : 1;
            //        NoOffset = -YModifier;// - XOffset + blocksize;
            //    }//NoOffset = XOffset == 0 ? true : false;
            //    else
            //    {
            //        //System.Windows.Forms.MessageBox.Show("CoordsY");
            //        TheCollision = DebugIntersectBlocksY[DebugintersectpointchosenY];
            //        //NoOffset = YOffset == 0 ? -1 : 1;
            //        NoOffset = -XModifier;// - YOffset + blocksize;
            //    }
            //return (hit, IschosenpointX, TheCollision, NoOffset, fullhit, XTimeValue, YTimeValue);
            double time = IschosenpointX ? XTimeValue : YTimeValue;

            return (hit, time, FinalCoords, HitX, HitY);
        }
        public (bool, int) CollisionWithAuxBlocksX(double StartPixel, double EndingPixel, int[,] world, int blocksize, int width, int height, int operatordirection, int Ypos)
        {

            for (int X = operatordirection < 0 ? ((int)Math.Floor(StartPixel / blocksize)) : ((int)Math.Ceiling(StartPixel / blocksize)); ((operatordirection < 0 && X < (int)Math.Ceiling(EndingPixel / blocksize)) || (operatordirection > 0 && X >= (int)Math.Floor(EndingPixel / blocksize))); X -= operatordirection)

                if (X > -1 && X < width && Ypos > -1 && Ypos < height)
                    if (world[X, Ypos] != 0)
                    {
                        //System.Windows.Forms.MessageBox.Show("Xaux");
                        return (true, X);

                    }
            return (false, 0);

        }
        public (bool, int) CollisionWithAuxBlocksY(double StartPixel, double EndingPixel, int[,] world, int blocksize, int width, int height, int operatordirection, int Xpos)
        {

            for (int Y = operatordirection < 0 ? ((int)Math.Floor(StartPixel / blocksize)) : ((int)Math.Ceiling(StartPixel / blocksize)); ((operatordirection < 0 && Y < (int)Math.Ceiling(EndingPixel / blocksize)) || (operatordirection > 0 && Y >= (int)Math.Floor(EndingPixel / blocksize))); Y -= operatordirection)
                if (Y > -1 && Y < height && Xpos > -1 && Xpos < width)
                    if (world[Xpos, Y] != 0)
                    {
                        //System.Windows.Forms.MessageBox.Show("Yaux");
                        return (true, Y);
                    }
            return (false, 0);
        }

        public (bool, double, DoubleCoordinates, DoubleCoordinates, DoubleCoordinates, DoubleCoordinates, int, int) DoubleVectorCollision(DoubleCoordinates StartPixel, DoubleCoordinates EndingPixel, DoubleCoordinates StartPixel2, DoubleCoordinates EndingPixel2, DoubleCoordinates OldVelocity1, DoubleCoordinates OldVelocity2, double mass1, double mass2, int blocksize)
        {
            double elasticitypercent =0.95;
            bool hit = false;
            int XOffsetPlayer1 = 0;
            int XOffsetPlayer2 = 0;
            int YOffsetPlayer1 = 0;
            int YOffsetPlayer2 = 0;
            DoubleCoordinates PlayerVelocity = AddDoubleCoordinates(EndingPixel, StartPixel, true);
            DoubleCoordinates Player2Velocity = AddDoubleCoordinates(EndingPixel2, StartPixel2, true);
            DoubleCoordinates FinalVelocity = PlayerVelocity;
            DoubleCoordinates FinalVelocity2 = Player2Velocity;
            if (PlayerVelocity.X * Player2Velocity.X > 0)
                if (Math.Abs(PlayerVelocity.X) > Math.Abs(Player2Velocity.X))
                {
                    XOffsetPlayer1 = 0;    //add this to the start
                    //XOffsetPlayer2 = Player2Velocity.X > 0 ? -blocksize : blocksize;
                    if (PlayerVelocity.X > 0)//Opposit cuz smaller one can be 0
                        XOffsetPlayer2 = -blocksize;
                    else if (PlayerVelocity.X < 0)
                        XOffsetPlayer2 = blocksize;

                }
                else
                {
                    //XOffsetPlayer1 = PlayerVelocity.X > 0 ? -blocksize : blocksize;
                    if (Player2Velocity.X > 0)
                        XOffsetPlayer1 = -blocksize;
                    else if (Player2Velocity.X < 0)
                        XOffsetPlayer1 = blocksize;
                    XOffsetPlayer2 = 0;
                }
            if (PlayerVelocity.Y * Player2Velocity.Y > 0)
                if (Math.Abs(PlayerVelocity.Y) > Math.Abs(Player2Velocity.Y))//For some reason the sign is oposite here, But it works, Actually it does not
                {
                    YOffsetPlayer1 = 0;
                    //YOffsetPlayer2 = Player2Velocity.Y > 0 ? -blocksize : blocksize;
                    if (PlayerVelocity.Y > 0)
                        YOffsetPlayer2 = -blocksize;
                    else if (PlayerVelocity.Y < 0)
                        YOffsetPlayer2 = blocksize;
                }
                else
                {
                    //YOffsetPlayer1 = PlayerVelocity.Y > 0 ? -blocksize : blocksize;
                    if (Player2Velocity.Y > 0)
                        YOffsetPlayer1 = -blocksize;
                    else if (Player2Velocity.Y < 0)
                        YOffsetPlayer1 = blocksize;
                    YOffsetPlayer2 = 0;
                }
            if (PlayerVelocity.X == 0 || Player2Velocity.X == 0)

                if (PlayerVelocity.X != 0 || Player2Velocity.X != 0)
                {
                    if (PlayerVelocity.X == 0)
                        XOffsetPlayer1 = Player2Velocity.X > 0 ? 0 : blocksize;
                    if (Player2Velocity.X == 0)
                        XOffsetPlayer2 = PlayerVelocity.X > 0 ? 0 : blocksize;
                }
                else
                {
                    XOffsetPlayer1 = 0;
                    XOffsetPlayer2 = blocksize;
                }


            if (PlayerVelocity.Y == 0 || Player2Velocity.Y == 0)
                if (PlayerVelocity.Y != 0 || Player2Velocity.Y != 0)
                {
                    if (PlayerVelocity.Y == 0)
                        YOffsetPlayer1 = Player2Velocity.Y > 0 ? 0 : blocksize;
                    if (Player2Velocity.Y == 0)
                        YOffsetPlayer2 = PlayerVelocity.Y > 0 ? 0 : blocksize;
                }
                else
                {
                    YOffsetPlayer1 = 0;
                    YOffsetPlayer2 = blocksize;
                }

            int XModifier = -blocksize;
            int YModifier = -blocksize;
            int XOffsetPlayerVelocity1 = blocksize;
            int YOffsetPlayerVelocity1 = blocksize;
            if (StartPixel.X < EndingPixel.X)
            {
                XModifier = +blocksize;
                EndingPixel.X += blocksize;
                StartPixel.X += blocksize;
                XOffsetPlayerVelocity1 = 0;
            }
            if (StartPixel.Y < EndingPixel.Y)
            {
                YModifier = +blocksize;
                EndingPixel.Y += blocksize;
                StartPixel.Y += blocksize;
                YOffsetPlayerVelocity1 = 0;
            }
            StartPixel.X += XOffsetPlayer1;
            StartPixel.Y += YOffsetPlayer1;
            EndingPixel.X += XOffsetPlayer1;
            EndingPixel.Y += YOffsetPlayer1;

            int XModifier2 = -blocksize;
            int YModifier2 = -blocksize;
            int XOffsetPlayerVelocity2 = blocksize;
            int YOffsetPlayerVelocity2 = blocksize;
            if (StartPixel2.X < EndingPixel2.X)
            {
                XModifier2 = +blocksize;
                EndingPixel2.X += blocksize;
                StartPixel2.X += blocksize;
                XOffsetPlayerVelocity2 = 0;
            }
            if (StartPixel2.Y < EndingPixel2.Y)
            {
                YModifier2 = +blocksize;
                EndingPixel2.Y += blocksize;
                StartPixel2.Y += blocksize;
                YOffsetPlayerVelocity2 = 0;
            }
            StartPixel2.X += XOffsetPlayer2;
            StartPixel2.Y += YOffsetPlayer2;
            EndingPixel2.X += XOffsetPlayer2;
            EndingPixel2.Y += YOffsetPlayer2;







            double BiggerXThisPlanet = StartPixel.X >= EndingPixel.X ? StartPixel.X - XModifier : EndingPixel.X;
            double BiggerYThisPlanet = StartPixel.Y >= EndingPixel.Y ? StartPixel.Y - YModifier : EndingPixel.Y;
            double SmallerXThisPlanet = StartPixel.X >= EndingPixel.X ? EndingPixel.X : StartPixel.X - XModifier;
            double SmallerYThisPlanet = StartPixel.Y >= EndingPixel.Y ? EndingPixel.Y : StartPixel.Y - YModifier;

            double BiggerXThisPlanet2 = StartPixel2.X >= EndingPixel2.X ? StartPixel2.X - XModifier2 : EndingPixel2.X;
            double BiggerYThisPlanet2 = StartPixel2.Y >= EndingPixel2.Y ? StartPixel2.Y - YModifier2 : EndingPixel2.Y;
            double SmallerXThisPlanet2 = StartPixel2.X >= EndingPixel2.X ? EndingPixel2.X : StartPixel2.X - XModifier2;
            double SmallerYThisPlanet2 = StartPixel2.Y >= EndingPixel2.Y ? EndingPixel2.Y : StartPixel2.Y - YModifier2;

            //int DebugintersectpointchosenY = 0;
            //int DebugintersectpointchosenX = 0;
            //bool IschosenpointX = true;
            int numberofintersectpointsX = 0;
            int numberofintersectpointsY = 0;

            Point[] DebugIntersectBlocksX = new Point[1];
            Point[] DebugIntersectBlocksY = new Point[1];
            Point[] DebugIntersectBlocksX2 = new Point[1];
            Point[] DebugIntersectBlocksY2 = new Point[1];

            Rectangle DebugDetectrange = new Rectangle((int)Math.Floor((SmallerXThisPlanet - XOffsetPlayer1) / blocksize), (int)Math.Floor((SmallerYThisPlanet - YOffsetPlayer1) / blocksize), (int)Math.Ceiling((BiggerXThisPlanet - XOffsetPlayer1) / blocksize) - (int)Math.Floor((SmallerXThisPlanet - XOffsetPlayer1) / blocksize), (int)Math.Ceiling((BiggerYThisPlanet - YOffsetPlayer1) / blocksize) - (int)Math.Floor((SmallerYThisPlanet - YOffsetPlayer1) / blocksize));
            Rectangle DebugDetectrange2 = new Rectangle((int)Math.Floor((SmallerXThisPlanet2 - XOffsetPlayer2) / blocksize), (int)Math.Floor((SmallerYThisPlanet2 - YOffsetPlayer2) / blocksize), (int)Math.Ceiling((BiggerXThisPlanet2 - XOffsetPlayer2) / blocksize) - (int)Math.Floor((SmallerXThisPlanet2 - XOffsetPlayer2) / blocksize), (int)Math.Ceiling((BiggerYThisPlanet2 - YOffsetPlayer2) / blocksize) - (int)Math.Floor((SmallerYThisPlanet2 - YOffsetPlayer2) / blocksize));


            DoubleCoordinates FinalCoords = new DoubleCoordinates(EndingPixel.X - blocksize + XOffsetPlayerVelocity1 - XOffsetPlayer1, EndingPixel.Y - blocksize + YOffsetPlayerVelocity1 - YOffsetPlayer1);
            DoubleCoordinates FinalCoords2 = new DoubleCoordinates(EndingPixel2.X - blocksize + XOffsetPlayerVelocity2 - XOffsetPlayer2, EndingPixel2.Y - blocksize + YOffsetPlayerVelocity2 - YOffsetPlayer2);





            DebugIntersectBlocksX[numberofintersectpointsX] = new Point(0, 0);
            DebugIntersectBlocksX2[numberofintersectpointsX] = new Point(0, 0);
            DebugIntersectBlocksY[numberofintersectpointsY] = new Point(0, 0);
            DebugIntersectBlocksY2[numberofintersectpointsY] = new Point(0, 0);

            bool Xhit = false;
            bool Yhit = false;
            double time = 1;
            int changedvelocityX = 1;
            int changedvelocityY = 1;
            if (DebugDetectrange.Intersects(DebugDetectrange2))
            {

                if (!(PlayerVelocity.X == Player2Velocity.X && PlayerVelocity.Y == Player2Velocity.Y))
                {
                    double IntersectAlongY = ((StartPixel2.X * EndingPixel.X - StartPixel.X * EndingPixel2.X) / (StartPixel2.X + EndingPixel.X - StartPixel.X - EndingPixel2.X));
                    double IntersectAlongX = ((StartPixel2.Y * EndingPixel.Y - StartPixel.Y * EndingPixel2.Y) / (StartPixel2.Y + EndingPixel.Y - StartPixel.Y - EndingPixel2.Y));
                    double TimeAlongY = 1;
                    double TimeAlongX = 1;
                    if (PlayerVelocity.X != 0)
                        TimeAlongY = (IntersectAlongY - StartPixel.X) / (EndingPixel.X - StartPixel.X);
                    else if (Player2Velocity.X != 0)
                        TimeAlongY = (IntersectAlongY - StartPixel2.X) / (EndingPixel2.X - StartPixel2.X);
                    if (PlayerVelocity.Y != 0)
                        TimeAlongX = (IntersectAlongX - StartPixel.Y) / (EndingPixel.Y - StartPixel.Y);
                    else if (Player2Velocity.Y != 0)
                        TimeAlongX = (IntersectAlongX - StartPixel2.Y) / (EndingPixel2.Y - StartPixel2.Y);


                    if (TimeAlongY >= 0 && 1 > TimeAlongY)
                    {
                        double YposForIntersectAlongY = StartPixel.Y + (EndingPixel.Y - StartPixel.Y) * TimeAlongY;
                        double YposForIntersectAlongY2 = StartPixel2.Y + (EndingPixel2.Y - StartPixel2.Y) * TimeAlongY;
                        DebugIntersectBlocksY[numberofintersectpointsY] = new Point((int)IntersectAlongY, (int)YposForIntersectAlongY);
                        DebugIntersectBlocksY2[numberofintersectpointsY] = new Point((int)IntersectAlongY, (int)YposForIntersectAlongY2);

                        if (YposForIntersectAlongY + blocksize - YOffsetPlayerVelocity2 + YOffsetPlayer2 >= YposForIntersectAlongY2 - YOffsetPlayerVelocity1 + YOffsetPlayer1 && YposForIntersectAlongY - YOffsetPlayerVelocity2 + YOffsetPlayer2 <= YposForIntersectAlongY2 + blocksize - YOffsetPlayerVelocity1 + YOffsetPlayer1)
                        {

                            Yhit = true;
                            hit = true;
                            //FinalCoords.X -= (EndingPixel.X - IntersectAlongY)*2;
                            //FinalCoords2.X -= (EndingPixel2.X - IntersectAlongY)*2;
                        }
                    }

                    if (TimeAlongX >= 0 && 1 > TimeAlongX)
                    {
                        double XposForIntersectAlongX = StartPixel.X + (EndingPixel.X - StartPixel.X) * TimeAlongX;
                        double XposForIntersectAlongX2 = StartPixel2.X + (EndingPixel2.X - StartPixel2.X) * TimeAlongX;
                        DebugIntersectBlocksX[numberofintersectpointsX] = new Point((int)XposForIntersectAlongX, (int)IntersectAlongX);
                        DebugIntersectBlocksX2[numberofintersectpointsX] = new Point((int)XposForIntersectAlongX2, (int)IntersectAlongX);

                        if (XposForIntersectAlongX + blocksize - XOffsetPlayerVelocity2 + XOffsetPlayer2 >= XposForIntersectAlongX2 - XOffsetPlayerVelocity1 + XOffsetPlayer1 && XposForIntersectAlongX - XOffsetPlayerVelocity2 + XOffsetPlayer2 <= XposForIntersectAlongX2 + blocksize - XOffsetPlayerVelocity1 + XOffsetPlayer1)
                        {

                            Xhit = true;
                            hit = true;

                        }
                    }
                    if (Xhit == true && Yhit != true || TimeAlongX <= TimeAlongY && Yhit == true && Xhit == true)
                    {

                        //double totalspeed1 = EndingPixel.Y - StartPixel.Y;//also total distance
                        //double totalspeed2 = EndingPixel2.Y - StartPixel2.Y;//also total distance
                        //double a1 = totalspeed1 - OldVelocity1.Y;
                        //double speedathit = a1 * TimeAlongX + OldVelocity1.Y;


                        double timehit = TimeAlongX > 0.001 ? TimeAlongX : 0.001;
                        //double timehit = TimeAlongX;


                        double totalspeed1 = ((EndingPixel.Y - StartPixel.Y) - OldVelocity1.Y) * timehit + OldVelocity1.Y;
                        double totalspeed2 = ((EndingPixel2.Y - StartPixel2.Y) - OldVelocity2.Y) * timehit + OldVelocity2.Y;


                        //if (Math.Abs(totalspeed1) < 0.0005 && Math.Abs(totalspeed2) < 0.0005)
                        //    elasticitypercent = 100;
                        double Speed1 = ((mass1 - mass2) / (mass1 + mass2) * totalspeed1 + (mass2 * 2) / (mass1 + mass2) * totalspeed2) * elasticitypercent;// this will also be the final speed
                        double Speed2 = ((mass2 - mass1) / (mass1 + mass2) * totalspeed2 + (mass1 * 2) / (mass1 + mass2) * totalspeed1) * elasticitypercent;
                        FinalCoords.Y += -(EndingPixel.Y - IntersectAlongX) + (Speed1) * (1 - TimeAlongX);
                        FinalCoords2.Y += -(EndingPixel2.Y - IntersectAlongX) + (Speed2) * (1 - TimeAlongX);
                        FinalVelocity.Y = Speed1;
                        FinalVelocity2.Y = Speed2;
                        time = TimeAlongX;
                        changedvelocityY = 0;
                        //FinalCoords.Y = StartPixel.Y + (EndingPixel.Y - StartPixel.Y) * TimeAlongX; // test
                        //FinalCoords2.Y = StartPixel2.Y + (EndingPixel2.Y - StartPixel2.Y) * TimeAlongX; // test
                        //FinalCoords.X = StartPixel.X + (EndingPixel.X - StartPixel.X) * TimeAlongX;
                        //FinalCoords2.X = StartPixel2.X + (EndingPixel2.X - StartPixel2.X) * TimeAlongX;
                    }
                    if (Yhit == true && Xhit != true || TimeAlongY <= TimeAlongX && Xhit == true && Yhit == true)
                    {
                        //FinalCoords.X -= (EndingPixel.X - IntersectAlongY) * 2;
                        //FinalCoords2.X -= (EndingPixel2.X - IntersectAlongY) * 2;

                        //double totalspeed1 = EndingPixel.X - StartPixel.X;//also total distance
                        //double totalspeed2 = EndingPixel2.X - StartPixel2.X;//also total distance


                        double timehit = TimeAlongY > 0.001 ? TimeAlongY : 0.001;
                        //double timehit = TimeAlongY;


                        double totalspeed1 = ((EndingPixel.X - StartPixel.X) - OldVelocity1.X) * timehit + OldVelocity1.X;
                        double totalspeed2 = ((EndingPixel2.X - StartPixel2.X) - OldVelocity2.X) * timehit + OldVelocity2.X;

                           
                        double Speed1 = ((mass1 - mass2) / (mass1 + mass2) * totalspeed1 + (mass2 * 2) / (mass1 + mass2) * totalspeed2) * elasticitypercent;// this will also be the final speed
                        double Speed2 = ((mass2 - mass1) / (mass1 + mass2) * totalspeed2 + (mass1 * 2) / (mass1 + mass2) * totalspeed1) * elasticitypercent;
                        FinalCoords.X += -(EndingPixel.X - IntersectAlongY) + (Speed1) * (1 - TimeAlongY);
                        FinalCoords2.X += -(EndingPixel2.X - IntersectAlongY) + (Speed2) * (1 - TimeAlongY);
                        FinalVelocity.X = Speed1;
                        FinalVelocity2.X = Speed2;
                        time = TimeAlongY;
                        changedvelocityX = 0;//need to give just pos and velocity at intersect point cus the acceleratoion breaks it
                        //FinalCoords.X = StartPixel.X + (EndingPixel.X - StartPixel.X) * TimeAlongY; // test
                        //FinalCoords2.X = StartPixel2.X + (EndingPixel2.X - StartPixel2.X) * TimeAlongY; // test
                        //FinalCoords.Y = StartPixel.Y + (EndingPixel.Y - StartPixel.Y) * TimeAlongY;
                        //FinalCoords2.Y = StartPixel2.Y + (EndingPixel2.Y - StartPixel2.Y) * TimeAlongY;
                    }

                }


                
            }

            return (hit, time, FinalCoords, FinalCoords2, FinalVelocity, FinalVelocity2, changedvelocityX, changedvelocityY);




        }
        public DoubleCoordinates AddDoubleCoordinates(DoubleCoordinates number1, DoubleCoordinates number2, bool SecondIsNegative = false)
        {
            DoubleCoordinates Sommation;
            if (SecondIsNegative)
                Sommation = new DoubleCoordinates(number1.X - number2.X, number1.Y - number2.Y);

            else
                Sommation = new DoubleCoordinates(number1.X + number2.X, number1.Y + number2.Y);
            return Sommation;
        }
        public DoubleCoordinates MultiplyDoubleCoordinates(DoubleCoordinates number1, double number2)
        {
            DoubleCoordinates Sommation;
                Sommation = new DoubleCoordinates(number1.X * number2, number1.Y * number2);
            return Sommation;
        }
    }
    public struct DoubleCoordinates
    {
        public double X;
        public double Y;

        public DoubleCoordinates(double X, double Y)
        {
            this.X = X;
            this.Y = Y;
        }
        //public override bool 
    }


}
