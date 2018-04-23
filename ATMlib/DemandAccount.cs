using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ATMlib
{
    public sealed class DemandAccount : Account
    {
        public DemandAccount(decimal sum = 0, uint percentage = 0)
            : base(sum, percentage)
        {
        }
        protected internal override void Open()
        {
            OnOpened(GetAccArgs($"Открыт новый счет до востребования!\nID счета: {Id}", _sum));
        }
    }
}
