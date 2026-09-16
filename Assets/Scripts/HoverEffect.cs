using UnityEngine;

public class HoverEffect : MonoBehaviour
{
    public void OnMouseEnter(GameObject obj)
    {
        // Change the color of the object to indicate hover
        obj.GetComponent<Renderer>().material.color = Color.yellow;
    }

    public void OnMouseExit(GameObject obj)
    {
        // Revert the color of the object when the mouse exits
        obj.GetComponent<Renderer>().material.color = Color.white;
    }
}
