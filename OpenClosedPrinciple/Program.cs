using System;
using System.Collections;
/*
Open/Closed Principle) establece que «una entidad de software (clase, módulo, función, etc.) 
debe quedarse abierta para su extensión, pero cerrada para su modificación». 
Es decir, se debe poder extender el comportamiento de tal entidad pero sin modificar su código fuente.
*/
namespace OpenClosedPrinciple
{
    public enum Color
    {
        Red, Green, Blue
    }
    public enum Size
    {
        Small, Medium, Large, Yuge
    }
    public class Product
    {
        public string Name {get; private set;}
        public Color Color {get; private set;}
        public Size Size {get; private set;}
        public Product(string name, Color color, Size size)
        {
            if (name == null)
            {
                throw new ArgumentNullException(paramName: nameof(name));
            }
            Name = name;
            Color = color;
            Size = size;
        }
    }
    public class ProductFilter
    {
        public IEnumerable<Product> FilterBySize(IEnumerable<Product> products, Size size)
        {
            foreach (var p in products)
                if (p.Size == size)
                    yield return p;
        }
        public IEnumerable<Product> FilterByColor(IEnumerable<Product> products, Color color)
        {
            foreach (var p in products)
                if (p.Color == color)
                    yield return p;
        }
        public IEnumerable<Product> FilterBySizeAndColor(IEnumerable<Product> products, Size size, Color color)
        {
            foreach (var p in products)
                if (p.Size == size && p.Color == color)
                    yield return p;
        }
    }
    public interface ISpecification<T>
    {
        bool IsSatisfield(T t);
    }
    public class AndSpecification<T> : ISpecification<T>
    {
        public ISpecification<T> First {get; private set;}
        public ISpecification<T> Second {get; private set;}
        public AndSpecification(ISpecification<T> first, ISpecification<T> second)
        {
            this.First = first;
            this.Second = second;
        }
        public bool IsSatisfield(T t)
        {
            return First.IsSatisfield(t) && Second.IsSatisfield(t);
        }
    }
    public interface IFilter<T>
    {
        IEnumerable<T> Filter(IEnumerable<T> items, ISpecification<T> spec);
    }
    public class ColorSpecification : ISpecification<Product>
    {
        public Color Color {get; private set;}
        public ColorSpecification(Color color)
        {
            this.Color = color;
        }
        public bool IsSatisfield(Product t)
        {
            return t.Color == Color;
        }
    }
    public class SizeSpecification : ISpecification<Product>
    {
        public Size Size {get; private set;}
        public SizeSpecification(Size size)
        {
            this.Size = size;
        }
        public bool IsSatisfield(Product t)
        {
            return t.Size == Size;
        }
    }

    public class BetterFilter : IFilter<Product>
    {
        public IEnumerable<Product> Filter(IEnumerable<Product> items, ISpecification<Product> spec)
        {
            foreach(var i in items)
                if(spec.IsSatisfield(i))
                    yield return i;
        }
    }
    public class Demo
    {
        static void Main(string[] args)
        {
            var apple = new Product("Apple", Color.Green, Size.Small);
            var tree = new Product("Tree", Color.Green, Size.Large);
            var house = new Product("House", Color.Blue, Size.Large);
            Product[] products = { apple, tree, house };
            var pf = new ProductFilter();
            Console.WriteLine("Green products (old):");
            foreach (var p in pf.FilterByColor(products, Color.Green))
            {
                Console.WriteLine($" - {p.Name} is green");
            }
            var bf = new BetterFilter();
            Console.WriteLine("Green products (new):");
            foreach(var p in bf.Filter(products, new ColorSpecification(Color.Green)))
            {
                Console.WriteLine($" - {p.Name} is green");
            }
            Console.WriteLine("Large blue items");
            foreach(var p  in bf.Filter(products, new AndSpecification<Product>(new ColorSpecification(Color.Blue),new SizeSpecification(Size.Large))))
            {
                Console.WriteLine($" - {p.Name} is big and blue");
            }

        }
    }
}