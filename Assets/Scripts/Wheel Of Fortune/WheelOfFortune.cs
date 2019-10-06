using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class WheelOfFortune : MonoBehaviour
{
    [SerializeField] Canvas canvas = null;
    [SerializeField] GameObject rulesSlots = null;
    [SerializeField] AudioSource clickAudio = null;
    [SerializeField] AudioSource tinAudio = null;
    [SerializeField] GameObject background = null;
    [SerializeField] float initSpeedAverage = 300f;
    [SerializeField] float initSpeedDeviation = 50f;
    [SerializeField] float friction = 1.005f;
    [SerializeField] float minSpeedAccepted = 10f;
    [SerializeField] float scaleOfRules = 0.4f;
    [SerializeField] List<Rarity> rarities;

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

    private List<Rule> rules;
    private List<GameObject> allRules;
    private List<GameObject> rulesObjects;
    private List<int> occurences;

    private const float MAGIC_HEIGHT = 17f;
    private const float SOUND_OFFSET = 12;
    
    public void ResetWheel()
    {
        x = rulesSlots.transform.position.x;
        y = rulesSlots.transform.position.y;
        width = rulesSlots.GetComponent<RectTransform>().rect.width;
        height = rulesSlots.GetComponent<RectTransform>().rect.height;
        clickDistance = -SOUND_OFFSET;

        // Generate all rules with the correspondingnumber of occurences
        allRules = new List<GameObject>();
        occurences = new List<int>();
        foreach (Rule rule in rules)
        {
            for (int i = 0; i < rule.Occurences; i++)
            {
                allRules.Add(rule.Object);
                occurences.Add(rule.Occurences);
            }
        }

        // Shuffle the rules
        ShuffleRules();

        // Instantiate corresponding game objects
        int counter = 1;
        rulesObjects = new List<GameObject>();
        foreach (GameObject rule in allRules)
        {
            GameObject ruleObject = Instantiate(rule);
            ruleObject.SetActive(true);

            // Prefab parameters
            ruleObject.transform.SetParent(rulesSlots.transform);
            ruleObject.name = "Rule " + counter;
            ruleObject.transform.localScale = new Vector3(scaleOfRules, scaleOfRules, scaleOfRules);
            ruleObject.transform.position = rulesSlots.transform.position;

            // Background image
            GameObject bg = Instantiate(background);
            bg.name = "Background";
            bg.transform.SetParent(ruleObject.transform);
            bg.transform.localScale = new Vector3(4, 4, 4);
            bg.transform.position = ruleObject.transform.position;
            bg.transform.SetAsFirstSibling();
            bg.GetComponent<Image>().color = RarityColor(occurences[counter - 1]);

            rulesObjects.Add(ruleObject);
            counter++;
        }

        speed = initSpeedAverage + Random.Range(-initSpeedDeviation, initSpeedDeviation);
        shift = 0;
        end = false;
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

            // End
            if (speed == 0 && !end)
            {
                end = true;
                tinAudio.Play();
                OnResultAction(ClosestToCenter());
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

    private void ShuffleRules()
    {
        for (int i = 0; i < allRules.Count; i++)
        {
            int j = Random.Range(0, allRules.Count);

            GameObject tmp = allRules[i];
            allRules[i] = allRules[j];
            allRules[j] = tmp;

            int tmpRarity = occurences[i];
            occurences[i] = occurences[j];
            occurences[j] = tmpRarity;
        }
    }

    private Color RarityColor(int occurences)
    {
        foreach(Rarity rarity in rarities)
        {
            if (occurences <= rarity.Occurences)
            {
                return rarity.Color;
            }
        }

        return Color.gray;
    }

    private GameObject ClosestToCenter()
    {
        GameObject winner = null;
        float minDist = float.MaxValue;

        foreach (GameObject rule in rulesObjects)
        {
            float dist = Mathf.Abs(rule.transform.position.x);
            if (dist < minDist)
            {
                minDist = dist;
                winner = rule;
            }
        }

        return Instantiate(winner);
    }
}
