using UnityEngine;
public class DetectedCollider : MonoBehaviour
{
    private void OnCollisionEnter(Collision other)
    {
        if (!other.gameObject.GetComponent<Checkpoint>())
        {
            Destroy(gameObject);
        }
    }   
}
