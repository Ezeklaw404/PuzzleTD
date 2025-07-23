using UnityEngine;
using System;
using UnityEngine.UIElements;


public class Player : MonoBehaviour
{
    public static Player Instance { get; private set; }

    [SerializeField] private int health = 100;
    [SerializeField] private int money = 100;
    private bool alive = true;

    public event Action OnStatsChanged;

    private void Awake()
    {
        Instance = this;
    }

    public int Health => health;
    public int Money => money;
    public bool IsAlive => alive;

    private Label healthLabel;
    private Label moneyLabel;

    
    
    private void Start()
    {
        var uiDoc = FindFirstObjectByType<UIDocument>();
        // Find the UIDocument in scene (make sure you have one)
        if (uiDoc != null)
        {
            var root = uiDoc.rootVisualElement;
            healthLabel = root.Q<Label>("Health");
            moneyLabel = root.Q<Label>("Money");

            UpdateUI();
        }
    }

    private void UpdateUI()
    {
        if (healthLabel != null)
            healthLabel.text = "Health: " + health;

        if (moneyLabel != null)
            moneyLabel.text = "Money: $" + money;
    }


    public void Yeouch(int dmgTaken)
    {
        health -= dmgTaken;
        if (health <= 0)
        {
            health = 0;
            alive = false;
        }
        OnStatsChanged?.Invoke();
        UpdateUI();
    }

    public void AddMoney(int amount)
    {
        money += amount;
        OnStatsChanged?.Invoke();
        UpdateUI();
    }

    public void Purchase(int amount)
    {
        money -= amount;
        OnStatsChanged?.Invoke();
        UpdateUI();
    }


}