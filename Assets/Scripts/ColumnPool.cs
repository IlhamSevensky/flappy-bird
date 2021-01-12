using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ColumnPool : MonoBehaviour
{
    public float spawnRate = 4f;
    public int columnPoolSize = 5;
    public float columnYMin = -2f;
    public float columnYMax = 2f;

    public GameObject columnsPrefab;

    private float timeSinceLastSpawned;
    private float spawnXPosition = 10f;
    private int currentColumn = 0;

    private float movingColumnSpeed = 0.25f;
    private float topVerticalPos = 2.5f;
    private float bottomVerticalPos = -2f;
   
    private enum ColumnMovementDirection {UP, DOWN, NOT_STARTED};
    private enum ColumnMovementState {EXCEED_TOP, EXCEED_BOTTOM, MIDDLE};
    private ColumnMovementDirection[] columnMovementDirectionState;
    private ColumnMovementState[] columnMovementState;

    private GameObject[] columns; 
   
    private Vector2 objectPoolPosition = new Vector2(-15, -25f);


 
    void Start()
    {
        columnMovementDirectionState = new ColumnMovementDirection[columnPoolSize];
        columnMovementState = new ColumnMovementState[columnPoolSize];
        columns = new GameObject[columnPoolSize];

        for (int i = 0; i < columnPoolSize; i++)
        {
            columns[i] = (GameObject)Instantiate(columnsPrefab, objectPoolPosition, Quaternion.identity);
            columnMovementDirectionState[i] = ColumnMovementDirection.NOT_STARTED;
            columnMovementState[i] = ColumnMovementState.MIDDLE;
        }
    }

    void Update()
    {
        timeSinceLastSpawned += Time.deltaTime;

        if (!GameControl.Instance.isGameOver)
        {

            if(timeSinceLastSpawned >= spawnRate) {
                spawnColumn();
            }

            for (int position = 0; position < columnPoolSize; position++)
            {
                moveColumn(position);
            }
            

        }
    }

    private void moveColumn(int position)
    {

        // if column has not been created, position will return -25 
        bool isColumnAlreadyCreated = columns[position].transform.position.y != -25;

        if(isColumnAlreadyCreated)
        {
            float centerVerticalPos = (topVerticalPos + bottomVerticalPos) / 2f;
            float columnPosY = columns[position].transform.position.y;
           
            columnMovementState[position] = checkMovementState(columnPosY);

            if (columnMovementState[position] == ColumnMovementState.EXCEED_BOTTOM) 
            {
                Debug.Log("EXCEED_BOTTOM column pos : " + position);
                moveColumn(position, ColumnMovementDirection.UP);
            }
            
            if (columnMovementState[position] == ColumnMovementState.EXCEED_TOP)
            {
                Debug.Log("EXCEED_TOP column pos : " + position);
                moveColumn(position, ColumnMovementDirection.DOWN);
            }

            if (columnMovementState[position] == ColumnMovementState.MIDDLE)
            {
                
                if(columnMovementDirectionState[position] == ColumnMovementDirection.NOT_STARTED)
                {
                    Debug.Log("MIDDLE NOT STARTED column pos : " + position);
                    bool firstMovementShouldUp = (Random.value > 0.5f);
                    if (firstMovementShouldUp) moveColumn(position, ColumnMovementDirection.UP);
                    if (!firstMovementShouldUp) moveColumn(position, ColumnMovementDirection.DOWN);
                }

                if(columnMovementDirectionState[position] == ColumnMovementDirection.UP)
                {
                    Debug.Log("MIDDLE UP column pos : " + position);
                    moveColumn(position, ColumnMovementDirection.UP);
                }

                if(columnMovementDirectionState[position] == ColumnMovementDirection.DOWN)
                {
                    Debug.Log("MIDDLE DOWN column pos : " + position);
                    moveColumn(position, ColumnMovementDirection.DOWN);
                }
            }

        } else {
            Debug.Log("Column has not been created");
        }

    }

    private void moveColumn(int position, ColumnMovementDirection direction)
    {
        if(direction == ColumnMovementDirection.DOWN)
        {
            // Down
            columns[position].transform.Translate(0f, -movingColumnSpeed * Time.deltaTime, 0f);
            columnMovementDirectionState[position] = ColumnMovementDirection.DOWN;
        }

        if (direction == ColumnMovementDirection.UP)
        {
            // Up
            columns[position].transform.Translate(0f, movingColumnSpeed * Time.deltaTime, 0f);
            columnMovementDirectionState[position] = ColumnMovementDirection.UP;
        }
    
    }

    private ColumnMovementState checkMovementState(float columnPositionY)
    {
         ColumnMovementState state;

         if (columnPositionY < bottomVerticalPos) state = ColumnMovementState.EXCEED_BOTTOM;
         else if (columnPositionY > topVerticalPos) state = ColumnMovementState.EXCEED_TOP;
         else state = ColumnMovementState.MIDDLE;

         return state;
    }

    private void spawnColumn()
    {
        timeSinceLastSpawned = 0;

        float spawnYPosition = Random.Range(columnYMin, columnYMax);

        columns[currentColumn].transform.position = new Vector2(spawnXPosition, spawnYPosition);

        currentColumn++;

        if (currentColumn >= columnPoolSize)
        {
            currentColumn = 0;
        }
    }
}
