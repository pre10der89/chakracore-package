using System;
using System.Collections.Generic;
using System.Reflection;
using System.Text;

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
                var chakraCoreDriver = new ChakraCoreWrapper();

                JavaScriptRuntime jsRuntime;

                var createErrorCode = chakraCoreDriver.CreateRuntime(JavaScriptRuntimeAttributes.None, out jsRuntime);

                if (createErrorCode == JavaScriptErrorCode.NoError)
                {
                    chakraCoreDriver.DisposeRuntime(jsRuntime);
                }

                success = true;
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
