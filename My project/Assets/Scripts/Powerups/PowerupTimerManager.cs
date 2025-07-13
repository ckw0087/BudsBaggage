using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class PowerupTimerManager : MonoBehaviour
{
    public enum PowerupType
    {
        Magnet, 
        SpeedBoost, 
        InstantDeposit
    }

    [SerializeField] private Transform _timerBarParent; // The layout group
    [SerializeField] private PowerupTimerSlot _slotPrefab; // Your slot prefab

    [SerializeField] private Sprite _magnetSprite;
    [SerializeField] private Sprite _speedSprite;
    [SerializeField] private Sprite _instantDepositSprite;

    private Dictionary<PowerupType, PowerupTimerSlot> _activeSlots = new();

    public static PowerupTimerManager Instance { get; private set; }

    private void Awake()
    {
        if (Instance != null && Instance != this)
        {
            Destroy(gameObject);
            return;
        }
        Instance = this;
    }

    public void ShowPowerupTimer(PowerupType type, float duration)
    {
        // If already showing this powerup, reset timer
        if (_activeSlots.TryGetValue(type, out var existingSlot))
        {
            existingSlot.Restart(duration);
            return;
        }

        // Create new slot
        var slot = Instantiate(_slotPrefab, _timerBarParent);
        slot.Init(GetSprite(type), duration, () => {
            _activeSlots.Remove(type);
            Destroy(slot.gameObject);
        });
        _activeSlots[type] = slot;
    }

    private Sprite GetSprite(PowerupType type)
    {
        return type switch
        {
            PowerupType.Magnet => _magnetSprite,
            PowerupType.SpeedBoost => _speedSprite,
            PowerupType.InstantDeposit => _instantDepositSprite,
            _ => null
        };
    }
}
