using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlaySound : MonoBehaviour
{
    public void PlayWalk()
    {
        AudioManager.GetInstance().PlayAudio(SoundType.WALK, 0.6f);
    }
    public void PlayRun()
    {
        AudioManager.GetInstance().PlayAudio(SoundType.RUN, 0.6f);
    }
    public void PlayAttack()
    {
        AudioManager.GetInstance().PlayAudio(SoundType.SLASH, 0.6f);
    }
    public void PlayJavelin()
    {
        AudioManager.GetInstance().PlayAudio(SoundType.PILUM, 0.4f);
    }
    public void PlayBash()
    {
        AudioManager.GetInstance().PlayAudio(SoundType.BASH, 1f);
    }
    public void PlayEquip()
    {
        AudioManager.GetInstance().PlayAudio(SoundType.SHEATHE, 0.2f);
    }
    public void PlayerEquip()
    {
        AudioManager.GetInstance().PlayAudio(SoundType.SHEATHE, 0.6f);
    }
    public void PlayerUnequip()
    {
        AudioManager.GetInstance().PlayAudio(SoundType.UNSHEATHE, 0.6f);
    }
    public void PlayUnequip()
    {
        AudioManager.GetInstance().PlayAudio(SoundType.UNSHEATHE, 0.2f);
    }
    public void PlayHammer()
    {
        AudioManager.GetInstance().PlayAudio(SoundType.Hammering, 0.7f);
    }
}
