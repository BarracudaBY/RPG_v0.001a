using UnityEngine;

public class Enemy_Spiders : Enemy
{
    [Header("Spider spesifics")]
    public GameObject[] wayPoints;
    public int nextWaypoint = 1;
    public float distToPoint; //Здесь будет храниться оставшееся расстояние между игроком и точкой NextWaypoint

    private void Update()
    {
        Move();
    }

    private void Move()
    {
        distToPoint = Vector2.Distance(transform.position, wayPoints[nextWaypoint].transform.position);

        transform.position = Vector2.MoveTowards(transform.position,wayPoints[nextWaypoint].transform.position, 
                                                moveSpeed * Time.deltaTime);
        if(distToPoint < 0.2f)
        {
            TakeTurn();
        }
    }
    
    private void TakeTurn()
    {
        Vector3 currRot = transform.eulerAngles;
        currRot.z += wayPoints[nextWaypoint].transform.eulerAngles.z;
        transform.eulerAngles = currRot;
        ChooseNextWaypoint();
        
    }

    private void ChooseNextWaypoint()
    {
        nextWaypoint++;
        if(nextWaypoint == wayPoints.Length)
        {
            nextWaypoint = 0; 
        }
    }
}


