using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using NETWORK_ENGINE;

[RequireComponent(typeof(NetworkRigidBody))]
public class RigidBodyController : NetworkComponent
{
    public Rigidbody MyRig;
    public override void HandleMessage(string flag, string value)
    {
        
    }

    public override void NetworkedStart()
    {
        
    }

    public override IEnumerator SlowUpdate()
    {
        while (IsServer)
        {
            MyRig.velocity = this.transform.forward * 15;
            MyRig.angularVelocity = new Vector3(0, Mathf.PI / 2, 0);
            yield return new WaitForSeconds(0.05f);
        }
    }

    
    void Start()
    {
        MyRig = GetComponent<Rigidbody>();
        if(MyRig == null)
        {
            throw new System.Exception("ERROR: Could not find Rigidbody!");
        }
    }

    
    void Update()
    {
        
    }
}
