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
            for (int i = 0; i < length; i++)
            {
                if (qualities[i] == "Custom")
                {
                    VisualSettings.CustomQualityIndex = i;
                    break;
                }
                else if (i == length - 1)
                {
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
        public struct VisualParameters
        {
            public int CustomQualityIndex;
        }

        [Serializable]
        public struct AudioParameters
        {
            public bool IsMusicOn;
            public bool IsSfxOn;
            public float MusicMixerFloat;
            public float SfxMixerFloat;
        }

        public static AudioParameters AudioSettings;
        public static VisualParameters VisualSettings;

        private static List<string> TextureOptions          = new List<string> { "Very High", "High", "Medium", "Low" };
        private static List<string> ShadowQualityOptions    = new List<string> { "Disable", "HardOnly", "All" };
        private static List<string> ShadowResolutionOptions = new List<string> { "Low", "Medium", "High", "VeryHigh" };
        private static List<string> AntialiasingOptions     = new List<string> { "Disabled", "2x Multi Sampling", "4x Multi Sampling", "8x Multi Sampling" };
        private static List<string> QualityOptions { get { return QualitySettings.names.ToList(); } }
        private static List<string> ResolutionOptions { get { return new List<string>(Screen.resolutions.ToStringWithtHZ()); } }
        //Resolution[] resolutions = Screen.resolutions;
        //int length = resolutions.Length;
        //List<string> options = new List<string>(length);

        //for (int i = 0; i < length; i++)
        //    options.Add(resolutions[i].ToString());

        //return options;


        [MethodImpl((MethodImplOptions)0x100)] // agressive inline
        public static void SetMusicToggler(Toggle toggle, AudioMixer audioMixer, string exposedMusicName)
        {
            toggle.onValueChanged.AddListener((value) => {
                AudioSettings.IsMusicOn = value;

                if (!AudioSettings.IsMusicOn) // cache float value before setting new value
                    AudioSettings.MusicMixerFloat = audioMixer.GetFloat(exposedMusicName);

                audioMixer.SetFloat(exposedMusicName, AudioSettings.IsMusicOn ? AudioSettings.MusicMixerFloat : -80);
            });
        }

        [MethodImpl((MethodImplOptions)0x100)] // agressive inline
        public static void SetSoundFXToggler(Toggle toggle, AudioMixer audioMixer, string exposedSfxName)
        {
            toggle.onValueChanged.AddListener((value) => {
                AudioSettings.IsSfxOn = value;

                if (!AudioSettings.IsSfxOn) // cache float value before setting new value
                    AudioSettings.SfxMixerFloat = audioMixer.GetFloat(exposedSfxName);

                audioMixer.SetFloat(exposedSfxName, AudioSettings.IsSfxOn ? AudioSettings.SfxMixerFloat : -80);
            });
        }

        [MethodImpl((MethodImplOptions)0x100)] // agressive inline
        public static void SetMusicToggler(Button button, Image image, Sprite musicOff, Sprite musicOn, AudioMixer audioMixer, string exposedMusicName)
        {
            button.onClick.AddListener(() => {
                AudioSettings.IsMusicOn = !AudioSettings.IsMusicOn;
                image.sprite = !AudioSettings.IsMusicOn ? musicOff : musicOn;

                if (!AudioSettings.IsMusicOn) // cache float value before setting new value
                    AudioSettings.MusicMixerFloat = audioMixer.GetFloat(exposedMusicName);

                audioMixer.SetFloat(exposedMusicName, AudioSettings.IsMusicOn ? AudioSettings.MusicMixerFloat : -80);
            });
        }

        [MethodImpl((MethodImplOptions)0x100)] // agressive inline
        public static void SetSoundFXToggler(Button button, Image image, Sprite sfxOff, Sprite sfxOn, AudioMixer audioMixer, string exposedSfxName)
        {
            button.onClick.AddListener(() => {
                AudioSettings.IsSfxOn = !AudioSettings.IsSfxOn;
                image.sprite = !AudioSettings.IsSfxOn ? sfxOff : sfxOn;

                if (!AudioSettings.IsSfxOn) // cache float value before setting new value
                    AudioSettings.SfxMixerFloat = audioMixer.GetFloat(exposedSfxName);

                audioMixer.SetFloat(exposedSfxName, AudioSettings.IsSfxOn ? AudioSettings.SfxMixerFloat : -80);
            });
        }

        [MethodImpl((MethodImplOptions)0x100)] // agressive inline
        public static void SetMusicVolumeSlider(Slider slider, AudioMixer audioMixer, string exposedMusicName)
        {
            slider.onValueChanged.AddListener((value) => {
                audioMixer.SetFloat(exposedMusicName, value);
                if (value <= -30)
                    audioMixer.SetFloat(exposedMusicName, -80);
                AudioSettings.MusicMixerFloat = audioMixer.GetFloat(exposedMusicName);
            });
        }

        [MethodImpl((MethodImplOptions)0x100)] // agressive inline
        public static void SetSoundFXSlider(Slider slider, AudioMixer audioMixer, string exposedSfxName)
        {
            slider.onValueChanged.AddListener((value) => {
                audioMixer.SetFloat(exposedSfxName, value);
                if (value <= -30)
                    audioMixer.SetFloat(exposedSfxName, -80);
                AudioSettings.SfxMixerFloat = audioMixer.GetFloat(exposedSfxName);
            });
        }

        [MethodImpl((MethodImplOptions)0x100)] // agressive inline
        public static void SetFullScreenToggler(Toggle toggle)
        {
            toggle.isOn = Screen.fullScreen;
            toggle.onValueChanged.AddListener((boolean) => {
                Screen.fullScreen = boolean;
            });
        }

        [MethodImpl((MethodImplOptions)0x100)] // agressive inline
        public static void SetVSyncToggler(Toggle toggle)
        {
            toggle.isOn = QualitySettings.vSyncCount > 0;
            toggle.onValueChanged.AddListener((boolean) => {
                QualitySettings.vSyncCount = boolean.ToInt();
            });
        }

        [MethodImpl((MethodImplOptions)0x100)] // agressive inline
        public static void SetResolutionDropdown(Dropdown dropdown, int setIndex = 0, List<string> options = null)
        {
            AddDropdownOptions(dropdown, setIndex, options ?? ResolutionOptions);

            dropdown.onValueChanged.AddListener((index) => {
                Resolution resolution = options != null ? options[index].ToResolution() : Screen.resolutions[index];
                Screen.SetResolution(resolution.width, resolution.height, Screen.fullScreen);
            });
        }

        [MethodImpl((MethodImplOptions)0x100)] // agressive inline
        public static void SetQualityDropdown(Dropdown dropdown, int setIndex = 0)
        {
            AddDropdownOptions(dropdown, setIndex, QualityOptions);
            dropdown.onValueChanged.AddListener((index) => {
                bool wasfullScreen = Screen.fullScreen;
                Resolution wasResolution = Screen.currentResolution;
                bool wasVSync = QualitySettings.vSyncCount == 0 ? false : true;

                QualitySettings.SetQualityLevel(index, true);

                Screen.SetResolution(wasResolution.width, wasResolution.height, wasfullScreen);
                QualitySettings.vSyncCount = wasVSync ? 1 : 0;
                // RefreshOthers();
            });
        }

        [MethodImpl((MethodImplOptions)0x100)] // agressive inline
        public static void SetTextureDropdown(Dropdown dropdown, int setIndex = 0, List<string> options = null)
        {
            AddDropdownOptions(dropdown, setIndex, options ?? TextureOptions);
            dropdown.onValueChanged.AddListener((index) => {
                SetToCustomLevel();
                QualitySettings.masterTextureLimit = index;
            });
        }

        [MethodImpl((MethodImplOptions)0x100)] // agressive inline
        public static void SetShadowQualityDropdown(Dropdown dropdown, int setIndex = 0)
        {
            AddDropdownOptions(dropdown, setIndex, ShadowQualityOptions);
            dropdown.onValueChanged.AddListener((index) => {
                SetToCustomLevel();
                QualitySettings.shadows = (ShadowQuality)index;
            });
        }

        [MethodImpl((MethodImplOptions)0x100)] // agressive inline
        public static void SetShadowResolutionDropdown(Dropdown dropdown, int setIndex = 0)
        {
            AddDropdownOptions(dropdown, setIndex, ShadowResolutionOptions);
            dropdown.onValueChanged.AddListener((index) => {
                SetToCustomLevel();
                QualitySettings.shadowResolution = (ShadowResolution)index;
            });
        }

        [MethodImpl((MethodImplOptions)0x100)] // agressive inline
        public static void SetAntialiasingDropdown(Dropdown dropdown, int setIndex = 0)
        {
            AddDropdownOptions(dropdown, setIndex, AntialiasingOptions);
            dropdown.onValueChanged.AddListener((index) => {
                SetToCustomLevel();
                QualitySettings.antiAliasing = index > 0 ? 1 << index : 0;
            });
        }

        [MethodImpl((MethodImplOptions)0x100)] // agressive inline
        public static void SetAnisotropicToggler(Toggle toggle)
        {
            toggle.onValueChanged.AddListener((value) => {
                SetToCustomLevel();
                QualitySettings.anisotropicFiltering = (AnisotropicFiltering)(value ? 2 : 0);
            });
        }



        [MethodImpl((MethodImplOptions)0x100)] // agressive inline
        private static void AddDropdownOptions(Dropdown dropdown, int setIndex, List<string> options)
        {
            dropdown.ClearOptions();
            dropdown.AddOptions(options);
            dropdown.value = setIndex;
            dropdown.RefreshShownValue();
        }

        [MethodImpl((MethodImplOptions)0x100)] // agressive inline
        private static void SetToCustomLevel()
        {
            if (QualitySettings.GetQualityLevel() == VisualSettings.CustomQualityIndex)
                return;

            QualityLevel savedLevel = new QualityLevel() {
                MasterTextureLimit = QualitySettings.masterTextureLimit,
                ShadowQuality = QualitySettings.shadows,
                ShadowResolution = QualitySettings.shadowResolution,
                AnisotropicFiltering = QualitySettings.anisotropicFiltering,
                Antialiasing = QualitySettings.antiAliasing,
                Resolution = Screen.currentResolution,
                VSync = QualitySettings.vSyncCount.ToBool(),
                FullScreen = Screen.fullScreen
            };

            QualitySettings.SetQualityLevel(VisualSettings.CustomQualityIndex, false);

            Screen.fullScreen = savedLevel.FullScreen;
            Screen.SetResolution(savedLevel.Resolution.width, savedLevel.Resolution.height, Screen.fullScreen);
            QualitySettings.masterTextureLimit = savedLevel.MasterTextureLimit;
            QualitySettings.shadowResolution = savedLevel.ShadowResolution;
            QualitySettings.shadows = savedLevel.ShadowQuality;
            QualitySettings.anisotropicFiltering = savedLevel.AnisotropicFiltering;
            QualitySettings.antiAliasing = savedLevel.Antialiasing;
            QualitySettings.vSyncCount = savedLevel.VSync.ToInt();
        }

    }
}
