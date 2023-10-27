#if UNITY_EDITOR
using UnityEngine;
using UnityEditor;

[CustomEditor(typeof(BulletHellSource))]
public class BulletHellSourceEditor : Editor
{
    public override void OnInspectorGUI()
    {

        BulletHellSource script = (BulletHellSource)target;
        script.selectedStyle = (ShootingStyle)EditorGUILayout.EnumPopup("Selected Style", script.selectedStyle);


        // Check the value of the enum
        if (script.selectedStyle == ShootingStyle.Spiral)
        {
            script.barrelPosition = EditorGUILayout.ObjectField("Barrel Position", script.barrelPosition, typeof(Transform), true) as Transform;
            script.shootingDuration = EditorGUILayout.FloatField("Shooting Duration", script.shootingDuration);
            script.shootInterval = EditorGUILayout.FloatField("Shoot Interval", script.shootInterval);
            script.restDuration = EditorGUILayout.FloatField("Rest Duration", script.restDuration);
            script.speed = EditorGUILayout.FloatField("Speed", script.speed);
            script.size = EditorGUILayout.FloatField("Size", script.size);
            script.projectilePrefab = EditorGUILayout.ObjectField("Projectile Prefab", script.projectilePrefab, typeof(GameObject), true) as GameObject;
            script.sound = EditorGUILayout.TextField("Sound", script.sound);
            script.aimAtPlayer = EditorGUILayout.Toggle("Aim At Player", script.aimAtPlayer);
            script.rotationSpeed = EditorGUILayout.IntField("Rotation Speed", script.rotationSpeed);
            script.distanceFromPlayerToStartShooting = EditorGUILayout.FloatField("Distance From Player To Start Shooting", script.distanceFromPlayerToStartShooting);
            script.flashParticles = EditorGUILayout.ObjectField("Flash Particles", script.flashParticles, typeof(GameObject), true) as GameObject;
            script.damageToPlayer = EditorGUILayout.FloatField("Damage To Player", script.damageToPlayer);
        }
        else if (script.selectedStyle == ShootingStyle.Default)
        {
            script.barrelPosition = EditorGUILayout.ObjectField("Barrel Position", script.barrelPosition, typeof(Transform), true) as Transform;
            script.shootingDuration = EditorGUILayout.FloatField("Shooting Duration", script.shootingDuration);
            script.shootInterval = EditorGUILayout.FloatField("Shoot Interval", script.shootInterval);
            script.restDuration = EditorGUILayout.FloatField("Rest Duration", script.restDuration);
            script.speed = EditorGUILayout.FloatField("Speed", script.speed);
            script.size = EditorGUILayout.FloatField("Size", script.size);
            script.projectilePrefab = EditorGUILayout.ObjectField("Projectile Prefab", script.projectilePrefab, typeof(GameObject), true) as GameObject;
            script.sound = EditorGUILayout.TextField("Sound", script.sound);
            script.aimAtPlayer = EditorGUILayout.Toggle("Aim At Player", script.aimAtPlayer);
            script.distanceFromPlayerToStartShooting = EditorGUILayout.FloatField("Distance From Player To Start Shooting", script.distanceFromPlayerToStartShooting);
            script.flashParticles = EditorGUILayout.ObjectField("Flash Particles", script.flashParticles, typeof(GameObject), true) as GameObject;
            script.damageToPlayer = EditorGUILayout.FloatField("Damage To Player", script.damageToPlayer);
        }
        else if (script.selectedStyle == ShootingStyle.Explode)
        {
            script.barrelPosition = EditorGUILayout.ObjectField("Barrel Position", script.barrelPosition, typeof(Transform), true) as Transform;
            script.shootingDuration = EditorGUILayout.FloatField("Shooting Duration", script.shootingDuration);
            script.shootInterval = EditorGUILayout.FloatField("Shoot Interval", script.shootInterval);
            script.restDuration = EditorGUILayout.FloatField("Rest Duration", script.restDuration);
            script.speed = EditorGUILayout.FloatField("Speed", script.speed);
            script.size = EditorGUILayout.FloatField("Size", script.size);
            script.projectilePrefab = EditorGUILayout.ObjectField("Projectile Prefab", script.projectilePrefab, typeof(GameObject), true) as GameObject;
            script.sound = EditorGUILayout.TextField("Sound", script.sound);
            script.aimAtPlayer = EditorGUILayout.Toggle("Aim At Player", script.aimAtPlayer);
            script.distanceFromPlayerToStartShooting = EditorGUILayout.FloatField("Distance From Player To Start Shooting", script.distanceFromPlayerToStartShooting);
            script.flashParticles = EditorGUILayout.ObjectField("Flash Particles", script.flashParticles, typeof(GameObject), true) as GameObject;
            script.damageToPlayer = EditorGUILayout.FloatField("Damage To Player", script.damageToPlayer);
            script.numberOfProjectiles = EditorGUILayout.IntField("Number of Projectiles", script.numberOfProjectiles);
        }
        else
        {
            // For all other styles, draw the default inspector properties
            DrawDefaultInspector();
        }
    }
}
#endif