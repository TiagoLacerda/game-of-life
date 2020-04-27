using System.Drawing;

namespace GameOfLife.assets.classes
{
    public class Canvas
    {
        private Bitmap image;
        public readonly int width;
        public readonly int height;

        public Canvas(int width, int height)
        {
            this.width = width;
            this.height = height;
            this.image = new Bitmap(this.width, this.height);
        }

        public void SetPixel(int x, int y, Color color)
        {
            image.SetPixel(x, y, color);
        }

        public void DrawBorders(Color color)
        {
            for (int j = 0; j < width; j++)
            {
                SetPixel(j, 0, color);
                SetPixel(j, height - 1, color);
            }

            for (int i = 1; i < height - 1; i++)
            {
                SetPixel(0, i, color);
                SetPixel(width - 1, i, color);
            }
        }

        public Bitmap ToBitmap()
        {
            return image;
        }
    }
}
