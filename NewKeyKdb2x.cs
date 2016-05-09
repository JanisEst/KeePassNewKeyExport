using KeePass.App;
using KeePass.DataExchange;
using KeePass.Forms;
using KeePassLib;
using KeePassLib.Interfaces;
using KeePassLib.Serialization;
using System.Diagnostics;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Windows.Forms;

namespace KeePassNewKeyExport
{
	internal sealed class NewKeyKdb2x : FileFormatProvider
	{
		public override bool SupportsImport { get { return false; } }
		public override bool SupportsExport { get { return true; } }

		public override string FormatName { get { return string.Format("KeePass KDBX (2.x) ({0})", KeePass.Resources.KPRes.ChangeMasterKey); } }
		public override string DefaultExtension { get { return AppDefs.FileExtension.FileExt; } }
		public override string ApplicationGroup { get { return PwDefs.ShortProductName; } }

		public override Image SmallIcon
		{
			get { return Properties.Resources.B16x16_KeePassPlus; }
		}

		public override bool Export(PwExportInfo pwExportInfo, Stream sOutput,
			IStatusLogger slLogger)
		{
			//Debugger.Launch();

			//ask for the database key
			KeyCreationForm kcf = new KeyCreationForm();
			DialogResult dr = kcf.ShowDialog();
			if (dr == DialogResult.Cancel || dr == DialogResult.Abort)
			{
				return false;
			}

			PwDatabase pd = new PwDatabase();
			pd.New(new IOConnectionInfo(), kcf.CompositeKey);

			//get used custom icons
			pwExportInfo.DataGroup.TraverseTree(
				TraversalMethod.PreOrder,
				null,
				e =>
				{
					if (!e.CustomIconUuid.Equals(PwUuid.Zero))
					{
						pd.CustomIcons.AddRange(pwExportInfo.ContextDatabase.CustomIcons.Where(i => i.Uuid.Equals(e.CustomIconUuid)));
					}

					return true;
				}
			);

			//get other database settings
			DatabaseSettingsForm dsf = new DatabaseSettingsForm();
			dsf.InitEx(true, pd);
			dr = dsf.ShowDialog();
			if (dr == DialogResult.Cancel || dr == DialogResult.Abort)
			{
				pd.Close();

				return false;
			}

			//and save the new database
			KdbxFile kdbx = new KdbxFile(pd);
			kdbx.Save(sOutput, pwExportInfo.DataGroup, KdbxFormat.Default, slLogger);

			return true;
		}
	}
}
