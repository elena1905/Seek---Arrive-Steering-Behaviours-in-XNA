using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;

namespace WindowsGame2
{
    class Path
    {
        List<Vector2> waypoints = new List<Vector2>();

        bool looped;

        private int current = 0;

        public bool Looped
        {
            get { return looped; }
            set { looped = value; }
        }

        public bool AtEnd
        {
            get { return (current == (waypoints.Count - 1)); }
        }

        public void AddWayPoint(Vector2 point)
        {
            waypoints.Add(point);
        }

        public Vector2 NextWaypoint()
        {
            return waypoints[current];
        }

        public void Advance()
        {
            if (looped)
            {
                current = (current + 1) % waypoints.Count;
            }
            else
            {
                if (! AtEnd)
                {
                    current++;
                }
            }
        }
    }
}
