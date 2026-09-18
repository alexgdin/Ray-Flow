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
        UnityEngine.SceneManagement.SceneManager.LoadScene("Main Menu");
    }

    public void pauseGame()
    {
        Time.timeScale = 0f;
    }

    public void resumeGame()
    {
        Time.timeScale = 1f;
    }

    public void modeToggle()
    {

        if (ObjectManipulator.Instance != null)
        {
            if (ObjectManipulator.Instance.mode == ObjectManipulator.EditorMode.Playground)
            {
                ObjectManipulator.Instance.mode = ObjectManipulator.EditorMode.LevelPlay;
                ObjectManipulator.Instance.ApplyMode();
            }
            else
            {
                ObjectManipulator.Instance.mode = ObjectManipulator.EditorMode.Playground;
                ObjectManipulator.Instance.ApplyMode();
            }
        }
    }
}
