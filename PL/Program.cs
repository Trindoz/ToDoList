using ConsolePL.View;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ConsolePL
{
    class Program
    {
        static void Main(string[] args)
        {
            Start();
        }
        public static void Start()
        {
            CRUD.HelloPage();
            int command = 0;
            string input = Console.ReadLine();
            while (!Int32.TryParse(input, out command) || !Enum.IsDefined(typeof(Commands), command))
            {
                Console.WriteLine("Введенное значение не соответствует ни одной из комманд!");
                CRUD.HelloPage();
                input = Console.ReadLine();
            }
            CRUD crud = new CRUD();
            switch (command)
            {
                case 0:
                    Environment.Exit(0);
                    break;
                case 1:
                    crud.ItemsList();
                    Start();
                    break;
                case 2:
                    crud.Add(1);
                    Start();
                    break;
                case 3:
                    crud.Update(1);
                    Start();
                    break;
                case 4:
                    crud.Delete();
                    Start();
                    break;
                case 5:
                    crud.Finish();
                    Start();
                    break;
            }
        }
    }
}
