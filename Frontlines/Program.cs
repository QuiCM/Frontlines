using HQ;
using HQ.Attributes;
using HQ.Interfaces;
using System;

namespace Frontlines
{
    [CommandClass]
    public class RollCommand
    {
        [CommandExecutor("Rolls a dice with a number of sides", "roll")]
        public object Execute(IContextObject ctx, int sides = 6)
        {
            return new Random().Next(1, sides + 1);
        }
    }

    [CommandClass]
    public class RandomCommand
    {
        [CommandExecutor("Rolls a random number", "random")]
        public object Execute(IContextObject ctx, int min = 1, int max = 100)
        {
            return new Random().Next(min, max + 1);
        }

        [CommandExecutor("Rolls a random number", "float random")]
        public object ExecuteFloat(IContextObject ctx, float min = 1.0f, float max = 100.0f)
        {
            return new Random().NextDouble() * (max - min + 1) + min; 
        }

        [SubcommandExecutor(nameof(Execute), "Returns a random name", "name")]
        public object SubExecutor(IContextObject ctx)
        {
            string[] names = new[] { "jeff", "john", "james", "jim", "dave", "doug", "donald", "ryan", "ross", "ray" };
            int num = new Random().Next(0, 10);

            return names[num];
        }
    }

    public class ExampleApplication
    {
        public static void Main(string[] args)
        {
            IContextObject context = null; //null is used in this example

            using (CommandRegistry registry = new CommandRegistry(new RegistrySettings()))
            {
                registry.AddCommand(typeof(RollCommand));
                registry.AddCommand(typeof(RandomCommand));
                
                string command;
                while ((command = Console.ReadLine()) != "quit")
                {
                    registry.HandleInput(command, context, (r, o) => { Console.WriteLine($"Result: {r} - Output: {o}"); });
                }
            }
        }
    }
}