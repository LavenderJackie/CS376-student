using UnityEngine;

/// <summary>
/// Control the player on screen
/// </summary>
[RequireComponent(typeof(Rigidbody2D))]
public class Player : MonoBehaviour
{
    // what are we applying forces to in the game?
    private Rigidbody2D player_body;

    /// <summary>
    /// Prefab for the orbs we will shoot
    /// </summary>
    public GameObject OrbPrefab;

    /// <summary>
    /// How fast our engines can accelerate us
    /// </summary>
    public float EnginePower = 1;
    
    /// <summary>
    /// How fast we turn in place
    /// </summary>
    public float RotateSpeed = 1;

    /// <summary>
    /// How fast we should shoot our orbs
    /// </summary>
    public float OrbVelocity = 10;

    /// <summary>
    /// Handle moving and firing.
    /// Called by Uniity every 1/50th of a second, regardless of the graphics card's frame rate
    /// </summary>
    // ReSharper disable once UnusedMember.Local
    void FixedUpdate()
    {
        Manoeuvre();
        MaybeFire();
    }

    /// <summary>
    /// Fire if the player is pushing the button for the Fire axis
    /// Unlike the Enemies, the player has no cooldown, so they shoot a whole blob of orbs
    /// </summary>
    void MaybeFire()
    {
        if(Input.GetAxis("Fire") != 0)
        {
            FireOrb();
        }
    }

    /// <summary>
    /// Fire one orb.  The orb should be placed one unit "in front" of the player.
    /// transform.right will give us a vector in the direction the player is facing.
    /// It should move in the same direction (transform.right), but at speed OrbVelocity.
    /// </summary>
    private void FireOrb()
    {
        Vector3 shot_pos = transform.right;
        shot_pos.x += player_body.position.x;
        shot_pos.y += player_body.position.y;
        GameObject shot = Instantiate(OrbPrefab, shot_pos, Quaternion.identity);
        shot.GetComponent<Rigidbody2D>().velocity = OrbVelocity * player_body.transform.right;
    }

    /// <summary>
    /// Accelerate and rotate as directed by the player
    /// Apply a force in the direction (Horizontal, Vertical) with magnitude EnginePower
    /// Note that this is in *world* coordinates, so the direction of our thrust doesn't change as we rotate
    /// Set our angularVelocity to the Rotate axis time RotateSpeed
    /// </summary>
    void Manoeuvre()
    {
        float x = Input.GetAxis("Horizontal");
        float y = Input.GetAxis("Vertical");

        Vector2 dir = new Vector2(x, y);
        player_body.AddForce(EnginePower * dir);

        player_body.angularVelocity = RotateSpeed * Input.GetAxis("Rotate");
    }

    /// <summary>
    /// If this is called, we got knocked off screen.  Deduct a point!
    /// </summary>
    // ReSharper disable once UnusedMember.Local
    void OnBecameInvisible()
    {
        ScoreKeeper.ScorePoints(-1);
    }

    void Start()
    {
        player_body = GetComponent<Rigidbody2D>();
    }
}
