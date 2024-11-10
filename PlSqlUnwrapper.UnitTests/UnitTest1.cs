using System.Text;
using System.Text.RegularExpressions;

namespace PlSqlUnwrapper.UnitTests
{
    [TestClass]
    public class UnitTest1
    {
        [TestMethod]
        public void TestFunctions()
        {
            string[] wrapped = Directory.GetFiles(@"C:\Users\Robert\Documents\MCE\Projets\GitHub\PlSqlUnwrapper\PlSqlUnwrapper.UnitTests\TestFiles\Functions", "*.wrapped.txt");

            foreach (string file in wrapped)
            {
                string wrappedPlSql = File.ReadAllText(file);
                string unwrappedPlSql = PlSqlUnwrapper.Unwrap(wrappedPlSql, Encoding.Latin1).Replace("\n", "\r\n").TrimEnd('\0');
                string plSql = File.ReadAllText(file.Replace(".wrapped", ""));
                Assert.AreEqual(unwrappedPlSql, plSql);
            }
        }

        [TestMethod]
        public void TestProcedures()
        {
            string[] wrapped = Directory.GetFiles(@"C:\Users\Robert\Documents\MCE\Projets\GitHub\PlSqlUnwrapper\PlSqlUnwrapper.UnitTests\TestFiles\Procedures", "*.wrapped.txt");

            foreach (string file in wrapped)
            {
                string wrappedPlSql = File.ReadAllText(file);
                string unwrappedPlSql = PlSqlUnwrapper.Unwrap(wrappedPlSql, Encoding.Latin1).Replace("\n", "\r\n").TrimEnd('\0');
                string plSql = File.ReadAllText(file.Replace(".wrapped", ""));
                Assert.AreEqual(unwrappedPlSql, plSql);
            }
        }

        [TestMethod]
        public void TestPackageBodies()
        {
            string[] wrapped = Directory.GetFiles(@"C:\Users\Robert\Documents\MCE\Projets\GitHub\PlSqlUnwrapper\PlSqlUnwrapper.UnitTests\TestFiles\PackageBodies", "*.wrapped.txt");

            foreach (string file in wrapped)
            {
                string wrappedPlSql = File.ReadAllText(file);
                string unwrappedPlSql = PlSqlUnwrapper.Unwrap(wrappedPlSql, Encoding.Latin1).Replace("\n", "\r\n").TrimEnd('\0');
                string plSql = File.ReadAllText(file.Replace(".wrapped", ""));
                Assert.AreEqual(unwrappedPlSql, plSql);
            }
        }

        [TestMethod]
        public void CreateFunctionTests()
        {
            string plsqlWrapped = File.ReadAllText(@"D:\Conversion\Parser\OracleForms\Database\Functions.txt");
            string[] wrapped = plsqlWrapped.Split("\r\n/\r\n");
            Dictionary<string, string> wrappedElements = new Dictionary<string, string>();

            foreach (string wrap in wrapped)
            {
                int pos = wrap.IndexOf("wrapped");

                if (pos > 0)
                {
                    Match m = Regex.Match(wrap, @"CREATE\s+(OR\s+REPLACE\s+)?(EDITIONABLE\s+)?(PROCEDURE|FUNCTION)\s+""(\w+)""\.""(\w+)""");
                    string name = m.Groups[5].ToString();
                    string code = $"{m.Groups[3].ToString()} {name} " + wrap.Substring(pos).Trim();
                    wrappedElements[name.ToUpperInvariant()] = code;
                }
            }

            string plsqlUnwrapped = File.ReadAllText(@"D:\Conversion\Parser\OracleForms\Database\Functions oddgen.txt");
            string[] unwrapped = plsqlUnwrapped.Split("\r\n/\r\n");
            Dictionary<string, string> unwrappedElements = new Dictionary<string, string>();

            foreach (string unwrap in unwrapped)
            {
                if (string.IsNullOrWhiteSpace(unwrap))
                    continue;

                Match m = Regex.Match(unwrap, @"CREATE\s+(OR\s+REPLACE\s+)?(PROCEDURE|FUNCTION)\s+(\w+)");
                string name = m.Groups[3].ToString();
                unwrappedElements[name.ToUpperInvariant()] = unwrap.Substring(m.Groups[2].Index);
            }

            foreach (string name in wrappedElements.Keys)
            {
                if (unwrappedElements.ContainsKey(name))
                {
                    File.WriteAllText($@"C:\Users\Robert\Documents\MCE\Projets\GitHub\PlSqlUnwrapper\PlSqlUnwrapper.UnitTests\TestFiles\Functions\{name}.wrapped.txt", wrappedElements[name]);
                    File.WriteAllText($@"C:\Users\Robert\Documents\MCE\Projets\GitHub\PlSqlUnwrapper\PlSqlUnwrapper.UnitTests\TestFiles\Functions\{name}.txt", unwrappedElements[name]);
                }
            }
        }

        [TestMethod]
        public void CreateProcedureTests()
        {
            string plsqlWrapped = File.ReadAllText(@"D:\Conversion\Parser\OracleForms\Database\Procedures.txt");
            string[] wrapped = plsqlWrapped.Split("\r\n/\r\n");
            Dictionary<string, string> wrappedElements = new Dictionary<string, string>();

            foreach (string wrap in wrapped)
            {
                int pos = wrap.IndexOf("wrapped");

                if (pos > 0)
                {
                    Match m = Regex.Match(wrap, @"CREATE\s+(OR\s+REPLACE\s+)?(EDITIONABLE\s+)?(PROCEDURE|FUNCTION)\s+""(\w+)""\.""(\w+)""");
                    string name = m.Groups[5].ToString();
                    string code = $"{m.Groups[3].ToString()} {name} " + wrap.Substring(pos).Trim();
                    wrappedElements[name.ToUpperInvariant()] = code;
                }
            }

            string plsqlUnwrapped = File.ReadAllText(@"D:\Conversion\Parser\OracleForms\Database\Procedures oddgen.txt");
            string[] unwrapped = plsqlUnwrapped.Split("\r\n/\r\n");
            Dictionary<string, string> unwrappedElements = new Dictionary<string, string>();

            foreach (string unwrap in unwrapped)
            {
                if (string.IsNullOrWhiteSpace(unwrap))
                    continue;

                Match m = Regex.Match(unwrap, @"CREATE\s+(OR\s+REPLACE\s+)?(PROCEDURE|FUNCTION)\s+(\w+)");
                string name = m.Groups[3].ToString();
                unwrappedElements[name.ToUpperInvariant()] = unwrap.Substring(m.Groups[2].Index);
            }

            foreach (string name in wrappedElements.Keys)
            {
                if (unwrappedElements.ContainsKey(name))
                {
                    File.WriteAllText($@"C:\Users\Robert\Documents\MCE\Projets\GitHub\PlSqlUnwrapper\PlSqlUnwrapper.UnitTests\TestFiles\Procedures\{name}.wrapped.txt", wrappedElements[name]);
                    File.WriteAllText($@"C:\Users\Robert\Documents\MCE\Projets\GitHub\PlSqlUnwrapper\PlSqlUnwrapper.UnitTests\TestFiles\Procedures\{name}.txt", unwrappedElements[name]);
                }
            }
        }

        [TestMethod]
        public void CreatePackageBodyTests()
        {
            string plsqlWrapped = File.ReadAllText(@"D:\Conversion\Parser\OracleForms\Database\Package bodies.txt");
            string[] wrapped = plsqlWrapped.Split("\r\n/\r\n");
            Dictionary<string, string> wrappedElements = new Dictionary<string, string>();

            foreach (string wrap in wrapped)
            {
                Match m = Regex.Match(wrap, @"wrapped");

                if (m.Success)
                {
                    string name = "";
                    Match m1 = Regex.Match(wrap, @"CREATE\s+OR\s+REPLACE\s+EDITIONABLE\s+PACKAGE BODY\s+""(\w+)""");
                    if (m1.Success)
                    {
                        name = m1.Groups[1].ToString();
                        string code = wrap.Substring(m.Groups[0].Index + m.Groups[0].Length);
                        wrappedElements[name.ToUpperInvariant()] = $"PACKAGE BODY {name} wrapped {code}";
                    }
                }
            }

            string plsqlUnwrapped = File.ReadAllText(@"D:\Conversion\Parser\OracleForms\Database\Package bodies oddgen.txt");
            string[] unwrapped = plsqlUnwrapped.Split("\r\n/\r\n");
            Dictionary<string, string> unwrappedElements = new Dictionary<string, string>();

            foreach (string unwrap in unwrapped)
            {
                if (string.IsNullOrWhiteSpace(unwrap))
                    continue;

                Match m = Regex.Match(unwrap, @"CREATE\s+(OR\s+REPLACE\s+)?(PROCEDURE|FUNCTION|PACKAGE BODY)\s+(\w+)");
                string name = m.Groups[3].ToString();
                unwrappedElements[name.ToUpperInvariant()] = unwrap.Substring(m.Groups[2].Index);
            }

            foreach (string name in wrappedElements.Keys)
            {
                if (unwrappedElements.ContainsKey(name))
                {
                    File.WriteAllText($@"C:\Users\Robert\Documents\MCE\Projets\GitHub\PlSqlUnwrapper\PlSqlUnwrapper.UnitTests\TestFiles\PackageBodies\{name}.wrapped.txt", wrappedElements[name]);
                    File.WriteAllText($@"C:\Users\Robert\Documents\MCE\Projets\GitHub\PlSqlUnwrapper\PlSqlUnwrapper.UnitTests\TestFiles\PackageBodies\{name}.txt", unwrappedElements[name]);
                }
            }
        }

    }
}