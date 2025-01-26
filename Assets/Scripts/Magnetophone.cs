using System.Collections;
using UnityEngine;

public class Magnetophone : MonoBehaviour
{
    [SerializeField] private Transform _mouthPosition = null;
    [SerializeField] private GameObject _startTape = null;
    [SerializeField] private AudioClip _startClip = null;
    [SerializeField] private GameObject _endTape = null;
    [SerializeField] private AudioClip _endClip = null;
    AudioSource audioSourceMagnetophone;
    Animator animatorMagnetophone;
    private GameObject currentTape = null;

    private void Start()
    {
        _startTape.SetActive(false);
        SetTape(_startTape.gameObject);
   
        animatorMagnetophone = GetComponent<Animator>();
        animatorMagnetophone.SetTrigger("hasTape");
        audioSourceMagnetophone = GetComponent<AudioSource>();
        audioSourceMagnetophone.PlayOneShot(_startClip);
        StartCoroutine(WaitForClipToEnd(_startClip.length));
    }
    public void PlayEnd(Transform pos) 
    {
        Drop();
        _startTape.SetActive(false);
        SetTape(_endTape.gameObject);
        transform.position = pos.position;
        animatorMagnetophone.SetTrigger("hasTape");
        audioSourceMagnetophone.PlayOneShot(_endClip);
        StartCoroutine(WaitForClipToEnd(_endClip.length));
    }
    private IEnumerator WaitForClipToEnd(float duration)
    {
        yield return new WaitForSeconds(duration);
        animatorMagnetophone.SetTrigger("hasFinishAudio");
    }
    public void SetTape(GameObject tape) 
    {
        currentTape = tape;
        currentTape.transform.position = _mouthPosition.position;
    }
    public void Drop()
    {
        Rigidbody rb = currentTape.GetComponent<Rigidbody>();
        currentTape.SetActive(true);
        currentTape.GetComponent<GrabbableObject>().Drop();
        rb.isKinematic = false;
        rb.AddForce(_mouthPosition.forward * 150);
        currentTape = null;
    }
}
