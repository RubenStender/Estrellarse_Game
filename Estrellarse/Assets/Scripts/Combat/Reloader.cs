using System.Collections;
using UnityEngine;
using UnityEngine.Events;

/// <summary>
/// Magazine + reserve ammo system. Attach next to Shooter.
/// Shooter calls HasAmmo() and ConsumeBullet() — Reloader handles the rest.
/// Works independently: you can also use it for anything else that consumes charges.
/// </summary>
public class Reloader : MonoBehaviour
{
    [Header("Ammo")]
    [SerializeField] private int magazineSize   = 30;
    [SerializeField] private int reserveAmmo    = 120;
    [SerializeField] private bool infiniteAmmo  = false;

    [Header("Reload")]
    [SerializeField] private float reloadTime   = 1.8f;
    [SerializeField] private bool  autoReload   = true;    // Reload automatically on empty

    [Header("Events")]
    public UnityEvent<int, int> OnAmmoChanged;  // (currentMag, reserve)
    public UnityEvent OnReloadStart;
    public UnityEvent OnReloadComplete;
    public UnityEvent OnOutOfAmmo;

    public int CurrentMag   { get; private set; }
    public int ReserveAmmo  { get; private set; }
    public bool IsReloading { get; private set; }

    private void Awake()
    {
        CurrentMag  = magazineSize;
        ReserveAmmo = reserveAmmo;
    }

    public bool HasAmmo() => infiniteAmmo || CurrentMag > 0;

    public void ConsumeBullet()
    {
        if (infiniteAmmo) return;

        CurrentMag = Mathf.Max(0, CurrentMag - 1);
        OnAmmoChanged?.Invoke(CurrentMag, ReserveAmmo);

        if (CurrentMag == 0)
        {
            if (ReserveAmmo > 0 && autoReload)
                TryReload();
            else if (ReserveAmmo == 0)
                OnOutOfAmmo?.Invoke();
        }
    }

    /// <summary>Called by InputBridge on R press.</summary>
    public void TryReload()
    {
        if (IsReloading)        return;
        if (CurrentMag == magazineSize) return;
        if (ReserveAmmo <= 0)  return;

        StartCoroutine(ReloadRoutine());
    }

    public void AddReserveAmmo(int amount)
    {
        ReserveAmmo += amount;
        OnAmmoChanged?.Invoke(CurrentMag, ReserveAmmo);
    }

    private IEnumerator ReloadRoutine()
    {
        IsReloading = true;
        OnReloadStart?.Invoke();

        yield return new WaitForSeconds(reloadTime);

        int needed  = magazineSize - CurrentMag;
        int loaded  = Mathf.Min(needed, ReserveAmmo);
        CurrentMag  += loaded;
        ReserveAmmo -= loaded;

        IsReloading = false;
        OnReloadComplete?.Invoke();
        OnAmmoChanged?.Invoke(CurrentMag, ReserveAmmo);
    }
}
