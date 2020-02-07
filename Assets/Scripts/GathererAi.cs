using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Numerics;
using UnityEngine;
using UnityEngine.AI;
using Vector3 = UnityEngine.Vector3;

public class GathererAi : MonoBehaviour
{
    private enum State
    {
        Idle,
        LookingForTarget,
        GotoTarget,
        FollowingPath,
        Gathering
    }

    public LayerMask pathMask;
    public int gatherSpeed = 4;

    private ResourceSource target;
    private State currentState;
    private NavMeshAgent _agent;
    private NavMeshPath currentPath;
    private List<Transform> allNearbyTargets;
    private int currentTargetIndex;

    void Awake()
    {
        currentState = State.LookingForTarget;
        _agent = GetComponent<NavMeshAgent>();
    }
    
    private bool FindNextTarget()
    {
        var hits = Physics.OverlapSphere(this.transform.position, 10, this.pathMask);
        this.allNearbyTargets = hits
            .Where(h => h.GetComponent<ResourceSource>() != null)
            .Select(h => h.transform)
            .OrderBy(h => Vector3.Distance(this.transform.position, h.transform.position))
            .ToList();
        this.currentTargetIndex = 0;
        return this.allNearbyTargets.Count > 0;
    }

    private IEnumerator FindPathToTargetCoroutine()
    {
        bool foundPath = false;
        while (this.currentTargetIndex < this.allNearbyTargets.Count)
        {
            var nextTarget = this.allNearbyTargets[this.currentTargetIndex];
            var path = new NavMeshPath();
            if (this._agent.CalculatePath(nextTarget.position, path))
            {
                this._agent.SetPath(path);
                this.target = nextTarget.GetComponent<ResourceSource>();
                SetNewState(State.FollowingPath);
                foundPath = true;
                break;
            }
            else
            {
                this.currentTargetIndex++;
                yield return null;
            }
        }

        if (!foundPath)
        {
            Debug.LogError("Tried all nearby targets and couldn't path to any.");
            SetNewState(State.Idle);
        }
    }

    private bool CheckPathComplete()
    {
        if (!this._agent.pathPending)
        {
            if (this._agent.remainingDistance <= this._agent.stoppingDistance)
            {
                if (!this._agent.hasPath || Math.Abs(this._agent.velocity.sqrMagnitude) < 0.01f)
                {
                    return true;
                }
            }
        }

        return false;
    }

    private IEnumerator GatherResources(ResourceSource source)
    {
        source.StartGathering();
        while (source.resourceAmount > 0)
        {
            source.GatherResource(gatherSpeed);
            if (source.resourceAmount > 0)
            {
                yield return new WaitForSeconds(1);
            }
        }

        SetNewState(State.LookingForTarget);
    }

    void Update()
    {
        switch (currentState)
        {
            case State.LookingForTarget:
                if (FindNextTarget())
                {
                    SetNewState(State.GotoTarget);
                    StartCoroutine(FindPathToTargetCoroutine());
                }
                else
                {
                    SetNewState(State.Idle);
                }
                break;
            //case State.GotoTarget:
                //SetNewState(FindPathToTargetCoroutine());
                //break;
            case State.FollowingPath:
                if (CheckPathComplete())
                {
                    SetNewState(State.Gathering);
                    StartCoroutine(GatherResources(this.target));
                }
                break;
        }
    }

    void SetNewState(State newState)
    {
        //Debug.Log($"Switch to state: " + newState);
        this.currentState = newState;
    }
}
