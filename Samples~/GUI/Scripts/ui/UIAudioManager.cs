using UnityEngine;

/// <summary>
/// UIAudioManager is a singleton class responsible for playing UI sound effects such as button clicks, hovers, etc.
/// It is designed to persist between scenes and provides a simple API to play common UI sounds.
/// </summary>
public class UIAudioManager : MonoBehaviour
{
    // Static instance property to access the manager globally.
    public static UIAudioManager Instance { get; private set; }

    [Header("UI Sound Clips")]
    [Tooltip("Sound played when a button is clicked.")]
    public AudioClip buttonClickClip;

    [Tooltip("Sound played when a UI element is hovered.")]
    public AudioClip hoverClip;

    // Add additional AudioClip references as needed.

    [Header("Audio Settings")]
    [Tooltip("AudioSource component used for playing UI sounds.")]
    public AudioSource audioSource;

    /// <summary>
    /// Ensures that only one instance of the UIAudioManager exists and persists across scenes.
    /// </summary>
    private void Awake()
    {
        // If an instance already exists and it's not this, destroy this duplicate.
        if (Instance != null && Instance != this)
        {
            Destroy(gameObject);
            return;
        }
        // Set the static instance and mark the GameObject to not be destroyed on scene load.
        Instance = this;
        DontDestroyOnLoad(gameObject);

        // Optionally, verify that an AudioSource is assigned.
        if (audioSource == null)
        {
            audioSource = GetComponent<AudioSource>();
            if (audioSource == null)
            {
                Debug.LogError("UIAudioManager: No AudioSource found. Please assign one in the inspector.");
            }
        }
    }

    /// <summary>
    /// Plays the button click sound effect.
    /// </summary>
    public void PlayButtonClick()
    {
        PlaySound(buttonClickClip);
    }

    /// <summary>
    /// Plays the hover sound effect.
    /// </summary>
    public void PlayHoverSound()
    {
        PlaySound(hoverClip);
    }

    /// <summary>
    /// Plays the provided AudioClip using the AudioSource.
    /// </summary>
    /// <param name="clip">The AudioClip to play.</param>
    public void PlaySound(AudioClip clip)
    {
        if (audioSource != null && clip != null)
        {
            audioSource.PlayOneShot(clip);
        }
        else if (clip == null)
        {
            Debug.LogWarning("UIAudioManager: Tried to play a null AudioClip.");
        }
    }
}
