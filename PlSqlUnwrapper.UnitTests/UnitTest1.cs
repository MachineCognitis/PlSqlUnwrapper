
using System.Text;
using System.Text.RegularExpressions;

namespace PlSqlUnwrapper.UnitTests
{
    [TestClass]
    public partial class UnitTest1
    {
        [TestMethod]
        public void TestLatin1()
        {
            string wrappedPlSql = @"
CREATE OR REPLACE FUNCTION get_latin1_string wrapped 
a000000
1f
abcd
abcd
abcd
abcd
abcd
abcd
abcd
abcd
abcd
abcd
abcd
abcd
abcd
abcd
abcd
8
8f b6
HQrJ6ceqqsLh5Gs63gvHZtDzzoUwg8eZgcfLCNL+XlquYvRyoVb6R4JfltE+l0cM6ufAsr2y
m17nx3TAM7h0ZQmldIsJaedtKMu9m9LHB5n2BKsEEaPHrb5FEpKo2boxxRatSejtw8I5GBVf
rPKjx76SvhZTmEDb+FdAMJKIFQKIpjusSSY=";

            string unwrappedPlSql = PlSqlUnwrapper.Unwrap(wrappedPlSql, Encoding.Latin1).Replace("\n", "\r\n");

            Assert.AreEqual(unwrappedPlSql, 
@"FUNCTION get_latin1_string RETURN VARCHAR2 IS
  V_STRING VARCHAR2(100);
BEGIN
  V_STRING := ""Café"";
  RETURN V_STRING;
END GET_LATIN1_STRING;
");
        }

        [TestMethod]
        public void TestFunctions()
        {
            string[] wrapped = Directory.GetFiles(@"C:\Users\Robert\Documents\MCE\Projets\GitHub\PlSqlUnwrapper\PlSqlUnwrapper.UnitTests\TestFiles\Functions", "*.wrapped.txt");

            foreach (string file in wrapped)
            {
                string wrappedPlSql = File.ReadAllText(file);
                string unwrappedPlSql = PlSqlUnwrapper.Unwrap(wrappedPlSql, Encoding.Latin1).Replace("\n", "\r\n");
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
                string unwrappedPlSql = PlSqlUnwrapper.Unwrap(wrappedPlSql, Encoding.Latin1).Replace("\n", "\r\n");
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
                string unwrappedPlSql = PlSqlUnwrapper.Unwrap(wrappedPlSql, Encoding.Latin1).Replace("\n", "\r\n");
                string plSql = File.ReadAllText(file.Replace(".wrapped", ""));
                Assert.AreEqual(unwrappedPlSql, plSql);
            }
        }

        [GeneratedRegex(@"CREATE\s+(OR\s+REPLACE\s+)?(EDITIONABLE\s+)?(PROCEDURE|FUNCTION)\s+""(\w+)""\.""(\w+)""")]
        private static partial Regex FunctionRegex();

        [GeneratedRegex(@"CREATE\s+(OR\s+REPLACE\s+)?(PROCEDURE|FUNCTION)\s+(\w+)")]
        private static partial Regex FunctionRegex2();

        [TestMethod]
        public void CreateFunctionTests()
        {
            string plsqlWrapped = File.ReadAllText(@"D:\Conversion\Parser\OracleForms\Database\Functions.txt");
            string[] wrapped = plsqlWrapped.Split("\r\n/\r\n");
            Dictionary<string, string> wrappedElements = [];

            foreach (string wrap in wrapped)
            {
                int pos = wrap.IndexOf("wrapped");

                if (pos > 0)
                {
                    Match m = FunctionRegex().Match(wrap);
                    string name = m.Groups[5].ToString();
                    string code = $"{m.Groups[3]} {name} " + wrap[pos..].Trim();
                    wrappedElements[name.ToUpperInvariant()] = code;
                }
            }

            string plsqlUnwrapped = File.ReadAllText(@"D:\Conversion\Parser\OracleForms\Database\Functions oddgen.txt");
            string[] unwrapped = plsqlUnwrapped.Split("\r\n/\r\n");
            Dictionary<string, string> unwrappedElements = [];

            foreach (string unwrap in unwrapped)
            {
                if (string.IsNullOrWhiteSpace(unwrap))
                    continue;

                Match m = FunctionRegex2().Match(unwrap);
                string name = m.Groups[3].ToString();
                unwrappedElements[name.ToUpperInvariant()] = unwrap[m.Groups[2].Index..];
            }

            foreach (string name in wrappedElements.Keys)
            {
                if (unwrappedElements.TryGetValue(name, out string? value))
                {
                    File.WriteAllText($@"C:\Users\Robert\Documents\MCE\Projets\GitHub\PlSqlUnwrapper\PlSqlUnwrapper.UnitTests\TestFiles\Functions\{name}.wrapped.txt", wrappedElements[name]);
                    File.WriteAllText($@"C:\Users\Robert\Documents\MCE\Projets\GitHub\PlSqlUnwrapper\PlSqlUnwrapper.UnitTests\TestFiles\Functions\{name}.txt", value);
                }
            }
        }

        [GeneratedRegex(@"CREATE\s+(OR\s+REPLACE\s+)?(EDITIONABLE\s+)?(PROCEDURE|FUNCTION)\s+""(\w+)""\.""(\w+)""")]
        private static partial Regex ProcedureRegex();

        [GeneratedRegex(@"CREATE\s+(OR\s+REPLACE\s+)?(PROCEDURE|FUNCTION)\s+(\w+)")]
        private static partial Regex ProcedureRegex2();

        [TestMethod]
        public void CreateProcedureTests()
        {
            string plsqlWrapped = File.ReadAllText(@"D:\Conversion\Parser\OracleForms\Database\Procedures.txt");
            string[] wrapped = plsqlWrapped.Split("\r\n/\r\n");
            Dictionary<string, string> wrappedElements = [];

            foreach (string wrap in wrapped)
            {
                int pos = wrap.IndexOf("wrapped");

                if (pos > 0)
                {
                    Match m = ProcedureRegex().Match(wrap);
                    string name = m.Groups[5].ToString();
                    string code = $"{m.Groups[3]} {name} " + wrap[pos..].Trim();
                    wrappedElements[name.ToUpperInvariant()] = code;
                }
            }

            string plsqlUnwrapped = File.ReadAllText(@"D:\Conversion\Parser\OracleForms\Database\Procedures oddgen.txt");
            string[] unwrapped = plsqlUnwrapped.Split("\r\n/\r\n");
            Dictionary<string, string> unwrappedElements = [];

            foreach (string unwrap in unwrapped)
            {
                if (string.IsNullOrWhiteSpace(unwrap))
                    continue;

                Match m = ProcedureRegex2().Match(unwrap);
                string name = m.Groups[3].ToString();
                unwrappedElements[name.ToUpperInvariant()] = unwrap[m.Groups[2].Index..];
            }

            foreach (string name in wrappedElements.Keys)
            {
                if (unwrappedElements.TryGetValue(name, out string? value))
                {
                    File.WriteAllText($@"C:\Users\Robert\Documents\MCE\Projets\GitHub\PlSqlUnwrapper\PlSqlUnwrapper.UnitTests\TestFiles\Procedures\{name}.wrapped.txt", wrappedElements[name]);
                    File.WriteAllText($@"C:\Users\Robert\Documents\MCE\Projets\GitHub\PlSqlUnwrapper\PlSqlUnwrapper.UnitTests\TestFiles\Procedures\{name}.txt", value);
                }
            }
        }

        [GeneratedRegex(@"wrapped")]
        private static partial Regex PackageBodyRegex();

        [GeneratedRegex(@"CREATE\s+OR\s+REPLACE\s+EDITIONABLE\s+PACKAGE BODY\s+""(\w+)""")]
        private static partial Regex PackageBodyRegex2();

        [GeneratedRegex(@"CREATE\s+(OR\s+REPLACE\s+)?(PROCEDURE|FUNCTION|PACKAGE BODY)\s+(\w+)")]
        private static partial Regex PackageBodyRegex3();

        [TestMethod]
        public void CreatePackageBodyTests()
        {
            string plsqlWrapped = File.ReadAllText(@"D:\Conversion\Parser\OracleForms\Database\Package bodies.txt");
            string[] wrapped = plsqlWrapped.Split("\r\n/\r\n");
            Dictionary<string, string> wrappedElements = [];

            foreach (string wrap in wrapped)
            {
                Match m = PackageBodyRegex().Match(wrap);

                if (m.Success)
                {
                    Match m1 = PackageBodyRegex2().Match(wrap);
                    if (m1.Success)
                    {
                        string name = m1.Groups[1].ToString();
                        string code = wrap[(m.Groups[0].Index + m.Groups[0].Length)..];
                        wrappedElements[name.ToUpperInvariant()] = $"PACKAGE BODY {name} wrapped {code}";
                    }
                }
            }

            string plsqlUnwrapped = File.ReadAllText(@"D:\Conversion\Parser\OracleForms\Database\Package bodies oddgen.txt");
            string[] unwrapped = plsqlUnwrapped.Split("\r\n/\r\n");
            Dictionary<string, string> unwrappedElements = [];

            foreach (string unwrap in unwrapped)
            {
                if (string.IsNullOrWhiteSpace(unwrap))
                    continue;

                Match m = PackageBodyRegex3().Match(unwrap);
                string name = m.Groups[3].ToString();
                unwrappedElements[name.ToUpperInvariant()] = unwrap[m.Groups[2].Index..];
            }

            foreach (string name in wrappedElements.Keys)
            {
                if (unwrappedElements.TryGetValue(name, out string? value))
                {
                    File.WriteAllText($@"C:\Users\Robert\Documents\MCE\Projets\GitHub\PlSqlUnwrapper\PlSqlUnwrapper.UnitTests\TestFiles\PackageBodies\{name}.wrapped.txt", wrappedElements[name]);
                    File.WriteAllText($@"C:\Users\Robert\Documents\MCE\Projets\GitHub\PlSqlUnwrapper\PlSqlUnwrapper.UnitTests\TestFiles\PackageBodies\{name}.txt", value);
                }
            }
        }
    }
}