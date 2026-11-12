using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UIElements;

[RequireComponent(typeof(PanelRenderer))]
public class MainMenuScript : MonoBehaviour
{
    public static bool IsPaused { get; private set; }

    private PanelRenderer panelRenderer;
    private VisualElement mainMenuRoot;
    private int lastUIVersion = -1;

    void OnEnable()
    {
        panelRenderer = GetComponent<PanelRenderer>();
        panelRenderer.RegisterUIReloadCallback(OnUIReloaded);
        SetPaused(false);
    }

    void Start()
    {
        if (mainMenuRoot == null)
            Debug.LogWarning("PauseMenuController: UI reload callback never fired, buttons are not hooked up.");
    }

    void OnDisable()
    {
        panelRenderer.UnregisterUIReloadCallback(OnUIReloaded);
    }

    void OnUIReloaded(PanelRenderer renderer, VisualElement root, int version)
    {
        if (version == lastUIVersion) return;
        lastUIVersion = version;

        Debug.Log("PauseMenuController: UI loaded (version " + version + "), hooking up buttons");
        mainMenuRoot = root.Q<VisualElement>("mainmenu-root");
        root.Q<Button>("start-button").clicked += StartGame;
        root.Q<Button>("exit-button").clicked += Exit;
        SetPaused(IsPaused);
    }

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Escape) && !GameOverMenuController.IsGameOver)
            SetPaused(!IsPaused);
    }

    void StartGame()
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex + 1);
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

    void SetPaused(bool paused)
    {
        IsPaused = paused;
        Time.timeScale = paused ? 0f : 1f;
        AudioListener.pause = paused;
    }
}
