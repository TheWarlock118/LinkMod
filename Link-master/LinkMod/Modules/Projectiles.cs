using R2API;
using RoR2;
using RoR2.Projectile;
using UnityEngine;
using UnityEngine.Networking;

namespace LinkMod.Modules
{
    internal static class Projectiles
    {
        internal static GameObject bombPrefab;

        internal static GameObject arrowPrefab;
        internal static GameObject bombArrowPrefab;
        internal static GameObject fireArrowPrefab;
        internal static GameObject iceArrowPrefab;

        internal static GameObject miphaPrefab;
        internal static GameObject urbosaPrefab;
        internal static GameObject darukPrefab;
        internal static GameObject revaliPrefab;

        internal static void RegisterProjectiles()
        {            
            CreateBomb();
            CreateBombArrow();
            CreateFireArrow();
            CreateIceArrow();
            CreateArrow();
            CreateMipha();
            CreateDaruk();
            CreateRevali();
            CreateUrbosa();  
            
            AddProjectile(bombPrefab);
            AddProjectile(arrowPrefab);
            AddProjectile(bombArrowPrefab);
            AddProjectile(fireArrowPrefab);
            AddProjectile(iceArrowPrefab);
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
            bombPrefab = CloneProjectilePrefab("CommandoGrenadeProjectile", "LinkBombProjectile");

            ProjectileImpactExplosion bombImpactExplosion = bombPrefab.GetComponent<ProjectileImpactExplosion>();
            InitializeImpactExplosion(bombImpactExplosion);

            bombImpactExplosion.blastRadius = 4f;
            bombImpactExplosion.destroyOnEnemy = false;
            bombImpactExplosion.bonusBlastForce = new Vector3(10f, 10f);
            bombImpactExplosion.lifetime = 24f;
            bombImpactExplosion.impactEffect = Modules.Assets.bombExplosionEffect;
            bombImpactExplosion.explosionEffect = Modules.Assets.bombExplosionEffect;
            bombImpactExplosion.explosionSoundString = "BombExplode";            
            bombImpactExplosion.timerAfterImpact = true;
            bombImpactExplosion.lifetimeAfterImpact = 0.001f;            

            ProjectileController bombController = bombPrefab.GetComponent<ProjectileController>();
            if (Modules.Assets.mainAssetBundle.LoadAsset<GameObject>("LinkBombRound") != null) bombController.ghostPrefab = CreateGhostPrefab("LinkBombRound");
            bombController.startSound = "";
        }
        
        private static void CreateArrow()
        {
            arrowPrefab = CloneProjectilePrefab("Arrow", "LinkArrowProjectile");
            ProjectileController arrowController = arrowPrefab.GetComponent<ProjectileController>();                               
            arrowController.ghostPrefab = CreateGhostPrefab("linkArrow");
            arrowController.startSound = "";
        }
        
        private static void CreateBombArrow()
        {
            bombArrowPrefab = CloneProjectilePrefab("MageIceBolt", "LinkBombArrowProjectile");
                               
            ProjectileImpactExplosion bombArrowImpactExplosion = bombArrowPrefab.GetComponent<ProjectileImpactExplosion>();
            InitializeImpactExplosion(bombArrowImpactExplosion);

            bombArrowImpactExplosion.blastRadius = 8f;
            bombArrowImpactExplosion.destroyOnEnemy = true;
            bombArrowImpactExplosion.blastDamageCoefficient = 16f;
            bombArrowImpactExplosion.bonusBlastForce = new Vector3(10f, 10f);
            bombArrowImpactExplosion.lifetime = 24f;
            bombArrowImpactExplosion.impactEffect = Modules.Assets.bombArrowExplosionEffect;
            bombArrowImpactExplosion.explosionEffect = Modules.Assets.bombArrowExplosionEffect;
            bombArrowImpactExplosion.explosionSoundString = "BombArrow_Explosion";
            bombArrowImpactExplosion.timerAfterImpact = true;
            bombArrowImpactExplosion.lifetimeAfterImpact = 0.00001f;

            ProjectileController arrowController = bombArrowPrefab.GetComponent<ProjectileController>();
            arrowController.ghostPrefab = CreateGhostPrefab("linkArrow");
            arrowController.startSound = "";
        }

        private static void CreateFireArrow()
        {
            fireArrowPrefab = CloneProjectilePrefab("MageIceBolt", "LinkFireArrowProjectile");

            ProjectileImpactExplosion fireArrowImpactExplosion = fireArrowPrefab.GetComponent<ProjectileImpactExplosion>();
            InitializeImpactExplosion(fireArrowImpactExplosion);

            fireArrowImpactExplosion.blastRadius = 2f;
            fireArrowImpactExplosion.destroyOnEnemy = false;            
            fireArrowImpactExplosion.lifetime = 24f;
            fireArrowImpactExplosion.blastDamageCoefficient = Modules.StaticValues.bowDamageCoefficient / 2;
            fireArrowImpactExplosion.impactEffect = Modules.Assets.fireArrowExplosionEffect;
            fireArrowImpactExplosion.explosionEffect = Modules.Assets.fireArrowExplosionEffect;
            fireArrowImpactExplosion.explosionSoundString = "FireArrow_Hit";
            fireArrowImpactExplosion.timerAfterImpact = true;
            fireArrowImpactExplosion.lifetimeAfterImpact = 0.001f;
            fireArrowImpactExplosion.GetComponent<ProjectileDamage>().damageType = DamageType.IgniteOnHit;

            ProjectileController arrowController = fireArrowPrefab.GetComponent<ProjectileController>();
            arrowController.ghostPrefab = CreateGhostPrefab("linkArrow");
            arrowController.startSound = "";
        }

        private static void CreateIceArrow()
        {
            iceArrowPrefab = CloneProjectilePrefab("MageIceBolt", "LinkIceArrowProjectile");
            // Adding a projectile explosion 
            ProjectileImpactExplosion iceArrowImpactExplosion = iceArrowPrefab.GetComponent<ProjectileImpactExplosion>();
            InitializeImpactExplosion(iceArrowImpactExplosion);

            iceArrowImpactExplosion.blastProcCoefficient = 2f;
            iceArrowImpactExplosion.blastRadius = 4f;
            iceArrowImpactExplosion.destroyOnEnemy = true;
            iceArrowImpactExplosion.lifetime = 1f;
            iceArrowImpactExplosion.impactEffect = Modules.Assets.iceArrowExplosionEffect;
            iceArrowImpactExplosion.explosionEffect = Modules.Assets.iceArrowExplosionEffect;
            iceArrowImpactExplosion.explosionSoundString = "IceArrow_Hit";
            iceArrowImpactExplosion.timerAfterImpact = true;
            iceArrowImpactExplosion.lifetimeAfterImpact = 0.001f;
            iceArrowImpactExplosion.GetComponent<ProjectileDamage>().damageType = DamageType.Freeze2s;

            ProjectileController arrowController = iceArrowPrefab.GetComponent<ProjectileController>();
            arrowController.procCoefficient = 0.5f;
            arrowController.ghostPrefab = CreateGhostPrefab("linkArrow");
            arrowController.startSound = "";
        }

        private static void CreateMipha()
        {
            miphaPrefab = CloneProjectilePrefab("FMJRamping", "MiphaProjectile");
            ProjectileController miphaController = miphaPrefab.GetComponent<ProjectileController>();
            miphaController.ghostPrefab = CreateGhostPrefab("mdlMipha");
            miphaController.startSound = "";            
            miphaController.canImpactOnTrigger = false;
            
        }

        private static void CreateUrbosa()
        {
            urbosaPrefab = CloneProjectilePrefab("FMJRamping", "urbosaProjectile");            
            ProjectileController urbosaController = urbosaPrefab.GetComponent<ProjectileController>();            
            urbosaController.ghostPrefab = CreateGhostPrefab("mdlUrbosa");
            urbosaController.startSound = "";
            urbosaController.canImpactOnTrigger = false;
        }

        private static void CreateRevali()
        {
            revaliPrefab = CloneProjectilePrefab("FMJRamping", "revaliProjectile");
            ProjectileController revaliController = revaliPrefab.GetComponent<ProjectileController>();
            revaliController.ghostPrefab = CreateGhostPrefab("mdlRevali");
            revaliController.startSound = "";
            revaliController.canImpactOnTrigger = false;            
            
        }

        private static void CreateDaruk()
        {
            darukPrefab = CloneProjectilePrefab("FMJRamping", "darukProjectile");
            ProjectileController darukController = darukPrefab.GetComponent<ProjectileController>();
            darukController.ghostPrefab = CreateGhostPrefab("mdlDaruk");
            darukController.startSound = ""; 
            darukController.canImpactOnTrigger = false;

            
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