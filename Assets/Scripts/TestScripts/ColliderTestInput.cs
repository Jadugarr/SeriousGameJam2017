using UnityEngine;

public class ColliderTestInput : MonoBehaviour
{
    [SerializeField] private float speed;

    // Update is called once per frame
    void Update()
    {
        if (Input.GetAxis("Horizontal") != 0)
        {
            gameObject.transform.Translate(new Vector3(Input.GetAxis("Horizontal") * speed, 0, 0));
        }
    }
}