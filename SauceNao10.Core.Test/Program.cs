using System;
using SauceNao10.Core.Services;

namespace SauceNao10.Core.Test
{
    class Program
    {
        static void Main(string[] args)
        {
            var service = new SauceNaoService();
            var results = service.GetSauceAsync(new Uri("https://pbs.twimg.com/media/EJFh7svVUAIUiDQ.jpg:large"));
            foreach (var result in results.Result)
            {
                Console.WriteLine(result);
            }
            Console.ReadLine();
        }
    }
}
