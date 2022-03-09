using UnityEngine;

public class ActivateNPC : MonoBehaviour
{
    public static int objectsInspected;
    
    [SerializeField] 
    private GameObject interviewNPC;

    // Update is called once per frame
    void Update()
    {
        if (objectsInspected >= 2)
        {
            interviewNPC.SetActive(true);
        }
    }
}
