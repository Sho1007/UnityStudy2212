using UnityEngine;
using UnityEngine.UI;
using RPG.Attributes;

namespace RPG.Combat
{
    public class EnemyHealthDisplay : MonoBehaviour
    {
        Health health;
        Fighter fighter;

        private void Awake()
        {
            fighter = GameObject.FindWithTag("Player").GetComponent<Fighter>();
        }
        
        private void Update()
        {
            // TODO : Update your text
            if (fighter.GetTarget() != null)
                GetComponent<Text>().text = string.Format("{0:0}%", fighter.GetTarget().GetPercentage());
            else
                GetComponent<Text>().text = "N/A";
        }
    }
}