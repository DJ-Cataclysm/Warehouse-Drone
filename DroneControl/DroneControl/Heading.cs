using System;

namespace DroneControl
{
    public class Heading
    {
        public readonly float left, right, front, back;

        public Heading(float initialRadiansWhileFacingFront)
        {
            const float pi = (float)Math.PI;


            front = initialRadiansWhileFacingFront;
            right = front + (pi * 0.5f);
            while(right > (pi * 2))
            {
                right -= pi * 2;
            }
            back = front + pi;
            while (back > (pi * 2))
            {
                back -= pi * 2;
            }
            left = front - (pi * 0.5f);
            while (left > (pi * 2))
            {
                left -= pi * 2;
            }
        }
    }
}
