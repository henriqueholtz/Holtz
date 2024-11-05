using System.Text;

namespace Holtz.DesignPattern.AbstractFactoryTests;

public class ProgramTests
{
    StringBuilder _consoleOutput = new StringBuilder();
    public ProgramTests()
    {
        var consoleOutputWriter = new StringWriter(_consoleOutput);
        Console.SetOut(consoleOutputWriter);
    }

    [Fact]
    public void MainTest()
    {
        var outputLines = RunMainAndGetConsoleOutput();

        Assert.Equal(7, outputLines.Length);
        Assert.Equal("Hello, Starting...", outputLines[0]);
        Assert.Equal("Type is Cake of chocolate. Ingredients: Sugar,Chocolate...", outputLines[1]);
        Assert.Equal("Type is Cake of orange. Ingredients: Sugar,Orange...", outputLines[2]);
        Assert.Equal("Type is Pizza of pepperoni. Ingredients: Pepperoni,Dough...", outputLines[3]);
        Assert.Equal("Type is Pizza of bacon. Ingredients: Bacon,Dough...", outputLines[4]);
        Assert.Equal("Finished.", outputLines[5]);
        Assert.Equal("", outputLines[6]);
    }

    private string[] RunMainAndGetConsoleOutput()
    {
        Program.Main(new string[] { });
        return _consoleOutput.ToString().Split(Environment.NewLine);
    }
}
