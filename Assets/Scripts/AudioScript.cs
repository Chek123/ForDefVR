using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AudioScript : MonoBehaviour
{
    public AudioClip clip;
    public AudioSource source;

    // Start is called before the first frame update
    void Start()
    {
        source.clip = clip;
    }

    // Update is called once per frame
    void Update()
    {
    }
}
