namespace Equipments
{
    public class Equipment
    {
        public string Name { get; }
        public int Price { get; }

        public Equipment(string name, int price)
        {
            Name = name;
            Price = price;
        }
    }
}