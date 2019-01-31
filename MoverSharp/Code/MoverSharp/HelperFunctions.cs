using System;
using Newtonsoft.Json.Linq;

namespace MoverSharp
{
    public class HelperFunctions
    {
        public static bool ParseMove(ref JObject obj, ref Direction direction, ref Int32 steps)
        {
            if (obj["d"] == null || obj["n"] == null)
            {
                return false;
            }

            // We should do these checks the same as in the C++ Mover sample code.
            // The best way is to get rid of the dynamic type and deserialize the JSON into a struct.
            // Those checks are easier to do.

            /*
			if (obj["d"].GetType() != typeof(string))
			{
				return false;
			}

			if (obj["n"].GetType() != typeof(Int32))
			{
				return false;
			}*/

            direction = ParseDirection(obj["d"].ToString());

            if (direction == Direction.NONE)
            {
                return false;
            }

            steps = (Int32)obj["n"];

            if (steps < 0 || steps > 1000000)
            {
                return false;
            }

            return true;
        }

        public static Direction ParseDirection(string str)
        {
            if (str == "l")
                return Direction.RIGHT;
            if (str == "h")
                return Direction.LEFT;
            if (str == "k")
                return Direction.UP;
            if (str == "j")
                return Direction.DOWN;
            if (str == "u")
                return Direction.RIGHT_UP;
            if (str == "n")
                return Direction.RIGHT_DOWN;
            if (str == "y")
                return Direction.LEFT_UP;
            if (str == "b")
                return Direction.LEFT_DOWN;

            return Direction.NONE;
        }


        public static void GetDirectionOffset(Direction dir, ref Int32 dx, ref Int32 dy)
        {
            switch (dir)
            {
                case Direction.RIGHT:
                    dx = 1;
                    dy = 0;
                    return;

                case Direction.LEFT:
                    dx = -1;
                    dy = 0;
                    return;

                case Direction.UP:
                    dx = 0;
                    dy = 1;
                    return;

                case Direction.DOWN:
                    dx = 0;
                    dy = -1;
                    return;

                case Direction.RIGHT_UP:
                    dx = 1;
                    dy = 1;
                    return;

                case Direction.RIGHT_DOWN:
                    dx = 1;
                    dy = -1;
                    return;

                case Direction.LEFT_UP:
                    dx = -1;
                    dy = 1;
                    return;

                case Direction.LEFT_DOWN:
                    dx = -1;
                    dy = -1;
                    return;

                default:
                    Console.WriteLine("Unexpected direction: " + dir);
                    return;
            }
        }


        public static string DirectionToString(Direction dir)
        {
            switch (dir)
            {
                case Direction.NONE:
                    return "none";
                case Direction.RIGHT:
                    return "right";
                case Direction.LEFT:
                    return "left";
                case Direction.UP:
                    return "up";
                case Direction.DOWN:
                    return "down";
                case Direction.RIGHT_UP:
                    return "right-up";
                case Direction.RIGHT_DOWN:
                    return "right-down";
                case Direction.LEFT_UP:
                    return "left-up";
                case Direction.LEFT_DOWN:
                    return "left-down";
            }

            Console.WriteLine("Unexpected direction: " + dir);
            return "";
        }
    }
}