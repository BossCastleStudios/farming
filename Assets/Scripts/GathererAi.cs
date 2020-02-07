using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Numerics;
using com.bosscastlestudios.farming;
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
        Gathering,
        GotoWoodBuilding
    }

    public WoodBuilding m_ClosestWoodBuilding;
    public int m_GatherSpeed = 4;
    public int m_ResourceCarryCapacity = 500;
    public float m_TreeCheckDistance = 10.0f;

    private static List<Transform> lockedTrees = new List<Transform>();

    private int currentResourceAmount;
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
        var hits = Physics.OverlapSphere(this.transform.position, this.m_TreeCheckDistance, LayerMask.NameToLayer("Everything"));
        this.allNearbyTargets = hits
            .Where(h => h.GetComponent<ResourceSource>() != null)
            .Select(h => h.transform)
            .Where(t => !lockedTrees.Contains(t))
            .OrderBy(h => Vector3.Distance(this.transform.position, h.position))
            .ToList();
        this.currentTargetIndex = 0;
        return this.allNearbyTargets.Count > 0;
    }

    private IEnumerator FindPathToTargetCoroutine()
    {
        bool pathFound = false;
        while (this.currentTargetIndex < this.allNearbyTargets.Count)
        {
            var nextTarget = this.allNearbyTargets[this.currentTargetIndex];
            var path = new NavMeshPath();
            if (this._agent.CalculatePath(nextTarget.position, path) && !lockedTrees.Contains(nextTarget))
            {
                lockedTrees.Add(nextTarget);
                this._agent.SetPath(path);
                this.target = nextTarget.GetComponent<ResourceSource>();
                SetNewState(State.FollowingPath);
                pathFound = true;
                break;
            }
            else
            {
                this.currentTargetIndex++;
                yield return null;
            }
        }

        if (!pathFound)
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
        while (source.resourceAmount > 0 && this.currentResourceAmount < this.m_ResourceCarryCapacity)
        {
            this.currentResourceAmount += source.GatherResource(m_GatherSpeed, this.m_ResourceCarryCapacity - this.currentResourceAmount);
            if (source.resourceAmount > 0)
            {
                yield return new WaitForSeconds(1);
            }
        }
        source.StopGathering();

        lockedTrees.Remove(source.transform);

        if (this.currentResourceAmount == this.m_ResourceCarryCapacity)
        {
            SetNewState(State.GotoWoodBuilding);
            StartCoroutine(GotoWoodBuilding(this.m_ClosestWoodBuilding));
        }
        else
        {
            SetNewState(State.LookingForTarget);
        }
    }

    private IEnumerator GotoWoodBuilding(WoodBuilding building)
    {
        var buildingPath = new NavMeshPath();
        if (this._agent.CalculatePath(building.transform.position, buildingPath))
        {
            this._agent.SetPath(buildingPath);
            yield return null;
            yield return null;
            while (!CheckPathComplete())
            {
                yield return null;
            }

            this.currentResourceAmount -= building.PlaceWood(this.currentResourceAmount);
            if (this.currentResourceAmount < this.m_ResourceCarryCapacity)
            {
                SetNewState(State.LookingForTarget);
            }
            else
            {
                Debug.Log("Job's done!");
            }
        }
        else
        {
            Debug.LogError("Could not find path to building!!!");
            SetNewState(State.Idle);
        }
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
                    Debug.LogWarning("Could not find new target");
                    SetNewState(State.Idle);
                }
                break;
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
