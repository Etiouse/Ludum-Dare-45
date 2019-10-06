using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class WheelOfFortune : MonoBehaviour
{
    [SerializeField] Canvas canvas = null;
    [SerializeField] GameObject rulesSlots = null;
    [SerializeField] AudioSource audioSource = null;
    [SerializeField] float initSpeedAverage = 300f;
    [SerializeField] float initSpeedDeviation = 50f;
    [SerializeField] float friction = 1.005f;
    [SerializeField] float minSpeedAccepted = 10f;

    private float speed;
    private float shift;
    private float x;
    private float y;
    private float width;
    private float height;
    private float clickDistance;

    private List<Rule> rules;
    private List<Image> allRules;
    private List<GameObject> rulesObjects;

    private const float MAGIC_HEIGHT = 17f;
    private const float SOUND_OFFSET = 20f;
    
    public void ResetWheel()
    {
        x = rulesSlots.transform.position.x;
        y = rulesSlots.transform.position.y;
        width = rulesSlots.GetComponent<RectTransform>().rect.width;
        height = rulesSlots.GetComponent<RectTransform>().rect.height;
        clickDistance = -SOUND_OFFSET;

        // Generate all rules with the correspondingnumber of occurences
        allRules = new List<Image>();
        foreach (Rule rule in rules)
        {
            for (int i = 0; i < rule.Occurences; i++)
            {
                allRules.Add(rule.Image);
            }
        }

        // Shuffle the rules
        ShuffleRules();

        // Instantiate corresponding game objects
        int counter = 1;
        rulesObjects = new List<GameObject>();
        foreach (Image rule in allRules)
        {
            GameObject ruleObject = Instantiate(rule.gameObject);

            ruleObject.transform.SetParent(rulesSlots.transform);
            ruleObject.name = "Rule " + counter;
            ruleObject.transform.localScale = new Vector3(1, 1, 1);
            ruleObject.GetComponent<RectTransform>().sizeDelta = new Vector2(height, height);
            ruleObject.transform.position = rulesSlots.transform.position;

            rulesObjects.Add(ruleObject);
            counter++;
        }

        speed = initSpeedAverage + Random.Range(-initSpeedDeviation, initSpeedDeviation);
        shift = 0;
    }

    public void SetRules(List<Rule> rules)
    {
        this.rules = rules;
        ResetWheel();
    }

    private void OnEnable()
    {
        WheelGameHandler.OnSetWheelRulesAction += SetRules;
    }
    
    private void OnDisable()
    {
        WheelGameHandler.OnSetWheelRulesAction -= SetRules;
    }

    private void Update()
    {
        x = rulesSlots.transform.position.x;
        y = rulesSlots.transform.position.y;
        width = rulesSlots.GetComponent<RectTransform>().rect.width;
        height = rulesSlots.GetComponent<RectTransform>().rect.height;

        if (allRules.Count > 0)
        {
            // Speed adjustment
            if (speed > minSpeedAccepted)
            {
                speed /= friction;
            }
            else if (speed < 10)
            {
                speed = 0;
            }
            else
            {
                speed /= Mathf.Pow(friction, 4);
            }

            shift = shift + speed * Time.deltaTime;
            clickDistance += speed * Time.deltaTime;

            // Responsivity
            float ratio = (float) Screen.width / Screen.height;
            float factor = 9.5543f * ratio + 0.0650f;
            float ruleDiameter = factor / canvas.scaleFactor;

            // Sound
            if (clickDistance > ruleDiameter)
            {
                clickDistance = shift % ruleDiameter - SOUND_OFFSET;
                audioSource.Play();
            }

            // Position of each rule
            float middle = ruleDiameter * (rulesObjects.Count / 2);
            for (int i = 0; i < rulesObjects.Count; i++)
            {
                GameObject ruleObject = rulesObjects[i];

                float indepShift = (ruleDiameter * i) + shift;
                int cycles = 1 + Mathf.FloorToInt((indepShift - middle) / (ruleDiameter * rulesObjects.Count));

                // Cycling shifting
                if (indepShift > middle)
                {
                    indepShift -= 2 * middle * cycles;

                    // Odd numbers cornercase
                    if (rulesObjects.Count % 2 != 0)
                    {
                        indepShift -= ruleDiameter + (cycles - 1) * ruleDiameter;
                    }
                }

                // Position modification
                ruleObject.transform.position = rulesSlots.transform.position
                    + new Vector3(indepShift * canvas.scaleFactor, 0, 0);

                // Hide or show depending on the position
                if (indepShift > -ruleDiameter * rulesObjects.Count / 3 
                    && indepShift < ruleDiameter * rulesObjects.Count / 3)
                {
                    ruleObject.SetActive(true);
                }
                else
                {
                    ruleObject.SetActive(false);
                }
            }
        }
    }

    private void ShuffleRules()
    {
        for (int i = 0; i < allRules.Count; i++)
        {
            int j = Random.Range(0, allRules.Count);

            Image tmp = allRules[i];
            allRules[i] = allRules[j];
            allRules[j] = tmp;
        }
    }
}
