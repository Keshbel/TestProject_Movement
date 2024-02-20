using UnityEngine;

public class ShootingController : MonoBehaviour
{
    [Header("Components")] 
    [SerializeField] private Animator playerAnimator;
    [SerializeField] private AudioSource audioSource;
    [SerializeField] private Transform aimedTransform;
    
    [Header("Projectile")] 
    [SerializeField] private GameObject projectilePrefab;
    [SerializeField] private Transform prefabSpawn;
    
    public void OnShoot()
    {
        //sound&animation
        audioSource.Play();
        playerAnimator.Play("Shoot", 1, 0f);
        
        //instantiate
        var projectile = Instantiate(projectilePrefab, prefabSpawn.position, Quaternion.identity);
        projectile.transform.forward = aimedTransform.forward;
    }
}
