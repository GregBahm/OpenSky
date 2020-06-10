using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;

public class SpaceShip : AnimationRecorder<SpaceshipKey>, IHitable
{
    public int TeamId { get; }
    public float MaxHP { get; }
    public float CurrentHP { get; private set; }
    public GameObject GameObject { get; }
    public SpaceManuverability Manuverability { get; }
    public IReadOnlyCollection<ISpaceshipWeapon> Weapons { get; }
    public IReadOnlyCollection<DamageEffect> DamageEffectsPool { get; }
    public FlightPath CurrentPath { get; set; }
    public IEnumerable<IAnimationRecorder> ViewableObjects
    {
        get
        {
            yield return this;
            foreach (ISpaceshipWeapon weapon in Weapons)
            {
                yield return weapon;
                foreach(IAnimationRecorder projectile in weapon.Projectiles)
                {
                    yield return projectile;
                }
            }
            foreach (IAnimationRecorder damageEffect in DamageEffectsPool)
            {
                yield return damageEffect;
            }
        }
    }

    public float CurrentSpeed { get; private set; }

    private bool isActive;
    public override bool IsActive => isActive;
    
    private readonly SpaceshipViewModel viewModel;

    private readonly MeshCollider hitZone;
    
    public SpaceShip(int teamId,
        float hitpoints,
        SpaceManuverability manuverability, 
        IEnumerable<Func<SpaceShip, ISpaceshipWeapon>> shipGetters,
        GameObject gameObject,
        MeshCollider hitZone)
    {
        isActive = true;
        TeamId = teamId;
        MaxHP = hitpoints;
        CurrentHP = hitpoints;
        GameObject = gameObject;
        Manuverability = manuverability;
        Weapons = shipGetters.Select(item => item(this)).ToList().AsReadOnly();
        this.hitZone = hitZone;
        this.viewModel = new SpaceshipViewModel(gameObject);
        CurrentSpeed = manuverability.MaxThrust / Game.KeyframesPerTurn;
    }

    public void RegisterDamage(IEnumerable<SpaceShip> activeShips, 
        IEnumerable<Projectile> activeProjectiles)
    {
        float totalDamage = 0;
        foreach (Projectile projectile in activeProjectiles)
        {
            if(IsHitBy(projectile))
            {
                totalDamage += projectile.Damage;
                projectile.OnSpaceshipHit(this);
            }
        }
        CurrentHP -= totalDamage;
    }

    public void MoveEntity(int turnStep)
    {
        Pose pose = CurrentPath.Poses[turnStep];

        // TODO: Handle near ship avoidance
        CurrentSpeed = (GameObject.transform.position - pose.position).magnitude;

        GameObject.transform.position = pose.position;
        GameObject.transform.rotation = pose.rotation;
    }
    
    public void UpdateState()
    {
        if(CurrentHP < 0)
        {
            this.isActive = false;
            //TODO: Spaceship destruction display
        }
        foreach (ISpaceshipWeapon item in Weapons)
        {
            item.UpdateState();
        }
    }

    public void InitiateNewAttacks(IEnumerable<SpaceShip> enemies)
    {
        foreach (ISpaceshipWeapon weapon in Weapons.Where(weapon => weapon.CanInitiateNewAttack))
        {
            weapon.TryInitiateNewAttack(enemies);
        }
    }

    public bool IsHitBy(IDamageSource source)
    {
        Physics.CapsuleCast
    }

    public override void ClearVisuals()
    {
        GameObject.SetActive(false);
    }

    protected override SpaceshipKey MakeKeyFromCurrentState()
    {
        float destruction = CurrentHP / MaxHP;
        return new SpaceshipKey(GameObject.transform.position,
            GameObject.transform.rotation,
            destruction);
    }

    protected override void Display(SpaceshipKey key)
    {
        viewModel.DisplayKey(key);
    }
}

public class SpaceshipViewModel
{
    private readonly GameObject gameObject;

    public SpaceshipViewModel(GameObject gameObject)
    {
        this.gameObject = gameObject;
    }

    public void DisplayKey(SpaceshipKey key)
    {
        // TODO: Show moar stuff with the key
        gameObject.SetActive(key.Destruction > 0);
        gameObject.transform.position = key.Position;
        gameObject.transform.rotation = key.Rotation;
    }
}