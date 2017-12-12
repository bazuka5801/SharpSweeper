using System;
using SharpSweeper.Enum;
using SharpSweeper.Struct;

namespace SharpSweeper.Interfaces
{
    public interface ISharpSweeperGame
    {
        Box GetBox(Coord coord);
        void PressLeftButton(Coord coord);
        void PressRightButton(Coord coord);

        GameState State { get; }
        Action<Box> OnOpened { get; set; }
        Action<GameState> OnGameStateChanged { get; set; }
    }
}