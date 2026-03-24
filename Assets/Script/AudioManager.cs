using System;
using UnityEngine;

public class AudioManager : MonoBehaviour
{
   public static AudioManager instance;
   
   [SerializeField] AudioSource audioSourceBG;
   [SerializeField] AudioSource audioSourceClips;
   [SerializeField] private AudioClip start, level, shabash, retry;

   private void Awake()
   {
      instance = this;
   }
   
   public void PlayStartSound()
   {
      audioSourceClips.PlayOneShot(start);
   }

   public void PlayLevelSound()
   {
      audioSourceClips.Stop();
      audioSourceClips.PlayOneShot(level);
      
   }

   public void PlayShabashSound()
   {
      audioSourceClips.Stop();
      audioSourceClips.PlayOneShot(shabash);
   }

   public void PlayRetrySound()
   {
      audioSourceClips.Stop();
      audioSourceClips.PlayOneShot(retry);
   }

   public void StopAllSound()
   {
      audioSourceClips.Stop();
   }
}
