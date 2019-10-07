using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class WheelOfFortune : MonoBehaviour
{
    [SerializeField] Canvas canvas = null;
    [SerializeField] GameObject rulesSlots = null;
    [SerializeField] AudioSource clickAudio = null;
    [SerializeField] AudioSource tinAudio = null;
    [SerializeField] Sprite background = null;
    [SerializeField] float initSpeedAverage = 300f;
    [SerializeField] float initSpeedDeviation = 50f;
    [SerializeField] float friction = 1.005f;
    [SerializeField] float minSpeedAccepted = 10f;
    [SerializeField] float scaleOfRules = 0.4f;
    [SerializeField] int powersCopies = 2;

    public delegate void ResultAction(GameObject rule);
    public static event ResultAction OnResultAction;

    private float speed;
    private float shift;
    private float x;
    private float y;
    private float width;
    private float height;
    private float clickDistance;
    private bool end;

    private List<GameObject> rules;
    private List<GameObject> rulesObjects;

    private const float MAGIC_HEIGHT = 4f;
    private const float SOUND_OFFSET = 12;
    
    public void ResetWheel()
    {
        x = rulesSlots.transform.position.x;
        y = rulesSlots.transform.position.y;
        width = rulesSlots.GetComponent<RectTransform>().rect.width;
        height = rulesSlots.GetComponent<RectTransform>().rect.height;
        clickDistance = -SOUND_OFFSET;

        // Destroy previous instances
        if (rulesObjects != null)
        {
            for (int i = rulesObjects.Count - 1; i >= 0; i--)
            {
                Destroy(rulesObjects[i]);
            }
        }

        // Instantiate corresponding game objects
        int counter = 1;
        rulesObjects = new List<GameObject>();
        foreach (GameObject rule in rules)
        {
            for (int i = 0; i < powersCopies; i++)
            {
                GameObject ruleObject = Instantiate(rule);
                ruleObject.SetActive(true);

                // Prefab parameters
                ruleObject.transform.SetParent(rulesSlots.transform);
                ruleObject.name = "Rule " + counter;
                ruleObject.transform.localScale = new Vector3(scaleOfRules, scaleOfRules, scaleOfRules);
                ruleObject.transform.position = rulesSlots.transform.position;

                // Background image
                Sprite bg = Instantiate(background);
                ruleObject.AddComponent<Image>();
                ruleObject.GetComponent<Image>().sprite = bg;
                ruleObject.GetComponent<Image>().color = ruleObject.GetComponent<PowerShape>().SpriteColor;

                rulesObjects.Add(ruleObject);
                counter++;
            }
        }

        // Shuffle powers
        ShufflePowers();

        speed = initSpeedAverage + Random.Range(-initSpeedDeviation, initSpeedDeviation);
        shift = 0;
        end = false;
    }

    public void SetRules(List<GameObject> rules)
    {
        this.rules = rules;
        ResetWheel();
    }

    private void OnEnable()
    {
        GameHandler.OnSetWheelRulesAction += SetRules;
    }
    
    private void OnDisable()
    {
        GameHandler.OnSetWheelRulesAction -= SetRules;
    }

    private void Update()
    {
        x = rulesSlots.transform.position.x;
        y = rulesSlots.transform.position.y;
        width = rulesSlots.GetComponent<RectTransform>().rect.width;
        height = rulesSlots.GetComponent<RectTransform>().rect.height;

        if (rules.Count > 0)
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

            // End
            if (speed == 0 && !end)
            {
                end = true;
                tinAudio.Play();
                OnResultAction(ClosestToCenter());
            }

            shift = shift + speed * Time.deltaTime;
            clickDistance += speed * Time.deltaTime;
            
            float ruleDiameter = MAGIC_HEIGHT / canvas.scaleFactor;

            // Sound
            if (clickDistance > ruleDiameter)
            {
                clickDistance = shift % ruleDiameter - SOUND_OFFSET;
                clickAudio.Play();
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
                if (indepShift > -ruleDiameter * rulesObjects.Count / 2 
                    && indepShift < ruleDiameter * rulesObjects.Count / 2)
                {
                    ruleObject.SetActive(true);
                }
                else
                {
                    ruleObject.SetActive(false);
                }
            }
        }

        if (Input.GetKeyDown("space"))
        {
            ResetWheel();
        }
    }

    private void ShufflePowers()
    {
        for (int i = 0; i < rulesObjects.Count; i++)
        {
            int j = Random.Range(0, rulesObjects.Count);

            GameObject tmp = rulesObjects[i];
            rulesObjects[i] = rulesObjects[j];
            rulesObjects[j] = tmp;
        }
    }

    private GameObject ClosestToCenter()
    {
        GameObject winner = null;
        float minDist = float.MaxValue;

        foreach (GameObject rule in rulesObjects)
        {
            float dist = Mathf.Abs(rule.transform.position.x - rulesSlots.transform.position.x);
            if (dist < minDist)
            {
                minDist = dist;
                winner = rule;
            }
        }

        string name = winner.GetComponent<PowerShape>().DisplayedName;
        foreach (GameObject rule in rules)
        {
            if (rule.GetComponent<PowerShape>().DisplayedName.Equals(name))
            {
                return rule;
            }
        }

        return null;
    }
}
