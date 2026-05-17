public interface ICell
{
    bool TryGetUnit(out Unit unit);

    ICell[] GetVerticalNeighbours();
    ICell[] GetHorizontalNeighbours();
    ICell[] GetDiagonalNeighbours();
}