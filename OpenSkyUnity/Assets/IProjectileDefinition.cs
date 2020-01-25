public interface IProjectileDefinition<T>
    where T : Projectile
{
    T ToProjectile();
}