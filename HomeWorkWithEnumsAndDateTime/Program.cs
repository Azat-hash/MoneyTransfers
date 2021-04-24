using System;
using HomeWorkWithEnumsAndDateTime.Common;
using HomeWorkWithEnumsAndDateTime.Enums;
using HomeWorkWithEnumsAndDateTime.Models;
using HomeWorkWithEnumsAndDateTime.Tariffs;

namespace HomeWorkWithEnumsAndDateTime
{
    class Program
    {
        static void Main(string[] args)
        {
            Console.WriteLine("Введите имя и фамилию отправителя (через пробел):");
            string valueToSplit = Console.ReadLine();
            string[] senderName = valueToSplit.Split(' ');

            // 0 - Ilyas, 1 - Masirov

            if(senderName == null)
                throw new Exception("Заполните имя и фамилию отправителя");

            Console.WriteLine("Введите имя и фамилию получателя (через пробел):");
            valueToSplit = Console.ReadLine();
            string[] receiverName = valueToSplit.Split(' ');

            if (receiverName == null)
                throw new Exception("Заполните имя и фамилию получателя");

            var senderCustomer = new Customer(senderName[0], senderName[1]);
            var receiverCustomer = new Customer(receiverName[0], receiverName[1]);

            Console.WriteLine("Введите сумму перевода:");
            var amount = decimal.Parse(Console.ReadLine());

            Console.WriteLine($"Введите валюту перевода ({(int)Currency.KGS}, {(int)Currency.USD}, {(int)Currency.EUR}):");
            var currency = (Currency) int.Parse(Console.ReadLine());

            Console.WriteLine($"Введите систему перевода ({PaymentSystemName.Contact}, {PaymentSystemName.Unistream}, {PaymentSystemName.GoldenCrown}):");
            var systemName = Console.ReadLine();

            // заполняем тарифы
            TransferTariffs[] tariffs =
            {
                new TransferTariffs(PaymentSystemName.Contact, 28),
                new TransferTariffs(PaymentSystemName.GoldenCrown, 46),
                new TransferTariffs(PaymentSystemName.Unistream, 81),
            };

            // заполянем курс валют
            CurrencyRate[] rates =
            {
                new CurrencyRate(Currency.EUR, 92.3m),
                new CurrencyRate(Currency.USD, 84.1m),
            };

            var comission = CalculateComission(tariffs, systemName);
            var amountN = amount * GetCurrencyRate(rates, currency);

            var transfer = new Transfer(PaymentDirection.Output, systemName, currency, amount, amountN, comission);
            transfer.Validate(transfer);
            ShowInfo(transfer, senderCustomer, receiverCustomer);
            Console.ReadKey();
            Console.WriteLine("Отправить перевод? (Да, Нет)");

            var isSend = Console.ReadLine() == "Да";

            if (isSend)
            {
                transfer = transfer.SendTransfer(transfer);
                ShowInfo(transfer, senderCustomer, receiverCustomer);
                Console.ReadKey();
            }

            // для теста создаем перевод для выплаты
            Console.WriteLine("Теперь попробуем выплатить перевод.");
            var inputTransfer = new Transfer(PaymentDirection.Input, systemName, currency, amount, amountN, comission);
            transfer.Validate(inputTransfer);
            ShowInfo(inputTransfer, senderCustomer, receiverCustomer);

            Console.ReadKey();
            Console.WriteLine("Выплатить перевод? (Да, Нет)");
            var isPaid = Console.ReadLine() == "Да";

            if (isPaid)
            {
                transfer = transfer.PayoutTranfer(transfer);
                ShowInfo(transfer, senderCustomer, receiverCustomer);
                Console.ReadKey();
            }
        }

        /// <summary>
        /// Возвращает текущий курс указанной валюты
        /// </summary>
        /// <param name="rates">Коллекция курса валют</param>
        /// <param name="currency">Тип валюты</param>
        public static decimal GetCurrencyRate(CurrencyRate[] rates, Currency currency)
        {
            foreach (var rate in rates)
            {
                if (rate.Currency == currency)
                    return rate.Value;
            }

            // если не нашли курс валют, то возращаем единицу как будто мы используем нацвалюту
            return 1;
        }

        /// <summary>
        /// Расчет комиссии
        /// </summary>
        /// <param name="tariffs">Коллекция тарифов</param>
        /// <param name="systemName">Наименование системы</param>
        public static decimal CalculateComission(TransferTariffs[] tariffs, string systemName)
        {
            foreach (var tariff in tariffs)
            {
                if (tariff.PaymentSystemName == systemName)
                    return tariff.Comission;
            }

            // если не нашли систему перевода, то используем комиссию по умолчанию
            return 40;
        }

        /// <summary>
        /// Отображение информации на экран
        /// </summary>
        /// <param name="transfer">Перевод</param>
        /// <param name="senderCustomer">Отправитель</param>
        /// <param name="receiverCustomer">Получатель</param>
        public static void ShowInfo(Transfer transfer, Customer senderCustomer, Customer receiverCustomer)
        {
            Console.WriteLine($"----------------{(transfer.PaymentDirection == PaymentDirection.Output ? "Отправка" : "Выплата")} перевода---------------");
            Console.WriteLine("------------------------------------------------");
            Console.WriteLine("-----------------Отправитель--------------------");
            Console.WriteLine("------------------------------------------------");
            Console.WriteLine($"{senderCustomer.CustomerName}");
            Console.WriteLine("------------------------------------------------");
            Console.WriteLine("------------------------------------------------");
            Console.WriteLine("-------------------Получатель-------------------");
            Console.WriteLine("------------------------------------------------");
            Console.WriteLine($"{receiverCustomer.CustomerName}");
            Console.WriteLine("------------------------------------------------");
            Console.WriteLine("--------------Информация о переводе-------------");
            Console.WriteLine("------------------------------------------------");
            Console.WriteLine($"Направление:    {(transfer.PaymentDirection == PaymentDirection.Output ? "Исходящий" : "Входящий")}");
            Console.WriteLine($"Валюта:         {transfer.Currency}");
            Console.WriteLine($"Система:        {transfer.TransferPaymentSystemName}");
            Console.WriteLine($"Статус:         {transfer.PaymentStatus}");
            Console.WriteLine($"Дата перевода:  {transfer.TransferDate.ToShortDateString()}");
            Console.WriteLine($"Сумма:          {transfer.Amount} {transfer.Currency}");
            Console.WriteLine($"Сумма (в нацвалюте): {transfer.AmountN} {Currency.KGS}");
            Console.WriteLine($"Комиссия:       {transfer.ComissionN} {Currency.KGS}");
            Console.WriteLine($"Общая сумма:    {transfer.TotalSumN} {Currency.KGS}");
            Console.WriteLine("------------------------------------------------");
            Console.WriteLine("------------------------------------------------");
        }
    }
}
