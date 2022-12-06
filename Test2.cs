

namespace TestTask
{
    public class Test2
    {
        public Test2() { }

        public void SumOfNumbers(int[,] array)
        {
            int rows = array.GetUpperBound(0) + 1;
            int columns = array.Length / rows;
            int[] diagonal1 = SumOfNumbersByDiagonal(array, true);
            int[] diagonal2 = SumOfNumbersByDiagonal(array, false);
            Console.WriteLine();
            PrintArray(diagonal1);
            PrintSumOfNumbers(diagonal1);
            PrintArray(diagonal2);
            PrintSumOfNumbers(diagonal2);

        }
        public int[] SumOfNumbersByDiagonal(int[,] array, bool route)
        {
            int rows = array.GetUpperBound(0) + 1;
            int columns = array.Length / rows;
            int[] numByDiagonal = new int[rows];
            if (route)
            {

                for (int i = 0; i < rows; i++)
                {
                    numByDiagonal[i] = array[i, i];
                }
            }
            else
            {
                for (int i = 0; i < rows; i++)
                {
                    numByDiagonal[i] = array[i, columns-1];
                    columns--;
                }
            }
            return numByDiagonal;
        }
        public void PrintArray(int[] array)
        {
            for (int j = 0; j < array.Length; j++)
            {
                Console.Write($"{array[j]} \t");
            }
            Console.WriteLine();
        }
        public void PrintSumOfNumbers(int[] array)
        {
            int sum = 0;
            for (int j = 0; j < array.Length; j++)
            {
                sum += array[j];
            }
            Console.Write($"Cумма чисел равна {sum} \t");
            Console.WriteLine();
        }
    }
}