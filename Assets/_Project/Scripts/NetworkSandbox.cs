using Unity.Netcode;

namespace _Project.Scripts
{
	public class NetworkSandbox : NetworkBehaviour
	{
		private void Start()
		{
			AbcdServerRpc(1);
			XyzwServerRpc(2, new RpcParams
			{
				Receive = new RpcReceiveParams
				{
					SenderClientId = 123
				},
				Send = new RpcSendParams
				{
					LocalDeferMode = LocalDeferMode.SendImmediate,
					Target = RpcTarget.Everyone
				}
			});
		}

		[Rpc(SendTo.Server)]
		private void AbcdServerRpc(int somenumber)
		{
			
		}

		[Rpc(SendTo.Server)]
		private void XyzwServerRpc(int somenumber, RpcParams rpcParams = default)
		{
			
		}
	}
}