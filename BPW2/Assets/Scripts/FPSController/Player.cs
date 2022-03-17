using UnityEngine;

public class Player : MonoBehaviour
{
    [SerializeField] 
    private GameObject DialogueBox;
    [SerializeField] 
    private GameObject InteractionPrompt;

    // Update is called once per frame
    void Update()
    {
        if (DialogueBox.activeSelf || PauseMenu.isPaused)
        {
            FirstPersonController.playerCanMove = false;
            FirstPersonController.cameraCanMove = false;
            FirstPersonController.enableHeadBob = false;
            InteractionPrompt.SetActive(false);
            Cursor.visible = true;
        }
        else
        {
            FirstPersonController.playerCanMove = true;
            FirstPersonController.cameraCanMove = true;
            FirstPersonController.enableHeadBob = true;
            InteractionPrompt.SetActive(true);
            Cursor.visible = false;
        }
    }
}
