using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class carrot_script : MonoBehaviour
{

    void Update()
    {
       transform.Rotate(new Vector3(0, 3, 0));
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.transform.CompareTag("Player"))
        {
            other.gameObject.GetComponent<player_controller_script>().SetCollision(true);
            other.gameObject.GetComponent<player_controller_script>().Point();
            transform.position = new Vector3(Random.Range(-7, 7), 0, Random.Range(-7, 7));
        }
        else if (other.transform.CompareTag("Babby"))
        {
            transform.position = new Vector3(Random.Range(-7, 7), 0, Random.Range(-7, 7));
        }
    }
}
