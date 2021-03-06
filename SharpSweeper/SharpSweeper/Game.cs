﻿using System;
using SharpSweeper.Enum;
using SharpSweeper.Interfaces;
using SharpSweeper.Struct;

namespace SharpSweeper
{
    public class Game : ISharpSweeperGame
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
    
        public GameState State => m_State;
        public Action<Box> OnOpened { get; set; }
        public Action<GameState> OnGameStateChanged { get; set; }
        public Action OnRepaint { get; set; }

        public bool RaiseCallbacks { get; set; } = true;

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
            if (RaiseCallbacks)OnRepaint?.Invoke();
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
            if (RaiseCallbacks)OnRepaint?.Invoke();
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
                    if (RaiseCallbacks)
                    {
                        OnOpened?.Invoke(m_Bomb[coord]);
                        OnRepaint?.Invoke();
                    }
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