// Factory method pattern
using System.Text;
namespace FactoryPattern
{
    /**********************************************/
    // public enum CoordinateSystem
    // {
    //     Cartesian,
    //     Polar
    // }
    public class Point
    {
        private double x, y;
        private Point(double x, double y)
        {
            this.x = x;
            this.y = y;
        }
        public override string ToString()
        {
            return $"{nameof(x)}: {x}, {nameof(y)}: {y}";
        }
        public class PointFactory
        {
            public Point NewCartesianPoint(double x, double y)
            {
                return new Point(x,y);
            }
            public Point NewPolarPoint(double rho, double theta)
            {
                return new Point(rho * Math.Cos(theta),rho * Math.Sin(theta));
            }
        }
        public static PointFactory Factory = new PointFactory();
    }

/**********************************************/    
public interface ITheme
{
    string TextColor {get;}
    string BgrColor {get;}
}
class LightTheme: ITheme
{
    public string TextColor => "black";
    public string BgrColor => "white";
}
class DarkTheme: ITheme
{
    public string TextColor => "white";
    public string BgrColor => "dark gray";
}
public class TrackingThemeFactory
{
    private readonly List<WeakReference<ITheme>> themes = new();
    public ITheme CreateTheme(bool dark)
    {
        ITheme theme = dark ? new DarkTheme() : new LightTheme();
        themes.Add(new WeakReference<ITheme>(theme));
        return theme;
    }
    public string Info
    {
        get
        {
            var sb = new StringBuilder();
            foreach( var reference in themes)
            {
                if(reference.TryGetTarget(out var theme))
                {
                    bool dark = theme is DarkTheme;
                        sb.Append(dark?"Dark":"Light")
                            .AppendLine(" theme");
                }
            }
            return sb.ToString();
        }
    }
}
public class Ref<T> where T : class
{
    public T Value;
    public Ref(T value)
    {
        Value = value;
    }
}
public class ReplaceableThemeFactory
{
    private readonly List<WeakReference<Ref<ITheme>>> themes = new();
    private ITheme CreateThemeImpl(bool dark)
    {
        return dark ? new DarkTheme() : new LightTheme();
    }
    public Ref<ITheme> CreateTheme(bool dark)
    {
        var r = new Ref<ITheme>(CreateThemeImpl(dark));
        themes.Add(new(r));
        return r;
    }
    public void ReplaceTheme(bool dark)
    {
        foreach(var wr in themes)
        {
            if(wr.TryGetTarget(out var reference))
            {
                reference.Value = CreateThemeImpl(dark);
            }
        }
    }
}
/**********************************************/    
 public class Program
    {
        static void Main(string[] args)
        {
            var point = Point.Factory.NewPolarPoint(1.0,Math.PI/2);
            Console.WriteLine(point);

            var factory =  new TrackingThemeFactory();
            var theme1 = factory.CreateTheme(false);
            var theme2 = factory.CreateTheme(true);
            Console.WriteLine(factory.Info);

            var factory2 = new ReplaceableThemeFactory();
            var magicTheme = factory2.CreateTheme(true);
            Console.WriteLine(magicTheme.Value.BgrColor);
            factory2.ReplaceTheme(false);
            Console.WriteLine(magicTheme.Value.BgrColor);
        }
    }
}
