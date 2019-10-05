using System.Collections.Generic;
using UnityEngine;

public class WheelGameHandler : MonoBehaviour
{
    public delegate void SetWheelRulesAction(List<Rule> rules);
    public static event SetWheelRulesAction OnSetWheelRulesAction;

    [SerializeField] List<Rule> rules = null;

    private void Start()
    {
        OnSetWheelRulesAction(rules);
    }

    private void Update()
    {
        
    }
}
