using System.Collections;
using UnityEditor.Animations;
using UnityEngine;

public class Hero : Character
{
    private static readonly int ThrowAttackAnim = Animator.StringToHash("throw");
    [SerializeField] private CheckCircleOverlapComponent _interactionCheck;
    [SerializeField] private float _fallHeight;
    [SerializeField] private AnimatorController _armed;
    [SerializeField] private AnimatorController _disarmed;
    [SerializeField] private int _throwComboSwordsCount;
    [SerializeField] private float _throwComboCooldown;

    [SerializeField] private Projectile _projectile;

    private bool _allowDoubleJump;
    private bool _isSpeedPotionCrRunning;
    private float _potionEffectDuration;
    private int swordCount => _session.PlayerData.Inventory.Count("Sword");

    private string SelectedId => _session.QuickInventory.SelectedItem.Id;

    private bool HaveItemToSelect
    {
        get
        {
            if (_session.QuickInventory.SelectedItem == null) return false;

            return true;
        }
    }

    private bool CanThrow
    {
        get
        {
            if (!HaveItemToSelect) return false;

            if (SelectedId == "Sword") return swordCount > 1;

            var def = DefsFacade.I.Items.Get(SelectedId);
            return def.HasTag(ItemTag.Throwable);
        }
    }

    private bool CanUse
    {
        get
        {
            if (HaveItemToSelect)
            {
                var def = DefsFacade.I.Items.Get(SelectedId);
                return def.HasTag(ItemTag.Usable);
            }

            return false;
        }
    }

    private void Start()
    {
        Debug.Log(Application.persistentDataPath);
        UpdateHeroWeaponStatus();
        _session.PlayerData.Inventory.OnChanged += OnInventoryChanged;
    }

    protected override void Update()
    {
        base.Update();
        FallHeightCheck();
    }

    private void OnDestroy()
    {
        _session.PlayerData.Inventory.OnChanged -= OnInventoryChanged;
    }

    private void OnInventoryChanged(string id, int value)
    {
        if (id == "Sword") UpdateHeroWeaponStatus();

        Debug.Log($"Inventory Changed: {id} {value}");
    }

    protected override float CalculateYVelocity()
    {
        if (IsGrounded)
        {
            if (IsFalling)
            {
                _spawnPf.Spawn("Landing");
                IsFalling = false;
            }

            _allowDoubleJump = true;
        }

        return base.CalculateYVelocity();
    }

    protected override float CalculateJumpVelocity(float yVelocity)
    {
        if (!IsGrounded && _allowDoubleJump && IsJumping)
        {
            DoJumpVfx();
            _allowDoubleJump = false;
            return _jumpImpulse;
        }

        return base.CalculateJumpVelocity(yVelocity);
    }

    private void FallHeightCheck()
    {
        var hit = Physics2D.Raycast(transform.position,
            Vector2.down, 10, _groundLayerCheck);
        if (hit.collider != null && !IsGrounded)
        {
            if (hit.distance > _fallHeight)
            {
                IsFalling = true;
            }
        }
    }

    public override void OnHealthChanges(int currentHealth)
    {
        _session.PlayerData.Hp.Value = currentHealth;
    }

    public void Interact()
    {
        _interactionCheck.Check();
    }

    private void UpdateHeroWeaponStatus()
    {
        Animator.runtimeAnimatorController = swordCount > 0 ? _armed : _disarmed;
    }

    public override void Attack()
    {
        if (swordCount <= 0) return;
        base.Attack();
    }

    public void ThrowAttack()
    {
        if (_throwCooldown.IsReady && CanThrow)
        {
            var throwableId = SelectedId;
            var throwableDef = DefsFacade.I.ThrowableItems.Get(throwableId);
            _projectile.SetRigidBodyToDynamic();
            base.ThrowAttack(throwableDef.ProjectilePf);
            _session.PlayerData.Inventory.Remove(throwableId, 1);
            _throwCooldown.Reset();
        }
    }

    private IEnumerator DoThrowComboAttack()
    {
        var throwableId = SelectedId;
        var throwableValue = _session.QuickInventory.SelectedItem.Value;
        var throwableDef = DefsFacade.I.ThrowableItems.Get(throwableId);
        var throwswordcount = _throwComboSwordsCount;
        while (throwswordcount > 0)
        {
            if (throwableValue > 0 && throwableId == "Sword" && swordCount > 1)
            {
                Debug.Log("throwableValue : " + throwableValue);
                base.ThrowAttack(throwableDef.ProjectilePf);
                _session.PlayerData.Inventory.Remove(throwableId, 1);
                throwableValue--;
            }
            else if (throwableValue > 0 && throwableId != "Sword")
            {
                Debug.Log("throwableValue : " + throwableValue);
                base.ThrowAttack(throwableDef.ProjectilePf);
                _session.PlayerData.Inventory.Remove(throwableId, 1);
                throwableValue--;
            }

            yield return new WaitForSeconds(_throwComboCooldown);
            throwswordcount--;
        }
    }

    public void ThrowComboAttack()
    {
        if (CanThrow)
        {
            _projectile.SetRigidBodyToKinematic();
            StartCoroutine(DoThrowComboAttack());
        }
    }

    private bool IsSelectedItem(ItemTag tag)
    {
        return _session.QuickInventory.SelectedDef.HasTag(tag);
    }
    
    public void QuickSlotUse()
    {
        if (IsSelectedItem(ItemTag.Throwable))
        {
            //Do Throw
            //Also need detect combothrow attack
        }
        
        if (CanUse && HaveItemToSelect)
        {
            UseItem();
        }
    }

    private void UseItem()
    {
        var usableId = SelectedId;
        var usableDef = DefsFacade.I.UsableItems.Get(usableId);

        switch (usableDef.UsableItemType)
        {
            case UsableItemType.HealthPotion:
                _session.PlayerData.Hp.Value += (int)usableDef.Value;
                break;
            case UsableItemType.SpeedPotion:
                _potionEffectDuration = usableDef.EffectTime;
                if (!_isSpeedPotionCrRunning) StartCoroutine(SpeedPotionEffect(usableDef.Value));

                break;
        }

        Sound.Play("usepotion");
        _session.PlayerData.Inventory.Remove(usableId, 1);
    }

    private IEnumerator SpeedPotionEffect(float value)
    {
        _isSpeedPotionCrRunning = true;
        ChangeSpeedTo(value);
        yield return new WaitForSeconds(_potionEffectDuration);
        ChangeSpeedTo(-value);
        _potionEffectDuration = 0f;
        _isSpeedPotionCrRunning = false;
    }

    public void NextItem()
    {
        _session.QuickInventory.SetNextItem();
    }
}