namespace StringPrep
{
  public interface IMappingTable
  {
    bool HasReplacement(int value);
    int[] GetReplacement(int value);
  }
}
