using System;

namespace GameOfLife.assets.classes
{
    public class Grid
    {
        private Cell[][] cells;

        public enum BorderType { LiveBorders, DeadBorders, WrapBorders };
        public BorderType borderType;

        public readonly int width;
        public readonly int height;

        public Grid(int width, int height, BorderType borderType)
        {
            if ((width < 1) || height < 1)
                throw new Exception("Invalid Grid dimensions!");

            this.width = width;
            this.height = height;
            this.borderType = borderType;

            cells = new Cell[height][];
            for (var i = 0; i < height; i++)
            {
                cells[i] = new Cell[width];
                for (var j = 0; j < width; j++)
                {
                    cells[i][j] = new Cell();
                }
            }
        }

        public Grid Copy()
        {
            var copy = new Grid(width, height, borderType);
            for (var i = 0; i < height; i++)
            {
                for (var j = 0; j < width; j++)
                {
                    copy.cells[i][j] = this.cells[i][j].Copy();
                }
            }
            return copy;
        }

        public void Clear()
        {
            for (var i = 0; i < height; i++)
            {
                for (var j = 0; j < width; j++)
                {
                    cells[i][j] = new Cell();
                }
            }
        }

        private int GetNeighborsDeadBorders(int x, int y)
        {
            if (y == 0)
            {
                if (x == 0)
                {
                    // Upper left
                    return (cells[y][x + 1].value + cells[y + 1][x].value + cells[y + 1][x + 1].value);
                }
                else if (x == width - 1)
                {
                    // Upper right
                    return (cells[y][x - 1].value + cells[y + 1][x - 1].value + cells[y + 1][x].value);
                }
                else
                {
                    // Upper center
                    return (cells[y][x - 1].value + cells[y][x + 1].value + cells[y + 1][x - 1].value + cells[y + 1][x].value + cells[y + 1][x + 1].value);
                }
            }
            else if (y == height - 1)
            {
                if (x == 0)
                {
                    // Lower left
                    return (cells[y - 1][x].value + cells[y - 1][x + 1].value + cells[y][x + 1].value);
                }
                else if (x == width - 1)
                {
                    // Lower right
                    return (cells[y - 1][x - 1].value + cells[y - 1][x].value + cells[y][x - 1].value);
                }
                else
                {
                    // Lower center
                    return (cells[y - 1][x - 1].value + cells[y - 1][x].value + cells[y - 1][x + 1].value + cells[y][x - 1].value + cells[y][x + 1].value);
                }
            }
            else
            {
                if (x == 0)
                {
                    // Left center
                    return (cells[y - 1][x].value + cells[y - 1][x + 1].value + cells[y][x + 1].value + cells[y + 1][x].value + cells[y + 1][x + 1].value);
                }
                else if (x == width - 1)
                {
                    // Right center
                    return (cells[y - 1][x - 1].value + cells[y - 1][x].value + cells[y][x - 1].value + cells[y + 1][x - 1].value + cells[y + 1][x].value);
                }
                else
                {
                    // Center
                    return (cells[y - 1][x - 1].value + cells[y - 1][x].value + cells[y - 1][x + 1].value + cells[y][x - 1].value + cells[y][x + 1].value + cells[y + 1][x - 1].value + cells[y + 1][x].value + cells[y + 1][x + 1].value);
                }
            }
        }

        private int GetNeighborsLiveBorders(int x, int y)
        {
            if (y == 0)
            {
                if (x == 0)
                {
                    // Upper left
                    return (5 + cells[y][x + 1].value + cells[y + 1][x].value + cells[y + 1][x + 1].value);
                }
                else if (x == width - 1)
                {
                    // Upper right
                    return (5 + cells[y][x - 1].value + cells[y + 1][x - 1].value + cells[y + 1][x].value);
                }
                else
                {
                    // Upper center
                    return (3 + cells[y][x - 1].value + cells[y][x + 1].value + cells[y + 1][x - 1].value + cells[y + 1][x].value + cells[y + 1][x + 1].value);
                }
            }
            else if (y == height - 1)
            {
                if (x == 0)
                {
                    // Lower left
                    return (5 + cells[y - 1][x].value + cells[y - 1][x + 1].value + cells[y][x + 1].value);
                }
                else if (x == width - 1)
                {
                    // Lower right
                    return (5 + cells[y - 1][x - 1].value + cells[y - 1][x].value + cells[y][x - 1].value);
                }
                else
                {
                    // Lower center
                    return (3 + cells[y - 1][x - 1].value + cells[y - 1][x].value + cells[y - 1][x + 1].value + cells[y][x - 1].value + cells[y][x + 1].value);
                }
            }
            else
            {
                if (x == 0)
                {
                    // Left center
                    return (3 + cells[y - 1][x].value + cells[y - 1][x + 1].value + cells[y][x + 1].value + cells[y + 1][x].value + cells[y + 1][x + 1].value);
                }
                else if (x == width - 1)
                {
                    // Right center
                    return (3 + cells[y - 1][x - 1].value + cells[y - 1][x].value + cells[y][x - 1].value + cells[y + 1][x - 1].value + cells[y + 1][x].value);
                }
                else
                {
                    // Center
                    return (cells[y - 1][x - 1].value + cells[y - 1][x].value + cells[y - 1][x + 1].value + cells[y][x - 1].value + cells[y][x + 1].value + cells[y + 1][x - 1].value + cells[y + 1][x].value + cells[y + 1][x + 1].value);
                }
            }
        }

        private int GetNeighborsWrapBorders(int x, int y)
        {
            if (y == 0)
            {
                if (x == 0)
                {
                    // Upper left
                    return (cells[height - 1][width - 1].value + cells[height - 1][x].value + cells[height - 1][x + 1].value + cells[y][width - 1].value + cells[y][x + 1].value + cells[y + 1][width - 1].value + cells[y + 1][x].value + cells[y + 1][x + 1].value);
                }
                else if (x == width - 1)
                {
                    // Upper right
                    return (cells[height - 1][x - 1].value + cells[height - 1][x].value + cells[height - 1][0].value + cells[y][x - 1].value + cells[y][0].value + cells[y + 1][x - 1].value + cells[y + 1][x].value + cells[y + 1][0].value);
                }
                else
                {
                    // Upper center
                    return (cells[height - 1][x - 1].value + cells[height - 1][x].value + cells[height - 1][x + 1].value + cells[y][x - 1].value + cells[y][x + 1].value + cells[y + 1][x - 1].value + cells[y + 1][x].value + cells[y + 1][x + 1].value);
                }
            }
            else if (y == height - 1)
            {
                if (x == 0)
                {
                    // Lower left
                    return (cells[y - 1][width - 1].value + cells[y - 1][x].value + cells[y - 1][x + 1].value + cells[y][width - 1].value + cells[y][x + 1].value + cells[0][width - 1].value + cells[0][x].value + cells[0][x + 1].value);
                }
                else if (x == width - 1)
                {
                    // Lower right
                    return (cells[y - 1][x - 1].value + cells[y - 1][x].value + cells[y - 1][0].value + cells[y][x - 1].value + cells[y][0].value + cells[0][x - 1].value + cells[0][x].value + cells[0][0].value);
                }
                else
                {
                    // Lower center
                    return (cells[y - 1][x - 1].value + cells[y - 1][x].value + cells[y - 1][x + 1].value + cells[y][x - 1].value + cells[y][x + 1].value + cells[0][x - 1].value + cells[0][x].value + cells[0][x + 1].value);
                }
            }
            else
            {
                if (x == 0)
                {
                    // Left center
                    return (cells[y - 1][width - 1].value + cells[y - 1][x].value + cells[y - 1][x + 1].value + cells[y][width - 1].value + cells[y][x + 1].value + cells[y + 1][width - 1].value + cells[y + 1][x].value + cells[y + 1][x + 1].value);
                }
                else if (x == width - 1)
                {
                    // Right center
                    return (cells[y - 1][x - 1].value + cells[y - 1][x].value + cells[y - 1][0].value + cells[y][x - 1].value + cells[y][0].value + cells[y + 1][x - 1].value + cells[y + 1][x].value + cells[y + 1][0].value);
                }
                else
                {
                    // Center
                    return (cells[y - 1][x - 1].value + cells[y - 1][x].value + cells[y - 1][x + 1].value + cells[y][x - 1].value + cells[y][x + 1].value + cells[y + 1][x - 1].value + cells[y + 1][x].value + cells[y + 1][x + 1].value);
                }
            }
        }

        private int GetNeighbors(int x, int y)
        {
            if (x < 0 || y < 0 || x >= width || y >= height)  // Coordinates exceed grid limits
                return 0;

            switch (borderType)
            {
                case BorderType.DeadBorders:
                    return GetNeighborsDeadBorders(x, y);
                case BorderType.LiveBorders:
                    return GetNeighborsLiveBorders(x, y);
                case BorderType.WrapBorders:
                    return GetNeighborsWrapBorders(x, y);
                default:
                    return 0;
            }
        }

        public int[][] GetNeighborMap()
        {
            int H = cells.Length, W = cells[0].Length;
            var map = new int[H][];
            for (int i = 0; i < H; i++)
            {
                map[i] = new int[W];
                for (int j = 0; j < W; j++)
                {
                    map[i][j] = GetNeighbors(j, i);
                }
            }

            return map;
        }

        public Cell GetCell(int x, int y)
        {
            return cells[y][x];
        }

        public void SetCell(int x, int y, Cell cell)
        {
            cells[y][x] = cell;
        }
    }
}
