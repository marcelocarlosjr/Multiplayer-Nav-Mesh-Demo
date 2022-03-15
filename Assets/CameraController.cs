using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraController : MonoBehaviour
{
    public Transform LocalTransform;
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if (LocalTransform)
        {
            this.transform.position = new Vector3(LocalTransform.position.x, 27.7f, LocalTransform.position.z);
        }
    }

    public void SetLocal(Transform value)
    {
        LocalTransform = value;
    }
}
