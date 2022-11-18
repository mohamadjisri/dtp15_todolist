namespace dtp15_todolist
{
    public class Todo
    {
        public static List<TodoItem> list = new List<TodoItem>();

        public const int Active = 1;
        public const int Waiting = 2;
        public const int Ready = 3;
        public static string StatusToString(int status)
        {
            switch (status) //Redan implementerad i koden
            {
                case Active: return "aktiv";
                case Waiting: return "väntande";
                case Ready: return "avklarad";
                default: return "(felaktig)";
            }
        }
        public class TodoItem //implementering av tasks, redan
        {
            public int status;
            public int priority;
            public string task;
            public string taskDescription;
            public TodoItem() //redan imple
            {
                status = Active;
                priority= 1;
                task = "";
                taskDescription = "";
            }
            public TodoItem(int priority, string task) //set aktiva
            {
                this.status = Active;
                this.priority = priority;
                this.task = task;
                this.taskDescription = "";
            }
            public TodoItem(string todoLine)
            {
                string[] field = todoLine.Split('|');
                status = Int32.Parse(field[0]);
                priority = Int32.Parse(field[1]);
                task = field[2];
                taskDescription = field[3];
            }
            public void Print(bool verbose = false) //printar listan
            {
                string statusString = StatusToString(status);
                Console.Write($"|{statusString,-12}|{priority,-6}|{task,-20}|");
                if (verbose)
                    Console.WriteLine($"{taskDescription,-40}|");
                else
                    Console.WriteLine();
            }
            
            public String toString() //ny print
            {
                return $"{status}|{priority}|{task}|{taskDescription}";
            }
            

        }

        public static void LoadFromFile() //imple av load
        {
            list.Clear();
            try
            {
                var text = File.ReadAllText("todo.lis");
                var records = text.Split('\n');
                foreach (var record in records)
                {
                    if (record == "") continue;
                    TodoItem todoItem = new TodoItem(record);
                    list.Add(todoItem);
                }
                
            }
            catch { }
        }
        public static void SaveToFIle() //imple av save med nya rad på listan
        {
            var text = "";
            foreach (var item in list)
            {
                text += item.toString()+"\n";
            }
            File.WriteAllText("todo.lis", text);
        }
        public static void ReadListFromFile() //imple av read (redan imple) + todo.list rader
        {
            string todoFileName = "todo.lis";
            Console.Write($"Läser från fil {todoFileName} ... ");
            StreamReader sr = new StreamReader(todoFileName);
            int numRead = 0;

            string line;
            while (((line = sr.ReadLine())) != null)
            {
                TodoItem item = new TodoItem(line);
                list.Add(item);
                numRead++;
            }
            sr.Close();
            Console.WriteLine($"Läste {numRead} rader.");
        }

        public static void setActive(String task) //set Aktiva task
        {
            foreach (var item in list)
            {
                if(item.task == task)
                {
                    item.status = Active;
                }
            }
        }

        public static void setReady(String task) //set Ready task
        {
            foreach (var item in list)
            {
                if (item.task == task)
                {
                    item.status = Ready;
                }
            }
        }

        public static void setWait(String task) //set Wait task
        {
            foreach (var item in list)
            {
                if (item.task == task)
                {
                    item.status = Waiting;
                }
            }
        }
        private static void PrintHeadOrFoot(bool head, bool verbose) //pinta listan (if-else loop)
        {
            if (head)
            {
                Console.Write("|status      |prio  |namn                |");
                if (verbose) Console.WriteLine("beskrivning                             |");
                else Console.WriteLine();
            }
            Console.Write("|------------|------|--------------------|");
            if (verbose) Console.WriteLine("----------------------------------------|");
            else Console.WriteLine();
        }
        private static void PrintHead(bool verbose)
        {
            PrintHeadOrFoot(head: true, verbose);
        }
        private static void PrintFoot(bool verbose)
        {
            PrintHeadOrFoot(head: false, verbose);
        }
        public static bool CreateNewTask() //set ny task
        {
            TodoItem task = new TodoItem();
            task.task = MyIO.ReadCommand("Uppgiftens namn:");
            var command = MyIO.ReadCommand("Prioritet:");
            if (MyIO.Equals(command, "1") || MyIO.Equals(command, "2") || MyIO.Equals(command, "3") || MyIO.Equals(command, "4"))
            {
                task.priority = int.Parse(command);
            }
            else
            {
                return false;
            }
            task.taskDescription = MyIO.ReadCommand("Beskrivning:");
            list.Add(task);
            return true;
        }
        public static void PrintTodoList(bool verbose = false,bool active=false) //printa aktiva (foreach loop)
        {
            PrintHead(verbose);
            foreach (TodoItem item in list)
            {
                if(active)
                {
                    if(item.status is Active) item.Print(verbose);
                }
                else
                {
                    item.Print(verbose);
                }
                
            }
            PrintFoot(verbose);
        }

        public static void PrintTodoList_Active(bool verbose = false) //printa aktiva (foreach loop)
        {
            PrintHead(verbose);
            foreach (TodoItem item in list)
            {
                if(item.status is Active) item.Print(verbose);
            }
            PrintFoot(verbose);
        }
        public static void PrintHelp() //redan imple + nya kommando på listan
        {
            Console.WriteLine("Kommandon:");
            Console.WriteLine("ny                   Skapa en ny uppgift");
            Console.WriteLine("beskriv              lista alla aktiva uppgifter, status, prioritet, namn och beskrivning");
            Console.WriteLine("spara                spara uppgifterna");
            Console.WriteLine("ladda                ladda listan todo.lis");
            Console.WriteLine("hjälp                lista denna hjälp");
            Console.WriteLine("lista                lista att-göra-listan");
            Console.WriteLine("sluta                spara att-göra-listan och sluta");
            Console.WriteLine("lista                list All task");
            Console.WriteLine("lista allt           list Active task");
            Console.WriteLine("aktivera /uppgift/   sätt status på uppgift till Active");
            Console.WriteLine("klar /uppgift/       sätt status på uppgift till Ready");
            Console.WriteLine("vänta /uppgift/      sätt status på uppgift till Waiting");
            Console.WriteLine("sluta                spara senast laddade filen och avsluta programmet!");

        }
    }
    class MainClass
    {
        public static void Main(string[] args) //redan imple + printa och skriva in kommando (CW) + nya okända kommando print (catch, try, if)
        {
            Console.WriteLine("Välkommen till att-göra-listan!");
            Todo.LoadFromFile();
            Todo.PrintHelp();
            string command;
            do
            {
                command = MyIO.ReadCommand("> ");
                if (MyIO.Equals(command, "hjälp"))
                {
                    Todo.PrintHelp();
                }
                else if (MyIO.Equals(command, "sluta"))
                {
                    Console.WriteLine("Hej då!");
                    break;
                }
                else if (MyIO.Equals(command, "lista"))
                {
                    if (MyIO.HasArgument(command, "allt"))
                        Todo.PrintTodoList(verbose: false, active: false);
                    else
                        Todo.PrintTodoList(verbose: false, active: true);
                }
                else if (MyIO.Equals(command, "beskriv"))
                {
                    Todo.PrintTodoList_Active(verbose: true);
                }
                else if (MyIO.Equals(command, "ny"))
                {
                    if (Todo.CreateNewTask())
                    {
                        Console.WriteLine("Uppgift tillagd");
                    }
                    else
                    {
                        Console.WriteLine("Något gick fel");
                    }
                }
                else if (MyIO.Equals(command, "spara"))
                {
                    Todo.SaveToFIle();
                }
                else if (MyIO.Equals(command, "ladda"))
                {
                    Todo.LoadFromFile();
                }
                else if (command.Contains("aktivera"))
                {
                    try
                    {
                        var task = command.Split('/')[1];
                        Todo.setActive(task);
                    }
                    catch
                    {

                    }
                }
                else if (command.Contains("klar"))
                {
                    try
                    {
                        var task = command.Split('/')[1];
                        Todo.setReady(task);
                    }
                    catch
                    {

                    }
                }
                else if (command.Contains("vänta"))
                {
                    try
                    {
                        var task = command.Split('/')[1];
                        Todo.setWait(task);
                    }
                    catch
                    {

                    }
                }

                else if (command.Contains("sluta"))
                {
                    Todo.SaveToFIle();
                    Console.WriteLine("Hej då!");
                    break;
                }
                else
                {
                    Console.WriteLine($"Okänt kommando: {command}");
                }
            }
            while (true);
        }
    }
    class MyIO //redan imple IO kod
    {
        static public string ReadCommand(string prompt)
        {
            Console.Write(prompt);
            return Console.ReadLine();
        }
        static public bool Equals(string rawCommand, string expected)
        {
            string command = rawCommand.Trim();
            if (command == "") return false;
            else
            {
                string[] cwords = command.Split(' ');
                if (cwords[0] == expected) return true;
            }
            return false;
        }
        static public bool HasArgument(string rawCommand, string expected) //redan imple IO kod
        {
            string command = rawCommand.Trim();
            if (command == "") return false;
            else
            {
                string[] cwords = command.Split(' ');
                if (cwords.Length < 2) return false;
                if (cwords[1] == expected) return true;
            }
            return false;
        }
    }
}
