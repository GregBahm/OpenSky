using System.Collections.Generic;

public class SpaceshipBlueprint
{
    public IEnumerable<IViewableSpaceObject> ViewableObjects
    {
        get
        {
            foreach (SpaceShip item in Spaceships)
            {
                yield return item;
            }
            foreach (Projectile item in Projectiles)
            {
                yield return item;
            }
        }
    }

    public IEnumerable<SpaceShip> Spaceships { get; }

    public IEnumerable<Projectile> Projectiles { get; }
}
