using System;
using System.IO;
using System.Resources;
using System.Text;

public class LocaleGen
{
    public static void Main(string[] args)
    {
        var ResWriter = new ResourceWriter(Path.Combine(@"..", "..", "PascalSharp.Compiler", "PascalSharp.Internal.Localization", "DefaultLang.resources"));
        BuildLocale("Rus", "DefaultLanguage", "Default (Russian)");
        BuildLocale("Rus", "RussianLanguage", "Russian");
        BuildLocale("Eng", "EnglishLanguage", "English");
        BuildLocale("Ukr", "UkrainianLanguage", "Ukrainian");
        ResWriter.Generate();
        ResWriter.Close();

        void BuildLocaleImpl(string code, string targetName, string prefix)
        {
            Console.Write($"{prefix}searching...".PadRight(80, ' '));
            var Files = new DirectoryInfo(Path.Combine(@"..", "InboxRelease", "Lng", code)).GetFiles("*.*");
            Console.Write($"\r{prefix}building out of {Files.Length} files...".PadRight(80, ' '));
            var res = "";
            for (var i=0; i < Files.Length; ++i)
                res += $"\r\n\r\n//{Files[i].Name}\r\n%PREFIX%=\r\n{File.ReadAllText(Files[i].FullName)}";
            Console.Write($"\r{prefix}saving...".PadRight(80, ' '));
            ResWriter.AddResource(targetName, Encoding.GetEncoding(1251).GetBytes(res));
            Console.Write($"\r{prefix}OK".PadRight(80, ' '));
        }

        bool BuildLocale(string code, string targetName, string langName)
        {
            var prefix = $"[{langName}]: ";
            try
            {
                BuildLocaleImpl(code, targetName, prefix);
                Console.WriteLine();
                return true;
            }
            catch (Exception e)
            {
                Console.WriteLine($"\r{prefix}fail".PadRight(80, ' '));
                Console.WriteLine(e);
                return false;
            }
        }
    }
}