using System.Collections.Generic;

namespace ProgComp2013
{
    public class Agent : IEnumerator<Direction>
    {
        private IEnumerator<Direction> _dirs;
        private Map _originalMap;

        private Map _workingMap;

        public int X { get; private set; }
        public int Y { get; private set; }

        public double Score { get; private set; }

        public Agent(Map map, IEnumerable<Direction> dirs)
        {
            _dirs = dirs.GetEnumerator();
            _originalMap = map;

            Reset();
        }

        public Direction Current
        {
            get { return _dirs.Current; }
        }

        public void Dispose()
        {
            _dirs.Dispose();
        }

        object System.Collections.IEnumerator.Current
        {
            get { return _dirs.Current; }
        }

        private void OnMove()
        {
            var normal = Current.Normal();

            X += normal.X;
            Y += normal.Y;

            Score += _workingMap[X, Y];
        }

        public bool MoveNext()
        {
            if (_dirs.MoveNext()) {
                OnMove();
                return true;
            } else {
                return false;
            }
        }

        public void Reset()
        {
            _dirs.Reset();

            _workingMap = _originalMap.Clone();

            Score = 0.0;
        }
    }
}
