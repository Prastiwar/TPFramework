using System;
using UnityEngine;
using TPFramework;
using System.Collections;

public class Examples : MonoBehaviour
{
    public TPAchievementExample TPAchievementExample;
    public TPPersistenceExample TPPersistenceExample;
    public TPObjectPoolExample  TPObjectPoolExample;
    public TPAttributeExample   TPAttributeExample;
    public TPAudioPoolExample   TPAudioPoolExample;
    public TPInventoryExample   TPInventoryExample;
    public TPSettingsExample    TPSettingsExample;
    public TPTooltipExample     TPTooltipExample;
    public TPRandomExample      TPRandomExample;
    public TPFaderExample       TPFaderExample;
    
    public void ExampleTPAchievement()
    {
        TPSettingsExample.ExampleCanvas.SetActive(false);
        TPAchievementExample ex = TPAchievementExample;
        throw new NotImplementedException();
    }

    public void ExampleTPPersistence()
    {
        TPSettingsExample.ExampleCanvas.SetActive(false);
        TPPersistenceExample ex = TPPersistenceExample;
        throw new NotImplementedException();
    }

    public void ExampleTPObjectPool()
    {
        TPSettingsExample.ExampleCanvas.SetActive(false);
        TPObjectPoolExample ex = TPObjectPoolExample;
        TPObjectPool.CreatePool(ex.PoolKey, ex.Prefab, ex.PoolCount, 10);
        StartCoroutine(SpawnObjects(ex, 20));
    }

    public void ExampleTPAttribute()
    {
        TPSettingsExample.ExampleCanvas.SetActive(false);
        TPAttributeExample ex = TPAttributeExample;
        ex.Health.Recalculate(); // we need to call this since changes from editor doesnt call it
        object goldenArmor = new object(); // some Item
        ex.HealthIncreaser.Source = goldenArmor;
        DrawLine();
        Debug.Log("TPAttribute Health Base value:" + ex.Health.BaseValue);
        Debug.Log("TPAttribute Health Value before armor equip: " + ex.Health.Value);
        ex.Health.AddModifier(ex.HealthIncreaser);
        Debug.Log("TPAttribute Health Value after armor equip: " + ex.Health.Value);
        DrawLine();
    }

    public void ExampleTPAudioPool()
    {
        TPSettingsExample.ExampleCanvas.SetActive(false);
        TPAudioPoolExample ex = TPAudioPoolExample;
        throw new NotImplementedException();
    }

    public void ExampleTPInventory()
    {
        TPSettingsExample.ExampleCanvas.SetActive(false);
        TPInventoryExample ex = TPInventoryExample;
        throw new NotImplementedException();
    }

    public void ExampleTPSettings()
    {
        TPSettingsExample ex = TPSettingsExample;

        ex.ExampleCanvas.SetActive(true);

        TPSettings.SetAnisotropicToggler(ex.AniosotropicToggler);
        TPSettings.SetFullScreenToggler(ex.FullScreenToggler);
        TPSettings.SetVSyncToggler(ex.VsyncToggler);

        TPSettings.SetMusicToggler(ex.MusicToggler, ex.Mixer, ex.MusicFloat);
        TPSettings.SetSoundFXToggler(ex.SfxToggler, ex.Mixer, ex.SfxFloat);
        TPSettings.SetMusicVolumeSlider(ex.MusicSlider, ex.Mixer, ex.MusicFloat);
        TPSettings.SetSoundFXVolumeSlider(ex.SfxSlider, ex.Mixer, ex.SfxFloat);

        TPSettings.SetAntialiasingDropdown(ex.Antialiasing);
        TPSettings.SetResolutionDropdown(ex.Resolution);
        TPSettings.SetShadowQualityDropdown(ex.ShadowQuality);
        TPSettings.SetShadowResolutionDropdown(ex.ShadowResolution);
        TPSettings.SetTextureDropdown(ex.Texture);
        TPSettings.SetQualityDropdown(ex.Quality);
    }

    public void ExampleTPTooltip()
    {
        TPSettingsExample.ExampleCanvas.SetActive(false);
        TPTooltipExample ex = TPTooltipExample;
        throw new NotImplementedException();
    }

    public void ExampleTPRandom()
    {
        TPSettingsExample.ExampleCanvas.SetActive(false);
        TPRandomExample ex = TPRandomExample;
        throw new NotImplementedException();
    }

    public void ExampleTPFader()
    {
        TPSettingsExample.ExampleCanvas.SetActive(false);
        TPFaderExample ex = TPFaderExample;
        throw new NotImplementedException();
    }


    private IEnumerator SpawnObjects(TPObjectPoolExample ex, float last)
    {
        while (last >= 0)
        {
            TPObjectPool.ToggleActive(ex.PoolKey, TPObjectState.Deactive, GetRandomPosition(), true);
            last--;
            yield return new WaitForSeconds(1);
        }
        // Alternative
        //while (last >= 0)
        //{
        //    GameObject obj = TPObjectPool.PopObject(ex.PoolKey, TPObjectState.Deactive, true);
        //    obj.transform.position = GetRandomPosition();
        //    TPObjectPool.ToggleActive(ex.PoolKey, obj, true);
        //    last--;
        //    yield return new WaitForSeconds(1);
        //}
    }

    private void DrawLine()
    {
        Debug.Log("-------------------------------------------------------------");
    }

    private Vector3 GetRandomPosition()
    {
        float randX = UnityEngine.Random.Range(-8, 8);
        float randY = UnityEngine.Random.Range(-8, 8);
        return new Vector3(randX, randY, 0);
    }
}
