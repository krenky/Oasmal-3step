using System;
namespace Oasmal_3step
{
    class Shop
    {
        public int OrderCount { get; set; } // счетчик заказов 
        private Order[] orderArray; // массива заказов 
        public int ArraySize { get; set; } // размер массива
        internal Order[] OrderArray { get => orderArray; set => orderArray = value; }

        public Shop(int arraysize) // конструктор
        {
            OrderCount = 0;
            ArraySize = arraysize;
            OrderArray = new Order[ArraySize]; // создание массива
        }
        public bool AddOrder(string lastName) // добавление заказа 
        {
            Order order = new Order(lastName); // создание заказа
            OrderArray[OrderCount] = order; // добавление заказа в конец
            OrderCount++; // увеличение счетчика
            if (OrderCount == ArraySize) // если массив заполнен
            {
                ArraySize *= 2;
                Array.Resize(ref orderArray, ArraySize); // увеличиваем его размер на 2 
                return true;
            }
            return false;
        }
        public bool AddOrder(Order order) // добавление заказа 
        {
            OrderArray[OrderCount] = order; // добавление заказа в конец
            OrderCount++; // увеличение счетчика
            if (OrderCount == ArraySize) // если массив заполнен
            {
                ArraySize *= 2;
                Array.Resize(ref orderArray, ArraySize); // увеличиваем его размер на 2 
                return true;
            }
            return false;
        }
        public bool DeleteOrder() // удаление заказа
        {
            if (OrderCount != 0) // если массив не пуст
            {
                OrderArray[0] = null; // удаление заказа из начала
                for (int i = 1; i < OrderCount; i++) // сдвиг
                    OrderArray[i - 1] = OrderArray[i];
                OrderCount--; // уменьшение счетчика
                return true;
            }
            else return false;
        }
        public int FindOrder(string lastName) // Поиск заказа
        {
            for (int i = 0; i < OrderCount; i++) // проход по массиву
            {
                if (OrderArray[i].LastName == lastName)// если имена совпали 
                {
                    return i; // вовращаем индекс 
                }
            }
            return -1;
        }
        public Order FindOrder(Order order) // Поиск заказа
        {
            for (int i = 0; i < OrderCount; i++) // проход по массиву
            {
                if (OrderArray[i].LastName == order.LastName)// если имена совпали 
                {
                    return OrderArray[i]; // вовращаем 
                }
            }
            return null;
        }
        public int ShopSum() // Подсчет суммы 
        {
            int shopSum = 0;
            for (int i = 0; i < OrderCount; i++) // проход по массиву с подсчетом суммы
            {
                shopSum += OrderArray[i].OrderSum();
            }
            return shopSum;
        }
        public string AllData()  // вывод информации о магазине 
        {
            string data = null;
            for (int i = 0; i < OrderCount; i++) // проход по массиву с выводом информации
            {
                data += OrderArray[i].LastName + "    " + OrderArray[i].Date + "    " + OrderArray[i].ProductCount + "\n";
            }
            return data;
        }
    }
}
