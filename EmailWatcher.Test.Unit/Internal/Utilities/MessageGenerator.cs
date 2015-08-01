using System;
using System.IO;
using OpenPop.Mime;

namespace EmailWatcher.Test.Unit.Internal.Utilities
{
    class MessageGenerator
    {
        private static readonly string TempEmailFilePath = AppDomain.CurrentDomain.BaseDirectory + "\\stubEmail.eml";
        
        private MessageGenerator()
        {
            
        }

        public static Message GenerateMessage(string subject, string body)
        {
            GenerateStubEmailFile(subject, body);
            return Message.Load(new FileInfo(TempEmailFilePath));
        }

        private static void GenerateStubEmailFile(string subject, string body)
        {
            String emailFileContents = String.Format(@"Date: Sun, 1 Apr 2012 14:25:25 -0600
From: noone@nowhere.com
Subject: {0}
To: someone@somewhere.com

{1}", subject, body);

            StreamWriter file = new StreamWriter(TempEmailFilePath);
            file.WriteLine(emailFileContents);

            file.Close();
        }
    }
}
