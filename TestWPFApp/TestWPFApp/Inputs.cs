namespace TestWPFApp
{
    public class Inputs
    {
        public required int Depth { get; set; }
        public required int[,] Board { get; set; }
        public required List<string> Pieces { get; set; }
    }
}
