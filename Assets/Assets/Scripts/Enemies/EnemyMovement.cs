using UnityEngine;

public class EnemyMovement : MonoBehaviour
{
    public float turnSpeed = 10f;
    public float rotationSpeed = 360f;
    public float moveSpeed = 10f;

    public GameObject player;
    public GameObject enemy_turret;

    public TankShooting tankShooting;
    
    public float turretRotationSpeed = 5f;

    public float shootingRange = 10f;

    void Start(){

    }

    void Update(){
        float distanceToPlayer = Vector3.Distance(transform.position, player.transform.position);
        if(distanceToPlayer > shootingRange)
        {
            //enemy movement towards the player
            Vector3 direction = (player.transform.position - transform.position).normalized;
            transform.Translate((direction * moveSpeed) * Time.deltaTime);
        }else
        {
            tankShooting.equippedWeapon.Fire();
        }


    }

    void LateUpdate(){
        RotateTank();
    }

    void RotateTank()
    {
    
        Vector3 directionToTarget = player.transform.position - enemy_turret.transform.position;
        directionToTarget.y = 0;  // Solo rotar en plano horizontal

        //aim toward the player
        Quaternion targetRotation = Quaternion.LookRotation(directionToTarget);
        enemy_turret.transform.rotation = Quaternion.Slerp(enemy_turret.transform.rotation, targetRotation, turretRotationSpeed * Time.deltaTime);
        transform.rotation = Quaternion.Slerp(transform.rotation, targetRotation, turretRotationSpeed * Time.deltaTime);
        //Rotate tank body toward the player

    }
}