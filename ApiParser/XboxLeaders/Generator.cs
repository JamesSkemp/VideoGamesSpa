using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace VideoGamesSpa.ApiParser.XboxLeaders
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
				DebugInfo.Add("xboxleaders generator");
			}
			return false;
		}
	}
}
