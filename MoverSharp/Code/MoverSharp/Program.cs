using XAYAWrapper;
using System;

namespace MoverSharp
{
    class Program
    {
        static string dPath = AppDomain.CurrentDomain.BaseDirectory;
        static int chainType = 0;
        static string storageType = "memory";
        static string FLAGS_xaya_rpc_url = "xayagametest:xayagametest@127.0.0.1:8396";
        static string host_s = "http://127.0.0.1";
        static string gamehostport_s = "8900";

        static void Main(string[] args)
        {
            string functionResult = "";
            XayaWrapper wrapper = new XayaWrapper(dPath, host_s, gamehostport_s, ref functionResult, CallbackFunctions.initialCallbackResult, CallbackFunctions.forwardCallbackResult, CallbackFunctions.backwardCallbackResult);

            Console.WriteLine(functionResult);
            Console.ReadLine();

            wrapper.Connect(dPath, FLAGS_xaya_rpc_url, gamehostport_s, chainType.ToString(), storageType, "mv", dPath + "\\..\\XayaStateProcessor\\database\\", dPath + "\\..\\XayaStateProcessor\\glogs\\");

            Console.WriteLine("Done");
            Console.ReadLine();
        }
    }
}
