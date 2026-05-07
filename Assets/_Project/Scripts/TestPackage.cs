using Unity.Netcode;

/// <summary>
/// Represents a serializable network package that contains data to be transmitted over a network.
/// Implements the INetworkSerializable interface to enable serialization and deserialization of its content.
/// </summary>
public struct TestPackage : INetworkSerializable
{
	public int Value;
	
	public void NetworkSerialize<T>(BufferSerializer<T> serializer) where T : IReaderWriter
	{
		serializer.SerializeValue(ref Value);
	}
}