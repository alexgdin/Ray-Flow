using UnityEngine;

// Attach this to the same GameObject as the LineRenderer your ray script
// draws to. It adds a second, wider, additive-blended "glow" LineRenderer
// behind the core line, plus a gentle width pulse, so the beam reads as
// light instead of a flat colored line.
[RequireComponent(typeof(LineRenderer))]
public class LightRayFX : MonoBehaviour
{
    [Header("Materials (set these up per the setup steps)")]
    [SerializeField] private Material coreMaterial;
    [SerializeField] private Material glowMaterial;

    [Header("Look")]
    [SerializeField] private float glowWidthMultiplier = 3.5f;
    [SerializeField, Range(0f, 1f)] private float glowAlpha = 0.3f;

    [Header("Pulse animation")]
    [SerializeField] private float pulseSpeed = 3f;
    [SerializeField, Range(0f, 0.5f)] private float pulseAmount = 0.12f;

    private LineRenderer m_core;
    private LineRenderer m_glow;
    private float m_baseCoreWidth;
    private float m_baseGlowWidth;

    void Awake()
    {
        m_core = GetComponent<LineRenderer>();
        m_baseCoreWidth = m_core.widthMultiplier;

        if (coreMaterial != null)
        {
            m_core.material = coreMaterial;
            if (m_core.material.mainTexture == null)
                m_core.material.mainTexture = BuildGradientTexture();
            m_core.textureMode = LineTextureMode.Tile;
        }

        GameObject glowObj = new GameObject("RayGlow");
        glowObj.transform.SetParent(transform, false);
        m_glow = glowObj.AddComponent<LineRenderer>();
        m_glow.sortingOrder = m_core.sortingOrder - 1; // draw behind the core
        m_glow.numCapVertices = m_core.numCapVertices;

        m_baseGlowWidth = m_baseCoreWidth * glowWidthMultiplier;
        m_glow.widthMultiplier = m_baseGlowWidth;

        if (glowMaterial != null)
        {
            m_glow.material = glowMaterial;
            if (m_glow.material.mainTexture == null)
                m_glow.material.mainTexture = BuildGradientTexture();
            m_glow.textureMode = LineTextureMode.Tile;
        }

        Color glowColor = m_core.startColor;
        glowColor.a = glowAlpha;
        m_glow.startColor = glowColor;
        m_glow.endColor = glowColor;
    }

    // Call this every time your ray script updates the core LineRenderer's
    // positions (e.g. right after Draw2DRay in ThrowRay), so the glow line
    // and the pulse stay in sync with the current beam path.
    public void SyncGlow()
    {
        m_glow.positionCount = m_core.positionCount;
        for (int i = 0; i < m_core.positionCount; i++)
        {
            m_glow.SetPosition(i, m_core.GetPosition(i));
        }

        float pulse = 1f + Mathf.Sin(Time.time * pulseSpeed) * pulseAmount;
        m_core.widthMultiplier = m_baseCoreWidth * pulse;
        m_glow.widthMultiplier = m_baseGlowWidth * pulse;
    }

    // Soft edge-to-center falloff, sampled across the line's width.
    // Used automatically if you don't assign your own texture.
    static Texture2D BuildGradientTexture()
    {
        const int h = 32;
        var tex = new Texture2D(1, h, TextureFormat.RGBA32, false);
        tex.wrapMode = TextureWrapMode.Clamp;
        for (int y = 0; y < h; y++)
        {
            float t = Mathf.Abs(y / (float)(h - 1) * 2f - 1f); // 0 at center, 1 at edges
            float alpha = 1f - Mathf.SmoothStep(0f, 1f, t);
            tex.SetPixel(0, y, new Color(1f, 1f, 1f, alpha));
        }
        tex.Apply();
        return tex;
    }
}