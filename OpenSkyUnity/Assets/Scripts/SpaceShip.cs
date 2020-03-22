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
        }
    }

    private bool isActive;
    public override bool IsActive => isActive;
    
    private readonly SpaceshipViewModel viewModel;
    
    public SpaceShip(int teamId,
        float hitpoints,
        SpaceManuverability manuverability, 
        IEnumerable<Func<SpaceShip, ISpaceshipWeapon>> shipGetters,
        GameObject gameObject)
    {
        TeamId = teamId;
        MaxHP = hitpoints;
        CurrentHP = hitpoints;
        GameObject = gameObject;
        Manuverability = manuverability;
        Weapons = shipGetters.Select(item => item(this)).ToList().AsReadOnly();
        this.viewModel = new SpaceshipViewModel(gameObject);
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
        
        // TODO: Handle near ship avoidance here

        GameObject.transform.position = pose.position;
        GameObject.transform.rotation = pose.rotation;
    }
    
    public void UpdateState()
    {
        if(CurrentHP < 0)
        {
            this.isActive = false;
            //TODO: Figure out how you're going to do a destruction animation
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
        throw new NotImplementedException();
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
        // TODO
    }
}