using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ATMlib
{
    class DepositAccount : Account
    {
        public DepositAccount(decimal sum = 0, uint percentage = 0)
            : base(sum, percentage)
        {
        }
        protected internal override void Open()
        {
            base.OnOpened(GetAccArgs($"Открыт депозитный счет!\nID счета: {Id}", this._sum));
        }
        public override void Put(decimal sum)
        {
            if (_days % 30 == 0)
                base.Put(sum);
            else
                base.OnAdded(GetAccArgs($"На счет можно положить только после 30-ти дней!", 0));
        }
        public override decimal Withdraw(decimal sum)
        {
            if (_days % 30 == 0)
                return base.Withdraw(sum);
            else
                base.OnWithdrawed(GetAccArgs("Вывести средства можно только после 30-ти дневного периода", 0));
            return 0;
        }

        protected internal override void Calculate()
        {
            if (_days % 30 == 0)
                base.Calculate();
        }
    }
}
