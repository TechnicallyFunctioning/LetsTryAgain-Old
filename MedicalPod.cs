using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MedicalPod : MonoBehaviour
{
    [SerializeField] private Animator lightAnimator;
    [SerializeField] private AudioClip[] audioClips;
    private Animator podAnim;
    private AudioSource audioSource;
    private void Start()
    {
        podAnim = GetComponent<Animator>();
        audioSource = GetComponent<AudioSource>();

        Invoke("DimLight", 1.0f);
        Invoke("OpenDoor", 3f);
    }

    private void DimLight()
    {
        lightAnimator.SetTrigger("DimLight");
    }

    private void OpenDoor()
    {
        podAnim.SetBool("DoorOpen", true);
        audioSource.PlayOneShot(audioClips[0]);
    }
}
