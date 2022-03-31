using UnityEngine;

public class ActivateMurderer : MonoBehaviour
{
    [SerializeField] 
    private GameObject CreepySpookyPerson;
    [SerializeField] 
    private GameObject ShopPerson;

    private void OnTriggerEnter(Collider other)
    {
        CreepySpookyPerson.SetActive(true);
        ShopPerson.SetActive(true);
    }
}
