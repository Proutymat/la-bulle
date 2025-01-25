using UnityEngine;

public class TeleportZone : MonoBehaviour
{
    [SerializeField] private Transform _destination;
    
    void OnTriggerEnter(Collider other)
    {
        if (other.GetComponent<CharacterController>())
        {
            Debug.Log("Teleporting player");
            other.GetComponent<CharacterController>().enabled = false;
            other.transform.position = _destination.position;
            other.GetComponent<CharacterController>().enabled = true;
        }
        else
        {
            other.transform.position = _destination.position;
        }
    }
    
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
