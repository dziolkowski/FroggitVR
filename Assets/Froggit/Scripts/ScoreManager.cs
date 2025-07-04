using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class ScoreManager : MonoBehaviour
{
    public static ScoreManager Instance;

    public TextMeshProUGUI ammoText;
    public TextMeshProUGUI scoreText;
    public TextMeshProUGUI lastHitText;
    public GameObject endScreen;
    public TextMeshProUGUI finalScoreText;
    public TextMeshProUGUI hitLogText;
    public Transform magazine;

    private int score = 0;
    private List<string> hitLog = new List<string>();

    private Dictionary<string, int> hitPointsByPart = new Dictionary<string, int>();
    private Dictionary<string, int> hitCountByPart = new Dictionary<string, int>();

    void Awake()
    {
        Instance = this;
    }

    public void RegisterHit(int points, string partName)
    {
        score += points;

        lastHitText.text = $"Last Hit: {points} pts";
        hitLog.Add($"{partName}: {points} pts");

        if (hitPointsByPart.ContainsKey(partName))
            hitPointsByPart[partName] += points;
        else
            hitPointsByPart[partName] = points;

        if (hitCountByPart.ContainsKey(partName))
            hitCountByPart[partName]++;
        else
            hitCountByPart[partName] = 1;

        UpdateUI();
    }

    public void UpdateAmmo(int remainingAmmo)
    {
        ammoText.text = $"Ammo: {remainingAmmo}";
    }

    void UpdateUI()
    {
        scoreText.text = $"Score: {score}";
    }

    public int GetPointsForPart(string part)
    {
        switch (part)
        {
            case "Head": return 10;
            case "Torso": return 5;
            case "Arm": return 3;
            case "Leg": return 3;
            default: return 1;
        }
    }

    public IEnumerator ForceEnd()
    {
        yield return new WaitForSeconds(2f);

        endScreen.SetActive(true);
        finalScoreText.text = $"Final Score: {score}";

        hitLogText.text = "";
        foreach (var entry in hitPointsByPart)
        {
            string part = entry.Key;
            int totalPoints = entry.Value;
            int hitCount = hitCountByPart.ContainsKey(part) ? hitCountByPart[part] : 0;
            hitLogText.text += $"{part}: {totalPoints} pts ({hitCount} hit{(hitCount > 1 ? "s" : "")})\n";
        }
    }
}