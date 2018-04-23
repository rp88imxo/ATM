using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ATMlib
{
    interface IAccount
    {
        //Положить деньги на счет
        void Put(decimal sum);
        //Вывести деньги со счета
        decimal Withdraw(decimal sum);
    }
}
