using System;
using System.Threading;

public class Producer
{
    private readonly int id;
    private readonly int itemCount;
    private readonly MyStorage storage;

    public Producer(int id, int itemCount, MyStorage storage)
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
            var item = new Item(id + i);
            storage.Produce(item, id);
            Console.WriteLine($"Producer {id} added item: {item.Id}");
        }
    }
}
