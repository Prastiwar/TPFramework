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
    public class TPSettings : MonoBehaviour
    {
        //[HideInInspector] public static Dropdown qualityDropdown;
        //[HideInInspector] public static Dropdown aliasingDropdown;
        //[HideInInspector] public static Dropdown shadowQualDropdown;
        //[HideInInspector] public static Dropdown shadowDropdown;
        //[HideInInspector] public static Dropdown textureDropdown;

        //static int CustomQualityIndex;
        //static int previousLevel;

        //static List<string> qualityOptions = new List<string>();
        //static List<string> shadowQualOptions = new List<string>();
        //static List<string> shadowOptions = new List<string>();
        //static List<string> aliasingOptions = new List<string>();
        //static List<string> textureOptions = new List<string>();


        //void Awake()
        //{
        //    Initialize();
        //}

        //static void Initialize()
        //{
        //    int length = QualitySettings.names.Length;
        //    for (int i = 0; i < length; i++)
        //    {
        //        if (QualitySettings.names[i] == "Custom")
        //        {
        //            CustomQualityIndex = i;
        //            break;
        //        }

        //        if (i == length - 1)
        //        {
        //            Debug.LogError("No 'Custom' quality level found. Create one!");
        //            return;
        //        }
        //    }

        //    InitializeAllDropdowns();
        //    InitializeAllToggles();
        //}

        //static void InitializeAllDropdowns()
        //{
        //    InitializeDropdown(InitializeShadowsQuality, shadowQualDropdown, shadowQualOptions, currentShadowQualIndex, 4);
        //    InitializeDropdown(InitializeShadows, shadowDropdown, shadowOptions, currentShadowIndex, 3);
        //    InitializeDropdown(InitializeAliasingQuality, aliasingDropdown, aliasingOptions, currentAliasingIndex, 4);
        //    InitializeDropdown(InitializeTextureQuality, textureDropdown, textureOptions, currentTextureIndex, 4);

        //    aliasingDropdown.onValueChanged.AddListener(SetAntiAliasing);
        //    shadowQualDropdown.onValueChanged.AddListener(SetShadowsQuality);
        //    shadowDropdown.onValueChanged.AddListener(SetShadows);
        //    textureDropdown.onValueChanged.AddListener(SetTexture);
        //}


        //// *** Initializers *** //

        //static void InitializeTextureQuality(int i)
        //{
        //    switch (i)
        //    {
        //        case 0:
        //            textureOptions.Add("Very High");
        //            break;
        //        case 1:
        //            textureOptions.Add("High");
        //            break;
        //        case 2:
        //            textureOptions.Add("Medium");
        //            break;
        //        case 3:
        //            textureOptions.Add("Low");
        //            break;
        //        default:
        //            break;
        //    }

        //    if (i == QualitySettings.masterTextureLimit)
        //        currentTextureIndex = i;
        //}

        //static void InitializeShadowsQuality(int i)
        //{
        //    string option = ((ShadowResolution)i).ToString();
        //    shadowQualOptions.Add(option);

        //    if (i == (int)QualitySettings.shadowResolution)
        //        currentShadowQualIndex = i;
        //}

        //static void InitializeShadows(int i)
        //{
        //    string option = ((ShadowQuality)i).ToString();
        //    shadowOptions.Add(option);

        //    if (i == (int)QualitySettings.shadows)
        //        currentShadowIndex = i;
        //}

        //static void InitializeAliasingQuality(int i)
        //{
        //    string option = "";
        //    switch (i)
        //    {
        //        case 0:
        //            option = "Disabled";
        //            break;
        //        case 1:
        //        case 2:
        //            option = (i * 2).ToString() + "x Multi Sampling";
        //            break;
        //        case 3:
        //            option = "8x Multi Sampling";
        //            break;
        //        default:
        //            break;
        //    }
        //    aliasingOptions.Add(option);

        //    if (i == QualitySettings.antiAliasing)
        //        currentAliasingIndex = i;
        //}

        //static void InitializeDropdown(Action<int> action, ref Dropdown dropdown, ref List<string> options, ref int currentIndex, int length)
        //{
        //    options.Clear();
        //    for (int i = 0; i < length; i++)
        //        action(i); // Initialize..()
        //    dropdown.onValueChanged.RemoveAllListeners();
        //    dropdown.ClearOptions();
        //    dropdown.AddOptions(options);
        //    dropdown.value = currentIndex;
        //    dropdown.RefreshShownValue();
        //}


        //// *** Setters *** ///



        //static void SetAntiAliasing(int index)
        //{
        //    SetToCustom(() => SetAntiAliasing(index));
        //    switch (index)
        //    {
        //        case 0:
        //            QualitySettings.antiAliasing = index;
        //            break;
        //        case 1:
        //            QualitySettings.antiAliasing = 2;
        //            break;
        //        case 2:
        //            QualitySettings.antiAliasing = 4;
        //            break;
        //        case 3:
        //            QualitySettings.antiAliasing = 8;
        //            break;
        //        default:
        //            break;
        //    }
        //    aliasingDropdown.value = index;
        //}

        //static void SetShadowsQuality(int index)
        //{
        //    SetToCustom(() => SetShadowsQuality(index));
        //    QualitySettings.shadowResolution = (ShadowResolution)index;
        //}

        //static void SetShadows(int index)
        //{
        //    SetToCustom(() => SetShadows(index));
        //    QualitySettings.shadows = (ShadowQuality)index;
        //}

        //static void SetTexture(int index)
        //{
        //    SetToCustom(() => SetTexture(index));
        //    QualitySettings.masterTextureLimit = index;
        //}

        //[HideInInspector] public static Toggle anisotropicToggle;
        //static void InitializeAllToggles()
        //{
        //    if (anisotropicToggle && anisotropicToggle.isOn != ((int)QualitySettings.anisotropicFiltering == 0 ? false : true))
        //        anisotropicToggle.isOn = (int)QualitySettings.anisotropicFiltering == 0 ? false : true;
        //}
        //static void SetAnisotropic(bool boolean)
        //{
        //    SetToCustom(() => SetAnisotropic(boolean));
        //    QualitySettings.anisotropicFiltering = (AnisotropicFiltering)(boolean ? 2 : 0);
        //}

        //static void SetToCustom(Action action)
        //{
        //    previousLevel = QualitySettings.GetQualityLevel();
        //    if (previousLevel == CustomQualityIndex)
        //        return;

        //    int _tex = QualitySettings.masterTextureLimit;
        //    int _shadRes = (int)QualitySettings.shadowResolution;
        //    int _shad = (int)QualitySettings.shadows;
        //    int _ani = (int)QualitySettings.anisotropicFiltering;
        //    int _anti = QualitySettings.antiAliasing;
        //    Resolution _res = Screen.currentResolution;
        //    bool _vsync = QualitySettings.vSyncCount == 0 ? false : true;
        //    bool _full = Screen.fullScreen;

        //    QualitySettings.SetQualityLevel(CustomQualityIndex);
        //    Screen.fullScreen = _full;
        //    Screen.SetResolution(_res.width, _res.height, Screen.fullScreen);
        //    QualitySettings.masterTextureLimit = _tex;
        //    QualitySettings.shadowResolution = (ShadowResolution)_shadRes;
        //    QualitySettings.shadows = (ShadowQuality)_shad;
        //    QualitySettings.anisotropicFiltering = (AnisotropicFiltering)_ani;
        //    QualitySettings.antiAliasing = _anti;
        //    QualitySettings.vSyncCount = _vsync ? 1 : 0;

        //    action();
        //    Initialize();
        //}




        // ************************************************************************************* //









            

        public struct VisualParameters
        {

        }

        public struct AudioParameters
        {
            public bool IsMusicOn;
            public bool IsSfxOn;
            public float MusicMixerFloat;
            public float SfxMixerFloat;
        }

        public static AudioParameters AudioSettings;

        public static VisualParameters VisualSettings;

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
            toggle.isOn = QualitySettings.vSyncCount == 1;
            toggle.onValueChanged.AddListener((boolean) => {
                QualitySettings.vSyncCount = boolean ? 1 : 0;
            });
        }

        [MethodImpl((MethodImplOptions)0x100)] // agressive inline
        public static void SetResolutionDropdown(Dropdown dropdown, int setIndex = 0, List<string> options = null)
        {
            dropdown.ClearOptions();
            dropdown.AddOptions(options ?? GetResolutionOptions());
            dropdown.value = setIndex;
            dropdown.RefreshShownValue();

            dropdown.onValueChanged.AddListener((index) => {
                Resolution resolution = options != null ? options[index].ToResolution() : Screen.resolutions[index];
                Screen.SetResolution(resolution.width, resolution.height, Screen.fullScreen);
            });
        }

        [MethodImpl((MethodImplOptions)0x100)] // agressive inline
        public static void SetQualityDropdown(Dropdown dropdown, int setIndex = 0)
        {
            dropdown.ClearOptions();
            dropdown.AddOptions(GetQualityOptions());
            dropdown.value = setIndex;
            dropdown.RefreshShownValue();

            dropdown.onValueChanged.AddListener((index) => {
                bool wasfullScreen = Screen.fullScreen;
                Resolution wasResolution = Screen.currentResolution;
                bool wasVSync = QualitySettings.vSyncCount == 0 ? false : true;

                QualitySettings.SetQualityLevel(index);

                Screen.SetResolution(wasResolution.width, wasResolution.height, wasfullScreen);
                QualitySettings.vSyncCount = wasVSync ? 1 : 0;
                //Initialize();
            });
        }

        [MethodImpl((MethodImplOptions)0x100)] // agressive inline
        public static List<string> GetResolutionOptions()
        {
            Resolution[] resolutions = Screen.resolutions;
            int length = resolutions.Length;
            List<string> options = new List<string>(length);

            for (int i = 0; i < length; i++)
                options.Add(resolutions[i].ToString());

            return options;
        }

        [MethodImpl((MethodImplOptions)0x100)] // agressive inline
        public static List<string> GetQualityOptions()
        {
            return QualitySettings.names.ToList();
        }


    }
}
