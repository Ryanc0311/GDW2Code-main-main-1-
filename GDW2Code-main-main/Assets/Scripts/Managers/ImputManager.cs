using UnityEngine;

public class ImputManager : MonoBehaviour
{
    private static Controls _controls = new Controls();
    public static bool inFight;

    public static void Init(Player MyPlayer)
    {
        _controls.Game.Jump.performed += ctx =>
        {
            if (!inFight)
            {
                MyPlayer.Jump();
            }
        };
        _controls.Game.Fly.performed += ctx =>
        {
            if (inFight)
            {
                MyPlayer.Fly();
            }
        };
    }

    public static void GameMode()
    {
        _controls.Game.Enable();
    }
}
