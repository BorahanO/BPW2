using UnityEngine;
using UnityEngine.SceneManagement;
using Yarn.Unity;

public class MainMenu : MonoBehaviour
{
   public void PlayGame()
   {
      SceneManager.LoadScene("CrimeScene");
   }
   
   [YarnCommand("Loadwin")]
   public void LoadWinScene()
   {
      SceneManager.LoadScene("WinScene");
   }
   
   public void QuitGame()
   {
      Application.Quit();
   }
}
