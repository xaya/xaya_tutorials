using System;
using System.Collections.Generic;

namespace MoverSharp
{
    // A possible direction of movement.
    public enum Direction
    {
        // NONE is the direction for players that are not moving,
        // in particular, after steps_left has counted down to zero.

        NONE = 0,
        RIGHT = 1,
        LEFT = 2,
        UP = 3,
        DOWN = 4,
        RIGHT_UP = 5,
        RIGHT_DOWN = 6,
        LEFT_UP = 7,
        LEFT_DOWN = 8
    }

    // The state of a particular player in Mover.
    public class PlayerState
    {
        // The current x coordinate.
        public int x;
        // The current y coordinate. 
        public int y;

        // The direction of movement.
        public Direction dir = Direction.UP;
        // The remaining number of movement steps left. 
        public Int32 steps_left;
    }


    public class GameStateResultFromResponse
    {
        public string blockhash;
        public string chain;
        public string gameid;
        public string state;
        public string gamestate;
    }

    // The full game state.
    public class GameState
    {
        // All players on the map and their current state. 
        public Dictionary<string, PlayerState> players;
    }

    // The undo data for a single player. 
    public class PlayerUndo
    {
        // Set to true if the player was not previously present, i.e. if it was
        // first moved and created on the map for this block.
        public bool is_new;

        // Previous direction of the player, if it was changed explicitly.
        public Direction previous_dir = Direction.NONE;

        // Previous steps left if the number was changed explicitly by a move.
        public Int32 previous_steps_left = 99999999;

        // Previous direction of the player if it counted down to zero and was
        // changed to NONE in this block.
        // 
        // In theory, this field could be merged with previous_dir. It is possible
        // that both are set, namely when a move with steps = 1 was made. But this case
        // could be reversed using the move data. The potential space savings
        // here seem minor though, so we use a separate field to simplify the logic.
        public Direction finished_dir = Direction.NONE;
    }

    // The full undo data for a block. 
    public class UndoData
    {
        // Undo data for each player that needs one.
        public Dictionary<string, PlayerUndo> players;
    }
}