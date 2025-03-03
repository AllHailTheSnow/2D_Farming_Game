using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraController : MonoBehaviour
{
    [SerializeField] private Transform clampMin, clampMax;

    private Transform target;   
    private Camera cam;
    private float halfWidth, halfHeight;

    // Start is called before the first frame update
    void Start()
    {
        cam = GetComponent<Camera>();
        halfHeight = cam.orthographicSize;
        halfWidth = cam.orthographicSize * cam.aspect;

        target = FindAnyObjectByType<PlayerController>().transform;

        clampMin.SetParent(null);
        clampMax.SetParent(null);
    }

    // Update is called once per frame
    void Update()
    {
        transform.position = new Vector3(target.position.x, target.position.y, transform.position.z);

        Vector3 clampedPos = transform.position;

        clampedPos.x = Mathf.Clamp(clampedPos.x, clampMin.position.x + halfWidth, clampMax.position.x - halfWidth);
        clampedPos.y = Mathf.Clamp(clampedPos.y, clampMin.position.y + halfHeight, clampMax.position.y - halfHeight);

        transform.position = clampedPos;
    }
}
