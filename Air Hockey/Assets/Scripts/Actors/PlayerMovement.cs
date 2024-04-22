using System.Collections;
using System.Collections.Generic;
using UnityEngine;

#region Class Description:
/*
 * This script controls the player character's movement using mouse input.
 */
#endregion

public class PlayerMovement : MonoBehaviour
{
    #region Fields

    // A boolean flag indicating whether the mouse button was just clicked.
    private bool _wasJustClicked = true;

    // A boolean flag indicating whether the player can move.
    private bool _canMove;

    // A Vector2 storing the size of the player character.
    private Vector2 _playerSize;

    // A Rigidbody2D field. 
    private Rigidbody2D _rigidbody;

    // a public Transform field 'BoundaryHolder' to reference the boundaries.
    public Transform BoundaryHolder;

    // A Boundary struct to store boundary positions.
    private Boundary _playerBoundary;

    #endregion

    #region Initialisations

    private void Start()
    {
        // Initializes _playerSize by getting the size of the sprite renderer's bounds.
        _playerSize = gameObject.GetComponentInChildren<SpriteRenderer>().bounds.extents;

        // Intializes _rigidbody.
        _rigidbody = gameObject.GetComponent<Rigidbody2D>();

        // Extracts boundary positions from BoundaryHolder.
        _playerBoundary = new Boundary(BoundaryHolder.GetChild(0).position.y,
                                       BoundaryHolder.GetChild(1).position.y,
                                       BoundaryHolder.GetChild(2).position.x,
                                       BoundaryHolder.GetChild(3).position.x);
    }

    #endregion

    #region Updates

    private void Update()
    {
        // Checks if the left mouse button is pressed.
        if (Input.GetMouseButton(0))
        {
            // Converts the mouse position from screen coordinates to world coordinates.
            Vector2 mousePosition = Camera.main.ScreenToWorldPoint(Input.mousePosition);

            // If the mouse button was just clicked, it checks if the mouse position is within the bounds of the player character.
            if (_wasJustClicked)
            {
                _wasJustClicked = false;

                if ((mousePosition.x >= transform.position.x && mousePosition.x < transform.position.x + _playerSize.x ||
                mousePosition.x <= transform.position.x && mousePosition.x > transform.position.x - _playerSize.x) &&
                (mousePosition.y >= transform.position.y && mousePosition.y < transform.position.y + _playerSize.y ||
                mousePosition.y <= transform.position.y && mousePosition.y > transform.position.y - _playerSize.y))
                {
                    // If it is, _canMove is set to true, allowing movement.
                    _canMove = true;
                }
                else
                {
                    // If not, _canMove is set to false.
                    _canMove = false;
                }
            }

            // If _canMove is true, it updates the player's position to the mouse position.
            if (_canMove)
            {
                // Clamping logic to restrict the player's movement within the defined boundaries.
                Vector2 clampedMousePosition = new Vector2(Mathf.Clamp(mousePosition.x, _playerBoundary.Left, _playerBoundary.Right),
                                                           Mathf.Clamp(mousePosition.y, _playerBoundary.Down, _playerBoundary.Up));

                // Uses _rigidbody.MovePosition() for smoother movement using physics.
                _rigidbody.MovePosition(clampedMousePosition);
            }
        }
        else
        {
            // If the mouse button is not pressed, _wasJustClicked is set to true, resetting the flag for the next click.
            _wasJustClicked = true;
        }
    }

    #endregion
}
