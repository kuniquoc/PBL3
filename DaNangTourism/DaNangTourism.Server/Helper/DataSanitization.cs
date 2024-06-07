namespace DaNangTourism.Server.Helper
{
  public class DataSanitization
  {
    // hàm khử các kí tự đặc biệt
    public static string RemoveSpecialCharacters(string input)
    {
      string allowedChars = "abcdefghijklmnopqrstuvwxyzABCDEFGHIJKLMNOPQRSTUVWXYZ0123456789-_., ";
      string output = "";

      foreach (char c in input)
      {
        if (allowedChars.Contains(c))
        {
          output += c;
        }
      }

      return output;
    }

    public static string UrlDecode(string input)
    {
      return System.Net.WebUtility.UrlDecode(input);
    }
    // có thể thêm các hàm để khử html code, sql injection, ...
  }
}
