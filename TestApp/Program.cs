
// See https://aka.ms/new-console-template for more information

using System.Diagnostics;
using TestApp;

Console.WriteLine("Hello, World!");
Console.ReadLine();
new SimpleWallTime().Start();

while (true)
{
    DoPiComputation();
}

    try
    {
        ThrowException();
    }
    catch
    {
    }

Console.WriteLine("Done");
Console.ReadLine();

static void ThrowException()
{
    throw new Exception("Test exception");
}

void DoPiComputation()
{
    // ~ 7 seconds on a P70 laptop
    const int maxIteration = 200000000;
    ulong denominator = 1;
    int numerator = 1;
    double pi = 1;
    var sw = new Stopwatch();
    sw.Start();

    int currentIteration = 0;
    while (
        (currentIteration < maxIteration))
    {
        numerator = -numerator;
        denominator += 2;
        pi += ((double)numerator) / ((double)denominator);

        currentIteration++;
    }

    pi *= 4.0;
    sw.Stop();


}
