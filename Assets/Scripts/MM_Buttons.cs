using UnityEngine;
using System.Collections.Generic;

public class MM_Buttons : MonoBehaviour
{

    public void SandBox()
    {
        UnityEngine.SceneManagement.SceneManager.LoadScene("Sandbox");
    }

    public void MainMenu()
    {
        UnityEngine.SceneManagement.SceneManager.LoadScene("MainMenu");
    }

}
