namespace ProgComp2013.Searchers
{
    /// <summary>
    /// We should at least beat this.
    /// </summary>
    public class RowScan : IterativeSearcher
    {
        protected override Direction Next(Agent agent)
        {
            var dir = Direction.None;

            if ((agent.Y & 1) == 1) {
                if (agent.X == 0) dir = Direction.South;
                else dir = Direction.West;
            } else {
                if (agent.X == Map.Width - 1) dir = Direction.South;
                else dir = Direction.East;
            }

            if (agent.Y == Map.Height - 1 && dir == Direction.South) {
                dir = Direction.None;
            }

            return dir;
        }
    }
}
