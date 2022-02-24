using UnityEngine;

public class FullScreen : MonoBehaviour
{
    public void SetFullScreen()
    {
        Screen.fullScreen = !Screen.fullScreen;
    }
}
