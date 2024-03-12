using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Xml.Schema;

namespace network_config
{
    internal class Program
    {
        static void Main(string[] args)
        {
            Subneting subneting = new Subneting();
            subnettingMenu(subneting);

            
        }

        public static void subnettingMenu(Subneting subneting)
        {
            while (true)
            {
                Console.Write("     IP Adress   : ");
                string ipAdress = Console.ReadLine();
                Console.Write("     Network Bit : ");
                int bit = Convert.ToInt32(Console.ReadLine());
                Console.WriteLine("     Enter how many host you want in each subnet(exit 0) ");
                List<int> ints = new List<int>();
                int host = -1;
                while (host != 0)
                {
                    Console.Write("host : ");
                    host = Convert.ToInt32(Console.ReadLine());
                    if (host != 0)
                    {
                        ints.Add(host);
                    }
                    
                }
                subneting.setMainIPAddressArr(ipAdress, bit);
                subneting.setWantSubNetArr(ints);
                subneting.printSubNets();
                ConsoleKeyInfo consoleKeyInfo = Console.ReadKey();
                Console.Clear();
                if (consoleKeyInfo.Key==ConsoleKey.Backspace)
                {
                    break;
                }

            }

        }

    }

    class Subneting
    {
        int[] mainIpAddress = new int[4];
        int networkbitLen;
        List<int> wantSubNet;
        int[,] devidedSubNetDetails;

        public void printSubNets()
        {
            string ip = null;
            for (int i = 0; i < 3; i++)
            {
                ip = ip + Convert.ToString(mainIpAddress[i]) + ".";
            }

            for (int i = 0; i < devidedSubNetDetails.Length/7-1; i++)
            {
                Console.WriteLine("Network Address : " +ip + devidedSubNetDetails[i, 0] + "/" + devidedSubNetDetails[i,6]);
                Console.WriteLine("subnet mask     : " +"255.255.255." + devidedSubNetDetails[i, 1]);
                Console.WriteLine("brodcast address: " +ip + devidedSubNetDetails[i, 2]);
                Console.WriteLine("Range           : " +ip + devidedSubNetDetails[i,3]+" - "+ip + devidedSubNetDetails[i,4]);
                Console.WriteLine("Dedault gateway : " +ip + devidedSubNetDetails[i,5]);
                Console.WriteLine("-----------------------------------------------------------\n");
            }
        }

        //set default gatway for each subnet
        private void setDG()
        {
            for (int i = 0; i < devidedSubNetDetails.Length/7-1; i++)
            {
                devidedSubNetDetails[i, 5] = devidedSubNetDetails[i, 4];
            }
        }

        //set broadcast Address for each subnet
        private void setBA()
        {
            for (int i = 0; i < devidedSubNetDetails.Length/7-1; i++)
            {
                devidedSubNetDetails[i, 2] = devidedSubNetDetails[i + 1, 0]-1;
            }
        }

        //genarage usable ip range for each subnet
        private void setRange()
        {
            for (int i = 0; i < devidedSubNetDetails.Length/7-1; i++)
            {
                devidedSubNetDetails[i, 3] = devidedSubNetDetails[i,0] + 1;
                devidedSubNetDetails[i, 4] = devidedSubNetDetails[i,2] - 1;
            }
        }

        //set subnet mask foe each subnet
        private void setSubnetMask()
        {
            for (int i = 0; i < wantSubNet.Count; i++)
            {
                int nearestNum = getnearestNumber(wantSubNet[i]);
                int[] subnetData = getSubnetMask(nearestNum);
                devidedSubNetDetails[i, 1] = subnetData[0];
                devidedSubNetDetails[i, 6] = subnetData[1];
            }
            
        }

        //set Network address of each subnet in devidedSubNetDetails arr
        private void setNA()
        {
            int currentIp = mainIpAddress[networkbitLen/8];
            devidedSubNetDetails[0,0]=currentIp;
            for (int i = 1; i < devidedSubNetDetails.Length/7; i++)
            {
                int nearestNum = getnearestNumber(wantSubNet[i-1]);
                devidedSubNetDetails[i,0] = currentIp + nearestNum;
                currentIp = devidedSubNetDetails[i, 0];

            }
        }
        //get number of host computers of each subnet and set it to wantsubNet arr
        //set size of subnet details arr
        public void setWantSubNetArr(List<int> wantSubNet)
        {
            this.wantSubNet = wantSubNet;
            int wlength = wantSubNet.Count;
            devidedSubNetDetails = new int[wlength+1, 7];
            setNA();
            setSubnetMask();
            setBA();
            setRange();
            setDG();
        }

        //get ip address and split by '.' and store mainIpAddress Arr
        public void setMainIPAddressArr(String IP,int networkbitLen)
        {
            this.networkbitLen = networkbitLen;
            string[] ip = IP.Split('.');
            int i = 0;
            foreach (var item in ip)
            {
                mainIpAddress[i] = Convert.ToInt32(item);
                i++;
            }
        }

        //return neares power of 2 to number
        private int getnearestNumber(int number)
        {
            int num = 0;
            if (number>64)
            {
                num = 128;
            }
            else if (number>32)
            {
                num = 64;
            }
            else if(number>16)
            {
                num = 32;
            }
            else if (number > 8)
            {
                num = 16;
            }
            else if (number > 4)
            {
                num = 8;
            }
            else if (number > 2)
            {
                num = 4;
            }
            else
            {
                num = 2;
            }
            return num;
        }

        //return subnet mask value for last okted based on nearest power of 2 value
        private int[] getSubnetMask(int num)
        {
            int[] dataarr = new int[2];
            switch (num)
            {
                case 128:
                    dataarr[0] = 128;
                    dataarr[1] = 25;
                    break;
                case 64:
                    dataarr[0] = 192;
                    dataarr[1] = 26;
                    break;
                case 32:
                    dataarr[0] = 224;
                    dataarr[1] = 27;
                    break ;
                case 16:
                    dataarr[0] = 240;
                    dataarr[1] = 28;
                    break ;
                case 8:
                    dataarr[0] = 248;
                    dataarr[1] = 29;
                    break ;
                case 4:
                    dataarr[0] = 252;
                    dataarr[1] = 30;
                    break ;
                case 2:
                    dataarr[0] = 254;
                    dataarr[1] = 31;
                    break ;
                case 1:
                    dataarr[0] = 255;
                    dataarr[1] = 32;
                    break ;
            }
            return dataarr;
        }
    }

}

