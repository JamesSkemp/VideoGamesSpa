using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace VideoGamesSpa.ApiParser.XboxApi
{
	public class Generator : BaseXblGenerator
	{
		public Generator()
			: base()
		{
		}

		public override bool Run()
		{
			if (base.Run())
			{
				DebugInfo.Add("xboxapi generator");
			}
			return false;
		}
	}
}
