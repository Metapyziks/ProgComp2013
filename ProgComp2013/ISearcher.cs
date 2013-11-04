namespace ProgComp2013
{
    /// <summary>
    /// Exposes the Search method, which generates a Route
    /// from a given Map.
    /// </summary>
    public interface ISearcher
    {
        Route Search(Map map, int maxLength);
    }
}
