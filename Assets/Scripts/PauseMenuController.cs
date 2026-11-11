using UnityEngine;
using UnityEngine.UIElements;

[RequireComponent(typeof(PanelRenderer))]
public class PauseMenuController : MonoBehaviour
{
    public static bool IsPaused { get; private set; }

    private PanelRenderer panelRenderer;
    private VisualElement pauseRoot;
    private int lastUIVersion = -1;

    void OnEnable()
    {
        panelRenderer = GetComponent<PanelRenderer>();
        panelRenderer.RegisterUIReloadCallback(OnUIReloaded);
        SetPaused(false);
    }

    void Start()
    {
        if (pauseRoot == null)
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
        pauseRoot = root.Q<VisualElement>("pause-root");
        root.Q<Button>("resume-button").clicked += Resume;
        root.Q<Button>("exit-button").clicked += Exit;
        SetPaused(IsPaused);
    }

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Escape))
            SetPaused(!IsPaused);
    }

    void Resume() => SetPaused(false);

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
        if (pauseRoot != null)
            pauseRoot.style.display = paused ? DisplayStyle.Flex : DisplayStyle.None;
    }
}
