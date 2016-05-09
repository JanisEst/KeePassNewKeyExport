using System.Diagnostics.Contracts;
using System.Drawing;

using KeePass.Plugins;
using KeePass.DataExchange;

namespace KeePassNewKeyExport
{
	public class KeePassNewKeyExportExt : Plugin
	{
		private IPluginHost host = null;
		private FileFormatProvider provider = null;

		public override Image SmallIcon
		{
			get { return Properties.Resources.B16x16_KeePassPlus; }
		}

		public override string UpdateUrl
		{
			get { return "https://github.com/KN4CK3R/KeePassNewKeyExport/raw/master/keepass.version"; }
		}

		public override bool Initialize(IPluginHost host)
		{
			Contract.Requires(host != null);

			this.host = host;

			provider = new NewKeyKdb2x();

			host.FileFormatPool.Add(provider);

			return true;
		}

		public override void Terminate()
		{
			if (host != null)
			{
				host.FileFormatPool.Remove(provider);
			}
		}
	}
}
