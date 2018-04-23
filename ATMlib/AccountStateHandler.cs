using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ATMlib
{
    public delegate void AccountStateHandler(object sender, AccountEventArgs e);
    public class AccountEventArgs
    {
        //Сообщение
        public string Message { get; private set; }
        //Сумма изменения счета
        public decimal Sum { get; private set; }
        public AccountEventArgs(string message, decimal sum)
        {
            Message = message;
            Sum = sum;
        }
    }
}
