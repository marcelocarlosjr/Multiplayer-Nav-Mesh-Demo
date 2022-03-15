using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using NETWORK_ENGINE;

[RequireComponent(typeof(NetworkRigidBody))]
public class RigidBodyController : NetworkComponent
{
    public Rigidbody MyRig;
    public float speed = 3;
    public float rotationSpeed;
    public Vector3 LastMove;

    public bool vZero;
    public bool hZero;

    public float v;

    public float h;


    public override void HandleMessage(string flag, string value)
    {
        if(flag == "MOVE" && IsServer)
        {
            string[] args = value.Split(',');
            float h = float.Parse(args[0]);
            float v = float.Parse(args[1]);
            LastMove = new Vector3(h, 0, v);
        }
    }

    public override void NetworkedStart()
    {
        if (IsServer)
        {
            this.transform.position = new Vector3(0, 1.1f, -3);
        }
    }

    public override IEnumerator SlowUpdate()
    {
        while (IsConnected)
        {
            if (IsLocalPlayer)
            {
                if (new Vector3(v, 0, h).magnitude > 0.1f)
                {
                    LastMove = new Vector3(h, 0, v);
                    SendCommand("MOVE", h + "," + v);
                }
                else if(LastMove.magnitude == 0)
                {
                    SendCommand("MOVE", 0 + "," + 0);
                }
            }
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
        if (IsServer)
        {
            if(LastMove.z > 0.1 || LastMove.z < -0.1)
            {
                MyRig.velocity = this.transform.forward * LastMove.z * speed;
            }
            if(LastMove.x != 0)
            {
                MyRig.angularVelocity = new Vector3(0, LastMove.x * rotationSpeed, 0);
            }
        }
        if (IsClient)
        {
            h = Input.GetAxis("Horizontal");
            v = Input.GetAxis("Vertical");
        }
    }
}
