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
        if (dialogueUI.IsOpen || PauseMenu.isPaused)
        {
            FirstPersonController.playerCanMove = false;
            FirstPersonController.cameraCanMove = false;
            FirstPersonController.enableHeadBob = false;
        }
        else
        {
            FirstPersonController.playerCanMove = true;
            FirstPersonController.cameraCanMove = true;
            FirstPersonController.enableHeadBob = true;
        }

        if (Input.GetKeyDown(KeyCode.E) && dialogueUI.IsOpen == false)
        {
            Interactable?.Interact(this);
        }
    }
}
