using System.ComponentModel.Design.Serialization;
using UnityEngine;
using UnityEngine.UIElements;

public class hudController : MonoBehaviour
{
    [SerializeField] private GameManagerScript gameManager;
    private PanelRenderer panelRenderer;
    private int lastUIVersion;
    private Label scoreLabel;

    void OnEnable()
    {
        panelRenderer = GetComponent<PanelRenderer>();
        panelRenderer.RegisterUIReloadCallback(OnUIReloaded);
    }

    void OnUIReloaded(PanelRenderer renderer, VisualElement root, int version)
    {
        if (version == lastUIVersion) return;
        lastUIVersion = version;

        scoreLabel = root.Q<Label>("score-field");
    }

    void Update()
    {
        scoreLabel.text = "Score: " + gameManager.Score;
    }
}
