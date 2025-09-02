using System;

namespace Echoes.Runtime
{
    /**
     * A rumor object represents a rumor progressing between two npc.
     */
    public class Rumor
    {
        private readonly EchoesNpcComponent _from;
        private readonly EchoesNpcComponent _to;
        private double _distanceRan = 0;
        private readonly double _distanceToRun;

        public Rumor(EchoesNpcComponent from, EchoesNpcComponent to)
        {
            _from = from;
            _to = to;
            try
            {
                _distanceToRun = GlobalStats.Instance.globalDistance.GetContactDistance(from.npcData.Name, to.npcData.Name);
            }
            catch (Exception ignore)
            {
                _distanceToRun = GlobalStats.Instance.globalDistance.minValue;
            }
        }

        /**
         * @param distance to be added to the total distance ran
         * @return whether the goal was reached and the rumor propagated
         */
        public bool Update(double distanceSinceLastUpdate)
        {
            _distanceRan += distanceSinceLastUpdate;

            if (_distanceRan >= _distanceToRun)
                return _to.ReceiveOpinion(_from);
            return false;
        }
    }
}