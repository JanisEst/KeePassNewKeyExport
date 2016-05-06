using System.Diagnostics.Contracts;
using System.Drawing;

using KeePass.Plugins;

namespace KeePassNewKeyExport
{
	public class KeePassNewKeyExportExt : Plugin
	{
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

			host.FileFormatPool.Add(new NewKeyKdb2x());

			return true;
		}
	}
}
