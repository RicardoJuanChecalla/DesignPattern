// Interface segregation principle
namespace InterfaceSegregationPrinciple
{
    public class Document
    {
    }
    //Sin segregacion
    public interface IMachine
    {
        void Print(Document d);
        void Scan(Document d);
        void Fax(Document d);
    }
    public class MultiFunctionPrinter : IMachine
    {
        public void Print(Document d)
        {

        }
        public void Scan(Document d)
        {

        }
        public void Fax(Document d)
        {

        }
    }
    public class OldFashionedPrinter : IMachine
    {
        public void Print(Document d)
        {

        }
        public void Scan(Document d)
        {

        }
        public void Fax(Document d)
        {

        }
    }
    //Con segregación ---------------------------------------
    public interface IPrinter
    {
        void Print(Document d);
    }
    public interface IScanner
    {
        void Scan(Document d);
    }
    public class Photocopier : IPrinter, IScanner
    {
        public void Print(Document d)
        {

        }
        public void Scan(Document d)
        {

        }
    }
    public interface IMultiFunctionDevice : IScanner, IPrinter
    {

    }

    public class MultiFuncionalMachine: IMultiFunctionDevice
    {
        private IPrinter Printer;
        private IScanner Scanner;
        public MultiFuncionalMachine(IPrinter printer,IScanner scanner)
        {
            this.Printer = printer;
            this.Scanner = scanner;
        }
        public void Print(Document d)
        {
            Printer.Print(d);
        }
        public void Scan(Document d)
        {
            Scanner.Scan(d);
        }
    }
    public class Demo
    {
        static void Main(string[] args)
        {
            Console.WriteLine("Hello, World!");
        }
    }
}