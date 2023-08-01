using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Daily3_UI
{
    public class WinningNumber
    {
        public int Number1 { get; set; }
        public int Number2 { get; set; }
        public int Number3 { get; set; }

        public WinningNumber(int number1, int number2, int number3)
        {
            Number1 = number1;
            Number2 = number2;
            Number3 = number3;
        }

        public WinningNumber(int number)
        {
            if (number < 100 || number > 999) 
                throw new ArgumentOutOfRangeException();
            Number1 = number / 100;
            Number2 = (number / 10) % 10;
            Number3 = number % 10;
        }

        public WinningNumber(IReadOnlyList<string> number)
        {
            if(number.Count < 3) throw new ArgumentOutOfRangeException();
            Number1 = int.Parse(number[0]);
            Number2 = int.Parse(number[1]);
            Number3 = int.Parse(number[2]);
        }
    }
}
