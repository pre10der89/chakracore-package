using System;
using System.Runtime.InteropServices;

namespace ChakraCore.NET
{
    public sealed class JavaScriptEngine : IDisposable
    {
        #region Private Fields

        private readonly JavaScriptRuntime runtime;
        private readonly JavaScriptContext context;

        JavaScriptSourceContext currentSourceContext = JavaScriptSourceContext.FromIntPtr(IntPtr.Zero);

        #endregion

        #region Constructor(s)

        public JavaScriptEngine()
            : this(JavaScriptRuntimeAttributes.None)
        {
        }

        public JavaScriptEngine(JavaScriptRuntimeAttributes attributes)
        {
            runtime = this.CreateRuntime(attributes);
            context = this.CreateContext(runtime);
        }

        ~JavaScriptEngine()
        {
            // Finalizer calls Dispose(false)  
            Dispose(false);
        }

        #endregion

        #region IDisposable Members

        private bool disposed = false;

        public void Dispose()
        {
            Dispose(true);
            GC.SuppressFinalize(this);
        }

        private void Dispose(bool disposing)
        {
            if (disposed) return;

            if (disposing)
            {
                try
                {
                    // Reset JavaScriptContext
                    JavaScriptErrorCode errorCode = Native.JsSetCurrentContext(JavaScriptContext.Invalid);

                    if (errorCode != JavaScriptErrorCode.NoError)
                    {
                        System.Diagnostics.Debug.WriteLine("Failed to reset JavaScriptContext with error [{0}]", errorCode);
                    }

                    // Dispose runtime
                    errorCode = Native.JsDisposeRuntime(runtime);

                    if (errorCode != JavaScriptErrorCode.NoError)
                    {
                        System.Diagnostics.Debug.WriteLine("Failed to dispose javascript runtime with error [{0}]", errorCode);
                    }

                    disposed = true;
                }
                catch (Exception ex)
                {
                    System.Diagnostics.Debug.WriteLine("An exception occurred when disposing the JavaScriptEngine with error [{0}]", ex.Message);
                }
            }
        }

        #endregion

        #region IJavaScriptEngine Members

        public JavaScriptValue RunScript(string script, string sourceURL = "")
        {
            JavaScriptValue result;

            try
            {
                JavaScriptErrorCode errorCode = Native.JsRunScript(script, currentSourceContext++, sourceURL, out result);

                if (errorCode != JavaScriptErrorCode.NoError)
                {
                    string errorMessage = string.Format("Failed to run script with error [{0}]", errorCode);

                    System.Diagnostics.Debug.WriteLine(errorMessage);

                    throw new Exception(errorMessage);
                }
            }
            catch (Exception ex)
            {
                System.Diagnostics.Debug.WriteLine("An exception occurred while executing script with error [{0}]", ex.Message);

                throw;
            }

            return result;
        }

        public string ConvertValueToString(JavaScriptValue value)
        {
            string result = null;

            try
            {
                // Convert your script result to String in JavaScript; redundant if your script returns a String
                JavaScriptValue resultJSString;

                JavaScriptErrorCode errorCode = Native.JsConvertValueToString(value, out resultJSString);

                if (errorCode != JavaScriptErrorCode.NoError)
                {
                    string errorMessage = string.Format("Failed to convert value to JsString with error [{0}]", errorCode);

                    System.Diagnostics.Debug.WriteLine(errorMessage);

                    throw new Exception(errorMessage);
                }

                // Project script result in JS back to C#.
                IntPtr resultPtr;
                UIntPtr stringLength;

                errorCode = Native.JsStringToPointer(resultJSString, out resultPtr, out stringLength);

                if (errorCode != JavaScriptErrorCode.NoError)
                {
                    string errorMessage = string.Format("Failed to convert JsString to Pointer with error [{0}]", errorCode);

                    System.Diagnostics.Debug.WriteLine(errorMessage);

                    throw new Exception(errorMessage);
                }

                result = Marshal.PtrToStringUni(resultPtr);
            }
            catch (Exception ex)
            {
                System.Diagnostics.Debug.WriteLine("An exception occurred while converting JavaScriptValue with error [{0}]", ex.Message);

                throw;
            }

            return result;
        }

        #endregion

        #region Private Methods

        private JavaScriptRuntime CreateRuntime(JavaScriptRuntimeAttributes attributes)
        {
            JavaScriptRuntime runtime;

            try
            {
                // Create a runtime. 
                JavaScriptErrorCode errorCode = Native.JsCreateRuntime(attributes, null, out runtime);
                
                if ( errorCode != JavaScriptErrorCode.NoError )
                {
                    string errorMessage = string.Format("Failed to create javascript runtime with error [{0}]", errorCode);

                    System.Diagnostics.Debug.WriteLine(errorMessage);

                    throw new Exception(errorMessage);
                }
            }
            catch(Exception ex)
            {
                System.Diagnostics.Debug.WriteLine("An exception occurred when creating the javascript runtime with error [{0}]", ex.Message);

                throw;
            }

            return runtime;
        }

        private JavaScriptContext CreateContext(JavaScriptRuntime runtime)
        {
            JavaScriptContext context;

            try
            {
                // Create an execution context. 
                JavaScriptErrorCode errorCode = Native.JsCreateContext(runtime, out context);

                if (errorCode != JavaScriptErrorCode.NoError)
                {
                    string errorMessage = string.Format("Failed to create JavaScriptContext with error [{0}]", errorCode);

                    System.Diagnostics.Debug.WriteLine(errorMessage);

                    throw new Exception(errorMessage);
                }

                // Now set the execution context as being the current one on this thread.
                errorCode = Native.JsSetCurrentContext(context);

                if (errorCode != JavaScriptErrorCode.NoError)
                {
                    string errorMessage = string.Format("Failed to set current JavaScriptContext with error [{0}]", errorCode);

                    System.Diagnostics.Debug.WriteLine(errorMessage);

                    throw new Exception(errorMessage);
                }
            }
            catch (Exception ex)
            {
                System.Diagnostics.Debug.WriteLine("An exception occurred when creating the javascript context with error [{0}]", ex.Message);

                throw;
            }

            return context;
        }

        #endregion        
    }
}
