using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.AI;

public class GameManager : MonoBehaviour
{
    [System.Serializable]

    public struct AiVariants
    {
        public TextMeshProUGUI textmeshPro;
        public Transform targetAI;
        public Vector3 offset; 
    }

    public AiVariants[] Aivariants;
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        foreach (var pair in Aivariants)
        {
            if (pair.textmeshPro != null)
            {
                Vector3 targetPos = pair.targetAI.position + pair.offset;
                pair.textmeshPro.transform.position = Camera.main.WorldToScreenPoint(targetPos);
                float targetAvoidanceQuality = pair.targetAI.GetComponent<NavMeshAgent>().avoidancePriority;
                pair.textmeshPro.text = targetAvoidanceQuality.ToString();
            }
        }
    }
}
