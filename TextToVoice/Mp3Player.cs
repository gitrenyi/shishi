using System;
using System.Runtime.InteropServices;

namespace TextToVoice
{
	public class Mp3Player
	{
		public enum State
		{
			mPlaying = 1,
			mPuase,
			mStop
		}

		public struct structMCI
		{
			public bool bMut;

			public int iDur;

			public int iPos;

			public int iVol;

			public int iBal;

			public string iName;

			public Mp3Player.State state;
		}

		[MarshalAs(UnmanagedType.ByValTStr, SizeConst = 260)]
		private string Name = "";

		[MarshalAs(UnmanagedType.ByValTStr, SizeConst = 128)]
		private string durLength = "";

		[MarshalAs(UnmanagedType.LPTStr)]
		private string TemStr = "";

		private int ilong;

		public Mp3Player.structMCI mc;

		public string FileName
		{
			get
			{
				return this.mc.iName;
			}
			set
			{
				try
				{
					this.TemStr = "";
					this.TemStr = this.TemStr.PadLeft(127, Convert.ToChar(" "));
					this.Name = this.Name.PadLeft(260, Convert.ToChar(" "));
					this.mc.iName = value;
					this.ilong = APIClass.GetShortPathName(this.mc.iName, this.Name, this.Name.Length);
					this.Name = this.GetCurrPath(this.Name);
					this.Name = string.Concat(new string[]
					{
						"open ",
						Convert.ToChar(34).ToString(),
						this.Name,
						Convert.ToChar(34).ToString(),
						" alias media"
					});
					this.ilong = APIClass.mciSendString("close all", this.TemStr, this.TemStr.Length, 0);
					this.ilong = APIClass.mciSendString(this.Name, this.TemStr, this.TemStr.Length, 0);
					this.ilong = APIClass.mciSendString("set media time format milliseconds", this.TemStr, this.TemStr.Length, 0);
					this.mc.state = Mp3Player.State.mStop;
				}
				catch
				{
				}
			}
		}

		public int Duration
		{
			get
			{
				this.durLength = "";
				this.durLength = this.durLength.PadLeft(128, Convert.ToChar(" "));
				APIClass.mciSendString("status media length", this.durLength, this.durLength.Length, 0);
				this.durLength = this.durLength.Trim();
				if (this.durLength == "")
				{
					return 0;
				}
				return (int)(Convert.ToDouble(this.durLength) / 1000.0);
			}
		}

		public int CurrentPosition
		{
			get
			{
				this.durLength = "";
				this.durLength = this.durLength.PadLeft(128, Convert.ToChar(" "));
				APIClass.mciSendString("status media position", this.durLength, this.durLength.Length, 0);
				this.mc.iPos = (int)(Convert.ToDouble(this.durLength) / 1000.0);
				return this.mc.iPos;
			}
		}

		public void play()
		{
			this.TemStr = "";
			this.TemStr = this.TemStr.PadLeft(127, Convert.ToChar(" "));
			APIClass.mciSendString("play media", this.TemStr, this.TemStr.Length, 0);
			this.mc.state = Mp3Player.State.mPlaying;
		}

		public void StopT()
		{
			this.TemStr = "";
			this.TemStr = this.TemStr.PadLeft(128, Convert.ToChar(" "));
			this.ilong = APIClass.mciSendString("close media", this.TemStr, 128, 0);
			this.ilong = APIClass.mciSendString("close all", this.TemStr, 128, 0);
			this.mc.state = Mp3Player.State.mStop;
		}

		public void Puase()
		{
			this.TemStr = "";
			this.TemStr = this.TemStr.PadLeft(128, Convert.ToChar(" "));
			this.ilong = APIClass.mciSendString("pause media", this.TemStr, this.TemStr.Length, 0);
			this.mc.state = Mp3Player.State.mPuase;
		}

		private string GetCurrPath(string name)
		{
			if (name.Length < 1)
			{
				return "";
			}
			name = name.Trim();
			name = name.Substring(0, name.Length - 1);
			return name;
		}
	}
}
