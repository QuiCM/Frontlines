using HQ;
using HQ.Attributes;
using HQ.Interfaces;
using System;
using System.Threading;

namespace Frontlines
{
    [CommandClass]
    public class RollCommand
    {
        [CommandExecutor]
        public object Execute(IContextObject ctx, int sides = 6)
        {
            return new Random().Next(1, sides + 1);
        }
    }

    [CommandClass]
    public class RandomCommand
    {
        [CommandExecutor]
        public object Execute(IContextObject ctx, int min = 1, int max = 100)
        {
            return new Random().Next(min, max + 1);
        }
    }

    public class ExampleApplication
    {

        public static void Main(string[] args)
        {
            IContextObject context = null; //null is used in this example

            using (CommandRegistry registry = new CommandRegistry(new RegistrySettings()))
            {
                registry.AddCommand(typeof(RollCommand), new RegexString[] { "roll" }, "A roll command!");
                registry.AddCommand(typeof(RandomCommand), new RegexString[] { "random" }, "A roll command!");
                registry.OnInputResult += Registry_OnInputResult;

                for (int i = 0; i < 500; i++)
                {
                    Thread thread = new Thread(() => { registry.HandleInput($"random 1 {i}", context); });
                    thread.Start();
                }

                string command;
                while ((command = Console.ReadLine()) != "quit")
                {
                    registry.HandleInput(command, context);
                }
            }
        }

        private static void Registry_OnInputResult(object sender, InputResultEventArgs e)
        {
            Console.WriteLine($"ID: {e.ID} - Result: {e.Result} - Output: {e.Output}");
        }
    }
}