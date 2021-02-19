using GXPEngine;
using GXPEngine.Core;
using System;

public class CameraFollow : Camera
{
    private GameObject target = null;
    private Vector2 size = new Vector2();
    private Vector2 boundriesX = new Vector2();
    private Vector2 boundriesY = new Vector2();

    private int lookAheadDistanceX = 50;
    private float currentLookAheadX = 0;
    private float targetLookAheadX = 10;
    private float lookSmoothingX = 0.1f;
    private float lookSmoothingY = 0.2f;
    private float lookAheadDirectionX = 0;

    private bool lookAheadStopped = false;

    private float verticalOffset = 0;

    FocusArea focusArea;

    public CameraFollow(GameObject _target, Vector2 _boundriesX, Vector2 _boundriesY) : base(0, 0, Game.main.width, Game.main.height)
    {
        target = _target;
        size = new Vector2(64, game.height / 3);
        focusArea = new FocusArea(size, new Vector2(_target.x, _target.y));
        verticalOffset = game.height / 12f;
        lookAheadDistanceX = game.width / 8;

        boundriesX = _boundriesX;
        boundriesY = _boundriesY;

        x = target.x;
        y = target.y;
    }

    void Update()
    {
        focusArea.Update(target);
        Gizmos.DrawRectangle(focusArea.center.x, focusArea.center.y, size.x, size.y, this, Convert.ToUInt32("ffffff", 16), 5);
        //Console.WriteLine("Center " + focusArea.center);

        Vector2 focusPosition = focusArea.center + new Vector2(0, -verticalOffset);
        if(focusArea.velocity.x != 0)
        {
            lookAheadDirectionX = Mathf.Sign(focusArea.velocity.x);
            if(Mathf.Sign(GameBehaviour.GetHorizontalAxis()) == Mathf.Sign(focusArea.velocity.x) && GameBehaviour.GetHorizontalAxis() != 0)
            {
                targetLookAheadX = lookAheadDirectionX * lookAheadDistanceX;
                lookAheadStopped = false;
            }
            else
            {
                if (!lookAheadStopped)
                {
                    targetLookAheadX = currentLookAheadX + (lookAheadDirectionX * lookAheadDistanceX - currentLookAheadX) / 4;
                    lookAheadStopped = true;
                }
            }
        }

        
        currentLookAheadX = Mathf.Lerp(currentLookAheadX, targetLookAheadX, lookSmoothingX);

        focusPosition.y = Mathf.Lerp(y, focusPosition.y, lookSmoothingY);
        focusPosition.x += currentLookAheadX;

        focusPosition.x = Mathf.Clamp(focusPosition.x, boundriesX.x, boundriesX.y);
        focusPosition.y = Mathf.Clamp(focusPosition.y, boundriesY.x, boundriesY.y);

        x = focusPosition.x;
        y = focusPosition.y;
    }

    struct FocusArea
    {
        public Vector2 center;
        public Vector2 velocity;

        float left, right;
        float top, bottom;

        public FocusArea(Vector2 _size, Vector2 _center)
        {
            center = _center;
            left = center.x - _size.x / 2;
            right = center.x + _size.x / 2;
            top = center.y - _size.y / 2;
            bottom = center.y + _size.y / 2;
            velocity = new Vector2();
        }

        public void Update(GameObject _target)
        {
            float shiftX = 0;
            float shiftY = 0;

            if(_target.x < left)
            {
                shiftX = _target.x - left;
            }else if(_target.x > right)
            {
                shiftX = _target.x - right;
            }

            if (_target.y < top)
            {
                shiftY = _target.y - top;
            }
            else if (_target.y > bottom)
            {
                shiftY = _target.y - bottom;
            }

            left += shiftX;
            right += shiftX;
            top += shiftY;
            bottom += shiftY;
            center = new Vector2((left + right) / 2, (top + bottom) / 2);
            velocity = new Vector2(shiftX, shiftY);
        }
    }
}
