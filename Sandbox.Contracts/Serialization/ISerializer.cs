namespace Sandbox.Contracts.Serialization
{
    public interface ISerializer
    {
        void Serialize<T>(T obj, string filename);
        
        T Deserialize<T>(string filename);
    }
}