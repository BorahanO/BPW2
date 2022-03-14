using UnityEngine;

//Item inspect tutorial from SpeedTutor on YT https://www.youtube.com/watch?v=Qfcu1QPGkMA
public class ObjectController : MonoBehaviour
{
    [SerializeField] private string objectName;

    [TextArea] [SerializeField] private string extraInfo;

    [SerializeField] private InspectController inspectController;

    public void ShowObjectName()
    {
        inspectController.ShowName(objectName);
    }

    public void HideObjectName()
    {
        inspectController.HideName();
    }

    public void ShowExtraInfo()
    {
        inspectController.ShowAdditionalInfo(extraInfo);
    }
}
