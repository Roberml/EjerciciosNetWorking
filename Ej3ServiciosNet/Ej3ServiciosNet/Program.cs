using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Sockets;
using System.Text;
using System.Threading.Tasks;

namespace Ej3ServiciosNet
{
    class Program
    {
        static void Main(string[] args)
        {

            Sala s1 = new Sala();
            s1.iniciaServicioChat();
            Console.ReadKey();

            }
        }
    
}
