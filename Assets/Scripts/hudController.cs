using System.ComponentModel.Design.Serialization;
using UnityEngine;
using UnityEngine.UIElements;

public class hudController : MonoBehaviour
{
    [SerializeField] private GameManagerScript gameManager;
    private PanelRenderer panelRenderer;
    private int lastUIVersion;
    private Label scoreLabel;
    private Label highScoreLabel;

    void OnEnable()
    {
        panelRenderer = GetComponent<PanelRenderer>();
        panelRenderer.RegisterUIReloadCallback(OnUIReloaded);
    }

    void OnDisable()
    {
        panelRenderer.UnregisterUIReloadCallback(OnUIReloaded);
    }

    void OnUIReloaded(PanelRenderer renderer, VisualElement root, int version)
    {
        if (version == lastUIVersion) return;
        lastUIVersion = version;

        scoreLabel = root.Q<Label>("score-field");
        highScoreLabel = root.Q<Label>("highscore-field");
        int highScore = PlayerPrefs.GetInt("highscore");
        highScoreLabel.text = "Highscore: " + highScore;
    }

    void Update()
    {
        scoreLabel.text = "Score: " + gameManager.Score;
    }
}
