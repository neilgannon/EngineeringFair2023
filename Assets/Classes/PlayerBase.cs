using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.Events;

public class PlayerBase : CharacterBase
{
    private Vector3 rawMousePosition;
    protected Vector3 mousePosition;
    [SerializeField] protected Projectile ProjectilePrefab;

    private List<Transform> astronauts = new List<Transform>();
    protected int closestAstronautIndex = -1;
    protected bool AnyAstronautsExist { get { return astronauts.Count > 0; } }
    [HideInInspector] public UnityEvent AstronautCollected;
    private LineRenderer waypointLineRenderer;

    [SerializeField] protected Transform LeftSpawnPoint;
    [SerializeField] protected Transform RightSpawnPoint;
    [SerializeField] Texture2D cursorTexture;


    [HideInInspector] public UnityEvent Died;
    [HideInInspector] public UnityEvent GameComplete;

    protected override void Start()
    {
        base.Start();

        waypointLineRenderer = GetComponent<LineRenderer>();
        astronauts = GameObject.FindGameObjectsWithTag("Astronaut").Select(t => t.transform).ToList();
        Cursor.SetCursor(cursorTexture, new Vector2(cursorTexture.width / 2, cursorTexture.height / 2), CursorMode.Auto);
    }

    protected override void Update()
    {
        rawMousePosition = Camera.main.ScreenToWorldPoint(Input.mousePosition);
        rawMousePosition.z = 0;
        mousePosition = rawMousePosition;

        if (AnyAstronautsExist)
        {
            FindIndexOfClosestAstronaut();

            waypointLineRenderer.SetPosition(0, transform.position);
            waypointLineRenderer.SetPosition(1, GetClosestAstronaut());
        }

        if(Input.GetKeyDown(KeyCode.Escape)) { Application.Quit(); }
        
        base.Update();
    }

    public void ShootProjectile(Vector2 shootFrom, Vector2 direction)
    {
        if (ProjectilePrefab != null)
        {
            Projectile instance = Instantiate(ProjectilePrefab, shootFrom, Quaternion.identity);
            instance.SetDirection(direction);
        }
    }

    private void FindIndexOfClosestAstronaut()
    {
        float distance = float.MaxValue;

        for (int i = 0; i < astronauts.Count; i++)
        {
            float currentDistance = (transform.position - astronauts[i].position).sqrMagnitude;
            if (currentDistance < distance)
            {
                distance = currentDistance;
                closestAstronautIndex = i;
            }
        }
    }

    protected Vector2 GetClosestAstronaut()
    {
        return astronauts[closestAstronautIndex].position;
    }

    protected void CollectAstronaut(GameObject astronaut)
    {
        int index = astronauts.IndexOf(astronaut.transform);
        astronauts.RemoveAt(index);

        if (AnyAstronautsExist)
        {
            FindIndexOfClosestAstronaut();
        }
        else
        {
            waypointLineRenderer.enabled = false;

            if (GameComplete != null)
                GameComplete.Invoke();
        }

        Destroy(astronaut);

        if (AstronautCollected != null)
            AstronautCollected.Invoke();
    }

    protected override void OnCharacterDeath()
    {
        if(Died != null)
            Died.Invoke();

        base.OnCharacterDeath();
    }
}
