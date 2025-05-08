using System;
using System.Threading;
using System.Collections.Generic;
using System.Text;

class Program
{
    static void Main()
    {
        Console.OutputEncoding = Encoding.UTF8;

        int storageSize = 3;
        int producerCount = 2;
        int consumerCount = 2;
        int productsCount = 5;

        var storage = new MyStorage(storageSize);

        var production = SplitProducts(productsCount, producerCount);
        var consumption = SplitProducts(productsCount, consumerCount);

        for (int i = 0; i < producerCount; i++)
        {
            new Producer(i + 1, production[i], storage);
        }

        for (int i = 0; i < consumerCount; i++)
        {
            new Consumer(i + 1, consumption[i], storage);
        }
    }

    static int[] SplitProducts(int total, int parts)
    {
        if (parts <= 0 || total < parts)
            throw new ArgumentException("Invalid input");

        var rand = new Random();
        var result = new int[parts];
        int remaining = total;

        for (int i = 0; i < parts - 1; i++)
        {
            int max = remaining - (parts - i - 1);
            result[i] = rand.Next(1, max + 1);
            remaining -= result[i];
        }

        result[parts - 1] = remaining;
        return result;
    }
}
