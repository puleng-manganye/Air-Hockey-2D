using System.Collections;
using System.Collections.Generic;
using UnityEngine;

#region Class Description:
/*
 * This script controls an AI opponent's behavior in a game, in response to the puck's movement.
 */
#endregion

public class AiOpponent : MonoBehaviour
{
    #region Fields

    // Public float defining the maximum movement speed of the AI opponent.
    public float maxMovementSpeed;
    // Rigidbody2D component of the AI opponent.
    private Rigidbody2D _rigidbody;
    // Vector2 storing the starting position of the AI opponent.
    private Vector2 _startingPosition;

    // Rigidbody2D representing the puck.
    public Rigidbody2D puck;

    // Transform holding the boundaries for the player's area.
    public Transform playerBoundaryHolder;
    // Boundary struct storing the player's area boundaries.
    private Boundary _playerBoundary;

    // Transform holding the boundaries for the puck's area.
    public Transform puckBoundaryHolder;
    // Boundary struct storing the puck's area boundaries.
    private Boundary _puckBoundary;

    // Vector2 representing the target position for the AI opponent.
    private Vector2 _targetPosition;

    // Boolean flag to track whether the puck is in the opponent's half for the first time.
    private bool _isFirstTimeInOpponentsHalf = true;
    // Float storing the offset from the target position in the opponent's half.
    private float _offsetXFromTarget;

    #endregion

    #region Intialisations

    private void Start()
    {
        // Initialisations
        _rigidbody = GetComponent<Rigidbody2D>();
        _startingPosition = _rigidbody.position;

        _playerBoundary = new Boundary(playerBoundaryHolder.GetChild(0).position.y,
                                      playerBoundaryHolder.GetChild(1).position.y,
                                      playerBoundaryHolder.GetChild(2).position.x,
                                      playerBoundaryHolder.GetChild(3).position.x);

        _puckBoundary = new Boundary(puckBoundaryHolder.GetChild(0).position.y,
                                    puckBoundaryHolder.GetChild(1).position.y,
                                    puckBoundaryHolder.GetChild(2).position.x,
                                    puckBoundaryHolder.GetChild(3).position.x);
    }

    #endregion

    #region Updates

    // The AI opponent's movement is controlled based on the position of the puck.
    private void FixedUpdate()
    {
        float movementSpeed;

        // If the puck is below the puck boundary's lower limit,
        // the opponent moves towards an adjusted target position within the player's boundary.
        // This adjustment is randomized and applied only the first time the puck enters the opponent's half.
        if (puck.position.y < _puckBoundary.Down)
        {
            if (_isFirstTimeInOpponentsHalf)
            {
                _isFirstTimeInOpponentsHalf = false;
                _offsetXFromTarget = Random.Range(-1f, 1f);
            }

            movementSpeed = maxMovementSpeed * Random.Range(0.1f, 0.3f);
            _targetPosition = new Vector2(Mathf.Clamp(puck.position.x + _offsetXFromTarget, _playerBoundary.Left,
                                                    _playerBoundary.Right),
                                        _startingPosition.y);
        }
        // If the puck is within the opponent's half,
        // the opponent moves towards the puck's position,
        // but with varying movement speed based on the distance from the puck.
        else
        {
            _isFirstTimeInOpponentsHalf = true;

            movementSpeed = Random.Range(maxMovementSpeed * 0.4f, maxMovementSpeed);
            _targetPosition = new Vector2(Mathf.Clamp(puck.position.x, _playerBoundary.Left,
                                        _playerBoundary.Right),
                                        Mathf.Clamp(puck.position.y, _playerBoundary.Down,
                                        _playerBoundary.Up));
        }

        _rigidbody.MovePosition(Vector2.MoveTowards(_rigidbody.position, _targetPosition,
                movementSpeed * Time.fixedDeltaTime));
    }

    #endregion
}
