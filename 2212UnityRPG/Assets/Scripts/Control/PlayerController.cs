using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using RPG.Movement;
using RPG.Combat;
using RPG.Attributes;

namespace RPG.Control
{
    public class PlayerController : MonoBehaviour
    {
        private Health health;

        // Start is called before the first frame update
        void Start()
        {
            health = GetComponent<Health>();
            if (health == null) print("[PlayerController] : health is null in " + gameObject.name);
        }

        // Update is called once per frame
        void Update()
        {
            if (health.IsDead()) return;
            if (InteractWithCombat()) return;
            if (InteractWithMovement()) return;
        }

        private static Ray GetMouseRay()
        {
            return Camera.main.ScreenPointToRay(Input.mousePosition);
        }


        private bool InteractWithCombat()
        {
            RaycastHit[] hits = Physics.RaycastAll(GetMouseRay());
            foreach (RaycastHit hit in hits)
            {
                if (hit.transform.GetComponent<CombatTarget>() == null) continue;
                if (GetComponent<Fighter>().CanAttack(hit.transform.gameObject))
                {
                    print("CanAttack");
                    if (Input.GetMouseButton(0))
                    {
                        GetComponent<Fighter>().Attack(hit.transform.gameObject);
                        return true;
                    }
                    
                }
            }
            return false;
        }

        private bool InteractWithMovement()
        {
            return MoveToCursor();
        }

        private bool MoveToCursor()
        {
            RaycastHit hit;
            bool hasHit = Physics.Raycast(GetMouseRay(), out hit);
            if (hasHit)
            {
                if (Input.GetMouseButton(0))
                {
                    GetComponent<Mover>().StartMoveAction(hit.point, 1.0f);
                }
                return true;
            }

            return false;
        }
    }
}