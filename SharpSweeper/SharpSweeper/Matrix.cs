using System;
using SharpSweeper.Enum;
using SharpSweeper.Struct;

namespace SharpSweeper
{
    internal class Matrix
    {
        private Box [,] m_Matrix;

        internal Matrix(Box defaultBox)
        {
            m_Matrix = new Box[Ranges.Size.x, Ranges.Size.y];
            foreach (Coord coord in Ranges.GetAllCoords())
                m_Matrix[coord.x, coord.y] = defaultBox;
        }

        internal Box this[Coord coord]
        {
            get
            {
                if (Ranges.InRange(coord))
                    return m_Matrix [coord.x, coord.y];
//                throw new IndexOutOfRangeException("Matrix[] out of range!");
                return Box.ERROR;
            }
            set
            {
                if (Ranges.InRange(coord))
                    m_Matrix [coord.x, coord.y] = value;
            }
        }
    }
}