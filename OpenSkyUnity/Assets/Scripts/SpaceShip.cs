using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;

public class SpaceShip : IViewableSpaceObject, IHitable
{
    public int TeamId { get; }
    public float MaxHP { get; }
    public float CurrentHP { get; private set; }
    public GameObject GameObject { get; }
    public ItemTimeline<SpaceshipKey> Timeline { get; }
    public SpaceManuverability Manuverability { get; }
    public IReadOnlyCollection<ISpaceshipWeapon> Weapons { get; }
    public bool IsActive { get; private set; }
    public Vector3 TargetPosition { get; set; }
    public Vector3 CurrentMomentum { get; set; }
    public IEnumerable<IViewableSpaceObject> ViewableObjects
    {
        get
        {
            yield return this;
            foreach (IViewableSpaceObject item in Weapons.SelectMany(item => item.Projectiles))
            {
                yield return item;
            }
        }
    }
    
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
        Timeline = new ItemTimeline<SpaceshipKey>(MakeKeyFromGameobject());
    }

    private SpaceshipKey MakeKeyFromGameobject()
    {
        float destruction = CurrentHP / MaxHP;
        float weaponAttack = 0;//TODO: Weapon attack
        return new SpaceshipKey(GameObject.transform.position,
            GameObject.transform.rotation,
            destruction,
            weaponAttack);
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

    public void MoveEntity()
    {
        Vector3 toTarget = GameObject.transform.position - TargetPosition;
        Quaternion lookRot = Quaternion.LookRotation(toTarget);
        GameObject.transform.rotation = Quaternion.RotateTowards(GameObject.transform.rotation, lookRot, Manuverability.MaxAngleChange);
        CurrentMomentum = CurrentMomentum + GameObject.transform.forward * Manuverability.Acceleration;
        if(CurrentMomentum.magnitude > Manuverability.MaxThrust)
        {
            CurrentMomentum = CurrentMomentum.normalized * Manuverability.MaxThrust;
        }
        GameObject.transform.position += CurrentMomentum;
    }
    
    public void UpdateState()
    {
        if(CurrentHP < 0)
        {
            IsActive = false;
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

    public void DisplayAtTime(float time)
    {
        SpaceshipKey key = Timeline.GetTransformAtTime(time);
        GameObject.transform.position = key.Position;
        GameObject.transform.rotation = key.Rotation;
        //TODO: display destruction
        //TODO: display weapon attacks
    }

    public void RegisterKeyframe()
    {
        Timeline.AddKeyframe(MakeKeyFromGameobject());
    }

    public bool IsHitBy(IDamageSource source)
    {
        throw new NotImplementedException();
    }
}
