using UnityEngine;

public class Sounds : MonoBehaviour
{
    public AudioClip[] sounds;
    AudioSource[] source;
    public static Sounds A;
    private void Awake()
    {
        source = new AudioSource[sounds.Length];
        A = this;
        for (int i = 0; i<sounds.Length; i++)
        {
            GameObject obj = new GameObject("sound");
            source[i] = obj.AddComponent<AudioSource>() as AudioSource;
            source[i].clip = sounds[i];
        }
    }
    public void Play (int i)
    {
        source[i].Play();
    }
}
