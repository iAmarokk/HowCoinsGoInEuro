namespace EuroDiffusion
{
	class Coord
    {

        public Coord(int x, int y)
        {
			this.X = x;
			this.Y = y;
        }

        public int X { get; set; }
        public int Y { get; set; }

        public override string ToString()
        {
            return this.X.ToString() + this.Y.ToString();
        }
    }
}
