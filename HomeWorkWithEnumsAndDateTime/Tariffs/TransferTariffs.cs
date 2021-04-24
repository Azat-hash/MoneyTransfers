namespace HomeWorkWithEnumsAndDateTime.Tariffs
{
    /// <summary>
    /// Тарифы комиссий денежных систем
    /// </summary>
    public struct TransferTariffs
    {
        public string PaymentSystemName;
        public decimal Comission;

        public TransferTariffs(string systemName, decimal comission)
        {
            PaymentSystemName = systemName;
            Comission = comission;
        }
    }
}
