namespace ChakraCore.NET
{
    public class ChakraCoreWrapper
    {
        public JavaScriptErrorCode CreateRuntime(JavaScriptRuntimeAttributes attributes, out JavaScriptRuntime runtime)
        {
            return Native.JsCreateRuntime(attributes, null, out runtime);
        }

        public JavaScriptErrorCode DisposeRuntime(JavaScriptRuntime runtime)
        {
            return Native.JsDisposeRuntime(runtime);
        }
    }
}
