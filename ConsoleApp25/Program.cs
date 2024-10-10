using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;

class Program
{
    static SemaphoreSlim semaphore;
    static List<int> createdThreads = new List<int>();
    static List<int> waitingThreads = new List<int>();
    static List<int> workingThreads = new List<int>();
    static int threadCounter = 1;

    static void Main()
    {
        semaphore = new SemaphoreSlim(3);
        CreateThread();
        DisplayThreads();
    }

    static void CreateThread()
    {
        int threadId = threadCounter++;
        createdThreads.Add(threadId);
        Console.WriteLine($"Thread {threadId} yaradıldı.");
        Task.Run(() => ProcessThread(threadId));
    }

    static async Task ProcessThread(int threadId)
    {
        Console.WriteLine($"Thread {threadId} gözleyir.");
        waitingThreads.Add(threadId);
        await semaphore.WaitAsync();
        waitingThreads.Remove(threadId);
        workingThreads.Add(threadId);
        Console.WriteLine($"Thread {threadId} işe başladı.");
        await Task.Delay(2000);
        workingThreads.Remove(threadId);
        semaphore.Release();
        Console.WriteLine($"Thread {threadId} bitdi və released olundu.");
    }

    static void DisplayThreads()
    {
        Console.WriteLine("Yaradılan Threadler:");
        foreach (var thread in createdThreads)
        {
            Console.WriteLine($"Thread {thread}");
        }

        Console.WriteLine("Gözleyen Threadler:");
        foreach (var thread in waitingThreads)
        {
            Console.WriteLine($"Thread {thread}");
        }

        Console.WriteLine("İşleyen Threadler:");
        foreach (var thread in workingThreads)
        {
            Console.WriteLine($"Thread {thread}");
        }
    }
}
