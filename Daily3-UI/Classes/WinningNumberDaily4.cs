using System.Text.Json.Serialization;

namespace Daily3_UI.Classes;

public class WinningNumberDaily4 : WinningNumberDaily3
{
    public WinningNumberDaily4(int number1, int number2, int number3, int number4)
        : base(number1, number2, number3)
    {
        Number4 = number4;
    }

    public WinningNumberDaily4()
    {
    }

    public WinningNumberDaily4(int number)
    {
        if (number < 1000 || number > 9999)
            throw new ArgumentOutOfRangeException();
        Number1 = number / 1000;
        Number2 = number / 100 % 10;
        Number3 = number / 10 % 10;
        Number4 = number % 10;
    }

    public WinningNumberDaily4(IReadOnlyList<string> number)
    {
        if (number.Count < 4) throw new ArgumentOutOfRangeException();
        Number1 = int.Parse(number[0]);
        Number2 = int.Parse(number[1]);
        Number3 = int.Parse(number[2]);
        Number4 = int.Parse(number[3]);
    }

    [JsonPropertyName("Number4")] public int Number4 { get; set; }
}