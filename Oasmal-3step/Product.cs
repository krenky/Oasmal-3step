namespace Osmakov_Course_work
{
    class Product

    {
        public string Name { get; set; } // Название товара 
        public int Price { get; set; } // Цена товара 
        public Product Next { get; set; } // указатель на следующий товар
        public Product(string name, int price) // конструктор 
        {
            Name = name;
            Price = price;
        }
    }
}
