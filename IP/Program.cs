using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Diagnostics;
using System.IO;
using System.Net.NetworkInformation;
using System.Text.RegularExpressions;

namespace IP
{
    class Program
    {
        static void Main(string[] args)
        {

            String pattern = @"(\d+\.){3}\d+([^\s]+)?";

            Regex regex = new Regex(pattern);

            List<String> list_ip_addresses = new List<String>();
            //Gets the machine IPs that are connected on LAN 

            Process netUtility = new Process();

            netUtility.StartInfo.FileName = "arp";

            netUtility.StartInfo.CreateNoWindow = true;

            netUtility.StartInfo.Arguments = "-a";

            netUtility.StartInfo.RedirectStandardOutput = true;

            netUtility.StartInfo.UseShellExecute = false;

            netUtility.StartInfo.RedirectStandardError = true;

            netUtility.Start();

            StreamReader streamReader = new StreamReader(netUtility.StandardOutput.BaseStream, netUtility.StandardOutput.CurrentEncoding);

            string line = "";
            list_ip_addresses.Add("192.168.1.148");
            while ((line = streamReader.ReadLine()) != null)

            {
                Match match = regex.Match(line);
                if(match.Value!="")
                list_ip_addresses.Add(match.Value);
            }
            foreach (String ip in list_ip_addresses)
            {
                Ping ping = new Ping();
               
                PingReply pingReply = ping.Send(ip);
                String time = (pingReply.Address != null) ? pingReply.RoundtripTime.ToString() + " ms" : "";
                String address = (pingReply.Address != null) ? pingReply.Address.ToString() : (ip + " failed");
                Console.WriteLine(address + " " + pingReply.Status.ToString() + " " + time);

            }

            streamReader.Close();

            Console.WriteLine("Press Enter to exit");
            Console.Read();

        }
    }
}
