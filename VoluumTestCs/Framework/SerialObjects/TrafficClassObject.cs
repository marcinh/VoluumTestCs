namespace VoluumTestCs.Framework.SerialObjects
{
    public class TrafficClassObject
    {
        public string id { get; private set; }

        private TrafficClassObject(string id)
        {
            this.id = id;
        }

        public static TrafficClassObject ZeroPark { get { return new TrafficClassObject("afea734c-8a4a-4f04-bfe6-2e720c1ccb86"); } }
    }
}
