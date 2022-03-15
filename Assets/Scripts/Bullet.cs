using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using NETWORK_ENGINE;

public class Bullet : NetworkComponent
{
    public float speed;
    public Rigidbody MyRig;
    public override void HandleMessage(string flag, string value)
    {
        
    }

    public override void NetworkedStart()
    {
        if (IsServer)
        {
            MyRig = GetComponent<Rigidbody>();
        }
    }

    public override IEnumerator SlowUpdate()
    {
        while (IsConnected)
        {
            if (IsClient)
            {

            }
            if (IsServer)
            {
                MyRig.velocity = transform.forward * speed;
                if (IsDirty)
                {
                    IsDirty = false;
                }
            }
            yield return new WaitForSeconds(.1f);
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if (IsServer)
        {
            if(other.tag == "WALL")
            {
                MyCore.NetDestroyObject(this.NetId);
            }

            if(other.tag == "Enemy")
            {
                other.GetComponent<NavMeshController>().TakeDamage(1);
                MyCore.NetDestroyObject(this.NetId);
            }
        }
    }
}
