// Dependency inversion principle

namespace DependencyInversionPrinciple
{
    public enum RelationShip
    {
        Parent,
        Child,
        Sibling
    }
    public class Person
    {
        public string Name {get; set;}
        // public DateTime DateOfBirth;
        public Person()
        {
            Name = string.Empty;
        }
    }
    public interface IRelationShipBrowser
    {
        IEnumerable<Person> FindAllChildrenOf(string name);
    }
    //low level
    public class RelationShips : IRelationShipBrowser
    {
        private List<(Person, RelationShip, Person)> Relations = new List<(Person, RelationShip, Person)>();        
        public void AddParentAndChild(Person parent, Person child)
        {
            Relations.Add((parent, RelationShip.Parent, child));
            Relations.Add((child, RelationShip.Parent, parent));
        }
        //public List<(Person, RelationShip, Person)> Relations1 => Relations0;
        public IEnumerable<Person> FindAllChildrenOf(string name)
        {
            return Relations.Where(
                x => x.Item1.Name == name && 
                     x.Item2 == RelationShip.Parent
            ).Select(r=>r.Item3);
            // foreach(var r in Relations.Where(x=>x.Item1.Name == name && x.Item2 == RelationShip.Parent))
            // {
            //     yield return r.Item3;
            // }
        }
    }
 public class Research
    {
        // public Research(RelationShips relationShips)
        // {
        //     var relations = relationShips.;
        //     foreach(var r in relations.Where(x=>x.Item1.Name == "Ricardo" && x.Item2 == RelationShip.Parent))
        //     {
        //         Console.WriteLine($"Ricardo has a child called {r.Item3.Name}");
        //     }
        // }
        public Research(IRelationShipBrowser browser)
        {
            foreach(var p in browser.FindAllChildrenOf("Ricardo"))
                Console.WriteLine($"Ricardo has a child called {p.Name}");
        }
        static void Main(string[] args)
        {
            var parent =  new Person{ Name = "Ricardo"};
            var child1 =  new Person{ Name = "Blanca"};
            var child2 =  new Person{ Name = "Negra"};
            var child3 =  new Person{ Name = "Chocolate"};
            var relationShips = new RelationShips();
            relationShips.AddParentAndChild(parent, child1);
            relationShips.AddParentAndChild(parent, child2);
            relationShips.AddParentAndChild(parent, child3);
            new Research(relationShips);
        }
    }
}
