

namespace TestTask
{
    public class Test1
    {
        public Test1() { }

        public int[,] CreateRandomArray(int x, int y)
        {
            int[,] rndArray = new int[x, y];
            Random rnd = new Random();
            for (int i = 0; i < x; i++)
            {
                for (int j = 0; j < y; j++)
                {
                    rndArray[i, j] = rnd.Next(1, 100);
                }
            }
            return rndArray;
        }
        public void PrintArray(int[,] array)
        {
            int rows = array.GetUpperBound(0) + 1;
            int columns = array.Length / rows;
            for (int i = 0; i < rows; i++)
            {
                for (int j = 0; j < columns; j++)
                {
                    Console.Write($"{array[i, j]} \t");
                }
                Console.WriteLine();
            }
        }
    }
}