using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.UI;

public class WheelGameHandler : MonoBehaviour
{
    public delegate void SetWheelRulesAction(List<Rule> rules);
    public delegate void TransmitNewPowerAction(GameObject newPower);
    public static event SetWheelRulesAction OnSetWheelRulesAction;
    public static event TransmitNewPowerAction OnTransmitNewPowerAction;

    [SerializeField] GameObject winner = null;
    [SerializeField] TMP_Text description = null;
    [SerializeField] Button continueButton = null;
    [SerializeField] List<Rule> rules = null;

    private GameObject display;

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

    private void ResultRule(GameObject power)
    {
        if (power != null)
        {
            if (display != null)
            {
                Destroy(display);
            }

            display = Instantiate(power);
            display.transform.SetParent(winner.transform);
            display.transform.localScale = new Vector3(SCALE, SCALE, SCALE);
            display.transform.localPosition = new Vector3(100, 0, 0);
            display.SetActive(true);

            description.text = power.GetComponent<PowerShape>().Description;
            continueButton.gameObject.SetActive(true);

            //OnTransmitNewPowerAction(power);
        }
        else
        {
            Debug.Log(":(");
        }
    }
}
