using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Unity.AI;
using UnityEngine.AI;

public enum OffMeshLinkMoveMethod
{
    Teleport,
    Normal,
    Parabola,
    Curve,
    Climb
}

public class AI_Behaviour : MonoBehaviour
{
    private NavMeshAgent agent;
    public Transform target;
    public float customSpeed;
    public OffMeshLinkMoveMethod moveMethod;
    public AnimationCurve jumpCurve = new();

    void Update()
    {
        agent.SetDestination(target.position);
    }
    IEnumerator Start()
    {   

        agent = GetComponent<NavMeshAgent>();
        agent.autoTraverseOffMeshLink = false;
        while (true)
        {
            if (agent.isOnOffMeshLink)
            {
                if(moveMethod == OffMeshLinkMoveMethod.Normal)
                        yield return StartCoroutine(NormalSpeed(agent));
                else if(moveMethod == OffMeshLinkMoveMethod.Parabola)
                        yield return StartCoroutine(Parabola(agent, 2.0f, 0.5f));
                else if(moveMethod == OffMeshLinkMoveMethod.Curve)
                        yield return StartCoroutine(Curve(agent, 0.5f));
                else if (moveMethod == OffMeshLinkMoveMethod.Climb)
                    yield return StartCoroutine(Climb());
                agent.CompleteOffMeshLink();
            }
            yield return null;
        }
    }

    IEnumerator NormalSpeed(NavMeshAgent agent)
    {
        OffMeshLinkData data = agent.currentOffMeshLinkData;
        Vector3 endpos = data.endPos + Vector3.up * agent.baseOffset;
        while (agent.transform.position != endpos)
        {
            agent.transform.position =
                Vector3.MoveTowards(agent.transform.position, endpos, customSpeed * Time.deltaTime);
            yield return null;
        }
    }

    IEnumerator Parabola(NavMeshAgent agent, float height, float duration)
    {
        OffMeshLinkData data = agent.currentOffMeshLinkData;
        Vector3 startPos = agent.transform.position;
        Vector3 endPos = data.endPos + Vector3.up * agent.baseOffset;

        float _normalizeTime = 0.0f;
        while (_normalizeTime < 1.0f)
        {
            float _yOffset = height * 4.0f * (_normalizeTime - _normalizeTime * _normalizeTime);
            agent.transform.position = Vector3.Lerp(startPos, endPos, _normalizeTime) + _yOffset * Vector3.up;
            _normalizeTime += Time.deltaTime / duration;
            yield return null;
        }
        yield return null;
    }

    IEnumerator Curve(NavMeshAgent agent, float duration)
    {
        OffMeshLinkData data = agent.currentOffMeshLinkData;
        Vector3 startPos = agent.transform.position;
        Vector3 endPos = data.endPos + Vector3.up * agent.baseOffset;

        float _normalizeTime = 0.0f;
        while (_normalizeTime < 1.0f)
        {
            float _yOffset = jumpCurve.Evaluate(_normalizeTime);
            agent.transform.position = Vector3.Lerp(startPos, endPos, _normalizeTime) + _yOffset * Vector3.up;
            _normalizeTime += Time.deltaTime / duration;
            yield return null;
        }
    }

    IEnumerator Climb()
    {
        OffMeshLinkData data = agent.currentOffMeshLinkData;
        Vector3 startPos = agent.transform.position;
        Vector3 endPos = data.endPos + Vector3.up * agent.baseOffset;
        Vector3 targetDir = endPos - startPos;
        float angle = Mathf.Atan2(targetDir.y, targetDir.x) * Mathf.Rad2Deg;

        while(agent.transform.position != endPos)
        {
            agent.transform.SetPositionAndRotation(Vector3.MoveTowards(agent.transform.position,endPos,agent.speed * Time.deltaTime),Quaternion.Euler(0, angle, 0));
            yield return null;
        }
        yield return null;
    }
}
