namespace GameOfLife.assets.classes.Mathematics
{
    public class Math
    {
        public static int Modulo(int n, int m)
        {
            return (n % m + m) % m;
        }

        public static int Max(int a, int b)
        {
            return a > b ? a : b;
        }

        public static int Min(int a, int b)
        {
            return a < b ? a : b;
        }
    }
}
