using Panda;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class RobotAI : MonoBehaviour
{
    public NavMeshAgent agent;
    public GameObject player;
    public GameObject bulletPrefab;
    public GameObject PlayerBullet;
    public GameObject HealthStation;
    public GameObject BatteryStation;
    public Transform spawnPoint;
    float visionDistance = 20;
    float visionAngle = 60;
    Vector3 target;

    public static float Health = 100f;
    public static float minHealth = 1f;

    public float MaxBattery = 100f;
    public float minBattery = 1f;


    // Use this for initialization
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {
        if (MaxBattery >= minBattery) {
            MaxBattery -= 0.05f;
        }
    }

    void OnCollisionEnter(Collision collisionobject)
    {
        if (collisionobject.gameObject.tag == "hi")
        {
            Health -= 10f;
            print(Health);

        }
    }


    //Metoderna nedan anropas av beteendeträdet och definierar "actions" för "leaf/task" noderna i trädet. 

    [Task]
    void Fire()
    {

        GameObject bullet = Instantiate(bulletPrefab, spawnPoint.position, spawnPoint.rotation);
        bullet.GetComponent<Rigidbody>().AddForce(this.transform.forward * 1000);
        Destroy(bullet, 3.0f);
        Task.current.Succeed();

    }

    [Task]
    void PickRandomDest()
    {


        agent.speed = 3f;
        Vector3 destination = new Vector3(Random.Range(-37, 37), 0, Random.Range(-36, 35));
        agent.SetDestination(destination);
        agent.isStopped = false;
        Task.current.Succeed();

    }

    [Task]
    void MoveToDest()
    {
        if (agent.remainingDistance <= agent.stoppingDistance)
        {
            Task.current.Succeed();
        }

        if (Health <= 10f || MaxBattery <= 1f) {
            Task.current.Succeed();
        }

    }

    [Task]
    void SetTarget()
    {
        agent.isStopped = true;
        target = player.transform.position;
        Task.current.Succeed();
    }

    [Task]
    void LookAtTarget()
    {
        Vector3 direction = player.transform.position - this.transform.position;
        this.transform.rotation = Quaternion.Slerp(this.transform.rotation, Quaternion.LookRotation(direction), 0.4f);

        if (Vector3.Angle(this.transform.forward, direction) < 10.0f)
            Task.current.Succeed();
    }

    [Task]
    bool IsVisible()
    {
        Vector3 direction = player.transform.position - this.transform.position;
        float angle = Vector3.Angle(this.transform.forward, direction);

        if (direction.magnitude <= visionDistance && angle <= visionAngle)
            return true;
        else
            return false;
    }

    [Task]
    bool LowHealth()
    {
        if (Health <= 10f)
            return true;
        else
            return false;
    }
    [Task]
    void MoveFast()
    {
        agent.isStopped = false;
        agent.speed = 10f;

        if ((agent.transform.position - HealthStation.transform.position).sqrMagnitude < 3 * 3)
        {
            Health = 100f;
        }

        Task.current.Succeed();
    }

    [Task]
    void SetFleePosition()
    {

        agent.SetDestination(HealthStation.transform.position);
        agent.isStopped = false;
        Task.current.Succeed();

    }

    [Task]
    bool LowBattery()
    {
        if (MaxBattery <= 1f)
            return true;
        else
            return false;
    }

    [Task]
    void FindBatteryStation()
    {
        agent.SetDestination(BatteryStation.transform.position);
        agent.isStopped = false;
        Task.current.Succeed();
    }

    [Task]
    void MoveToBatteryStation()
    {
        agent.isStopped = false;
        agent.speed = 8f;

        if ((agent.transform.position - BatteryStation.transform.position).sqrMagnitude < 3 * 3)
        {
            MaxBattery = 100f;
        }

        Task.current.Succeed();
    }

    void OnGUI()
    {
        GUI.Label(new Rect(10, 10, 400, 20), "Robot Health Left: " + Health.ToString());
        GUI.Label(new Rect(10, 40, 400, 20), "Robot Speed: " + agent.speed.ToString());
        GUI.Label(new Rect(200, 10, 400, 20), "Robot Battery: " + MaxBattery.ToString());
    }
}

