using System;

namespace uninst
{
    class Program
    {
        static void Main(string[] args)
        {
            try{
                Clear.Run();
            }
            catch (Exception)
            {
                Console.WriteLine("请使用管理员权限运行该程序");
                Console.WriteLine("若仍有异常，请联系管理员");
            }
        }
    }
}
