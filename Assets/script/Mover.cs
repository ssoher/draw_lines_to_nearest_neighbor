/*
Ironbelly Programming Test
Sarper Soher - https://www.accidentalee.com
Mar 04 2021 - 09:17
*/

using UnityEngine;

public sealed class Mover : MonoBehaviour {
    public float UnitsPerFrame;

    private Vector3 _destinationPosition;
    private Vector3 _moveStartPosition;
    private float _moveStartTime;
    private float _moveDuration;
    private bool _isMoving;

    private void OnDisable() {
        _isMoving = false;
    }

    private void Update() {
        // NOTE(sarper Mar 04 21): Issue a new movement as soon as one movement is complete
        if(!_isMoving) {
            MoveTo(MovementSceneBounds.Instance.RandomPosition());
        } else {
            // NOTE(sarper Mar 04 21): Calculate the linear interpolation blend value based on the elapsed time since last movement started
            float t = (Time.time - _moveStartTime) / _moveDuration;
            transform.position = Vector3.Lerp(_moveStartPosition, _destinationPosition, t);

            // NOTE(sarper Mar 04 21): Linear interpolation finished, meaning the movement is finished
            if(t >= 1f) {
                _isMoving = false;
            }
        }
    }

    // NOTE(sarper Mar 04 21): Setup a new movement command, calculating all the required values for linear interpolation towards the destination
    private void MoveTo(Vector3 destination) {
        _destinationPosition = destination;
        _moveStartPosition = transform.position;
        _moveStartTime = Time.time;
        _moveDuration = Vector3.Distance(_moveStartPosition, _destinationPosition) / UnitsPerFrame;
        _isMoving = true;
    }
}