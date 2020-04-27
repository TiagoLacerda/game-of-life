using System;
using GameOfLife.assets.classes.IO;

namespace GameOfLife.assets.classes
{
    public class Game
    {
        private Grid thisGrid;
        private Grid lastGrid;
        public bool paused;
        public int width;
        public int height;

        public Game(int width, int height)
        {
            if ((width < 1) || height < 1)
                throw new Exception("Invalid Game dimensions!");

            this.paused = true;
            this.width = width;
            this.height = height;

            lastGrid = new Grid(width, height, Grid.BorderType.WrapBorders);
            thisGrid = new Grid(width, height, Grid.BorderType.WrapBorders);
        }

        public void Iterate()
        {
            lastGrid = thisGrid.Copy();

            var map = thisGrid.GetNeighborMap();
            for (int i = 0; i < height; i++)
            {
                for (int j = 0; j < width; j++)
                {
                    var cell = thisGrid.GetCell(j, i);
                    var neighbors = map[i][j];

                    if (cell.value == 0)
                    {
                        if (neighbors == 3)
                        {
                            cell.value = 1;
                        }
                    }
                    else
                    {
                        if (neighbors < 2 || neighbors > 3)
                        {
                            cell.value = 0;
                        }
                    }
                }
            }
        }

        public void Undo()
        {
            thisGrid = lastGrid.Copy();
        }

        public void Clear()
        {
            thisGrid.Clear();
        }

        public void Randomize(int density)
        {
            var RNG = new Random();
            var d = Math.Max(0, Math.Min(100, density));
            thisGrid.Clear();

            for (int n = 0; n < width * height * d / 100;)
            {
                var x = RNG.Next(0, width);
                var y = RNG.Next(0, height);
                var cell = thisGrid.GetCell(x, y);

                if (cell.value == 0)
                {
                    cell.value = 1;
                    n++;
                }
            }
        }

        public byte GetCellValue(int x, int y)
        {
            return thisGrid.GetCell(x, y).value;
        }

        public void SetCellValue(int x, int y, byte value)
        {
            thisGrid.GetCell(x, y).value = value;
        }

        public byte GetLastCellValue(int x, int y)
        {
            return lastGrid.GetCell(x, y).value;
        }

        public void SetBorderType(Grid.BorderType borderType)
        {
            thisGrid.borderType = borderType;
            lastGrid.borderType = borderType;
        }
    }
}