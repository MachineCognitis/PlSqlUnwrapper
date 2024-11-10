# Welcome to the PL/SQL Unwrapper for .NET

This project provides a `PlSqlUnwrapper` class that can be used to unwrap wrapped PL/SQL code.
The Unwrap function takes a string of wrapped PL/SQL code, the text encoding of the original PL/SQL code,
and returns the unwrapped PL/SQL code. The function has been tested with Oracle 19c wrapped PL/SQL code.
It most likely works with other versions of Oracle as well.

### NuGet Package

- To come.

### Example

Here is an example of how to use the `PlSqlUnwrapper` class to unwrap a wrapped PL/SQL function:

```csharp
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

        string unwrappedPlSql = PlSqlUnwrapper.Unwrap(wrappedPlSql, Encoding.Latin1).Replace("\n", "\r\n").TrimEnd('\0');

        Assert.AreEqual(unwrappedPlSql, 
@"FUNCTION get_latin1_string RETURN VARCHAR2 IS
V_STRING VARCHAR2(100);
BEGIN
V_STRING := ""Café"";
RETURN V_STRING;
END GET_LATIN1_STRING;
");
    }
```





