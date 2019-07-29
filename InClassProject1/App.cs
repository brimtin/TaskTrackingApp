using System;
using System.Collections.Generic;
using System.IO;
using System.Text;

namespace InClassProject1
{
    class App
    {
        List<string> tasks = new List<string>();
        List<bool> isActioned = new List<bool>();

        private int selectedTask = 0;

        const int pageLength = 20;

        public App()
        {
            Console.OutputEncoding = Encoding.Unicode;

           RetrieveItemsFromList();
        }
        
        public void Execute()
        {
            bool quit;
            do
            {
                DisplayTasks();
                RemoveFirstActionedItem();
               var key = Menu();
               quit  = MenuSelection(key);

            } while (!quit);

            StoreItemsInFile();
            Console.WriteLine();
        }

        private void RemoveFirstActionedItem()
        {
            try
            {
                while (isActioned[0])
                {
                    tasks.RemoveAt(0);
                    isActioned.RemoveAt(0);
                    selectedTask--;
                }
                if (selectedTask < 0)
                {
                    selectedTask = 0;
                }
            }
            catch (System.ArgumentOutOfRangeException)
            {
                CreateNewTaskList();
            }
        }
        private void DisplayTasks()
        {
            Console.Clear();

            ShowOptions();
            var page = selectedTask / pageLength;
            var startingPoint = page * pageLength;

            for (int i = startingPoint; (i < startingPoint + pageLength) && (i < tasks.Count)  ; i++)
            {
                if (isActioned[i])
                {
                    Console.ForegroundColor = ConsoleColor.DarkGray;
                }
                else if (i== selectedTask)
                {
                    Console.BackgroundColor = ConsoleColor.Gray;
                    Console.ForegroundColor = ConsoleColor.Black;
                }
                Console.WriteLine(tasks[i]);

                Console.ForegroundColor = ConsoleColor.Gray;
                Console.BackgroundColor = ConsoleColor.Black; 
            }
            
            Console.WriteLine();
        }

        private ConsoleKey Menu()
        {
                ConsoleKey key;
           
               // ShowOptions();
                key = GetUserInput();

            return key;
        }

        private ConsoleKey GetUserInput()
        {
            return Console.ReadKey().Key;
        }

        private bool MenuSelection(ConsoleKey key)
        {
            switch (key)
            {
                //case ConsoleKey.V:
                //    {
                //        DisplayTasks();
                //        break;
                //    }
                case ConsoleKey.A:
                    {
                        AddNewTaskToList();
                        break;
                    }
                case ConsoleKey.DownArrow:
                    {
                        SelectNextTask();
                        break;
                    }
                case ConsoleKey.D:
                    {
                        DeleteItem();
                        break;
                    }
                case ConsoleKey.Enter:
                    {
                        WorkOnSelectedTask();
                        break;
                    }
                case ConsoleKey.Escape:
                    {
                        return true;
                    }
               

            }
            return false;
        }

        private void WorkOnSelectedTask()
        {
            bool valid = false;
            do
            {
                Console.Clear();
                Console.WriteLine($"Working on: {tasks[selectedTask]}");
                Console.WriteLine("R: Re-enter\t||C: Complete\t||Q: Cancel");


                var key = GetUserInput();

                switch (key)
                {
                    case ConsoleKey.R:
                        {
                            AddTaskToList(tasks[selectedTask]);
                            DeleteItem();
                            valid = true;
                            break;
                        }
                    case ConsoleKey.C:
                        {
                            DeleteItem();
                            valid = true;
                            break;
                        }
                    case ConsoleKey.Q:
                        {
                            valid = true;
                            break;
                        }
                }
            } while (!valid);

        }

        private void DeleteItem()
        {
            isActioned[selectedTask] = true;
            SelectNextTask();
        }

        private void SelectNextTask()
        {
            bool overflowed = false;

            do
            {
                selectedTask += 1;

                if (selectedTask >= isActioned.Count)
                {
                    selectedTask = 0;
                    overflowed = true;
                       
                }

            } while ( isActioned[selectedTask]);
        }

        private void ShowOptions()
        {
            Console.WriteLine("--------------------------------------------------------------------");
            Console.WriteLine("A: Add    ||\u2193: Next Item    ||Enter: Select     ||Esc: Quit     ");
            Console.WriteLine("--------------------------------------------------------------------");
        }

        private void AddNewTaskToList()
        {
            Console.Clear();
            Console.WriteLine("Input a new task: ");

            var input = Console.ReadLine();
            AddTaskToList(input);
            StoreItemsInFile();
        }

        private void AddTaskToList(string input)
        {
            if (!string.IsNullOrWhiteSpace(input))
            {
                tasks.Add(input);
                isActioned.Add(false);
            }
        }

        
        private void RetrieveItemsFromList()
        {

            try
            {
                using (StreamReader sr = new StreamReader(@"C:\Users\brimt\OneDrive\Desktop\MSSA\ITSA220\CodeFiles\InClassProject1\Tasks.txt"))
                {
                    while (!sr.EndOfStream)
                    {
                        var input = sr.ReadLine();

                        var splits = input.Split(new char[] { '\x1e' });

                        if (splits.Length == 2)
                        {
                            tasks.Add(splits[0]);       
                            isActioned.Add(bool.Parse(splits[1]));
                        }
                        
                    }
                    
                }
            }
            catch (FileNotFoundException)
            {
            //    CreateNewTaskList();
                Console.WriteLine("This file Does not exists");
            }
           
        }

        public void CreateNewTaskList()
        {
            Console.WriteLine("The current task List is empty");
            Console.WriteLine("Please Create a new list to start by pressing Enter to add an item to the list");
            Console.ReadLine();
            AddNewTaskToList();

        }

        public void StoreItemsInFile()
        {
            using (StreamWriter sw = new StreamWriter(@"C:\Users\brimt\OneDrive\Desktop\MSSA\ITSA220\CodeFiles\InClassProject1\Tasks.txt"))
            {
                for (int i = 0; i < tasks.Count; ++i)
                {
                    sw.WriteLine($"{tasks[i]}\x1e {isActioned[i]}");
                }
            }
        }

    }
}
