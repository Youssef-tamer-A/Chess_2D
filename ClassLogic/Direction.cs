namespace ChessLogic
{

    public class Direction
    {
        public readonly static Direction North = new Direction(-1, 0);
        public readonly static Direction South = new Direction(1, 0);
        public readonly static Direction East = new Direction(0, 1);
        public readonly static Direction West = new Direction(0, -1);
        public readonly static Direction NorthEast = North + East;
        public readonly static Direction NorthWest = North + West;
        public readonly static Direction SouthEast = South + East;
        public readonly static Direction SouthWest = South + West;

        public int RowDalte { get; }
        public int ColumnDelta { get; }

        public Direction(int rowDalte, int columnDelta)
        {
            RowDalte = rowDalte;
            ColumnDelta = columnDelta;
        }

        public static Direction operator +(Direction Dir1, Direction Dir2)
        {
            return new Direction(Dir1.RowDalte + Dir2.RowDalte, Dir1.ColumnDelta + Dir2.ColumnDelta);
        }

        public static Direction operator *(Direction direction, int scalar)
        {
            return new Direction(direction.RowDalte * scalar, direction.ColumnDelta * scalar);
        }
    }
}
