namespace GameOfLife.assets.classes
{
    public class Cell
    {
        public byte value;

        public Cell()
        {
            this.value = 0;
        }

        public Cell(byte value)
        {
            this.value = value;
        }

        public Cell Copy()
        {
            return new Cell(value);
        }

        public override string ToString()
        {
            return $"value:[{value}]";
        }
    }
}
