﻿using Assets.Source.Game;
using Assets.Source.Model;
using UnityEngine;

public class PlayerBehaviour : MonoBehaviour
{
    Vector2 mov;

    public Vector2 interactionVector;
    public const string PLAYER_ID = "Jogador";

    float moveCooldown;

    void Update()
    {
        CenterCamera();

        // entity being executed
        if (GameState.main.currentExecutedEntity != null || GameMasterBehaviour.main.IsPlayerLocked)
        {
            return;
        }
        
        if (moveCooldown > 0)
        {
            moveCooldown -= Time.deltaTime;
        }

        mov.x = 0;
        mov.y = 0;

        EntityBehaviour interactionEntity = null;

        if (Input.GetKey(KeyCode.RightArrow))
            mov.x = 1;
        if (Input.GetKey(KeyCode.LeftArrow))
            mov.x = -1;
        if (Input.GetKey(KeyCode.UpArrow))
            mov.y = 1;
        if (Input.GetKey(KeyCode.DownArrow))
            mov.y = -1;

        if (mov != Vector2.zero && moveCooldown <= 0)
        {
            moveCooldown = 0.2f;
            interactionVector = mov;

            var finalPos = new Vector2(transform.localPosition.x + mov.x, transform.localPosition.y + mov.y);

            // collision
            var blockedByEntity = false;
            foreach (var entity in GameState.main.currentEntityBehaviours)
            {
                if (entity.location == finalPos)
                {
                    if (entity.currentState.passable && entity.currentState.execution == EntityExecution.Contact)
                    {
                        interactionEntity = entity;
                    }
                    else
                    {
                        blockedByEntity = !entity.currentState.passable;
                    }
                    
                    break;
                }
            }

            if (GameState.main.currentGameMap.IsInside((int)finalPos.x, (int)finalPos.y) && !blockedByEntity && GameState.main.currentGameMap.constructionLayer.tids[(int)finalPos.x, (int)finalPos.y] == -1)
            {
                transform.localPosition = new Vector3(finalPos.x, finalPos.y, transform.localPosition.z);
                GetComponent<EntityBehaviour>().location = new Vector2(finalPos.x, finalPos.y);

                CenterCamera();
            }
        }

        if (Input.GetKeyDown(KeyCode.Z))
        {
            foreach (var entity in GameState.main.currentEntityBehaviours)
            {
                var interactionLocation = new Vector2(transform.localPosition.x + interactionVector.x,
                    transform.localPosition.y + interactionVector.y);

                if (entity.location == interactionLocation && entity.currentState.execution == EntityExecution.Interaction)
                {
                    interactionEntity = entity;
                    break;
                }
            }
        }

        if (interactionEntity != null)
        {
            GameState.main.ExecuteEntity(interactionEntity);
        }
    }

    public void CenterCamera()
    {
        Camera.main.transform.localPosition = new Vector3(transform.position.x, transform.position.y, Camera.main.transform.localPosition.z);
    }

    void MoveCamera()
    {
        Camera.main.transform.localPosition = Vector3.MoveTowards(Camera.main.transform.localPosition,
            new Vector3(transform.position.x, transform.position.y, Camera.main.transform.localPosition.z), 0.1f);
    }
}
