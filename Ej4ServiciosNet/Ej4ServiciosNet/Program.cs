﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Ej4ServiciosNet
{
    class Program
    {
        static void Main(string[] args)
        {
            
            ServidorArchivos sa = new ServidorArchivos();
            sa.iniciaServidorArchivos();
            Console.ReadLine();

        }
    }
}
