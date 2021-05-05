using System;

namespace ConsoleNSwag
{
    class Program
    {
        static void Main(string[] args)
        {
            Console.WriteLine("Welcome to Nswag Generated Client");

            // Created object of class ValuesClient  

            var testClient = new ValueTuple();

            // Call GetAsync Values API  
            var getresult = testClient.GetAsync(1).GetAwaiter().GetResult();

            // Call GetAllAsync Values API  
            var getAllresult = testClient.GetAllAsync().GetAwaiter().GetResult();
        }
    }
}
