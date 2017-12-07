using SharpSweeper.Enum;
using SharpSweeper.Struct;

namespace SharpSweeper
{
    internal class Flag
    {
        private Matrix flagMap;
        private int countOfClosedBoxes;

        internal int GetCountOfClosedBoxes() => countOfClosedBoxes;
        internal void Start()
        {
            flagMap = new Matrix(Box.CLOSED);
            countOfClosedBoxes = Ranges.Size.x * Ranges.Size.y;
        }

        internal Box this[Coord coord] => flagMap[coord];
        
        internal void SetOpenedToBox(Coord coord)
        {
            flagMap[coord] = Box.OPENED;
            countOfClosedBoxes--;
        }


        #region [Flags]
        internal void ToggleFlaggedBox(Coord coord)
        {
            switch (flagMap[coord])
            {
                case Box.FLAGED: SetClosedToBox(coord); break;
                case Box.CLOSED: SetFlaggedToBox(coord); break;
            }
        }
        
        private void SetFlaggedToBox(Coord coord) => flagMap[coord] = Box.FLAGED;
        private void SetClosedToBox(Coord coord) => flagMap[coord] = Box.CLOSED;
        #endregion

        internal void SetBombedToBox(Coord coord) => flagMap[coord] = Box.BOMBED;

        internal void SetOpenedToClosedBombBox(Coord coord)
        {
            if (flagMap[coord] == Box.CLOSED)
                flagMap[coord] = Box.OPENED;
        }

        internal void SetNobombToFlagedSafeBox(Coord coord)
        {
            if (flagMap[coord] == Box.FLAGED)
                flagMap[coord] = Box.NOBOMB;
        }

        internal int GetCountOfFlaggedBoxesAround(Coord coord)
        {
            int count = 0;
            foreach (var around in Ranges.GetCoordsAround(coord))
                if (flagMap[around] == Box.FLAGED)
                    count++;
            return count;
        }
    }
}