/**
*   Authored by Tomasz Piowczyk
*   MIT LICENSE: https://github.com/Prastiwar/TPFramework/blob/master/LICENSE
*   Repository: https://github.com/Prastiwar/TPFramework 
*/
using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.CompilerServices;
using UnityEngine;
using UnityEngine.Audio;
using UnityEngine.UI;

namespace TPFramework
{
#if UNITY_EDITOR
    [UnityEditor.InitializeOnLoad]
#endif 
    public static class TPSettings
    {
#if UNITY_EDITOR
        static TPSettings()
        {
            string[] qualities = QualitySettings.names;
            int length = qualities.Length;
            customQuality.CustomOption = new Dropdown.OptionData("Custom");
            for (int i = 0; i < length; i++)
            {
                if (qualities[i] == customQuality.CustomOption.text)
                {
                    customQuality.CustomQualityIndex = i;
                    break;
                }
                else if (i == length - 1)
                {
                    customQuality.CustomQualityIndex = -1;
                    Debug.LogError("No 'Custom' quality level found. Create one!");
                }
            }
        }
#endif
        [Serializable]
        public struct QualityLevel
        {
            public bool VSync;
            public bool FullScreen;
            public int Antialiasing;
            public int MasterTextureLimit;
            public ShadowQuality ShadowQuality;
            public ShadowResolution ShadowResolution;
            public AnisotropicFiltering AnisotropicFiltering;
            public Resolution Resolution;
        }

        [Serializable]
        public struct AudioParameters
        {
            public bool IsMusicOn;
            public bool IsSfxOn;
            public float MusicMixerFloat;
            public float SfxMixerFloat;
        }

        [Serializable]
        private struct CustomQuality
        {
            public int CustomQualityIndex;
            public Dropdown.OptionData CustomOption;
        }

        public static AudioParameters AudioSettings;

        private static readonly CustomQuality customQuality;
        private static readonly List<string> textureOptions          = new List<string> { "Very High", "High", "Medium", "Low" };
        private static readonly List<string> shadowQualityOptions    = new List<string> { "Disable", "HardOnly", "All" };
        private static readonly List<string> shadowResolutionOptions = new List<string> { "Low", "Medium", "High", "VeryHigh" };
        private static readonly List<string> antialiasingOptions     = new List<string> { "Disabled", "2x Multi Sampling", "4x Multi Sampling", "8x Multi Sampling" };
        private static readonly List<string> resolutionOptions       = new List<string>(Screen.resolutions.ToStringWithtHZ());
        private static List<string> qualityOptions {
            get {
                List<string> list = QualitySettings.names.ToList();
                list.Remove("Custom");
                return list;
            }
        }
        private static Action refreshSettings = delegate { };
        private static Action onCustomQualitySet = delegate { };


        [MethodImpl((MethodImplOptions)0x100)] // agressive inline
        public static void SetMusicToggler(Toggle toggle, AudioMixer audioMixer, string exposedParam, bool startValue = true)
        {
            toggle.onValueChanged.AddListener((value) => {
                AudioSettings.IsMusicOn = value;

                if (!AudioSettings.IsMusicOn) // cache float value before setting new value
                    AudioSettings.MusicMixerFloat = audioMixer.GetFloat(exposedParam);

                audioMixer.SetFloat(exposedParam, AudioSettings.IsMusicOn ? AudioSettings.MusicMixerFloat : -80);
            });
            toggle.isOn = startValue;
        }

        [MethodImpl((MethodImplOptions)0x100)] // agressive inline
        public static void SetSoundFXToggler(Toggle toggle, AudioMixer audioMixer, string exposedParam, bool startValue = true)
        {
            toggle.onValueChanged.AddListener((value) => {
                AudioSettings.IsSfxOn = value;

                if (!AudioSettings.IsSfxOn) // cache float value before setting new value
                    AudioSettings.SfxMixerFloat = audioMixer.GetFloat(exposedParam);

                audioMixer.SetFloat(exposedParam, AudioSettings.IsSfxOn ? AudioSettings.SfxMixerFloat : -80);
            });
            toggle.isOn = startValue;
        }

        [MethodImpl((MethodImplOptions)0x100)] // agressive inline
        public static void SetMusicToggler(Button button, Image image, Sprite musicOff, Sprite musicOn, AudioMixer audioMixer, string exposedParam, bool startValue = true)
        {
            AudioSettings.IsMusicOn = !startValue;
            button.onClick.AddListener(() => {
                AudioSettings.IsMusicOn = !AudioSettings.IsMusicOn;
                image.sprite = AudioSettings.IsMusicOn ? musicOn : musicOff;

                if (!AudioSettings.IsMusicOn) // cache float value before setting new value
                    AudioSettings.MusicMixerFloat = audioMixer.GetFloat(exposedParam);

                audioMixer.SetFloat(exposedParam, AudioSettings.IsMusicOn ? AudioSettings.MusicMixerFloat : -80);
            });
        }

        [MethodImpl((MethodImplOptions)0x100)] // agressive inline
        public static void SetSoundFXToggler(Button button, Image image, Sprite sfxOff, Sprite sfxOn, AudioMixer audioMixer, string exposedParam, bool startValue = true)
        {
            AudioSettings.IsSfxOn = !startValue;
            button.onClick.AddListener(() => {
                AudioSettings.IsSfxOn = !AudioSettings.IsSfxOn;
                image.sprite = AudioSettings.IsSfxOn ? sfxOn : sfxOff;

                if (!AudioSettings.IsSfxOn) // cache float value before setting new value
                    AudioSettings.SfxMixerFloat = audioMixer.GetFloat(exposedParam);

                audioMixer.SetFloat(exposedParam, AudioSettings.IsSfxOn ? AudioSettings.SfxMixerFloat : -80);
            });
        }

        [MethodImpl((MethodImplOptions)0x100)] // agressive inline
        public static void SetMusicVolumeSlider(Slider slider, AudioMixer audioMixer, string exposedParam, float startValue = 1, float minValue = -60, float maxValue = 25)
        {
            slider.onValueChanged.AddListener((value) => {
                audioMixer.SetFloat(exposedParam, value);
                AudioSettings.MusicMixerFloat = audioMixer.GetFloat(exposedParam);
            });
            SetSliderValues(slider, startValue, minValue, maxValue);
        }

        [MethodImpl((MethodImplOptions)0x100)] // agressive inline
        public static void SetSoundFXVolumeSlider(Slider slider, AudioMixer audioMixer, string exposedParam, float startValue = 1, float minValue = -60, float maxValue = 25)
        {
            slider.onValueChanged.AddListener((value) => {
                audioMixer.SetFloat(exposedParam, value);
                AudioSettings.SfxMixerFloat = audioMixer.GetFloat(exposedParam);
            });
            SetSliderValues(slider, startValue, minValue, maxValue);
        }

        [MethodImpl((MethodImplOptions)0x100)] // agressive inline
        public static void SetFullScreenToggler(Toggle toggle, bool startValue = false)
        {
            toggle.onValueChanged.AddListener((boolean) => {
                Screen.fullScreen = boolean;
            });
            toggle.isOn = startValue;
        }

        [MethodImpl((MethodImplOptions)0x100)] // agressive inline
        public static void SetVSyncToggler(Toggle toggle, bool startValue = false)
        {
            toggle.onValueChanged.AddListener((boolean) => {
                QualitySettings.vSyncCount = boolean.ToInt();
            });
            toggle.isOn = startValue;
        }

        [MethodImpl((MethodImplOptions)0x100)] // agressive inline
        public static void SetAnisotropicToggler(Toggle toggle, bool startValue = false)
        {
            toggle.onValueChanged.AddListener((value) => {
                QualitySettings.anisotropicFiltering = (AnisotropicFiltering)(value ? 2 : 0);
            });
            toggle.isOn = startValue;
        }

        [MethodImpl((MethodImplOptions)0x100)] // agressive inline
        public static void SetResolutionDropdown(Dropdown dropdown, int startIndex = 0, List<string> options = null)
        {
            AddDropdownOptions(dropdown, startIndex, options ?? resolutionOptions);

            dropdown.onValueChanged.AddListener((index) => {
                Resolution resolution = options != null ? options[index].ToResolution() : Screen.resolutions[index];
                Screen.SetResolution(resolution.width, resolution.height, Screen.fullScreen);
            });
        }

        [MethodImpl((MethodImplOptions)0x100)] // agressive inline
        public static void SetTextureDropdown(Dropdown dropdown, int startIndex = 0, List<string> options = null)
        {
            AddDropdownOptions(dropdown, startIndex, options ?? textureOptions);
            dropdown.onValueChanged.AddListener(SetToCustomLevel);
            dropdown.onValueChanged.AddListener(SetTexture);
            refreshSettings += () => DropdownRefresher(dropdown, () => dropdown.value = QualitySettings.masterTextureLimit);
        }

        [MethodImpl((MethodImplOptions)0x100)] // agressive inline
        public static void SetShadowQualityDropdown(Dropdown dropdown, int startIndex = 0)
        {
            AddDropdownOptions(dropdown, startIndex, shadowQualityOptions);
            dropdown.onValueChanged.AddListener(SetToCustomLevel);
            dropdown.onValueChanged.AddListener(SetShadowQuality);
            refreshSettings += () => DropdownRefresher(dropdown, () => dropdown.value = (int)QualitySettings.shadows);
        }

        [MethodImpl((MethodImplOptions)0x100)] // agressive inline
        public static void SetShadowResolutionDropdown(Dropdown dropdown, int startIndex = 0)
        {
            AddDropdownOptions(dropdown, startIndex, shadowResolutionOptions);
            dropdown.onValueChanged.AddListener(SetToCustomLevel);
            dropdown.onValueChanged.AddListener(SetShadowResolution);
            refreshSettings += () => DropdownRefresher(dropdown, () => dropdown.value = (int)QualitySettings.shadowResolution);
        }

        [MethodImpl((MethodImplOptions)0x100)] // agressive inline
        public static void SetAntialiasingDropdown(Dropdown dropdown, int startIndex = 0)
        {
            AddDropdownOptions(dropdown, startIndex, antialiasingOptions);
            dropdown.onValueChanged.AddListener(SetToCustomLevel);
            dropdown.onValueChanged.AddListener(SetAntialiasing);
            refreshSettings += () => DropdownRefresher(dropdown, () => dropdown.value = QualitySettings.antiAliasing == 8 ? 3 : QualitySettings.antiAliasing >> 1);
        }

        [MethodImpl((MethodImplOptions)0x100)] // agressive inline
        public static void SetQualityDropdown(Dropdown dropdown, int startIndex = 0)
        {
            AddDropdownOptions(dropdown, startIndex, qualityOptions);
            dropdown.onValueChanged.AddListener((index) => {
                SetQuality(index);
                refreshSettings();
            });

            onCustomQualitySet = () => {
                dropdown.options.Add(customQuality.CustomOption);
                dropdown.value = customQuality.CustomQualityIndex;
                dropdown.options.Remove(customQuality.CustomOption);
            };

            refreshSettings();
        }

        [MethodImpl((MethodImplOptions)0x100)] // agressive inline
        public static QualityLevel GetCurrentQualityLevel()
        {
            return new QualityLevel() {
                MasterTextureLimit = QualitySettings.masterTextureLimit,
                ShadowQuality = QualitySettings.shadows,
                ShadowResolution = QualitySettings.shadowResolution,
                AnisotropicFiltering = QualitySettings.anisotropicFiltering,
                Antialiasing = QualitySettings.antiAliasing,
                Resolution = Screen.currentResolution,
                VSync = QualitySettings.vSyncCount.ToBool(),
                FullScreen = Screen.fullScreen
            };
        }

        [MethodImpl((MethodImplOptions)0x100)] // agressive inline
        public static void SetQualityLevel(QualityLevel level)
        {
            QualitySettings.masterTextureLimit = level.MasterTextureLimit;
            QualitySettings.shadowResolution = level.ShadowResolution;
            QualitySettings.shadows = level.ShadowQuality;
            QualitySettings.anisotropicFiltering = level.AnisotropicFiltering;
            QualitySettings.antiAliasing = level.Antialiasing;
            QualitySettings.vSyncCount = level.VSync.ToInt();
            Screen.fullScreen = level.FullScreen;
            Screen.SetResolution(level.Resolution.width, level.Resolution.height, Screen.fullScreen);
            refreshSettings();
        }

        [MethodImpl((MethodImplOptions)0x100)] // agressive inline
        private static void SetToCustomLevel(int unusedParam)
        {
            if (customQuality.CustomQualityIndex < 0)
                throw new Exception("You have to set 'Custom' quality level!");
            if (QualitySettings.GetQualityLevel() == customQuality.CustomQualityIndex)
                return;

            QualityLevel savedLevel = GetCurrentQualityLevel();
            QualitySettings.SetQualityLevel(customQuality.CustomQualityIndex, true);
            SetQualityLevel(savedLevel);
            onCustomQualitySet();
        }

        [MethodImpl((MethodImplOptions)0x100)] // agressive inline
        private static void AddDropdownOptions(Dropdown dropdown, int startIndex, List<string> options)
        {
            dropdown.ClearOptions();
            dropdown.AddOptions(options);
            dropdown.value = startIndex;
            dropdown.RefreshShownValue();
        }

        [MethodImpl((MethodImplOptions)0x100)] // agressive inline
        private static void DropdownRefresher(Dropdown dropdown, Action action)
        {
            dropdown.onValueChanged.RemoveListener(SetToCustomLevel);
            action();
            dropdown.onValueChanged.AddListener(SetToCustomLevel);
        }

        [MethodImpl((MethodImplOptions)0x100)] // agressive inline
        private static void SetSliderValues(Slider slider, float startValue, float minValue, float maxValue)
        {
            slider.minValue = minValue;
            slider.maxValue = maxValue;
            slider.value = startValue;
        }


        [MethodImpl((MethodImplOptions)0x100)] // agressive inline
        private static void SetTexture(int index)
        {
            QualitySettings.masterTextureLimit = index;
        }

        [MethodImpl((MethodImplOptions)0x100)] // agressive inline
        private static void SetShadowQuality(int index)
        {
            QualitySettings.shadows = (ShadowQuality)index;
        }

        [MethodImpl((MethodImplOptions)0x100)] // agressive inline
        private static void SetShadowResolution(int index)
        {
            QualitySettings.shadowResolution = (ShadowResolution)index;
        }

        [MethodImpl((MethodImplOptions)0x100)] // agressive inline
        private static void SetAntialiasing(int index)
        {
            QualitySettings.antiAliasing = index > 0 ? 1 << index : 0;
        }

        [MethodImpl((MethodImplOptions)0x100)] // agressive inline
        private static void SetQuality(int index)
        {
            int wasVSync = QualitySettings.vSyncCount;
            AnisotropicFiltering wasAnisotropic = QualitySettings.anisotropicFiltering;

            QualitySettings.SetQualityLevel(index, true);

            QualitySettings.anisotropicFiltering = wasAnisotropic;
            QualitySettings.vSyncCount = wasVSync;
        }
    }
}
