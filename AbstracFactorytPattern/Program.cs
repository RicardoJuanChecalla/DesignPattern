﻿// See https://aka.ms/new-console-template for more information
using System.Threading.Tasks;
namespace AbstracFactorytPattern
{
    public class Foo
    {
        private Foo()
        {
            
        }
        private async Task<Foo> InitAsync()
        {
            await Task.Delay(1000);
            return this;
        }
        public static Task<Foo> CreateAsync()
        {
            var result = new Foo();
            return result.InitAsync();
        }
    }
public interface IHotDrink
{
    void Consume();
}
internal class Tea : IHotDrink
{
    public void Consume()
    {
        Console.WriteLine("This tea is nice but I'd prefer it with milk");
    }
}
internal class Coffee : IHotDrink
{
    public void Consume()
    {
        Console.WriteLine("This coffee is sensational");
    }
}
public interface IHotDrinkFactory
{
    IHotDrink Prepare(int amount);
}
internal class TeaFactory : IHotDrinkFactory
{
    public IHotDrink Prepare(int amount)
    {
        Console.WriteLine($"Put in a tea bag, boil water, pour {amount} ml, add lemon, enjoy!");
        return new Tea();
    }
}
internal class CoffeeFactory : IHotDrinkFactory
{
    public IHotDrink Prepare(int amount)
    {
        Console.WriteLine($"Grind some beans, boil water, pour  {amount} ml, add cream and sugar, enjoy!");
        return new Coffee();
    }
}
public class HotDrinkMachine
{
    // public enum AvailableDrink
    // {
    //     Coffee, Tea
    // }
    // private Dictionary<AvailableDrink, IHotDrinkFactory> factories = new Dictionary<AvailableDrink, IHotDrinkFactory>();
    // public HotDrinkMachine()
    // {
    //     foreach(AvailableDrink drink in Enum.GetValues(typeof(AvailableDrink)))
    //     {
    //         var factory = (IHotDrinkFactory)Activator.CreateInstance(
    //             Type.GetType("AbstracFactorytPattern."+Enum.GetName(typeof(AvailableDrink),drink)+"Factory"));
    //         factories.Add(drink, factory);    
    //     }
    // }
    // public IHotDrink MakeDrink(AvailableDrink drink, int amount)
    // {
    //     return factories[drink].Prepare(amount);
    // }
    /***************************************************************/
    private List<Tuple<string,IHotDrinkFactory>> factories = new List<Tuple<string, IHotDrinkFactory>>();
    public HotDrinkMachine()
    {
        foreach(var t in typeof(HotDrinkMachine).Assembly.GetTypes())
        {
            if(typeof(IHotDrinkFactory).IsAssignableFrom(t) && !t.IsInterface)
            {
                factories.Add(Tuple.Create(
                    t.Name.Replace("Factory",string.Empty),
                    (IHotDrinkFactory)Activator.CreateInstance(t)
                    ));
            }
        }
    }
    public IHotDrink MakeDrink()
    {
        Console.WriteLine("Available drinks:");
        for (int index = 0; index < factories.Count; index++)
        {
            var tuple = factories[index];
            Console.WriteLine($"{index}: {tuple.Item1}");            
        }
        while (true)
        {
            string s;
            if((s = Console.ReadLine())!=null
                && int.TryParse(s, out int i)
                && i>=0
                && i< factories.Count)
            {
                Console.Write("Specify amount: ");
                s = Console.ReadLine();
                if( s!=null
                    && int.TryParse(s, out int amount)
                    && amount > 0
                )
                {
                    return factories[i].Item2.Prepare(amount);
                }
            }
            Console.WriteLine("Incorrect input, try again!");    
        }
    }
}
 public class Program
    {
        public static async Task Main(string[] args)
        {
            var x = await Foo.CreateAsync();
            // /***********************************/
            // var machine = new HotDrinkMachine();
            // var drink = machine.MakeDrink(HotDrinkMachine.AvailableDrink.Coffee,100);
            // drink.Consume();   
            var machine = new HotDrinkMachine();
            var drink = machine.MakeDrink();
            drink.Consume();
        }
    }
}

