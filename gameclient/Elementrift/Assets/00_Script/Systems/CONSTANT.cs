using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class CONSTANT
{
    public static readonly string PLAYER_TAG = "Player";
    public static readonly string ENEMY_TAG = "Enemy";
    public static readonly string ITEM_TAG = "Item";
    public static readonly string GROUND_TAG = "Ground";
    public static readonly string PLATFORM_TAG = "Platform";
    public static readonly string UI_CANVAS = "UICanvas";
    public static readonly string GAME_MANAGER = "GameManager";



    public static readonly string GETAXIS_HORIZONTAL = "Horizontal";
    public static readonly string GETAXIS_VERTICAL = "Vertical";
    public static readonly string GETAXIS_MOUSE_X = "Mouse X";
    public static readonly string GETAXIS_MOUSE_Y = "Mouse Y";

    public static readonly float PLAYER_MOVE_SPEED = 30f;
    public static readonly float PLAYER_JUMP_FORCE = 10f;

    public static readonly string PLAYER_PARAMETER_IsIdle = "IsIdle";
    public static readonly string PLAYER_PARAMETER_IsWalking = "IsWalking";
    public static readonly string PLAYER_PARAMETER_IsRunning = "IsRunning";
    public static readonly string PLAYER_PARAMETER_IsJumping = "IsJumping";
    public static readonly string PLAYER_PARAMETER_IsAttacking = "IsAttacking";
    public static readonly string PLAYER_PARAMETER_IsDead = "IsDead";
}
