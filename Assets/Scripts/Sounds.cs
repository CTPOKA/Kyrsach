using UnityEngine;

public class Sounds : MonoBehaviour
{
    public AudioClip[] sounds;

    private AudioSource[] source;

    public static Sounds A;

    private void Awake()
    {
        this.source = new AudioSource[this.sounds.Length];
        Sounds.A = this;
        for (int i = 0; i < this.sounds.Length; i++)
        {
            GameObject gameObject = new GameObject("sound");
            this.source[i] = gameObject.AddComponent<AudioSource>();
            this.source[i].clip = this.sounds[i];
        }
    }

    public void Play(int i)
    {
        this.source[i].Play();
    }
}
