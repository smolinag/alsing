using System;
using System.Data;

namespace MyMeta.VistaDB
{

	using System.Runtime.InteropServices;
	[ComVisible(true), ClassInterface(ClassInterfaceType.AutoDual), ComDefaultInterface(typeof(IResultColumn))]

	public class VistaDBResultColumn : ResultColumn
	{
		public VistaDBResultColumn()
		{

		}

		#region Properties

		override public string Alias
		{
			get
			{
				return name;
			}
		}

		override public string DataTypeName
		{
			get
			{
				return typeName;
			}
		}

		override public System.Int32 Ordinal
		{
			get
			{
				return ordinal;
			}
		}

		internal string name = "";
		internal string typeName = "";
		internal System.Int32 ordinal = 0;

		#endregion
	}
}
