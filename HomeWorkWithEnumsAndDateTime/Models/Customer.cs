namespace HomeWorkWithEnumsAndDateTime.Models
{
    /// <summary>
    /// Структура модели клиента
    /// </summary>
    public struct Customer
    {
        /// <summary>
        /// Имя клиента
        /// </summary>
        public string Name;

        /// <summary>
        /// Фамилия клиента
        /// </summary>
        public string Surname;

        /// <summary>
        /// Полное ФИО клиента
        /// </summary>
        public string CustomerName => string.Concat(Name," ", Surname);

        public Customer(string name, string surname)
        {
            Name = name;
            Surname = surname;
        }
    }
}
