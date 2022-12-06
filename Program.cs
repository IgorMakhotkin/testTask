
namespace TestTask
{
    public class Programm
    {
        static async Task Main(string[] args)
        {
            var test1 = new Test1();
            var test2 = new Test2();
            var test3 = new Test3();
            var test4 = new Test4();
            var testArray = test1.CreateRandomArray(4, 5);
            test1.PrintArray(testArray);
            test2.SumOfNumbers(testArray);
            test3.SortLines();
            await test4.FromExcelFileAsync();
        }
    }
}