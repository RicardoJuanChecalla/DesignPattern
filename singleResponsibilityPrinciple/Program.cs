using System.Diagnostics;
// Single Responsability Principle: cada módulo o clase debe tener responsabilidad 
// sobre una sola parte de la funcionalidad proporcionada por el software y esta 
// responsabilidad debe estar encapsulada en su totalidad por la clase. 
// Todos sus servicios deben estar estrechamente alineados con esa responsabilidad.
namespace singleResponsibilityPrinciple 
{
    public class Journal
    {
        private readonly List<string> entries = new List<string>();
        private static int count = 0;
        public int AddEntry(string text)
        {
            entries.Add($"{++count}: {text}");
            return count;
        }
        public void RemoveEntry(int index)
        {
            entries.RemoveAt(index);
        }
        public override string ToString()
        {
            return string.Join(Environment.NewLine,entries);
        }
    }
    public class Persistence
    {
        public void SaveToFile(Journal j, string filename, bool overwrite = false)
        {
            if(overwrite || !File.Exists(filename))
                File.WriteAllText(filename,j.ToString());
        }
    }
    internal class Demo
    {
        static void Main(string[] args)
        {
            var j = new Journal();
            j.AddEntry("I cried today");
            j.AddEntry("I ate a bug");
            Console.WriteLine(j);

            var p = new Persistence();
            var filename = @"D:\Design Patterns\journal.txt";
            p.SaveToFile(j,filename,true);
            Process.Start(filename);
        }
    }
}
