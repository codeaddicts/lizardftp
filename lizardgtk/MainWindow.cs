using System;
using Gtk;
using Codeaddicts.Lizard;
using Codeaddicts.Lizard.Raw;

public partial class MainWindow: Gtk.Window
{
    FtpClient Client;

    public MainWindow () : base (Gtk.WindowType.Toplevel) {
        Build ();
        InitializeComponent ();
    }

    void InitializeComponent () {
        textviewLog.ModifyFont (Pango.FontDescription.FromString ("Consolas"));
        Client = new FtpClient ();
        Client.ResponseReceived += (sender, e)
            => AddMessage (MessageKind.Response, e.Response.Message);
    }

    enum MessageKind { Status, Response, Error };

    void AddMessage (MessageKind kind, string format, params object[] args) {
        var msg1 = string.Format (format, args);
        var msg2 = string.Format ("{0}: {1}\n", kind.ToString ("G").PadRight (16, ' '), msg1);
        textviewLog.Buffer.Text += msg2;
    }

    protected void OnDeleteEvent (object sender, DeleteEventArgs a) {
        Application.Quit ();
        a.RetVal = true;
    }

    protected void OnBtnConnectActivated (object sender, EventArgs e) {
        AddMessage (MessageKind.Status, "Connecting to server");
        Client.Connect (
            host: entryHost.Text,
            user: entryUsername.Text,
            password: entryPassword.Text,
            port: spinbtnPort.ValueAsInt
        );
        Client.Login ();
        Client.SYST ();
        Client.FEAT ();
    }
}
