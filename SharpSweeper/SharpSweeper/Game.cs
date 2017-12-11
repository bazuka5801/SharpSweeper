using System;
using SharpSweeper.Enum;
using SharpSweeper.Struct;

namespace SharpSweeper
{
    public class Game
    {
        private Bomb m_Bomb;
        private Flag m_Flag;
        private GameState _state;

        private GameState m_State
        {
            get => _state;
            set
            {
                _state = value;
                OnGameStateChanged?.Invoke(value);
            }
        }
    
        public GameState MState => m_State;
        public Action<Box> OnOpened;
        public Action<GameState> OnGameStateChanged;
        
        public Game (int cols, int rows, int bombs)
        {
            Ranges.Size = new Coord(cols, rows);
            m_Bomb = new Bomb(bombs);
            m_Flag = new Flag();
        }
    
        public void Start()
        {
            m_Bomb.Start();
            m_Flag.Start();
            m_State = GameState.PLAYED;
        }
    
        public Box GetBox(Coord coord)
        {
            if (m_Flag[coord] == Box.OPENED)
                return m_Bomb[coord];
            return m_Flag[coord];
        }

        public void PressLeftButton(Coord coord)
        {
            if (GameOver()) return;
            OpenBox(coord);
            CheckWinner();
        }
    
        public void PressRightButton(Coord coord)
        {
            if (GameOver()) return;
            m_Flag.ToggleFlaggedBox(coord);
        }
    
        private void CheckWinner()
        {
            if (m_State != GameState.PLAYED) return;
            
            if (m_Flag.GetCountOfClosedBoxes() == m_Bomb.GetTotalBombs())
                m_State = GameState.WINNER;
        }
    
        private bool GameOver()
        {
            if (m_State == GameState.PLAYED)
                return false;
            Start();
            return true;
        }
    
        private void OpenBombs(Coord coord)
        {
            m_State = GameState.BOMBED;
            m_Flag.SetBombedToBox(coord);
            foreach (var around in Ranges.GetAllCoords())
                if (m_Bomb[around] == Box.BOMB)
                    m_Flag.SetOpenedToClosedBombBox(around);
                else
                    m_Flag.SetNobombToFlagedSafeBox(around);
        }
    
        private void OpenBox(Coord coord)
        {
            switch (m_Flag[coord])
            {
                case Box.OPENED: SetOpenedToClosedBoxesAroundNumber(coord); return;
                case Box.FLAGED: return;
                case Box.CLOSED:
                    switch (m_Bomb[coord])
                    {
                        case Box.ZERO: OpenBoxesAround(coord); break;
                        case Box.BOMB: OpenBombs(coord); break;
                        default      : m_Flag.SetOpenedToBox(coord); break;
                    }
                    OnOpened?.Invoke(m_Bomb[coord]);
                    break;
            }
        }
    
        private void  SetOpenedToClosedBoxesAroundNumber(Coord coord)
        {
            if (m_Bomb[coord] == Box.BOMB) return;
            if (m_Flag.GetCountOfFlaggedBoxesAround(coord) != (int) m_Bomb[coord]) return;
            
            foreach (var around in Ranges.GetCoordsAround(coord))
                if (m_Flag[around] == Box.CLOSED)
                    OpenBox(around);
        }
    
        private void OpenBoxesAround(Coord coord)
        {
            m_Flag.SetOpenedToBox(coord);
            foreach (var around in Ranges.GetCoordsAround(coord))
                OpenBox(around);
        }
    }
}