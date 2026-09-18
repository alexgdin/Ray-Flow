using UnityEngine;

// Attach this alongside the Collider2D on any mirror, wall, prism, or other
// object you want to be selectable at runtime via ObjectManipulator.
// The three flags let a specific instance opt out of an action even when
// the global mode would otherwise allow it (e.g. a boundary wall that
// should never be deletable, even in Playground mode).
[RequireComponent(typeof(Collider2D))]
public class PlaceableObject : MonoBehaviour
{
    [Header("Allowed actions (per-instance override)")]
    public bool canMove = true;
    public bool canRotate = true;
    public bool canDelete = true;
}