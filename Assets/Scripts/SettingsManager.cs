using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class SettingsManager : MonoBehaviour
{
    [Header("Video Settings UI (TextMeshPro)")]
    public TMP_Dropdown resolutionDropdown;
    public TMP_Dropdown fullscreenModeDropdown;
    public TMP_Dropdown graphicsQualityDropdown;
    public Slider brightnessSlider;
    public TMP_Dropdown fpsLimitDropdown;
    public TMP_Dropdown vsyncDropdown;

    private Resolution[] resolutions;

    void Start()
    {
        // Fetch available screen resolutions
        resolutions = Screen.resolutions;
        PopulateResolutionDropdown();

        // Fill fullscreen mode dropdown
        PopulateFullscreenModeDropdown();

        // Fill VSync dropdown
        PopulateVSyncDropdown();

        // Load saved settings
        LoadSettings();
    }

    void PopulateResolutionDropdown()
    {
        resolutionDropdown.ClearOptions();
        List<string> options = new List<string>();

        int currentResolutionIndex = 0;
        for (int i = 0; i < resolutions.Length; i++)
        {
            string resolutionOption = resolutions[i].width + " x " + resolutions[i].height;
            options.Add(resolutionOption);

            if (resolutions[i].width == Screen.currentResolution.width &&
                resolutions[i].height == Screen.currentResolution.height)
            {
                currentResolutionIndex = i;
            }
        }

        resolutionDropdown.AddOptions(options);
        resolutionDropdown.value = currentResolutionIndex;
        resolutionDropdown.RefreshShownValue();
    }

    void PopulateFullscreenModeDropdown()
    {
        fullscreenModeDropdown.ClearOptions();
        fullscreenModeDropdown.AddOptions(new List<string> { "Fullscreen", "Windowed", "Borderless" });
        fullscreenModeDropdown.value = (int)Screen.fullScreenMode;
        fullscreenModeDropdown.RefreshShownValue();
    }

    void PopulateVSyncDropdown()
    {
        vsyncDropdown.ClearOptions();
        vsyncDropdown.AddOptions(new List<string> { "Off", "On" });
        vsyncDropdown.value = QualitySettings.vSyncCount > 0 ? 1 : 0;
        vsyncDropdown.RefreshShownValue();
    }

    public void SetResolution(int resolutionIndex)
    {
        Resolution resolution = resolutions[resolutionIndex];
        Screen.SetResolution(resolution.width, resolution.height, Screen.fullScreenMode);
    }

    public void SetFullscreenMode(int modeIndex)
    {
        switch (modeIndex)
        {
            case 0: // Fullscreen
                Screen.fullScreenMode = FullScreenMode.FullScreenWindow;
                break;
            case 1: // Windowed
                Screen.fullScreenMode = FullScreenMode.Windowed;
                break;
            case 2: // Borderless
                Screen.fullScreenMode = FullScreenMode.MaximizedWindow;
                break;
        }
    }

    public void SetGraphicsQuality(int qualityIndex)
    {
        QualitySettings.SetQualityLevel(qualityIndex);
    }

    public void SetBrightness(float brightness)
    {
        RenderSettings.ambientLight = Color.white * brightness;
    }

    public void SetFpsLimit(int fpsIndex)
    {
        int[] fpsLimits = { 30, 60, 120, -1 }; // -1 = Unlimited
        Application.targetFrameRate = fpsLimits[fpsIndex];
    }

    public void SetVSync(int vsyncIndex)
    {
        QualitySettings.vSyncCount = vsyncIndex;
    }

    public void SaveSettings()
    {
        PlayerPrefs.SetInt("Resolution", resolutionDropdown.value);
        PlayerPrefs.SetInt("FullscreenMode", fullscreenModeDropdown.value);
        PlayerPrefs.SetInt("GraphicsQuality", graphicsQualityDropdown.value);
        PlayerPrefs.SetFloat("Brightness", brightnessSlider.value);
        PlayerPrefs.SetInt("FpsLimit", fpsLimitDropdown.value);
        PlayerPrefs.SetInt("VSync", vsyncDropdown.value);
    }

    public void LoadSettings()
    {
        resolutionDropdown.value = PlayerPrefs.GetInt("Resolution", 0);
        fullscreenModeDropdown.value = PlayerPrefs.GetInt("FullscreenMode", 0);
        graphicsQualityDropdown.value = PlayerPrefs.GetInt("GraphicsQuality", 2);
        brightnessSlider.value = PlayerPrefs.GetFloat("Brightness", 1.0f);
        fpsLimitDropdown.value = PlayerPrefs.GetInt("FpsLimit", 1);
        vsyncDropdown.value = PlayerPrefs.GetInt("VSync", 1);

        // Apply loaded settings
        SetResolution(resolutionDropdown.value);
        SetFullscreenMode(fullscreenModeDropdown.value);
        SetGraphicsQuality(graphicsQualityDropdown.value);
        SetBrightness(brightnessSlider.value);
        SetFpsLimit(fpsLimitDropdown.value);
        SetVSync(vsyncDropdown.value);
    }
}
