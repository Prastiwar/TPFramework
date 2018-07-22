﻿using System;
using UnityEngine;
using UnityEngine.Audio;
using UnityEngine.UI;
using TPFramework;

[Serializable]
public struct TPSettingsExample
{
    public GameObject ExampleCanvas;

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
}

[Serializable]
public struct TPFaderExample
{
}

[Serializable]
public struct TPInventoryExample
{
}

[Serializable]
public struct TPAchievementExample
{
}

[Serializable]
public struct TPAudioPoolExample
{
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