namespace StringPrep
{
  public interface IValueRangeTableBuilder
  {
    IValueRangeTableBuilder Include(int include);
    IValueRangeTableBuilder IncludeRange(int start, int end);
    IValueRangeTableBuilder Remove(int remove);
    IValueRangeTableBuilder RemoveRange(int start, int end);
    IValueRangeTable Compile();
  }
}
