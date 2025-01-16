using UnityEngine;
using UnityEngine.ParticleSystemJobs;
using UnityEngine.Rendering.Universal;

public class Engine : MonoBehaviour
{
    public SpriteRenderer spriteRenderer;
    public Light2D lightSource;
    public ParticleSystem particleEmitter;
    public AudioSource audioSource; // Firing Sound
    public float audioVolumeModifier = 1f; // should be a float between 0 and 1 to modify how loud the volume of its audio is
}
