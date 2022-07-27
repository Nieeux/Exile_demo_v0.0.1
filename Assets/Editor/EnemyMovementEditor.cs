using UnityEditor;
using UnityEngine;
using UnityEngine.AI;

[CustomEditor(typeof(AIController))]
public class EnemyMovementEditor : Editor
{
    private Collider[] Colliders = new Collider[10];

    private void OnSceneGUI()
    {
        //VIEW
        AIController AIenemy = (AIController)target;

        Handles.color = Color.red;
        Handles.DrawWireArc(AIenemy.transform.position, Vector3.up, Vector3.forward, 360, AIenemy.radius);

        Handles.color = Color.white;
        Handles.DrawWireArc(AIenemy.transform.position, Vector3.up, Vector3.forward, 360, AIenemy.radiusHide);

        Vector3 viewAngle01 = DirectionFromAngle(AIenemy.transform.eulerAngles.y, -AIenemy.angle / 2);
        Vector3 viewAngle02 = DirectionFromAngle(AIenemy.transform.eulerAngles.y, AIenemy.angle / 2);

        Handles.color = Color.yellow;
        Handles.DrawLine(AIenemy.transform.position, AIenemy.transform.position + viewAngle01 * AIenemy.radius);
        Handles.DrawLine(AIenemy.transform.position, AIenemy.transform.position + viewAngle02 * AIenemy.radius);

        if (AIenemy.canSee)
        {
            Handles.color = Color.red;
            Handles.DrawLine(AIenemy.transform.position, AIenemy.playerRef.transform.position);
        }

        //HIDE
        if (AIenemy == null || AIenemy.target == null || AIenemy.IndexState != StateType.AttackCover)
        {
            return;
        }

        int Hits = Physics.OverlapSphereNonAlloc(AIenemy.Agent.transform.position, AIenemy.radius, Colliders, AIenemy.CanHideMask);
        if (Hits > 0)
        {
            int HitReduction = 0;
            for (int i = 0; i < Hits; i++)
            {
                if (Vector3.Distance(Colliders[i].transform.position, AIenemy.target.position) < AIenemy.MinPlayerDistance)
                {
                    Handles.color = Color.red;
                    Handles.SphereHandleCap(GUIUtility.GetControlID(FocusType.Passive), Colliders[i].transform.position, Quaternion.identity, 0.25f, EventType.Repaint);
                    Handles.Label(Colliders[i].transform.position, $"{i} too close to target");
                    Colliders[i] = null;
                    HitReduction++;
                }
                else if (Colliders[i].bounds.size.y < AIenemy.MinObstacleHeight)
                {
                    Handles.color = Color.red;
                    Handles.SphereHandleCap(GUIUtility.GetControlID(FocusType.Passive), Colliders[i].transform.position, Quaternion.identity, 0.25f, EventType.Repaint);
                    Handles.Label(Colliders[i].transform.position, $"{i} too small");
                    Colliders[i] = null;
                    HitReduction++;
                }

            }
            Hits -= HitReduction;

            System.Array.Sort(Colliders, AIenemy.ColliderArraySortComparer);

            bool FoundTarget = false;

            for (int i = 0; i < Hits; i++)
            {
                if (NavMesh.SamplePosition(Colliders[i].transform.position, out NavMeshHit hit, 2f, AIenemy.Agent.areaMask))
                {
                    if (!NavMesh.FindClosestEdge(hit.position, out hit, AIenemy.Agent.areaMask))
                    {
                        Handles.color = Color.red;
                        Handles.SphereHandleCap(GUIUtility.GetControlID(FocusType.Passive), hit.position, Quaternion.identity, 0.25f, EventType.Repaint);
                        Handles.Label(hit.position, $"{i} Hide(1) no edge found");
                    }

                    if (Vector3.Dot(hit.normal, (AIenemy.target.position - hit.position).normalized) < AIenemy.HideSensitivity)
                    {
                        Handles.color = FoundTarget ? Color.yellow : Color.green;
                        FoundTarget = true;
                        Handles.SphereHandleCap(GUIUtility.GetControlID(FocusType.Passive), hit.position, Quaternion.identity, 0.25f, EventType.Repaint);
                        Handles.Label(hit.position, $"{i} Hide(1): {Vector3.Dot(hit.normal, (AIenemy.target.position - hit.position).normalized):0.0}");
                    }
                    else
                    {
                        Handles.color = Color.red;
                        Handles.SphereHandleCap(GUIUtility.GetControlID(FocusType.Passive), hit.position, Quaternion.identity, 0.25f, EventType.Repaint);
                        Handles.Label(hit.position, $"{i} Hide(1): {Vector3.Dot(hit.normal, (AIenemy.target.position - hit.position).normalized):0.0}");

                        if (NavMesh.SamplePosition(Colliders[i].transform.position - (AIenemy.target.position - hit.position).normalized * 2, out NavMeshHit hit2, 2f, AIenemy.Agent.areaMask))
                        {
                            if (!NavMesh.FindClosestEdge(hit2.position, out hit2, AIenemy.Agent.areaMask))
                            {
                                Handles.color = Color.red;
                                Handles.SphereHandleCap(GUIUtility.GetControlID(FocusType.Passive), hit2.position, Quaternion.identity, 0.25f, EventType.Repaint);
                                Handles.Label(hit.position, $"{i} Hide(2) no edge found");
                            }

                            if (Vector3.Dot(hit2.normal, (AIenemy.target.position - hit2.position).normalized) < AIenemy.HideSensitivity)
                            {
                                Handles.color = FoundTarget ? Color.yellow : Color.green;
                                FoundTarget = true;
                                Handles.SphereHandleCap(GUIUtility.GetControlID(FocusType.Passive), hit2.position, Quaternion.identity, 0.25f, EventType.Repaint);
                                Handles.Label(hit2.position, $"{i} Hide(2): {Vector3.Dot(hit2.normal, (AIenemy.target.position - hit2.position).normalized):0.0}");
                            }
                            else
                            {
                                Handles.color = Color.red;
                                Handles.SphereHandleCap(GUIUtility.GetControlID(FocusType.Passive), hit2.position, Quaternion.identity, 0.25f, EventType.Repaint);
                                Handles.Label(hit2.position, $"{i} Hide(2): {Vector3.Dot(hit2.normal, (AIenemy.target.position - hit2.position).normalized):0.0}");
                            }
                        }
                        else
                        {
                            Handles.color = Color.red;
                            Handles.SphereHandleCap(GUIUtility.GetControlID(FocusType.Passive), hit2.position, Quaternion.identity, 0.25f, EventType.Repaint);
                            Handles.Label(hit.position, $"{i} Hide(2) could not sampleposition");
                        }
                    }
                }
            }
        }
    }

    private Vector3 DirectionFromAngle(float eulerY, float angleInDegrees)
    {
        angleInDegrees += eulerY;

        return new Vector3(Mathf.Sin(angleInDegrees * Mathf.Deg2Rad), 0, Mathf.Cos(angleInDegrees * Mathf.Deg2Rad));
    }
}
