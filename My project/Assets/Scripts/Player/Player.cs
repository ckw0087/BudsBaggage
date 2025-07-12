using UnityEngine;
using UnityEngine.Rendering;

public class Player : MonoBehaviour
{
    [SerializeField] private AudioSource audioSource;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void PlayPickupSound(AudioClip clip, float volume = 1f)
    {
        if (audioSource && clip)
            audioSource.PlayOneShot(clip, volume);
    }
}
