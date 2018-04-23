using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ATMlib
{
    public enum AccountType
    {
        DemandAccount,
        DepositAccount
    }
    
    public class BankException : Exception
    {
        public BankException(string message = "Ошибка!")
            : base(message)
        {
            Console.BackgroundColor = ConsoleColor.Red;
            Console.WriteLine(message);
            Console.ResetColor();
        }
    }
    public class Bank<T> where T : Account
    {
        private List<T> accounts;
        public string Name { get; private set; }
        public Bank(string name)
        {
            Name = name;
            accounts = new List<T>();
        }

        //Метод для создания счета
        public void Open
            (
              AccountType         accountType, 
              AccountStateHandler addSumHandler,
              AccountStateHandler withdrawSumHandler,
              AccountStateHandler calculationHandler, 
              AccountStateHandler closeAccountHandler,
              AccountStateHandler openAccountHandler,
              decimal             sum = 0,
              uint                percentage = 0
            )
        {
            T account = null;
            switch (accountType)
            {
                case AccountType.DemandAccount:
                    account = new DemandAccount(sum, percentage) as T;
                    break;
                case AccountType.DepositAccount:
                    account = new DepositAccount(sum, percentage) as T;
                    break;
                default:
                    break;
            }

            if (account == null)
            {
                throw new BankException("Ошибка создания счета!");
            }
            
            //Добавление счета в БД
            accounts.Add(account);
            
            //Установка обработчиков событий
            account.Added += addSumHandler;
            account.Withdrawed += withdrawSumHandler;
            account.Calculated += calculationHandler;
            account.Closed += closeAccountHandler;
            account.Opened += openAccountHandler;

            account.Open();
        }
        
        //Добавление средств
        public void Put(decimal sum, uint id)
        {
            T tacc = accounts.Find(x => x.Id == id);
            if (tacc == null)
            {
                throw new BankException("Счет не найден!");
            }
            tacc.Put(sum);
        }

        //Вывод средств
        public void Withdraw(decimal sum, uint id)
        {
            T tacc = accounts.Find(x => x.Id == id);
            if (tacc == null)
            {
                throw new BankException("Счет не найден!");
            }
            tacc.Withdraw(sum);
        }
        public void Close(uint id)
        {
            T tacc = accounts.Find(x => x.Id == id);
            if (tacc == null)
            {
                throw new BankException("Счет не найден!");
            }
            //Закрываем аккаунт
            tacc.Close();
            //Удаляем из базы
            accounts.Remove(tacc);
        }
        public void CalculatePercentage()
        {
            foreach (var acc in accounts)
            {
                acc.IncDays();
                acc.Calculate();
            }
        }
    }
}
