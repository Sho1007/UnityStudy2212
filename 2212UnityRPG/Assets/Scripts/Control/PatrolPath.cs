using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace RPG.Control
{
    public class PatrolPath : MonoBehaviour
    {
        [SerializeField] float gizmoSize = 0.5f;
        [SerializeField] Color gizmoColor = Color.white;
        private void OnDrawGizmos()
        {
            SetColor();
             for (int i = 0; i < transform.childCount; i++)
            {
                int j = GetNextIndex(i);
                Gizmos.DrawSphere(GetWayPointPosition(i), gizmoSize);
                Gizmos.DrawLine(GetWayPointPosition(i), GetWayPointPosition(j));
            }
        }

        public int GetNextIndex(int nowIndex)
        {
            if (nowIndex + 1 == transform.childCount) return 0;
            return nowIndex + 1;
        }

        public Vector3 GetWayPointPosition(int i)
        {
            return transform.GetChild(i).position;
        }

        private void SetColor()
        {
            Gizmos.color = gizmoColor;
        }
    }
}