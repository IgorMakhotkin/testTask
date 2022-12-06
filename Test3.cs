using System.Text;
using System.Text.RegularExpressions;
using Microsoft.VisualBasic.FileIO;

namespace TestTask
{
    public class Test3
    {
        private const char vbTab = '\t';
        public void SortLines()
        {
            Encoding.RegisterProvider(CodePagesEncodingProvider.Instance);
            var win1251 = Encoding.GetEncoding(1251);


            using (TextFieldParser parser = new TextFieldParser(@"C:\repo\testTask\Задание3.txt", win1251))
            {

                parser.SetDelimiters("\t");
                parser.HasFieldsEnclosedInQuotes = true;
                List<string[]?> text = new List<string[]?>();
                while (!parser.EndOfData)
                {
                    string[]? fields = parser.ReadFields();
                    text.Add(fields);
                }
                NumberComparer nc = new NumberComparer();
                text.Sort(nc);
                var result = CellDataType(text);
                using (StreamWriter writer = new StreamWriter(@"C:\repo\testTask\РезультатЗадание3.txt", true))
                {
                    foreach (var item in result)
                    {
                        writer.WriteLine(ReturnString(item));
                    }
                }
            }
        }
        private string ReturnString(string[] array)
        {
            string returnString = string.Empty;
            foreach (var item in array)
            {
                if(item == "")
                {
                    returnString += "\t";
                }
                returnString += item;
            }
            return returnString;
        }
        private List<string[]> CellDataType(List<string[]?> text)
        {
            for (int j = 1; j < text.Count; j++)
            {
                var array = text[j];
                for (int i = 3; i < array!.Length; i++)
                {
                    if (array[i] != "")
                    {
                        if (IsDigitsOnly(array[i]))
                        {
                            if (short.TryParse(array[i], out var ShortNumber))
                            { array[i] += "(short)"; }
                            if (sbyte.TryParse(array[i], out var SbyteNumber))
                            { array[i] += "(sbyte)"; }
                            if (long.TryParse(array[i], out var LongNumber))
                            { array[i] += "(long)"; }
                        }
                        else
                        {
                            array[i] += "(string)";
                        }
                    }
                }
            }
            return text!;
        }
        private bool IsDigitsOnly(string str)
        {
            return str.All(c => c >= '0' && c <= '9');
        }
    }

    public class NumberComparer : IComparer<string[]?>
    {
        public int Compare(string[]? array1, string[]? array2)
        {
            if (int.TryParse(array1![2], out int a)) { }
            else
            { return 0; }

            if (int.TryParse(array2![2], out int b)) { }
            else
            { return 0; }


            if (a > b)
            {
                return 1;
            }
            else if (a < b)
            {
                return -1;
            }

            return 0;
        }
    }

}