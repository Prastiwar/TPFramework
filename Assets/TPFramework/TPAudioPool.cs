/**
*   Authored by Tomasz Piowczyk
*   MIT LICENSE: https://github.com/Prastiwar/TPAudioPool/blob/master/LICENSE
*   Repository: https://github.com/Prastiwar/TPAudioPool 
*/
using System;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using UnityEngine;

namespace TPFramework
{
    [Serializable]
    public struct TPAudioObject
    {
        public string Name;
        public AudioClip Clip;
    }

    public sealed class TPAudioPool
    {
        private static Dictionary<string, TPAudioBundle> audioPool = new Dictionary<string, TPAudioBundle>();
        private static AudioSource sfxSource;
        private static AudioSource themeSource;

        public enum Sources
        {
            SFX,
            Theme
        }

        /// <summary> Gets AudioSource from GameObject named "TPAudioSource"
        /// <para> On Get - returns source - if null, create it </para>
        /// <para> On Set - behaves like preset </para>
        /// </summary>
        public static AudioSource SFXSource {
            get {
                if (sfxSource == null)
                {
                    sfxSource = GetOrCreateSource("TPAduioSFXSource", false);
                }
                return sfxSource;
            }
            set {
                sfxSource = value;
                if (!ReferenceEquals(sfxSource, null))
                {
                    sfxSource = GetOrCreateSource("TPAduioSFXSource", false);
                    CopySource(ref sfxSource, value);
                }
            }
        }

        /// <summary> Gets AudioSource from GameObject named "TPAudioThemeSource"
        /// <para> On Get - returns source - if null, create it </para>
        /// <para> On Set - behaves like preset </para>
        /// </summary>
        public static AudioSource ThemeSource {
            get {
                if (themeSource == null)
                {
                    themeSource = GetOrCreateSource("TPAudioThemeSource", true);
                }
                return themeSource;
            }
            set {
                themeSource = value;
                if (!ReferenceEquals(themeSource, null))
                {
                    themeSource = GetOrCreateSource("TPAudioThemeSource", true);
                    CopySource(ref themeSource, value);
                }
            }
        }

        [MethodImpl((MethodImplOptions)0x100)] // agressive inline
        public static void AddToPool(string bundleName, TPAudioBundle bundle)
        {
            audioPool[bundleName] = bundle;
        }

        [MethodImpl((MethodImplOptions)0x100)] // agressive inline
        public static void AddToPool(TPAudioBundle bundle)
        {
            AddToPool(bundle.name, bundle);
        }

        [MethodImpl((MethodImplOptions)0x100)] // agressive inline
        public static void RemoveFromPool(string bundleName)
        {
            audioPool.Remove(bundleName);
        }

        [MethodImpl((MethodImplOptions)0x100)] // agressive inline
        public static void RemoveFromPool(TPAudioBundle bundle)
        {
            audioPool.Remove(bundle.name);
        }

        /// <summary> Removes all Audio Bundles from pool </summary> 
        [MethodImpl((MethodImplOptions)0x100)] // agressive inline
        public static void Dispose()
        {
            audioPool.Clear();
        }

        [MethodImpl((MethodImplOptions)0x100)] // agressive inline
        public static AudioSource SetClip(TPAudioBundle bundle, string audioName, Sources source = Sources.SFX)
        {
            AudioClip clip = GetClip(bundle, audioName);
            GetSource(source).clip = clip;
            return SFXSource;
        }

        [MethodImpl((MethodImplOptions)0x100)] // agressive inline
        public static AudioSource SetClip(string bundleName, string audioName, Sources source = Sources.SFX)
        {
            return SetClip(GetBundle(bundleName), audioName, source);
        }

        [MethodImpl((MethodImplOptions)0x100)] // agressive inline
        public static void Play(TPAudioBundle bundle, string audioName, Sources source = Sources.SFX, ulong delay = 0)
        {
            AudioClip clip = GetClip(bundle, audioName);
            GetSource(source).clip = clip;
            GetSource(source).Play(delay);
        }

        [MethodImpl((MethodImplOptions)0x100)] // agressive inline
        public static void Play(string bundleName, string audioName, Sources source = Sources.SFX, ulong delay = 0)
        {
            Play(GetBundle(bundleName), audioName, source, delay);
        }

        [MethodImpl((MethodImplOptions)0x100)] // agressive inline
        public static void PlayOneShot(TPAudioBundle bundle, string audioName, float volumeScale = 1.0f)
        {
            SFXSource.PlayOneShot(GetClip(bundle, audioName), volumeScale);
        }

        [MethodImpl((MethodImplOptions)0x100)] // agressive inline
        public static void PlayOneShot(string bundleName, string audioName, float volumeScale = 1.0f)
        {
            PlayOneShot(GetBundle(bundleName), audioName, volumeScale);
        }

#if NET_2_0 || NET_2_0_SUBSET
        [MethodImpl((MethodImplOptions)0x100)] // agressive inline
        public static void PlayOneShot(MonoBehaviour mono, TPAudioBundle bundle, string audioName, Action onAudioEnd, float volumeScale = 1.0f)
        {
            AudioClip clip = GetClip(bundle, audioName);
            SFXSource.PlayOneShot(clip, volumeScale);
            mono.StartCoroutine(DelayAction(clip.length, onAudioEnd));
        }

        [MethodImpl((MethodImplOptions)0x100)] // agressive inline
        public static void PlayOneShot(MonoBehaviour mono, string bundleName, string audioName, Action onAudioEnd, float volumeScale = 1.0f)
        {
            PlayOneShot(mono, GetBundle(bundleName), audioName, onAudioEnd, volumeScale);
        }
#else
        [MethodImpl((MethodImplOptions)0x100)] // agressive inline
        public static void PlayOneShot(TPAudioBundle bundle, string audioName, Action onAudioEnd, float volumeScale = 1.0f)
        {
            AudioClip clip = GetClip(bundle, audioName);
            SFXSource.PlayOneShot(clip, volumeScale);
            DelayAction(clip.length, onAudioEnd);
        }
               
        [MethodImpl((MethodImplOptions)0x100)] // agressive inline 
        public static void PlayOneShot(string bundleName, string audioName, Action onAudioEnd, float volumeScale = 1.0f)
        {
            PlayOneShot(GetBundle(bundleName), audioName, volumeScale, onAudioEnd);
        }
#endif

#if NET_2_0 || NET_2_0_SUBSET
        [MethodImpl((MethodImplOptions)0x100)] // agressive inline
        public static void Play(MonoBehaviour mono, TPAudioBundle bundle, string audioName, Action onAudioEnd, Sources source = Sources.SFX, ulong delay = 0)
        {
            AudioClip clip = GetClip(bundle, audioName);
            GetSource(source).clip = clip;
            GetSource(source).Play(delay);
            mono.StartCoroutine(DelayAction(clip.length + delay, onAudioEnd));
        }

        [MethodImpl((MethodImplOptions)0x100)] // agressive inline
        public static void Play(MonoBehaviour mono, string bundleName, string audioName, Action onAudioEnd, Sources source = Sources.SFX, ulong delay = 0)
        {
            Play(mono, GetBundle(bundleName), audioName, onAudioEnd, source, delay);
        }
#else
        [MethodImpl((MethodImplOptions)0x100)] // agressive inline
        public static void Play(TPAudioBundle bundle, string audioName, Sources source = Sources.Source, ulong delay = 0, Action onAudioEnd = null)
        {
            AudioClip clip = GetClip(bundle, audioName);
            GetSource().clip = clip;
            GetSource().Play(delay);
            DelayAction(clip.length + delay, onAudioEnd);
        }
        
        [MethodImpl((MethodImplOptions)0x100)] // agressive inline
        public static void Play(string bundleName, string audioName, Sources source = Sources.Source, ulong delay = 0, Action onAudioEnd = null)
        {
            Play(GetBundle(bundleName), audioName, source, delay, onAudioEnd);
        }
#endif

        [MethodImpl((MethodImplOptions)0x100)] // agressive inline
        public static AudioClip GetClip(TPAudioBundle bundle, string audioName)
        {
            int length = bundle.AudioObjects.Length;
            for (int i = 0; i < length; i++)
            {
                if (bundle.AudioObjects[i].Name == audioName)
                {
                    return bundle.AudioObjects[i].Clip;
                }
            }
            Debug.LogError("Audio clip named " + audioName + " in this bundle not found");
            return null;
        }

        [MethodImpl((MethodImplOptions)0x100)] // agressive inline
        public static AudioClip GetClip(string bundleName, string audioName)
        {
            return GetClip(GetBundle(bundleName), audioName);
        }

        [MethodImpl((MethodImplOptions)0x100)] // agressive inline
        public static TPAudioBundle GetBundle(string bundleName)
        {
            return SafeKey(bundleName) ? audioPool[bundleName] : null;
        }

        [MethodImpl((MethodImplOptions)0x100)] // agressive inline
        private static AudioSource GetSource(Sources source)
        {
            return source == Sources.SFX ? SFXSource : ThemeSource;
        }

        [MethodImpl((MethodImplOptions)0x100)] // agressive inline
        private static AudioSource GetOrCreateSource(string gameObjectName, bool loop)
        {
            GameObject obj = GameObject.Find(gameObjectName);

            if (ReferenceEquals(obj, null))
            {
                return CreateNewSource(gameObjectName, loop);
            }
            return obj.GetComponent<AudioSource>();
        }

        [MethodImpl((MethodImplOptions)0x100)] // agressive inline
        public static bool HasKey(string key)
        {
            return audioPool.ContainsKey(key);
        }

        [MethodImpl((MethodImplOptions)0x100)] // agressive inline
        private static AudioSource CreateNewSource(string gameObjectName, bool loop)
        {
            GameObject newObj = new GameObject(gameObjectName);
            AudioSource audioSource = newObj.AddComponent<AudioSource>();
            audioSource.playOnAwake = false;
            audioSource.loop = loop;
            UnityEngine.Object.DontDestroyOnLoad(newObj);
            return audioSource;
        }

        [MethodImpl((MethodImplOptions)0x100)] // agressive inline
        private static void CopySource(ref AudioSource source, AudioSource copySource)
        {
            source.bypassEffects = copySource.bypassEffects;
            source.bypassListenerEffects = copySource.bypassListenerEffects;
            source.bypassReverbZones = copySource.bypassReverbZones;
            source.dopplerLevel = copySource.dopplerLevel;
            source.enabled = copySource.enabled;
            source.hideFlags = copySource.hideFlags;
            source.ignoreListenerPause = copySource.ignoreListenerPause;
            source.ignoreListenerVolume = copySource.ignoreListenerVolume;
            source.loop = copySource.loop;
            source.maxDistance = copySource.maxDistance;
            source.minDistance = copySource.minDistance;
            source.mute = copySource.mute;
            source.outputAudioMixerGroup = copySource.outputAudioMixerGroup;
            source.panStereo = copySource.panStereo;
            source.pitch = copySource.pitch;
            source.playOnAwake = copySource.playOnAwake;
            source.priority = copySource.priority;
            source.reverbZoneMix = copySource.reverbZoneMix;
            source.rolloffMode = copySource.rolloffMode;
            source.spatialBlend = copySource.spatialBlend;
            source.spatialize = copySource.spatialize;
            source.spatializePostEffects = copySource.spatializePostEffects;
            source.spread = copySource.spread;
            source.tag = copySource.tag;
            source.time = copySource.time;
            source.timeSamples = copySource.timeSamples;
            source.velocityUpdateMode = copySource.velocityUpdateMode;
            source.volume = copySource.volume;
            source.SetCustomCurve(AudioSourceCurveType.CustomRolloff, copySource.GetCustomCurve(AudioSourceCurveType.CustomRolloff));
            source.SetCustomCurve(AudioSourceCurveType.ReverbZoneMix, copySource.GetCustomCurve(AudioSourceCurveType.ReverbZoneMix));
            source.SetCustomCurve(AudioSourceCurveType.SpatialBlend, copySource.GetCustomCurve(AudioSourceCurveType.SpatialBlend));
            source.SetCustomCurve(AudioSourceCurveType.Spread, copySource.GetCustomCurve(AudioSourceCurveType.Spread));
        }

#if NET_2_0 || NET_2_0_SUBSET
        [MethodImpl((MethodImplOptions)0x100)] // agressive inline
        private static System.Collections.IEnumerator DelayAction(float delay, Action action)
        {
            while (delay-- >= 0)
                yield return null;
            if (action != null)
                action();
        }
#else
        [MethodImpl((MethodImplOptions)0x100)] // agressive inline
        private static async void DelayAction(float delay, Action action)
        {
            await System.Threading.Tasks.Task.Delay(TimeSpan.FromSeconds(delay));
            if (action != null)
                action();
        }
#endif
        
        private static bool SafeKey(string key)
        {
            if (HasKey(key))
            {
                return true;
            }
            Debug.LogError("This key doesn't exist: " + key);
            return false;
        }

    }

}
