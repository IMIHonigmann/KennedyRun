using UnityEngine;
using UnityEngine.Rendering;
using UnityEngine.Rendering.Universal;
using UnityEngine.SceneManagement;
using UnityEngine.UIElements;

[RequireComponent(typeof(PanelRenderer))]
public class GameOverMenuController : MonoBehaviour
{
    [SerializeField] private Volume volume;
    private Vignette vignette;

    public static bool IsGameOver { get; private set; }
    private PanelRenderer panelRenderer;
    private VisualElement gameOverRoot;
    private Label scoreLabel;
    private int lastUIVersion = -1;
    private int finalScore;

    void Awake()
    {
        if (!volume.profile.TryGet<Vignette>(out vignette))
            Debug.LogWarning("Volume profile has no Vignette override");
    }

    void OnEnable()
    {
        panelRenderer = GetComponent<PanelRenderer>();
        panelRenderer.RegisterUIReloadCallback(OnUIReloaded);
        SetGameOver(false);
    }

    void OnDisable()
    {
        panelRenderer.UnregisterUIReloadCallback(OnUIReloaded);
    }

    void OnUIReloaded(PanelRenderer renderer, VisualElement root, int version)
    {
        if (version == lastUIVersion) return;
        lastUIVersion = version;

        gameOverRoot = root.Q<VisualElement>("gameover-root");
        scoreLabel = root.Q<Label>("score-field");
        root.Q<Button>("restart-button").clicked += Restart;
        root.Q<Button>("exit-button").clicked += Exit;
        SetGameOver(IsGameOver);
    }

    public void ShowGameOver(int score)
    {
        finalScore = score;
        SetGameOver(true);
        Debug.Log("GAME IS OVER!!!");
    }

    void Restart()
    {
        Time.timeScale = 1f;
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
    }

    void Exit()
    {
        Time.timeScale = 1f;
#if UNITY_EDITOR
        UnityEditor.EditorApplication.isPlaying = false;
#else
        Application.Quit();
#endif
    }

    void SetGameOver(bool gameOver)
    {
        IsGameOver = gameOver;
        Time.timeScale = gameOver ? 0f : 1f;
        AudioListener.pause = gameOver;

        if (vignette != null)
            vignette.intensity.Override(gameOver ? 0.5f : 0f);


        if (scoreLabel != null)
            scoreLabel.text = "Your Score: " + finalScore;
        if (gameOverRoot != null)
            gameOverRoot.style.display = gameOver ? DisplayStyle.Flex : DisplayStyle.None;

        int lastHighscore = PlayerPrefs.GetInt("highscore");
        if (finalScore > lastHighscore)
        {
            PlayerPrefs.SetInt("highscore", finalScore);
        }
    }
}
