namespace MContracts.View
{
    public class ObjectSelector
    {
        public long? Id { get; private set; }
        public object Item { get; set; }
        public ObjectSelector(long? id)
        {
            Id = id;
        }


    }
}