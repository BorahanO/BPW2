using UnityEngine;
using Yarn.Unity;

public class YarnInteractable : MonoBehaviour {
    // internal properties exposed to editor
    [SerializeField] private string conversationStartNode;
    [SerializeField] private AudioSource InterrogationMusic;

    // internal properties not exposed to editor
    private DialogueRunner dialogueRunner;
    private Light lightIndicatorObject = null;
    private bool interactable = true;
    private bool isCurrentConversation = false;
    private float defaultIndicatorIntensity;
    
    public void Start() {
        dialogueRunner = FindObjectOfType<Yarn.Unity.DialogueRunner>();
        dialogueRunner.onDialogueComplete.AddListener(EndConversation);
        lightIndicatorObject = GetComponentInChildren<Light>();
        // get starter intensity of light then
        // if we're using it as an indicator => hide it 
        if (lightIndicatorObject != null) {
            defaultIndicatorIntensity = lightIndicatorObject.intensity;
            lightIndicatorObject.intensity = 0;
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if (interactable && !dialogueRunner.IsDialogueRunning) {
            StartConversation();
        }
    }

    void OnTriggerExit(Collider other)
    {
    }

    private void StartConversation() {
        InterrogationMusic.Play();
        Cursor.visible = true;
        Debug.Log($"Started conversation with {name}.");
        isCurrentConversation = true;
        // if (lightIndicatorObject != null) {
        //     lightIndicatorObject.intensity = defaultIndicatorIntensity;
        // }
        dialogueRunner.StartDialogue(conversationStartNode);
        FirstPersonController.playerCanMove = false;
        FirstPersonController.cameraCanMove = false;
        FirstPersonController.enableHeadBob = false;
        FirstPersonController.sprintSpeed = 0f;
        FirstPersonController.walkSpeed = 0f;
    }

    private void EndConversation() {
        Cursor.visible = false;
        if (isCurrentConversation) {
            // if (lightIndicatorObject != null) {
            //     lightIndicatorObject.intensity = 0;
            // }
            isCurrentConversation = false;
            Debug.Log($"Started conversation with {name}.");
            FirstPersonController.playerCanMove = true;
            FirstPersonController.cameraCanMove = true;
            FirstPersonController.enableHeadBob = true;
            FirstPersonController.sprintSpeed = 7f;
            FirstPersonController.walkSpeed = 5f;
            InterrogationMusic.Stop();
        }
    }
    
    [YarnCommand("disable")]
    public void DisableConversation() {
        interactable = false;
    }
    
}