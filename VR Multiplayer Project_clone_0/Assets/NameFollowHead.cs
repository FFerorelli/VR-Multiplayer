using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NameFollowHead : MonoBehaviour
{
    public float verticalOffset;
    public Transform head;
    private Transform playerCamera;

    // Start is called before the first frame update
    void Start()
    {
        playerCamera = Camera.main.transform;
    }

    // Update is called once per frame
    void Update()
    {
        transform.position = head.position + Vector3.up * verticalOffset;
        transform.LookAt(playerCamera, Vector3.up);
    }

}
