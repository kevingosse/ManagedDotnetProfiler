
// See https://aka.ms/new-console-template for more information
Console.WriteLine("Hello, World!");

    Console.WriteLine("Press return to throw an exception");
    Console.ReadLine();

    try
    {
        ThrowException();
    }
    catch
    {
    }

static void ThrowException()
{
    throw new Exception("Test exception");
}