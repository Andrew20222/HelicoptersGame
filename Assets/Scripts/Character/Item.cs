using UnityEngine;

public class Item : MonoBehaviour
{
    [SerializeField] private int priceCharacter;
    public int PriceCharacter => priceCharacter;
    [SerializeField] private float speed;
    public float Speed => speed;
    [SerializeField] private float weight;
    public float Weight => weight;
    
}
