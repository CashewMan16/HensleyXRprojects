using System.Diagnostics.Contracts;
using UnityEngine;

public enum FeatureUsage
{
    Once, //use once, need comma at end if there are more in the list
    Toggle //use more than once
}

public class CoreFeatures : MonoBehaviour
{
    //accessors - getting code outside this class, can be done with public variables but it's more condensed 

    //Property values - encapsulates filed
    //Get accessor (returns encapsulated values) and set accessor (assigning or editing encapsulated values)
    //Property Values use pascel and not camelcase
    public bool AudioSFXSourceCreated { get; set; }

    //audio plays when something opens
    [field : Header("Core Audio")]
    [field : SerializeField]
    public AudioClip AudioClipOnStart { get; set; }
    //audio plays on end/when it closes
    [field : SerializeField]
    [Header("Core Features")]
    public AudioClip AudioClipOnEnd { get; set; }
    
    private AudioSource audioSource;

    public FeatureUsage featureUsage = FeatureUsage.Once;

    protected virtual void Awake()
    {
        MakeSFXAudioSource();

    }

    private void MakeSFXAudioSource()
    {
        audioSource = GetComponent<AudioSource>();
        //need an if statement to make sure there is an audio source to play
        if(audioSource == null)
        {
            audioSource = gameObject.AddComponent<AudioSource>();
        }
        ///wheather its null or not, still need to be true
        AudioSFXSourceCreated = true;
    }

    //Audio play commands
    protected void PlayOnStart()
    {
        if(AudioSFXSourceCreated && AudioClipOnStart != null)
        {
            ///assigning the source to the clip
            audioSource.clip = AudioClipOnStart;
            audioSource.Play();
        }
    }

    protected void PlayOnEnd()
    {
        if (AudioSFXSourceCreated && AudioClipOnEnd != null)
        {
            ///assigning the source to the clip
            audioSource.clip = AudioClipOnEnd;
            audioSource.Play();
        }
    }
}
