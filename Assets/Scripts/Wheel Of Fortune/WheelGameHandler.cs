using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.UI;

public class WheelGameHandler : MonoBehaviour
{
    public delegate void SetWheelRulesAction(List<Rule> rules);
    public static event SetWheelRulesAction OnSetWheelRulesAction;

    [SerializeField] GameObject winner = null;
    [SerializeField] TMP_Text description = null;
    [SerializeField] Button continueButton = null;
    [SerializeField] List<Rule> rules = null;

    private const float SCALE = 0.5f;

    private void OnEnable()
    {
        WheelOfFortune.OnResultAction += ResultRule;
    }

    private void OnDisable()
    {
        WheelOfFortune.OnResultAction -= ResultRule;
    }

    private void Start()
    {
        OnSetWheelRulesAction(rules);
        continueButton.gameObject.SetActive(false);
    }

    private void Update()
    {
        
    }

    private void ResultRule(GameObject rule)
    {
        rule.transform.SetParent(winner.transform);
        rule.transform.localScale = new Vector3(SCALE, SCALE, SCALE);
        rule.transform.localPosition = new Vector3(100, 0, 0);

        description.text = rule.GetComponent<PowerShape>().Description;
        continueButton.gameObject.SetActive(true);
    }
}
