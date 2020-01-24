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
    public GameObject GameObject { get; }
    public ItemTimeline<SpaceshipKey> Timeline { get; }
    public SpaceManuverability Manuverability { get; }
    public IReadOnlyCollection<SpaceshipWeapon> Weapons { get; }
    public bool IsActive { get; private set; }
    public Vector3 TargetPosition { get; set; }
    public Vector3 CurrentMomentum { get; set; }
    public IEnumerable<IViewableSpaceObject> ViewableObjects
    {
        get
        {
            yield return this;
            foreach (IViewableSpaceObject item in Weapons.SelectMany(item => item.ProjectilesPool))
            {
                yield return item;
            }
        }
    }


    public SpaceShip(int teamId, 
        SpaceManuverability manuverability, 
        IEnumerable<SpaceshipWeapon> weapons,
        GameObject gameObject)
    {
        TeamId = teamId;
        GameObject = gameObject;
        Manuverability = manuverability;
        Weapons = weapons.ToList().AsReadOnly();
        Timeline = new ItemTimeline<SpaceshipKey>(MakeKeyFromGameobject());
    }

    private SpaceshipKey MakeKeyFromGameobject()
    {
        float destruction = 0;//TODO: Destruction
        float weaponAttack = 0;//TODO: Weapon attack
        return new SpaceshipKey(GameObject.transform.position,
            GameObject.transform.rotation,
            destruction,
            weaponAttack);
    }

    public void RegisterDamage(IEnumerable<SpaceShip> activeShips, 
        IEnumerable<Projectile> activeProjectiles)
    {

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
        throw new NotImplementedException();
    }

    internal void InitiateNewAttacks(IEnumerable<SpaceShip> friends, 
        IEnumerable<SpaceShip> enemies)
    {
        throw new NotImplementedException();
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

public class SpaceshipWeapon
{
    public IEnumerable<Projectile> ProjectilesPool { get; }


}