namespace OpenRasta.Tests.Unit.Codecs.MultipartFormDataCodec
{
    using OpenRasta.Extensions;

    public static class Scenarios
    {        
        public static string OneFieldOneFile = @"
preamble



--boundary42
Content-Disposition: form-data; name=""username""

johndoe
--boundary42
Content-Type: text/plain;charset=utf-8
Content-Disposition: form-data; name=""document"";filename=""document.txt""

Content of a document
--boundary42--
";
        public static string TwoFields =
            @"
--boundary42
Content-Disposition: form-data; name=""username""

johndoe
--boundary42
Content-Disposition: form-data; name=""dateofbirth""

12/10/2001
--boundary42--
";
        public static string TwoFieldsComposedNames =
            @"
preamble
--boundary42
Content-Disposition: form-data;name=""Customer.Username""

johndoe
--boundary42
Content-Disposition: form-data; name=""Customer.DateOfBirth.Year""

2001
--boundary42
Content-Type: application/x-www-form-urlencoded
Content-Disposition: form-data; name=""additions""

oneplusone=two&oneplustwo=three
--boundary42--
";
        public static string NestedContentTypes = 
            @"
--boundary42
COntent-Disposition: form-data; name=""Customer.DateOfBirth""
Content-Type: application/x-www-form-urlencoded

year=2001&month=12&day=10
--boundary42--
";
        public static string TelephoneFieldUtf8Quoted = @"
--boundary42
Content-Disposition: form-data; name=""=?UTF-8?Q?T=C3=A9l=C3=A9phone?=""

077 777 7777
--boundary42--";
        public static string TelephoneFieldUtf8Base64 = @"
--boundary42
Content-Disposition: form-data; name=""=?UTF-8?B?VMOpbMOpcGhvbmU=?=""

077 777 7777
--boundary42--";
        public static string TelephoneFieldIso88591Quoted = @"
--boundary42
Content-Disposition: form-data; name=""=?ISO-8859-1?Q?T=E9l=E9phone?=""

077 777 7777
--boundary42--";
        public static string LargeField = @"
--boundary42
Content-Disposition: form-data; name=""field""

{0}END
--boundary42--".With(new string('-',85000));

        public static string FileField = @"
--boundary42
Content-Disposition: form-data; name=""file"";filename=""temp.txt""
Content-Type: application/octet-stream

{0}
--boundary42--".With(new string('-', 85000));
    }
}