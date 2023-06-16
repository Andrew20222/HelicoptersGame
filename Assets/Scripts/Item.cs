using TMPro;
using UnityEngine;

public class Item : MonoBehaviour
{
    public Shop shop;
    public string NameItem { get; set; }
    public int PriceItem { get; set; }

    public TMP_Text textPrice;

    public bool isBuy;

    public void BuyItem()
    {
        if (!isBuy)
        {
            shop.NameItem = NameItem;
            shop.PriceItem = PriceItem;
        
            shop.BuyItem();
        }
    }
}
