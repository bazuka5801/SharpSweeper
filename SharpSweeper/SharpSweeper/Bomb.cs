using System;
using System.Collections.Generic;
using System.Linq;
using SharpSweeper.CSharp;
using SharpSweeper.Enum;
using SharpSweeper.Struct;

namespace SharpSweeper
{
    internal class Bomb
    {
        private Matrix m_BombMap;
        private int m_TotalBombs;

        internal Box this[Coord coord] => m_BombMap[coord];
        internal int GetTotalBombs() => m_TotalBombs;
        
        internal Bomb (int totalBombs)
        {
            this.m_TotalBombs = totalBombs;
            FixBombsCount();
        }

        internal void Start()
        {
            m_BombMap = new Matrix(Box.ZERO);
            var availableCoords = Ranges.GetAllCoords().ToList();
            for (int j = 0; j < m_TotalBombs; j++)
                PlaceBomb(availableCoords);
        }



        private void PlaceBomb(List<Coord> availableCoords)
        {
            var coord = availableCoords.GetRandom();
            availableCoords.Remove(coord);
            
            m_BombMap[coord] = Box.BOMB;
            IncNumberAroundBomb(coord);
        }

        private void FixBombsCount()
        {
            int maxBombs = Ranges.Size.x * Ranges.Size.y - ((Ranges.Size.x + Ranges.Size.y)/2);
            if (m_TotalBombs > maxBombs)
                m_TotalBombs = maxBombs;
        }

        private void  IncNumberAroundBomb(Coord coord)
        {
            foreach (var around in Ranges.GetCoordsAround(coord))
                if (Box.BOMB != m_BombMap[around])
                    m_BombMap[around]++;
        }

    }
}