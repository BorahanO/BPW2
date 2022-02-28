using UnityEngine;

public class Player : MonoBehaviour
{
    [SerializeField]
    private DialogueUI dialogueUI;

    public DialogueUI DialogueUI => dialogueUI;
    
    public IInteractable Interactable { get; set; }

    // Update is called once per frame
    void Update()
    {
        if (dialogueUI.IsOpen)
        {
            FirstPersonController.playerCanMove = false;
            FirstPersonController.cameraCanMove = false;
        }
        else
        {
            FirstPersonController.playerCanMove = true;
            FirstPersonController.cameraCanMove = true;
        }
        
        if (Input.GetKeyDown(KeyCode.E) && dialogueUI.IsOpen == false)
        {
            Interactable?.Interact(this);
        }
    }
}
