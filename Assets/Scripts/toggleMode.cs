using UnityEngine;
using UnityEngine.UI;

public class toggleMode : MonoBehaviour
{
    [SerializeField] private Text modeText;

    public void UpdateModeText()
    {
        if (ObjectManipulator.Instance != null && ObjectManipulator.Instance.mode == ObjectManipulator.EditorMode.Playground)
            modeText.text = "Mode: Sandbox";
        else
            modeText.text = "Mode: Level";
    }
}
