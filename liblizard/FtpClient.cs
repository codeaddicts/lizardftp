﻿using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net.Sockets;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace Codeaddicts.Lizard
{
	public class FtpClient : IDisposable
	{
		public const string	DEFAULT_USER		= "Anonymous";
		public const string	DEFAULT_PASSWORD	= "";
		public const string	DEFAULT_HOST		= "127.0.0.1";
		public const string	DEFAULT_PATH		= "/";
		public const int	DEFAULT_PORT		= 21;

		Socket	Client;
		string	Host;
		string	User;
		string	Path;
		string	Password;
		int		Port;

		public FtpClient () {
			InitializeParameters ();
		}

		public FtpClient (string formatUri) {
			InitializeParameters (
				parser: new UriParser (formatUri)
			);
		}

		public void Connect () {
			Client = new Socket (
				addressFamily: AddressFamily.InterNetwork,
				socketType: SocketType.Stream,
				protocolType: ProtocolType.Tcp
			);
			Client.Connect (Host, Port);
			Task.Factory.StartNew (StartListening);
			SendCredentials ();
		}

		public void Connect (
			string host,
			int port = DEFAULT_PORT,
			string user = DEFAULT_USER,
			string password = DEFAULT_PASSWORD,
			string path = DEFAULT_PATH) {
			InitializeParameters (
				host: host,
				port: port,
				user: user,
				path: path,
				password: password
			);
			Connect ();
		}

		public void Connect (string user, string password = DEFAULT_PASSWORD) {
			Connect (
				host: Host,
				port: Port,
				user: user,
				password: password
			);
		}

		public void Wait () {
			while (Client.Connected)
				Thread.Sleep (1);
		}

		void StartListening () {
			var ns = new NetworkStream (Client, false);
			var reader = new StreamReader (ns, Encoding.ASCII, false);
			while (Client.Connected) {
				AcceptLine (reader);
			}
			reader.Dispose ();
			ns.Dispose ();
		}

		void AcceptLine (StreamReader reader) {
			var line = reader.ReadLine ();
			int code = 0;
			bool dash;
			if (line.Length > 2) {
				var arr = line.Take (3).ToArray ();
				var str = new string (arr);
				code = int.Parse (str);
			}
			dash = line.Length > 3 && line [3] == '-';
			string message = line
				.Substring (3 + (dash ? 1 : 0))
				.TrimStart (' ');
			ProcessMessage (code, dash, message);
		}

		void ProcessMessage (int code, bool dashed, string message) {
			switch (code) {
			case 110:
				// Restart marker reply
				break;
			case 211:
				// System status, or system help reply
				break;
			case 200:
				// Command okay
				break;
			case 202:
				// Command not implemented, superfluous at this site
				break;
			case 220:
				// Service ready for new user
				break;
			case 500:
				// Syntax error or
				// Command unrecognized
				break;
			case 501:
				// Syntax error in parameters or arguments
				break;
			case 502:
				// Command not implemented
				break;
			case 503:
				// Bad sequence of commands
				break;
			case 504:
				// Command not implemented for that parameter
				break;
			}
			Console.WriteLine ("[{0:000}] {1}", code, message);
		}

		void SendCredentials () {
			Send ("USER {0}\r\n", User);
			if (!string.IsNullOrEmpty (Password))
				Send ("PASS {0}\r\n", Password);
		}

		void Send (string format, params object[] args) {
			var text = string.Format (format, args);
			var data = Encoding.ASCII.GetBytes (text);
			SendRaw (data);
		}

		void SendRaw (byte[] data) {
			SocketError error;
			Client.NoDelay = true;
			Client.Send (
				buffer: data,
				offset: 0,
				size: data.Length,
				socketFlags: SocketFlags.None,
				errorCode: out error
			);
			Client.NoDelay = false;
		}

		void InitializeParameters (
			string host,
			int port,
			string user,
			string path,
			string password) {
			Host = host;
			Port = port;
			User = user;
			Path = path;
			Password = password;
		}

		void InitializeParameters (UriParser parser) {
			InitializeParameters (
				host: parser.Host,
				port: parser.Port,
				user: parser.User,
				path: parser.Path,
				password: parser.Password
			);
		}

		void InitializeParameters () {
			InitializeParameters (
				host: DEFAULT_HOST,
				port: DEFAULT_PORT,
				user: DEFAULT_USER,
				path: DEFAULT_PATH,
				password: DEFAULT_PASSWORD
			);
		}

		#region IDisposable implementation

		public void Dispose () {
			Client.Dispose ();
		}

		#endregion
	}
}
