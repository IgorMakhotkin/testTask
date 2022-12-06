namespace TestTask;

public static class StringExtensions
{
    public static string Capitalize(this string s)
    {
        if (String.IsNullOrEmpty(s)) {
            throw new ArgumentException("String is mull or empty");
        }
 
        return s[0].ToString().ToUpper() + s.Substring(1);
    }
        public static string Code(this string s)
    {
        if (String.IsNullOrEmpty(s)) {
            throw new ArgumentException("String is mull or empty");
        }
 
        return "1" + s.Substring(0,s.Length - 3) + "000";
    }
}