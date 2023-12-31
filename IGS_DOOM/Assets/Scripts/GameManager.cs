using System;
using System.Collections;
using System.Collections.Generic;
using Player;
using Player.Pickups;
using UnityEditor;
using UnityEngine;
using UnityEngine.SceneManagement;

public enum GameState
{
    gameStart,
    gameUpdate,
    gameEnd
}

public class GameManager : MonoBehaviour
{
    [SerializeField] private LayerMask damageableLayer;

    public static GameManager Instance { get; private set; }

    public GameState State;
    public static event Action<GameState> OnGameStateStateChanged;
    public GameObject menu;
    public GameObject end;
    public delegate void  Action();
    public static Action  GlobalAwake;
    public static Action  GlobalStart;
    public static Action  GlobalUpdate;
    public static Action  GlobalFixedUpdate;
    public static Action  GlobalOnEnable;
    public static Action  GlobalOnDisable;

    private void Awake()
    {
        if (Instance != null && Instance != this) { Destroy(this); }
        else { Instance = this; }

        UpdateGameState(GameState.gameStart);
    }

    private WeaponCarrier weapons;

    [ContextMenu("Fire weapon debug")]
    public void FireWeaponDebug()
    {
        StartCoroutine(FireWeaponDebugCoroutine());
    }
    private IEnumerator FireWeaponDebugCoroutine()
    {
        Debug.Log("Start firing");
        weapons.CurrentWeapon.FirePressed();
        yield return new WaitForSeconds(1);
        weapons.CurrentWeapon.FireReleased();
    }

    private void OnEnable()
    {
        GlobalOnEnable?.Invoke();
    }
    
    private void OnDisable()
    {
        GlobalOnDisable?.Invoke();
    }

    private void Start()
    {
        GlobalStart?.Invoke();
    }

    private void Update()
    {
        GlobalUpdate?.Invoke();
    }

    private void FixedUpdate()
    {
        GlobalFixedUpdate?.Invoke();
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            UpdateGameState(GameState.gameEnd);
        }
    }

    private EnemyManager enem;
    private Player.Player play;
    private PickupManager pick;
    public void UpdateGameState(GameState _newState)
    {
        State = _newState;

        switch (_newState)
        {
            case GameState.gameStart:
                end.SetActive(false);
                Cursor.lockState = CursorLockMode.None;
                Cursor.visible = true;
                menu.SetActive(true);
                break;
            case GameState.gameUpdate:
                menu.SetActive(false);
                enem = new EnemyManager(damageableLayer);
                play = new Player.Player();
                pick = new PickupManager(play);
                break;
            case GameState.gameEnd:
                StartCoroutine(EndGame());
                break;
            default:
                throw new ArgumentOutOfRangeException(nameof(_newState), _newState, null);
        }

        OnGameStateStateChanged?.Invoke(_newState);
    }

    private IEnumerator EndGame()
    {
        // Right here should be the logic to destroy <-- DOESNT WORK ON NON MONO BEHAVIOURSSS!!!!!
        // You can make some functions or use the OnEnable or OnDisable functions and invoke them here
        // And in the gameUpdate State. So instead of instantiating every time you start, you just teleport the player
        // to the start and reset the enemyManager and pickupManager
        end.SetActive(true);
        menu.SetActive(true);
        yield return new WaitForSeconds(5);
        UpdateGameState(GameState.gameStart);
    }
    
    public void PlayGame()
    {
        UpdateGameState(GameState.gameUpdate);
    }
}
