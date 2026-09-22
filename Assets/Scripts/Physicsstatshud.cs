using System.Text;
using UnityEngine;
using UnityEngine.UI; // swap for `using TMPro;` and Text -> TMP_Text if you use TextMeshPro

public class PhysicsStatsHUD : MonoBehaviour
{
    [SerializeField] private Text readout;
    [SerializeField] private int decimalPlaces = 1;

    private readonly StringBuilder m_sb = new StringBuilder();

    public void UpdateStats(RayStats stats)
    {
        if (readout == null) return;

        m_sb.Clear();
        m_sb.Append("Bounces: ").Append(stats.bounces.Count).AppendLine();
        m_sb.Append("Path length: ").Append(stats.totalPathLength.ToString("F" + decimalPlaces)).Append(" units").AppendLine();
        m_sb.Append("Target hit: ").Append(stats.hitTarget ? "yes" : "no").AppendLine();
        m_sb.AppendLine();

        for (int i = 0; i < stats.bounces.Count; i++)
        {
            BounceReading b = stats.bounces[i];
            m_sb.Append("B ").Append(i + 1)
                .Append("   \u03B8i = ").Append(b.incidenceAngle.ToString("F" + decimalPlaces)).Append("\u00B0")
                .Append("   \u03B8r = ").Append(b.reflectionAngle.ToString("F" + decimalPlaces)).Append("\u00B0")
                .AppendLine();
        }

        readout.text = m_sb.ToString();
    }
}