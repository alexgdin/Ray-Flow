using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class ObjectManipulator : MonoBehaviour
{
    // Playground: free sandbox, Add/Delete available.
    // LevelPlay: fixed layout, only Move/Rotate available (Add + Delete switched off).
    public enum EditorMode { Playground, LevelPlay }

    [System.Serializable]
    public class SpawnableEntry
    {
        public string id;      // referenced by SpawnById(...) from a button's OnClick
        public GameObject prefab;
    }

    public static ObjectManipulator Instance { get; private set; }

    [Header("Mode")]
    [SerializeField] public EditorMode mode = EditorMode.Playground;

    [Header("Selection")]
    [SerializeField] private LayerMask selectableLayer = ~0; // set to your mirror/wall/prism layer for best results
    [SerializeField] private Color selectedTint = new Color(1f, 1f, 0.6f);

    [Header("Rotation")]
    [SerializeField] private float rotationStep = 15f; // degrees per button press

    [Header("Spawning")]
    [SerializeField] private List<SpawnableEntry> spawnables = new List<SpawnableEntry>();
    [SerializeField] private Vector2 spawnPoint = Vector2.zero;
    [SerializeField] private GameObject addToolbar; // whole "Add" panel; hidden automatically outside Playground mode

    [Header("Floating action panel (shown above the selected object)")]
    [SerializeField] private RectTransform actionPanel;
    [SerializeField] private Vector2 panelWorldOffset = new Vector2(0f, 0.75f);
    [SerializeField] private Button deleteButton;

    private PlaceableObject m_selected;
    private SpriteRenderer m_selectedRenderer;
    private Color m_originalColor;
    private bool m_moveMode;
    private Camera m_cam;

    void Awake()
    {
        Instance = this;
        m_cam = Camera.main;
        ApplyMode();
        if (actionPanel != null) actionPanel.gameObject.SetActive(false);
    }

    void Update()
    {
        HandleSelectionClick();

        if (m_moveMode && m_selected != null && m_selected.canMove)
        {
            Vector3 mouseWorld = m_cam.ScreenToWorldPoint(Input.mousePosition);
            m_selected.transform.position = new Vector3(mouseWorld.x, mouseWorld.y, m_selected.transform.position.z);
        }

        UpdateActionPanelPosition();
    }

    public void ApplyMode()
    {
        bool sandbox = mode == EditorMode.Playground;
        if (addToolbar != null) addToolbar.SetActive(sandbox);
        if (deleteButton != null) deleteButton.interactable = sandbox && m_selected != null && m_selected.canDelete;
    }

    public void SetMode(EditorMode newMode)
    {
        mode = newMode;
        ApplyMode();
    }

    void HandleSelectionClick()
    {
        if (!Input.GetMouseButtonDown(0)) return;
        if (EventSystem.current != null && EventSystem.current.IsPointerOverGameObject()) return; // ignore clicks on UI

        if (m_moveMode)
        {
            SetMoveMode(false); // click again to drop whatever you're dragging
            return;
        }

        Vector2 worldPos = m_cam.ScreenToWorldPoint(Input.mousePosition);
        Collider2D hit = Physics2D.OverlapPoint(worldPos, selectableLayer);

        if (hit != null && hit.TryGetComponent(out PlaceableObject placeable))
        {
            Select(placeable);
        }
        else
        {
            Deselect();
        }
    }

    public void Select(PlaceableObject obj)
    {
        Deselect();
        m_selected = obj;

        if (m_selected.TryGetComponent(out SpriteRenderer sr))
        {
            m_selectedRenderer = sr;
            m_originalColor = sr.color;
            sr.color = selectedTint;
        }

        if (actionPanel != null) actionPanel.gameObject.SetActive(true);
        if (deleteButton != null) deleteButton.interactable = mode == EditorMode.Playground && m_selected.canDelete;
    }

    public void Deselect()
    {
        if (m_selectedRenderer != null) m_selectedRenderer.color = m_originalColor;
        m_selected = null;
        m_selectedRenderer = null;
        m_moveMode = false;
        if (actionPanel != null) actionPanel.gameObject.SetActive(false);
    }

    void UpdateActionPanelPosition()
    {
        if (actionPanel == null || m_selected == null) return;
        Vector3 worldPos = m_selected.transform.position + (Vector3)panelWorldOffset;
        // Assumes the Canvas Render Mode is Screen Space - Overlay.
        // For Screen Space - Camera or World Space canvases this needs different math.
        actionPanel.position = m_cam.WorldToScreenPoint(worldPos);
    }

    // ---------- Hook these up to your UI Buttons' OnClick() ----------

    public void OnMoveButtonPressed()
    {
        if (m_selected == null || !m_selected.canMove) return;
        SetMoveMode(!m_moveMode);
    }

    void SetMoveMode(bool enabled) => m_moveMode = enabled;

    public void OnRotateButtonPressed()
    {
        if (m_selected == null || !m_selected.canRotate) return;
        m_selected.transform.Rotate(0f, 0f, -rotationStep);
    }

    public void OnRotateCCWButtonPressed()
    {
        if (m_selected == null || !m_selected.canRotate) return;
        m_selected.transform.Rotate(0f, 0f, rotationStep);
    }

    public void OnDeleteButtonPressed()
    {
        if (m_selected == null || !m_selected.canDelete || mode != EditorMode.Playground) return;
        GameObject toDelete = m_selected.gameObject;
        Deselect();
        Destroy(toDelete);
    }

    // Wire a button's OnClick() to this with a string argument matching
    // a SpawnableEntry.id you set up in the Spawnables list below.
    public void SpawnById(string id)
    {
        if (mode != EditorMode.Playground) return; // adding is a Playground-only feature

        SpawnableEntry entry = spawnables.Find(e => e.id == id);
        if (entry == null || entry.prefab == null) return;

        GameObject instance = Instantiate(entry.prefab, spawnPoint, Quaternion.identity);
        if (instance.TryGetComponent(out PlaceableObject placeable))
        {
            Select(placeable);
            SetMoveMode(true); // drop straight into move mode so you can place it immediately
        }
    }
}