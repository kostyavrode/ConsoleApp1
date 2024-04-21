using System;
using System.Collections.Generic;
using System.Diagnostics;
namespace ConsoleApp1
{
    class Program
    {
        static void Main(string[] args)
        {
            bool isActive = true;
            ProcessesWorker processWorker = new ProcessesWorker();
            InputWorker inputWorker = new InputWorker(); 
            processWorker.GetProcessesList();
            processWorker.ShowProcesses(processWorker.Processes);
            processWorker.ShowCommands();
            while (isActive)
            {
                string input = Console.ReadLine();
                switch(input)
                {
                    case "exit":
                        isActive = false;
                        break;
                    case "update":
                        processWorker.GetProcessesList();
                        processWorker.ShowProcesses(processWorker.Processes);
                        break;
                    case "mem":
                        Console.WriteLine("Enter process ID or process name");
                        input = Console.ReadLine();
                        int parseResult = inputWorker.TryParseToInt(input);
                        if (parseResult!=-1 && processWorker.FindProcess(parseResult))
                        {
                            Console.WriteLine("Memory size: "+processWorker.GetMemorySize(processWorker.FoundProcesses)+" bytes");
                        }
                        else if (processWorker.FindProcess(input))
                        {
                            Console.WriteLine("Memory size: " + processWorker.GetMemorySize(processWorker.FoundProcesses) + " bytes");
                        }
                        else
                        {
                            Console.WriteLine("Failed to found process ");
                        }
                        break;
                    case "shut":
                        Console.WriteLine("Enter process ID or process name");
                        input = Console.ReadLine();
                        int parseRes = inputWorker.TryParseToInt(input);
                        if (parseRes != -1 && processWorker.FindProcess(parseRes))
                        {
                            processWorker.ShutDownProcess(processWorker.FoundProcesses);
                        }
                        else if (processWorker.FindProcess(input))
                        {
                            processWorker.ShutDownProcess(processWorker.FoundProcesses);
                        }
                        else
                        {
                            Console.WriteLine("Failed to found process ");
                        }
                        break;
                    case "find":
                        Console.WriteLine("Enter process ID or process name");
                        input = Console.ReadLine();
                        int parseR = inputWorker.TryParseToInt(input);
                        if (parseR != -1 && processWorker.FindProcess(parseR))
                        {
                            processWorker.ShowProcess(processWorker.FoundProcesses[0]);
                        }
                        else if (processWorker.FindProcess(input))
                        {
                            processWorker.ShowProcesses(processWorker.FoundProcesses);
                        }
                        else
                        {
                            Console.WriteLine("Failed to found process ");
                        }
                        break;
                    default:
                        Console.WriteLine("Error");
                        break;
                }
                
            }
        }
    }
    class ProcessesWorker
    {
        private List<Process> processes = new List<Process>();
        private List<Process> foundProcesses=new List<Process>();
        public List<Process> Processes
        {
            get { return processes; }
            private set { value = processes; }
        }
        public List<Process> FoundProcesses
        {
             get {return foundProcesses; }
            private set { value = foundProcesses; }
            }
        public List<Process> GetProcessesList()
        {
            processes.Clear();
            Process[] tempProcesses = Process.GetProcesses();
            foreach (Process proc in tempProcesses)
            {
                try
                {
                    processes.Add(proc);
                    
                }
                catch (Exception e)
                {
                    Console.WriteLine(e);
                }
                
            }
            return processes;
        }
        public void ShowProcesses(List<Process> processesToShow)
        {
            if (processesToShow.Count>0)
            {
                foreach (Process proc in processesToShow)
                {
                    ShowProcess(proc);
                }
            }
        }
        public void ShowProcess(Process proc, string extraData="")
        {
            Console.WriteLine("{0,-10} {1,-22}", "ID:" + "[" + proc.Id + "]", proc.ProcessName+ extraData);
        }
        public void ShowCommands()
        {
            Console.WriteLine("Commands:");
            Console.WriteLine("update - Update processes list, mem - get process memory size, shut - shut down process, find - find process, exit - exit application");
        }
        public bool FindProcess(string processName)
        {
            foundProcesses.Clear();
            bool isFound = false;
            foreach(Process proc in processes)
            {
                if (proc.ProcessName==processName)
                {
                    foundProcesses.Add(proc);
                    isFound = true;
                }
            }
            FoundProcesses = foundProcesses;
            return isFound;
        }
        public bool FindProcess(int id)
        {
            foundProcesses.Clear();
            bool isFound = false;
            foreach (Process proc in processes)
            {
                if (proc.Id==id)
                {
                    foundProcesses.Add(proc);
                    isFound = true;
                }
            }
            return isFound;
        }
        public void ShutDownProcess(List<Process> processesToWork)
        {
            foreach (Process proc in processesToWork)
            {
                ShowProcess(proc, " shutted down");
                proc.Kill();
            }
        }
        public long GetMemorySize(List<Process> processesToWork)
        {
            long memorySumm=0;
            foreach(Process proc in processesToWork)
            {
                memorySumm += proc.PrivateMemorySize64;
            }
            return memorySumm;
        }
    }
    class InputWorker
    {
        public int TryParseToInt(string inputData)
        {
            int tryParseResult;
            bool success=Int32.TryParse(inputData,out tryParseResult);
            if (success)
            {
                return tryParseResult;
            }
            else
            {
                return -1;
            }
        }
    }
    }
