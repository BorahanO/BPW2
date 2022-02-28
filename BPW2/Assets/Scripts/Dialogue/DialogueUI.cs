using TMPro;
using UnityEngine;
using System.Collections;

public class DialogueUI : MonoBehaviour
{
    [SerializeField] 
    private GameObject dialogueBox;
    [SerializeField] 
    private TMP_Text textLabel;

    private ResponseHandler responseHandler;
    private DialoguePerLetter dialoguePerLetter;
    
    public bool IsOpen { get; private set;  }

    private void Start()
    {
        dialoguePerLetter = GetComponent<DialoguePerLetter>();
        responseHandler = GetComponent<ResponseHandler>();
        
        CloseDialogueBox();
    }

    public void ShowDialogue(DialogueObject dialogueObject)
    {
        IsOpen = true;
        dialogueBox.SetActive(true);
        StartCoroutine(StepThroughDialogue(dialogueObject));
    }

    public void AddResponseevents(ResponseEvent[] responseEvents)
    {
        responseHandler.AddResponseevents(responseEvents);
        Cursor.visible = true;
    }
    private IEnumerator StepThroughDialogue(DialogueObject dialogueObject)
    {
        for (int i = 0; i < dialogueObject.Dialogue.Length; i++)
        {
            string dialogue = dialogueObject.Dialogue[i];

            yield return RunTypingEffect(dialogue);

            textLabel.text = dialogue;

            if (i == dialogueObject.Dialogue.Length - 1 && dialogueObject.HasResponses) break;

            yield return null;

            yield return new WaitUntil(() => Input.GetKeyDown(KeyCode.Space));
        }

        if (dialogueObject.HasResponses)
        {
            responseHandler.ShowResponses(dialogueObject.Responses);
        }
        else
        {
            CloseDialogueBox();
            Cursor.visible = false;
        }
    }

    private IEnumerator RunTypingEffect(string dialogue)
    {
        dialoguePerLetter.Run(dialogue, textLabel);

        while (dialoguePerLetter.IsRunning)
        {
            yield return null;

            if (Input.GetKeyDown(KeyCode.Space))
            {
                dialoguePerLetter.Stop();
            }
        }
    }

    public void CloseDialogueBox()
    {
        IsOpen = false;
        dialogueBox.SetActive(false);
        textLabel.text = string.Empty;
    }
}
