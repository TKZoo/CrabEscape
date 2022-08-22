using System.Collections;
using UnityEditor.Animations;
using UnityEngine;

public class Hero : Character
{
    //private static readonly int ThrowAttackAnim = Animator.StringToHash("throw");
    [SerializeField] private CheckCircleOverlapComponent _interactionCheck;
    [SerializeField] private float _fallHeight;
    [SerializeField] private AnimatorController _armed;
    [SerializeField] private AnimatorController _disarmed;
    [SerializeField] private int _throwComboSwordsCount;
    [SerializeField] private float _throwComboCooldown;
    [SerializeField] private ShieldSkillComponent _shieldSkill;

    [SerializeField] private Projectile _projectile;
    
    private bool _allowDoubleJump;
    private bool _additionalSpeed = false;
    private float _additionalSpeedValue = 0;
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

            if (SelectedId == "Sword") return swordCount > 1; // check condition looks wired

            var def = DefsFacade.I.Items.Get(SelectedId);
            return def.HasTag(ItemTag.Throwable);
        }
    }

    private bool CanConsume
    {
        get
        {
            if (HaveItemToSelect)
            {
                var def = DefsFacade.I.Items.Get(SelectedId);
                return def.HasTag(ItemTag.Consumable);
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
        FallHeightCheck();
        base.Update();
    }

    private void OnDestroy()
    {
        _session.PlayerData.Inventory.OnChanged -= OnInventoryChanged;
    }

    private void OnInventoryChanged(string id, int value)
    {
        if (id == "Sword") UpdateHeroWeaponStatus();

        //Debug.Log($"Inventory Changed: {id} {value}");
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
        if (!IsGrounded && _allowDoubleJump && IsJumping && _session.PerksModel.IsDoubleJumpEnabled)
        {
            DoJumpVfx();
            _allowDoubleJump = false;
            return _jumpImpulse;
        }

        return base.CalculateJumpVelocity(yVelocity);
    }

    protected override float CalculateSpeed()
    {
        return _session.StatsModel.GetValue(statId.Speed) + _additionalSpeedValue;
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

    public override void TakeDamage()
    {
        _allowDoubleJump = false;
        base.TakeDamage();
    }

    public override void OnHealthChanges(int currentHealth)
    {
        Health.Hp.Value = currentHealth;
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
        var throwableId = SelectedId;
        var throwableDef = DefsFacade.I.ThrowableItems.Get(throwableId);
        _projectile.SetRigidBodyToDynamic();
        base.ThrowAttack(throwableDef.ProjectilePf);
        _session.PlayerData.Inventory.Remove(throwableId, 1);
        _throwCooldown.Reset();
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
        _projectile.SetRigidBodyToKinematic();
        StartCoroutine(DoThrowComboAttack());
    }

    public void QuickSlotUse()
    {
        if (CanThrow && _throwCooldown.IsReady)
        {
            ThrowAttack();
            return;
        }
        
        if (CanConsume && HaveItemToSelect)
        {
            UseConsumableItem();
            return;
        }
    }

    private void UseConsumableItem()
    {
        var consumableId = SelectedId;
        var consumableDef = DefsFacade.I.ConsumableItems.Get(consumableId);

        switch (consumableDef.ConsumableItemType)
        {
            case ConsumableItemType.HealthPotion:
                _session.PlayerData.Hp.Value += (int)consumableDef.Value;
                break;
            case ConsumableItemType.SpeedPotion:
                _potionEffectDuration = consumableDef.EffectTime;
                if (!_additionalSpeed) StartCoroutine(SpeedPotionEffect(consumableDef.Value));

                break;
        }

        Sound.Play("usepotion");
        _session.PlayerData.Inventory.Remove(consumableId, 1);
    }

    public void UsePerk()
    {
        if (_session.PerksModel.IsShieldEnabled)
        {
           _shieldSkill.UseShield();
           _session.PerksModel.Cooldown.Reset();
        }
        if (_session.PerksModel.IsSuperThrowEnabled && CanThrow)
        {
            ThrowComboAttack();
            _session.PerksModel.Cooldown.Reset();
        }
    }

    private IEnumerator SpeedPotionEffect(float value)
    {
        _additionalSpeed = true;
        _additionalSpeedValue = value; 
        yield return new WaitForSeconds(_potionEffectDuration);
        _additionalSpeedValue = 0;
        _potionEffectDuration = 0f;
        _additionalSpeed = false;
    }

    public void NextItem()
    {
        _session.QuickInventory.SetNextItem();
        Debug.Log(SelectedId);
    }
}