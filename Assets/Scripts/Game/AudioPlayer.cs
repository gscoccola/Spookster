using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Audio;
using UnityEngine.SceneManagement;


public class AudioPlayer : Singleton<AudioPlayer>
{



    [Header("Shooting")]
    [SerializeField] AudioClip shootingClip1;
    [SerializeField][Range(0f, 1f)] float shootingVolume1 = 1f;
    [SerializeField] AudioClip shootingClip2;
    [SerializeField][Range(0f, 1f)] float shootingVolume2 = 1f;
    [SerializeField] AudioClip shootingClip3;
    [SerializeField][Range(0f, 1f)] float shootingVolume3 = 1f;

    [SerializeField] List<AudioSource> shootingClips = new List<AudioSource>();


    [Header("Reloading")]
    [SerializeField] AudioClip reloadingClip;
    [SerializeField][Range(0f, 1f)] float reloadingVolume = 1f;
    [SerializeField] AudioClip openingClip;
    [SerializeField][Range(0f, 1f)] float openingVolume = 1f;
    [SerializeField] AudioClip dryFireClip;
    [SerializeField][Range(0f, 1f)] float dryFireVolume = 1f;

    [Header("Footsteps")]
    [SerializeField] List<AudioSource> footstepsClips = new List<AudioSource>();

    [Header("Jump Grunts")]
    [SerializeField] List<AudioClip> jumpClips = new List<AudioClip>();
    [SerializeField][Range(0f, 1f)] float jumpVolume = 1f;

    [Header("Jump Landing Sounds")]
    [SerializeField] List<AudioClip> landingClips = new List<AudioClip>();
    [SerializeField][Range(0f, 1f)] float landingVolume = 1f;

    [Header("Pickup Sounds")]
    [SerializeField] List<AudioClip> pickupClips = new List<AudioClip>();
    [SerializeField][Range(0f, 1f)] float pickupVolume = 1f;

    [Header("Death")]
    [SerializeField] AudioClip deathClip;
    [SerializeField][Range(0f, 1f)] float deathVolume = 1f;

    [Header("Sword slash")]
    [SerializeField] List<AudioClip> slashClips = new List<AudioClip>();
    [SerializeField][Range(0f, 1f)] float slashVolume = 1f;

    [Header("Sword parry")]
    [SerializeField] AudioClip parryClip;
    [SerializeField][Range(0f, 1f)] float parryVolume = 0.5f;

    [Header("Goober Splatter")]
    [SerializeField] List<AudioClip> splatterClips = new List<AudioClip>();
    [SerializeField][Range(0f, 1f)] List<float> splatterVolumes = new List<float>();

    [Header("Damage")]
    [SerializeField] AudioClip[] damageClips;
    [SerializeField][Range(0f, 1f)] float[] damageVolumes;

    [Header("Heartbeat")]
    [SerializeField] AudioClip heartbeatClip;
    [SerializeField][Range(0f, 1f)] float heartbeatVolume;

    [Header("Music")]
    [SerializeField] AudioResource song0;
    [SerializeField] AudioResource song1;
    [SerializeField] AudioResource song2;

    static AudioPlayer instance;

    AudioSource audioSource;

    private void Start()
    {
        audioSource = GetComponent<AudioSource>();
        audioSource.ignoreListenerPause = true;
    }

    public void SwitchTrack()
    {
        StartCoroutine(SwitchTrackCoroutine());

        IEnumerator SwitchTrackCoroutine()
        {
            yield return new WaitForSecondsRealtime(0.3f);
            switch (SceneManager.GetActiveScene().buildIndex)
            {
                case >= 5:
                    if (audioSource.resource != song2)
                    {
                        audioSource.resource = song2;
                        audioSource.Play();
                    }
                    break;

                case >= 2:
                    if (audioSource.resource != song1)
                    {
                        audioSource.resource = song1;
                        audioSource.Play();
                    }
                    break;

                case 1:
                    audioSource.Stop();
                    break;

                case 0:
                    audioSource.resource = song0;
                    audioSource.Play();
                    break;


            }
        }
    }



    public void PlayShootingClip1(bool dontDestroyOnLoad=false)
    {
        if (shootingClip1 != null)
        {
            PlayClipAtPoint(shootingClip1,
                Vector3.zero, shootingVolume1, UnityEngine.Random.Range(0.85f, 1.3f), dontDestroyOnLoad);
        }
    }

    public void PlayShootingClip2()
    {
        if (shootingClip2 != null)
        {
            PlayClipAtPoint(shootingClip2,
                Vector3.zero, shootingVolume2, UnityEngine.Random.Range(0.85f, 1.3f));
        }
    }

    public void PlayShootingClip3()
    {
        if (shootingClip3 != null)
        {
            PlayClipAtPoint(shootingClip3,
                Vector3.zero, shootingVolume3, UnityEngine.Random.Range(0.85f, 1.3f));
        }
    }

    public void PlayReloadingClip()
    {
        if (reloadingClip != null)
        {
            PlayClipAtPoint(reloadingClip,
                Vector3.zero, reloadingVolume, 1);
        }
    }

    public void PlayOpeningClip()
    {
        if (reloadingClip != null)
        {
            PlayClipAtPoint(openingClip,
                Vector3.zero, openingVolume);
        }
    }

    public void PlayDryFireClip()
    {
        if (dryFireClip != null)
        {
            PlayClipAtPoint(dryFireClip,
                Vector3.zero, dryFireVolume, 1f);
        }
    }

    public void PlayFootstepsClip()
    {

        if (footstepsClips != null)
        {
            int randomInt = UnityEngine.Random.Range(0, footstepsClips.Count);
            footstepsClips[randomInt].Play();
        }
    }

    public void PlayJumpClip()
    {

        if (jumpClips != null)
        {
            int randomInt = UnityEngine.Random.Range(0, jumpClips.Count);
            AudioSource.PlayClipAtPoint(jumpClips[randomInt],
                Vector3.zero, jumpVolume);
        }
    }

    public void PlayPickupClip()
    {
        int randomInt = UnityEngine.Random.Range(0, pickupClips.Count);
        AudioSource.PlayClipAtPoint(pickupClips[randomInt],
            Vector3.zero, pickupVolume);
    }

    public void PlayLandingClip()
    {
        int randomInt = UnityEngine.Random.Range(0, landingClips.Count);
        AudioSource.PlayClipAtPoint(landingClips[randomInt],
            Vector3.zero, landingVolume);
    }

    public void PlayDeathClip()
    {
        if (deathClip != null)
        {
            AudioSource.PlayClipAtPoint(deathClip,
                Vector3.zero, deathVolume);
        }
    }


    public void PlaySlashClip()
    {

        int randomInt = UnityEngine.Random.Range(0, slashClips.Count);
        AudioSource.PlayClipAtPoint(slashClips[randomInt],
            Vector3.zero, slashVolume);
    }

    public void PlayParryClip()
    {
        AudioSource.PlayClipAtPoint(parryClip,
            Vector3.zero, parryVolume);
    }

    public void PlaySplatterClip()
    {
        PlayClipAtPoint(splatterClips[0],
            Vector3.zero, splatterVolumes[0], UnityEngine.Random.Range(0.7f, 1.5f));
    }

    public void PlayLoudSplatterClip()
    {
        PlayClipAtPoint(splatterClips[1],
            Vector3.zero, splatterVolumes[1], UnityEngine.Random.Range(0.7f, 1.5f));
    }

    public void PlayDamageClip()
    {
        int randomInt = UnityEngine.Random.Range(0, damageClips.Length);
        PlayClipAtPoint(damageClips[randomInt],
            Vector3.zero, damageVolumes[randomInt], UnityEngine.Random.Range(0.9f, 1.1f));
    }


    internal void PlayHearbeatClip()
    {
        PlayClipAtPoint(heartbeatClip,
            Vector3.zero, heartbeatVolume, UnityEngine.Random.Range(0.95f, 1.05f));
    }

    public static AudioSource PlayClipAtPoint(AudioClip clip, Vector3 pos, float volume = 1, float pitch = 1, bool dontDestroyOnLoad = false)
    {
        GameObject tempGO = new GameObject("TempAudio"); // create the temp object
        tempGO.transform.position = pos; // set its position
        AudioSource tempASource = tempGO.AddComponent<AudioSource>(); // add an audio source
        if (dontDestroyOnLoad)
        {
            DontDestroyOnLoad(tempASource);
        }
        tempASource.clip = clip;
        tempASource.pitch = pitch;
        tempASource.volume = volume;
        // set other aSource properties here, if desired
        tempASource.Play(); // start the sound
        MonoBehaviour.Destroy(tempGO, tempASource.clip.length); // destroy object after clip duration (this will not account for whether it is set to loop)
        return tempASource; // return the AudioSource reference
    }

}
