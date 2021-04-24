using System;
using HomeWorkWithEnumsAndDateTime.Enums;

namespace HomeWorkWithEnumsAndDateTime.Models
{
    /// <summary>
    /// Структура модели денежных переводов
    /// </summary>
    public struct Transfer
    {
        public PaymentDirection PaymentDirection;
        public PaymentStatus PaymentStatus;
        public string TransferPaymentSystemName;
        public Currency Currency;
        public DateTime TransferDate;
        public decimal Amount;
        public decimal AmountN;
        public decimal ComissionN;
        public decimal TotalSumN => AmountN + ComissionN;

        public Transfer(PaymentDirection direction, string systemName, 
            Currency currency, decimal amount, decimal amountN, decimal comissionN = 40)
        {
            PaymentDirection = direction;
            PaymentStatus = PaymentStatus.Created;
            TransferPaymentSystemName = systemName;
            Currency = currency;
            TransferDate = DateTime.Now;
            Amount = amount;
            AmountN = amountN;
            ComissionN = PaymentDirection.Output == direction ? comissionN : 0;
        }

        /// <summary>
        /// Валидация перевода
        /// </summary>
        /// <param name="transfer">Модель, содержащая данные о переводе</param>
        public void Validate(Transfer transfer)
        {
            if(transfer.PaymentStatus != PaymentStatus.Created)
                throw new Exception($"Невозможно выполнить операцию, перевод в статусе {transfer.PaymentStatus}");

            if(transfer.Amount <= 0)
                throw new Exception("Сумма перевода не может быть равна нулю или меньше нуля.");

            var isPaymentSystemExists = transfer.TransferPaymentSystemName == PaymentSystemName.Contact
                                           || transfer.TransferPaymentSystemName == PaymentSystemName.GoldenCrown
                                           || transfer.TransferPaymentSystemName == PaymentSystemName.Unistream;

            if (!isPaymentSystemExists)
                throw new Exception("Не найдена система переводов. Введите корректное значение. " +
                                    $"Текущее значение: {transfer.TransferPaymentSystemName}");
        }

        /// <summary>
        /// Отправка перевода
        /// </summary>
        /// <param name="transfer">Модель, содержащая данные о переводе</param>
        public Transfer SendTransfer(Transfer transfer)
        {
            transfer.PaymentStatus = PaymentStatus.Sent;
            Console.WriteLine("Перевод успешно отправлен");

            return transfer;
        }

        /// <summary>
        /// Выплата перевода
        /// </summary>
        /// <param name="transfer">Модель, содержащая данные о переводе</param>
        public Transfer PayoutTranfer(Transfer transfer)
        {
            transfer.PaymentStatus = PaymentStatus.Paid;
            Console.WriteLine("Перевод успешно выплачен");

            return transfer;
        }

        /// <summary>
        /// Отзыв перевода
        /// </summary>
        /// <param name="transfer">Модель, содержащая данные о переводе</param>
        public Transfer RecallTranfer(Transfer transfer)
        {
            transfer.PaymentStatus = PaymentStatus.Recalled;
            Console.WriteLine("Перевод успешно отозван");

            return transfer;
        }

        /// <summary>
        /// Аннулирование перевода
        /// </summary>
        /// <param name="transfer">Модель, содержащая данные о переводе</param>
        public Transfer CancelTranfer(Transfer transfer)
        {
            transfer.PaymentStatus = PaymentStatus.Canceled;
            Console.WriteLine("Перевод успешно аннулирован");

            return transfer;
        }
    }
}
