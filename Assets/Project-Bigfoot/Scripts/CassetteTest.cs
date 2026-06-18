using UnityEngine;
using UnityEngine.Events;

public class CassetteTest : MonoBehaviour
{
    public UnityEvent PlayAudio;
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
    private void OnTriggerEnter(Collider other)
    {
        PlayAudio?.Invoke();
    }
}
