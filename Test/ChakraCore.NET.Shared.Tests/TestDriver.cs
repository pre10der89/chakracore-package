using ChakraCore.NET;
using System;
using System.Reflection;

namespace ChakraCore.NET.Tests
{
    public class TestDriver
    {
        public bool RunSanityCheck(out string result)
        {
            bool success = false;
            string error = string.Empty;

            result = "Success";

            try
            {
                using (var javaScriptEngine = new JavaScriptEngine())
                {
                    // The Script...
                    string script = "(()=>{return \'Hello world!\';})()";

                    var javaScriptValue = javaScriptEngine.RunScript(script);

                    var stringValue = javaScriptEngine.ConvertValueToString(javaScriptValue);

                    if ( string.Compare("Hello world!", stringValue, false) == 0 )
                    {
                        success = true;
                    }
                }
            }
            catch (Exception ex)
            {
                error = ex.Message + Environment.NewLine + ex.StackTrace;
            }

            if (success)
            {
                result = string.Format("SUCCESS -----------> {0}", GetExecutingAssemblyName());
            }
            else
            {
                result = string.Format("FAILED -----------> {0}{1}{2}", GetExecutingAssemblyName(), Environment.NewLine + Environment.NewLine, error);
            }

            return success;
        }

        private string GetExecutingAssemblyName()
        {
#if !WINDOWS_UWP
            Assembly currentAssem = Assembly.GetExecutingAssembly();

            return currentAssem.Location;
#else
            return "UWP Application";
#endif
        }
    }
}
