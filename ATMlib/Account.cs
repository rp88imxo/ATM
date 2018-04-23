using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ATMlib
{
    public abstract class Account : IAccount
    {
        /*
        События класса:
        */
        //Событие при выводе денег
        protected internal event AccountStateHandler Withdrawed;
        //Событие при добавление денег
        protected internal event AccountStateHandler Added;
        //Событе при открытие счета
        protected internal event AccountStateHandler Opened;
        //Собыите при закрытии счета
        protected internal event AccountStateHandler Closed;
        //Собыите при начислении процентов
        protected internal event AccountStateHandler Calculated;

        protected ulong _id;
        static ulong counter = 0;

        protected decimal _sum; // Переменная для хранения суммы
        protected uint _percentage; // Переменная для хранения процента
        protected uint _days = 0; // Время с момента открытия счета
        public Account(decimal sum,uint percentage = 0)
        {
            _sum = sum;
            _percentage = percentage;
            _id = (++counter ^ (ulong)DateTime.Now.Ticks);
        }

        //Текущая сумма на счету
        public decimal CurrentSum => _sum;

        //Текущий процент
        public uint Percentage => _percentage / 100;

        //ID клиента
        public ulong Id => _id;

        //Вызов событий
        private void CallEvent(AccountEventArgs e, AccountStateHandler handler)
        {
            if (e != null && handler != null)
                handler(this, e);
        }

        //Вызов конкретных событий
        protected virtual void OnOpened(AccountEventArgs e)
        {
            CallEvent(e, Opened);
        }
        protected virtual void OnWithdrawed(AccountEventArgs e)
        {
            CallEvent(e, Withdrawed);
        }
        protected virtual void OnAdded(AccountEventArgs e)
        {
            CallEvent(e, Added);
        }
        protected virtual void OnClosed(AccountEventArgs e)
        {
            CallEvent(e, Closed);
        }
        protected virtual void OnCalculated(AccountEventArgs e)
        {
            CallEvent(e, Calculated);
        }

        //Положить деньги
        public virtual void Put(decimal sum)
        {
            _sum += sum;
            OnAdded(new AccountEventArgs("На счет поступило: " + sum, sum));
        }
        //Снять
        public virtual decimal Withdraw(decimal sum)
        {
            decimal res = 0;
            if (sum <= _sum)
            {
                _sum -= sum;
                res = sum;
                OnWithdrawed(new AccountEventArgs($"На счет поступило: {sum}", sum));
            }
            else
            {
                OnWithdrawed(new AccountEventArgs($"Недостаточно денег на счете " + _id, 0));
            }
            return res;
        }
        protected internal AccountEventArgs GetAccArgs(string message, decimal sum)
        {
            return new AccountEventArgs(message, sum);
        }
        //Открытие счета
        protected internal virtual void Open()
        {
            OnOpened(GetAccArgs($"Открыт новый депозитный счет!ID счета: {this._id}", this._sum));
        }
        //закрытие счета
        protected internal virtual void Close()
        {
            OnClosed(GetAccArgs($"Счет: {Id} успешно закрыт!\nИтоговоя сумма:{CurrentSum}", CurrentSum));
        }
        protected internal virtual void IncDays()
        {
            _days++;
        }
        //Начисление процентов
        protected internal virtual void Calculate()
        {
            decimal increment = _sum * Percentage;
            _sum += increment;
            OnCalculated(GetAccArgs($"Начислен процент в размере: {increment}$", increment));
        }
    }
}
