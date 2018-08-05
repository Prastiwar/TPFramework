﻿using System;
using UnityEngine;
using UnityEngine.Audio;
using UnityEngine.UI;
using TPFramework;

[Serializable]
public struct TPSettingsExample
{
    public GameObject Scene;

    [Header("Toggles")]
    public Toggle AniosotropicToggler;
    public Toggle FullScreenToggler;
    public Toggle VsyncToggler;

    [Header("Audio")]
    public Toggle MusicToggler;
    public Toggle SfxToggler;
    public Slider MusicSlider;
    public Slider SfxSlider;
    public AudioMixer Mixer;
    public string MusicFloat;
    public string SfxFloat;

    [Header("Dropdowns")]
    public Dropdown Antialiasing;
    public Dropdown Quality;
    public Dropdown Resolution;
    public Dropdown ShadowQuality;
    public Dropdown ShadowResolution;
    public Dropdown Texture;
}

[Serializable]
public struct TPTooltipExample
{
    public GameObject Scene;
}

[Serializable]
public struct TPFadeExample
{
    public GameObject Scene;
    public Button FadeButton;
    public TPAlphaFade AlphaFade;
    public TPFadeInfo FadeInfo;
}

[Serializable]
public struct TPInventoryExample
{
}

[Serializable]
public struct TPAchievementExample
{
    public TPAchievement Achievement;
}

[Serializable]
public struct TPAudioPoolExample
{
    public TPAudioBundle AudioBundle;
}

[Serializable]
public struct TPObjectPoolExample
{
    public int PoolKey;
    public GameObject Prefab;
    public int PoolCount;
}

[Serializable]
public struct TPRandomExample
{
    public GameObject Scene;
    public GameObject[] GameObjects;
    public ProbabilityElementInt<GameObject>[] ProbabilityElements;
}

[Serializable]
public struct TPPersistenceExample
{
}

[Serializable]
public struct TPAttributeExample
{
    public TPAttribute Health;
    public TPModifier HealthIncreaser;
}

[Serializable]
public struct TPUIExample
{
    public GameObject Scene;
    public Button ToggleWindowBtn;
    [HideInInspector] public bool WindowEnabled;
    public TPModalWindow ModalWindow;
}