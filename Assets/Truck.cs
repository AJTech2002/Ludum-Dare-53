using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Truck : MonoBehaviour
{
    public bool collided = false;
    public ColorSide side;
    void OnCollisionEnter (Collision collision) {
        if (collision.gameObject.CompareTag("Truck") && !collided) {
           collided = true;

            GetComponent<Rigidbody>().constraints = RigidbodyConstraints.None;
            GetComponent<Rigidbody>().useGravity = true;
            
            Vector3 dir = transform.position - collision.transform.position;
            dir += Vector3.up * 2f;

            GetComponent<Rigidbody>().AddForce(dir.normalized*5, ForceMode.Impulse);
            GetComponent<Rigidbody>().AddTorque(Random.insideUnitSphere*10, ForceMode.Impulse);
            
            GameManager.Death();

        }
    }
}
