using System;
using System.Data;
using System.Data.OleDb;

namespace MyMeta.Access
{

	using System.Runtime.InteropServices;
	[ComVisible(true), ClassInterface(ClassInterfaceType.AutoDual), ComDefaultInterface(typeof(IDomains))]

	public class AccessDomains : Domains
	{
		public AccessDomains()
		{

		}
	}
}
