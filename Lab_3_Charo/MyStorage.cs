using System;
using System.Threading;

public class MyStorage
{
    private readonly SemaphoreSlim emptyStorage = new SemaphoreSlim(0);
    private readonly SemaphoreSlim fullStorage;
    private readonly object mutex = new object();
    private readonly Item[] items;
    private int putIndex = 0;
    private int takeIndex = 0;

    public MyStorage(int size)
    {
        items = new Item[size];
        fullStorage = new SemaphoreSlim(size);
    }

    public void Produce(Item item, int producerId)
    {
        Console.WriteLine($"Виробник {producerId}: підходить до складу");

        Console.WriteLine($"Виробник {producerId}: чекає на вільне місце");
        fullStorage.Wait();

        Console.WriteLine($"Виробник {producerId}: отримав дозвіл на вхід");

        lock (mutex)
        {
            Console.WriteLine($"Виробник {producerId}: зайшов у критичну секцію");

            Console.WriteLine($"Виробник {producerId}: кладе продукт {item.Id}");
            AddItem(item);
            Console.WriteLine($"Виробник {producerId}: поклав продукт {item.Id}");
        }

        emptyStorage.Release();
        Console.WriteLine($"Виробник {producerId}: повідомив про новий предмет");
    }

    public void Consume(int consumerId)
    {
        Console.WriteLine($"Споживач {consumerId}: підходить до складу");

        Console.WriteLine($"Споживач {consumerId}: чекає на наявність предмета");
        emptyStorage.Wait();

        Console.WriteLine($"Споживач {consumerId}: отримав дозвіл на вхід");

        Item item;
        lock (mutex)
        {
            Console.WriteLine($"Споживач {consumerId}: зайшов у критичну секцію");

            item = RemoveItem();
            Console.WriteLine($"Споживач {consumerId}: взяв предмет з id: {item.Id}");
        }

        fullStorage.Release();
        Console.WriteLine($"Споживач {consumerId}: повідомив про звільнення місця");
    }

    private void AddItem(Item item)
    {
        items[putIndex] = item;
        putIndex = (putIndex + 1) % items.Length;
    }

    private Item RemoveItem()
    {
        Item item = items[takeIndex];
        items[takeIndex] = null;
        takeIndex = (takeIndex + 1) % items.Length;
        return item;
    }
}
