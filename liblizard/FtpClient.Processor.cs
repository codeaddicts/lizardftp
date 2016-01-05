﻿using System;
using System.Text.RegularExpressions;

namespace Codeaddicts.Lizard
{
    public partial class FtpClient
    {
        void ProcessMessage (int code, bool dashed, string message, string raw) {
            Ready = false;
            LogMessage (code, message);
            switch (code) {
            case 110:
                // Restart marker reply
                break;
            case 120:
                // Service ready in nnn minutes
                break;
            case 125:
                // Data connection already open; traClientStreamfer starting
                break;
            case 150:
                // File status okay; about to open data connection
                break;
            case 200:
                // Command okay
                break;
            case 202:
                // Command not implemented, superfluous at this site
                break;
            case 211:
                // System status, or system help reply
                // Response to FEAT
                if (Regex.IsMatch (raw, REGEX_FEAT_BEGIN)) {
                    string line = ClientReader.ReadLine ();
                    while (!Regex.IsMatch (line, REGEX_FEAT_END)) {
                        ProcessMessage (000, false, line, line);
                        line = ClientReader.ReadLine ();
                    }
                    AcceptResponse (line);
                }
                break;
            case 212:
                // Directory status
                break;
            case 213:
                // File status
                break;
            case 214:
                // Help message
                break;
            case 215:
                // NAME system type
                break;
            case 220:
                // Service ready for new user
                break;
            case 221:
                // Service closing control connection
                break;
            case 225:
                // Data connection open; no traClientStreamfer in progress
                break;
            case 226:
                // Closing data connection
                break;
            case 227:
                // Entering passive mode (h1,h2,h3,h4,p1,p2)
                var ep = ParsePasvResponse (message);
                LogMessage (000, ep.ToString ());
                if (Data.Available)
                    Data.Close ();
                LogMessage (000, "Opening data connection");
                Data.SetAvailable (ep);
                break;
            case 230:
                // User logged in, proceed
                break;
            case 250:
                // Requested file action okay, completed
                break;
            case 257:
                // "PATHNAME" created
            case 331:
                // User name okay, need password
                Quit |= string.IsNullOrEmpty (Password);
                break;
            case 332:
                // Need account for login
                break;
            case 350:
                // Requested file action pending further information
            case 421:
                // Service not available, closing control connection
                // Also: Login time exceeded
                Quit = true;
                break;
            case 425:
                // Can't open data connection
                break;
            case 426:
                // Connection closed; traClientStreamfer aborted
                break;
            case 450:
                // Requested file action not taken
                break;
            case 451:
                // Requested action aborted. Local error in processing
                break;
            case 452:
                // Requested action not taken
                // IClientStreamufficient storage space in system
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
            case 530:
                // Not logged in
                break;
            case 532:
                // Need account for storing files
                break;
            case 550:
                // Requested action not taken
                break;
            case 551:
                // Requested action aborted. Page type unknown
                break;
            case 552:
                // Requested file action aborted
                // Exceeded storage allocation
                break;
            case 553:
                // Requested action not taken
                // File name not allowed
                break;
            }
            Ready = true;
        }
    }
}
