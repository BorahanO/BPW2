using UnityEngine;

public class InspectedObjects : MonoBehaviour
{
    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            ActivateNPC.objectsInspected += 1;
            Destroy(gameObject);
        }
    }
}
