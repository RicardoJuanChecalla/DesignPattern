// Command Pattern
// using System;
// using System.Collections.Generic;
using static System.Console;

namespace CommandPattern
{
    public class BankAccount
    {
        private int balance;
        private int overDrafLimit = -500;
        public void Deposit(int amount)
        {
            balance += amount;
            WriteLine($"Deposited ${amount}, balance is now {balance}");
        }
         public bool WithDraw(int amount)
        {
            if(balance - amount >= overDrafLimit)
            {
                balance -= amount;
                WriteLine($"WithDraw ${amount}, balance is now {balance}");
                return true;
            }
            return false;
        }
        public override string ToString()
        {
            return $"{nameof(balance)}: {balance}";
        }
    }
    public interface ICommand
    {
        void Call();
        void Undo();
        bool Success {get; set;}
    }
    public class BankAccountCommand : ICommand
    {
        private BankAccount account;
        public enum Action
        {
            Deposit, WithDraw
        }
        private Action action;
        private int amount;
        public BankAccountCommand(BankAccount account,Action action,int amount)
        {
            this.account = account;
            this.action = action;
            this.amount = amount;
        }
        public void Call()
        {
            switch (action)
            {
                case Action.Deposit:
                    account.Deposit(amount);
                    Success = true;
                    break;
                case Action.WithDraw:
                    Success = account.WithDraw(amount);
                    break;
                default:
                throw new ArgumentOutOfRangeException();
            }
        }
         public void Undo()
         {
            if(!Success)return;
            switch (action)
            {
                case Action.Deposit:
                    account.WithDraw(amount);
                    break;
                case Action.WithDraw:
                    account.Deposit(amount);
                    break;
                default:
                throw new ArgumentOutOfRangeException();
            }
         }
        public bool Success {get ; set;}
    }
    public class CompositeBankAccountCommand: List<BankAccountCommand>, ICommand
    {
        public CompositeBankAccountCommand()
        {

        }
        public CompositeBankAccountCommand(IEnumerable<BankAccountCommand> collection): base(collection)
        {

        }
        public virtual void Call()
        {
            ForEach(cmd=>cmd.Call());
        }
        public virtual void Undo()
        {
            foreach(var cmd in ((IEnumerable<BankAccountCommand>)this).Reverse())
            {
                if (cmd.Success) cmd.Undo();
            }
        }
        public virtual bool Success 
        {
            get { return this.All(cmd => cmd.Success); } 
            set
            {
                foreach(var cmd in this)
                    cmd.Success = value;
            }
        }
    }
    public class MoneyTransferCommand : CompositeBankAccountCommand
    {
        public MoneyTransferCommand(BankAccount from, BankAccount to, int amount)
        {
            AddRange(new[]{
                new BankAccountCommand(from, BankAccountCommand.Action.WithDraw, amount),
                new BankAccountCommand(to, BankAccountCommand.Action.Deposit, amount),
            });
        }
        public override void Call()
        {
            BankAccountCommand last = null;
            foreach(var cmd in this)
            {
                if(last == null || last.Success)
                {
                    cmd.Call();
                    last = cmd;
                }
                else
                {
                    cmd.Undo();
                    break;
                }
            }
        }
    }
 public class Program
    {
        static void Main(string[] args)
        {
            /*
            var ba = new BankAccount();
            var commands =  new List<BankAccountCommand>{
                new BankAccountCommand(ba, BankAccountCommand.Action.Deposit, 100),
                new BankAccountCommand(ba, BankAccountCommand.Action.WithDraw, 1000),
            };
            WriteLine(ba);
            foreach (var c in commands)
                c.Call();
            WriteLine(ba);
            foreach (var c in Enumerable.Reverse(commands))
                c.Undo();
            WriteLine(ba);    
            */
            /*
            var ba = new BankAccount();
            var deposit =  new BankAccountCommand(ba, BankAccountCommand.Action.Deposit, 100);
            var withDraw = new BankAccountCommand(ba, BankAccountCommand.Action.WithDraw, 50);
            var composite = new CompositeBankAccountCommand(new[]{deposit, withDraw});
            composite.Call();
            WriteLine(ba);
            composite.Undo();
            WriteLine(ba);
            */
            var from = new BankAccount();
            from.Deposit(100);
            var to = new BankAccount();
            var mtc = new MoneyTransferCommand(from, to, 75);
            mtc.Call();
            WriteLine(from);
            WriteLine(to);

            mtc.Undo();
            WriteLine(from);
            WriteLine(to);
        }
    }
}
