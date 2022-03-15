using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using NETWORK_ENGINE;
using UnityEngine.AI;

[RequireComponent(typeof(NavMeshAgent))]
public class NavMeshController : NetworkComponent
{
    public Vector3 Goal1;
    public Vector3 Goal2;

    public bool goal;

    NavMeshAgent MyAgent;
    public override void HandleMessage(string flag, string value)
    {    }

    public override void NetworkedStart()
    {    }

    public override IEnumerator SlowUpdate()
    {
        if (IsClient)
        {    }
        if (IsServer)
        {
            goal = true;
            MyAgent.SetDestination(Goal2);
        }

        while (IsServer)
        {
            foreach (RigidBodyController player in FindObjectsOfType<RigidBodyController>())
            {
                if (Vector3.Distance(player.transform.position, transform.position) <= 2.5f)
                {
                    MyAgent.SetDestination(player.transform.position);
                }
                else
                {
                    if (!goal)
                    {
                        MyAgent.SetDestination(Goal1);
                    }
                    if (goal)
                    {
                        MyAgent.SetDestination(Goal2);
                    }
                    if (MyAgent.remainingDistance < .1f)
                    {
                        goal = !goal;
                        if (!goal)
                        {
                            MyAgent.SetDestination(Goal1);
                        }
                        if (goal)
                        {
                            MyAgent.SetDestination(Goal2);
                        }
                    }
                }
            }

            if (IsDirty)
            {
                IsDirty = false;
            }
            yield return new WaitForSeconds(0.5f);
        }
    }

    void Start()
    {
        MyAgent = this.GetComponent<NavMeshAgent>();
        if(MyAgent == null)
        {
            throw new System.Exception("ERROR: COULD NOT FIND NAV MESH AGENT");
        }
    }


    void Update()
    {
        
    }
}
