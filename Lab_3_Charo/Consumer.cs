using System;
using System.Threading;

public class Consumer
{
    private readonly int id;
    private readonly int itemCount;
    private readonly MyStorage storage;

    public Consumer(int id, int itemCount, MyStorage storage)
    {
        this.id = id;
        this.itemCount = itemCount;
        this.storage = storage;
        new Thread(Run).Start();
    }

    private void Run()
    {
        for (int i = 0; i < itemCount; i++)
        {
            try
            {
                storage.Consume(id);
            }
            catch (Exception e)
            {
                Console.WriteLine(e);
            }
        }
    }
}
