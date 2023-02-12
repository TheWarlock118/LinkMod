using R2API;
using RoR2;
using RoR2.Projectile;
using UnityEngine;
using UnityEngine.Networking;

namespace HenryMod.Modules
{
    internal static class Projectiles
    {
        internal static GameObject bombPrefab;
        internal static GameObject arrowPrefab;
        internal static GameObject miphaPrefab;
        internal static GameObject urbosaPrefab;
        internal static GameObject darukPrefab;
        internal static GameObject revaliPrefab;

        internal static void RegisterProjectiles()
        {
            // only separating into separate methods for my sanity
            CreateBomb();
            CreateArrow();
            CreateMipha();
            CreateDaruk();
            CreateRevali();
            CreateUrbosa();
            AddProjectile(bombPrefab);
            AddProjectile(arrowPrefab);
            AddProjectile(miphaPrefab);
            AddProjectile(urbosaPrefab);
            AddProjectile(darukPrefab);
            AddProjectile(revaliPrefab);
        }

        internal static void AddProjectile(GameObject projectileToAdd)
        {
            Modules.Prefabs.projectilePrefabs.Add(projectileToAdd);
        }

        private static void CreateBomb()
        {
            bombPrefab = CloneProjectilePrefab("CommandoGrenadeProjectile", "HenryBombProjectile");

            ProjectileImpactExplosion bombImpactExplosion = bombPrefab.GetComponent<ProjectileImpactExplosion>();
            InitializeImpactExplosion(bombImpactExplosion);

            bombImpactExplosion.blastRadius = 16f;
            bombImpactExplosion.destroyOnEnemy = false;
            bombImpactExplosion.lifetime = 24f;
            bombImpactExplosion.impactEffect = Modules.Assets.bombExplosionEffect;
            bombImpactExplosion.explosionEffect = Modules.Assets.bombExplosionEffect;
            bombImpactExplosion.explosionSoundString = "BombExplode";            
            bombImpactExplosion.timerAfterImpact = true;
            bombImpactExplosion.lifetimeAfterImpact = 0.1f;            

            ProjectileController bombController = bombPrefab.GetComponent<ProjectileController>();
            if (Modules.Assets.mainAssetBundle.LoadAsset<GameObject>("LinkBombRound") != null) bombController.ghostPrefab = CreateGhostPrefab("LinkBombRound");
            bombController.startSound = "";
        }
        
        private static void CreateArrow()
        {
            arrowPrefab = CloneProjectilePrefab("WindbladeProjectile", "LinkArrowProjectile");
            ProjectileController arrowController = arrowPrefab.GetComponent<ProjectileController>();
            arrowController.ghostPrefab = CreateGhostPrefab("linkArrow");
            arrowController.startSound = "";
        }
        
        private static void CreateMipha()
        {
            miphaPrefab = CloneProjectilePrefab("WindbladeProjectile", "MiphaProjectile");
            ProjectileController miphaController = miphaPrefab.GetComponent<ProjectileController>();
            miphaController.ghostPrefab = CreateGhostPrefab("mdlMipha");
            miphaController.startSound = ""; // TODO: Add sounds            
            miphaController.canImpactOnTrigger = false;

            //ProjectileFuse miphaFuse = miphaPrefab.GetComponent<ProjectileFuse>();
            //miphaFuse.fuse = 10f;
        }

        private static void CreateUrbosa()
        {
            urbosaPrefab = CloneProjectilePrefab("WindbladeProjectile", "urbosaProjectile");            
            ProjectileController urbosaController = urbosaPrefab.GetComponent<ProjectileController>();            
            urbosaController.ghostPrefab = CreateGhostPrefab("mdlUrbosa");
            urbosaController.startSound = ""; // TODO: Add sounds
            urbosaController.canImpactOnTrigger = false;

            //ProjectileFuse urbosaFuse = urbosaPrefab.GetComponent<ProjectileFuse>();
            //urbosaFuse.fuse = 10f;
        }

        private static void CreateRevali()
        {
            revaliPrefab = CloneProjectilePrefab("WindbladeProjectile", "revaliProjectile");
            ProjectileController revaliController = revaliPrefab.GetComponent<ProjectileController>();
            revaliController.ghostPrefab = CreateGhostPrefab("mdlRevali");
            revaliController.startSound = ""; // TODO: Add sounds
            revaliController.canImpactOnTrigger = false;

            //ProjectileFuse revaliFuse = revaliPrefab.GetComponent<ProjectileFuse>();
            //revaliFuse.fuse = 10f;
        }

        private static void CreateDaruk()
        {
            darukPrefab = CloneProjectilePrefab("WindbladeProjectile", "darukProjectile");
            ProjectileController darukController = darukPrefab.GetComponent<ProjectileController>();
            darukController.ghostPrefab = CreateGhostPrefab("mdlDaruk");
            darukController.startSound = ""; // TODO: Add sounds
            darukController.canImpactOnTrigger = false;

            //ProjectileFuse darukFuse = darukPrefab.GetComponent<ProjectileFuse>();
            //darukFuse.fuse = 10f;
        }

        private static void InitializeImpactExplosion(ProjectileImpactExplosion projectileImpactExplosion)
        {
            projectileImpactExplosion.blastDamageCoefficient = 1f;
            projectileImpactExplosion.blastProcCoefficient = 1f;
            projectileImpactExplosion.blastRadius = 1f;
            projectileImpactExplosion.bonusBlastForce = Vector3.zero;
            projectileImpactExplosion.childrenCount = 0;
            projectileImpactExplosion.childrenDamageCoefficient = 0f;
            projectileImpactExplosion.childrenProjectilePrefab = null;
            projectileImpactExplosion.destroyOnEnemy = false;
            projectileImpactExplosion.destroyOnWorld = false;
            projectileImpactExplosion.explosionSoundString = "";
            projectileImpactExplosion.falloffModel = RoR2.BlastAttack.FalloffModel.None;
            projectileImpactExplosion.fireChildren = false;
            projectileImpactExplosion.impactEffect = null;
            projectileImpactExplosion.lifetime = 0f;
            projectileImpactExplosion.lifetimeAfterImpact = 0f;
            projectileImpactExplosion.lifetimeExpiredSoundString = "BombExplode";
            projectileImpactExplosion.lifetimeRandomOffset = 0f;
            projectileImpactExplosion.offsetForLifetimeExpiredSound = 0f;
            projectileImpactExplosion.timerAfterImpact = false;

            projectileImpactExplosion.GetComponent<ProjectileDamage>().damageType = DamageType.Generic;
        }

        private static GameObject CreateGhostPrefab(string ghostName)
        {
            GameObject ghostPrefab = Modules.Assets.mainAssetBundle.LoadAsset<GameObject>(ghostName);
            if (!ghostPrefab.GetComponent<NetworkIdentity>()) ghostPrefab.AddComponent<NetworkIdentity>();
            if (!ghostPrefab.GetComponent<ProjectileGhostController>()) ghostPrefab.AddComponent<ProjectileGhostController>();

            Modules.Assets.ConvertAllRenderersToHopooShader(ghostPrefab);

            return ghostPrefab;
        }

        private static GameObject CloneProjectilePrefab(string prefabName, string newPrefabName)
        {
            GameObject newPrefab = PrefabAPI.InstantiateClone(RoR2.LegacyResourcesAPI.Load<GameObject>("Prefabs/Projectiles/" + prefabName), newPrefabName);
            return newPrefab;
        }
    }
}