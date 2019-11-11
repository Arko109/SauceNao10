using System;
using SauceNao10.Core.Services;

namespace SauceNao10.Core.Test
{
    class Program
    {
        static void Main(string[] args)
        {
            var service = new SauceNaoService();
            try
            {
                var results = service.GetSauceAsync(new Uri("https://bing.com")).Result;
                //var results = service.GetSauceAsync(new Uri("https://pbs.twimg.com/media/EJFh7svVUAIUiDQ.jpg:large")).Result;
                foreach (var result in results)
                {
                    Console.WriteLine(result);
                }
            }
            catch (Exception e)
            {
                Console.WriteLine(e);
            }
            finally
            {
                Console.ReadLine();
            }
        }
    }
}
