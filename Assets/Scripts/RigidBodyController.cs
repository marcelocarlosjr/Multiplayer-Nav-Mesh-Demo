using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using NETWORK_ENGINE;
using UnityEngine.InputSystem;

[RequireComponent(typeof(NetworkRigidBody))]
public class RigidBodyController : NetworkComponent
{
    public Rigidbody MyRig;
    public float speed = 3;
    public float rotationSpeed;
    public Vector3 LastMove;
    public Vector2 input;
    public float shoot;

    public bool vZero;
    public bool hZero;

    public float v;
    public float h;
    public bool shooting;
    public override void HandleMessage(string flag, string value)
    {
        if(flag == "MOVE" && IsServer)
        {
            string[] args = value.Split(',');
            float h = float.Parse(args[0]);
            float v = float.Parse(args[1]);
            LastMove = new Vector3(h, 0, v);
        }

        if(flag == "FIRE" && IsServer)
        {
            shoot = float.Parse(value);
            if(shoot > 0)
            {
                if (!shooting)
                {
                    StartCoroutine(Fire());
                }
            }
        }
    }

    public IEnumerator Fire()
    {
        shooting = true;
        MyCore.NetCreateObject(3, this.Owner, this.transform.position, this.transform.rotation);
        yield return new WaitForSeconds(1f);
        shooting = false;
    }
    public override void NetworkedStart()
    {
        if (IsServer)
        {
            this.transform.position = GameObject.FindGameObjectWithTag("p" + this.Owner).GetComponent<Transform>().position;
        }
        if (IsLocalPlayer)
        {
            FindObjectOfType<Camera>().GetComponent<CameraController>().SetLocal(this.transform);
        }
    }

    public override IEnumerator SlowUpdate()
    {
        while (IsConnected)
        {
            if (IsLocalPlayer)
            {
                /*if(Vector3.Distance(LastMove, new Vector3(h, 0, v)) > 0.1)
                {
                    LastMove = new Vector3(h, 0, v);
                    SendCommand("MOVE", h + "," + v);
                }

                if(fire > 0)
                {
                    SendCommand("FIRE", fire.ToString());
                }*/
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

    public void MoveInput(InputAction.CallbackContext context)
    {
        input = context.ReadValue<Vector2>();
        SendCommand("MOVE", input.x + "," + input.y);
    }

    public void Shoot(InputAction.CallbackContext context)
    {
        shoot = context.ReadValue<float>();
        SendCommand("FIRE", shoot.ToString());
    }

    void Update()
    {
        if (IsServer)
        {
            MyRig.velocity = this.transform.forward * LastMove.z * speed;

            MyRig.angularVelocity = new Vector3(0, LastMove.x * rotationSpeed, 0);
        }
        if (IsClient)
        {
            //h = Input.GetAxis("Horizontal");
            //v = Input.GetAxis("Vertical");
            //fire = Input.GetAxis("Fire1");
        }
    }
}
