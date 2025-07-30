using UnityEngine;
using System;
using UnityEngine.UIElements;


public class Player : MonoBehaviour
{
    public static Player Instance { get; private set; }

    [SerializeField] private int health;
    [SerializeField] private int money;
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
    private Label waveLabel;

    private EnemySpawner enemySpawner;

    void OnEnable()
    {
        Enemy.OnDeath += HandleEnemyDeath;
    }

    void OnDisable()
    {
        Enemy.OnDeath -= HandleEnemyDeath;
    }

    private void HandleEnemyDeath(Enemy dead)
    {
        AddMoney(dead.GetWeight() * 10);
    }

    private void Start()
    {
        var uiDoc = FindFirstObjectByType<UIDocument>();
        // Find the UIDocument in scene (make sure you have one)
        if (uiDoc != null)
        {
            var root = uiDoc.rootVisualElement;
            healthLabel = root.Q<Label>("Health");
            moneyLabel = root.Q<Label>("Money");
            waveLabel = root.Q<Label>("Wave");

            enemySpawner = FindFirstObjectByType<EnemySpawner>();

            UpdateUI();
        }
    }

    public void UpdateUI()
    {
        if (healthLabel != null)
            healthLabel.text = $"Health: {health}";

        if (moneyLabel != null)
            moneyLabel.text = $"Money: ${money}";

        if (waveLabel != null)
            waveLabel.text = $"Wave: { enemySpawner.GetWaveNumber() + 1}";
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