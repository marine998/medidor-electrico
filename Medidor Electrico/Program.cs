using MedidorModel.DAL;
using MedidorModel.DTO;
using ServidorSocketUtils;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Net.Sockets;
using System.Text;
using System.Threading.Tasks;

namespace MedidorElectrico
{
    class Program
    {
        private static IMedidorDAL medidorDAL = MedidorDALArchivo.GetInstancia();

        static void Main(string[] args)
        {
            IniciarServidor();
            while (Menu()) ;
        }

        static void IniciarServidor()
        {
            int puerto = Convert.ToInt32(ConfigurationManager.AppSettings["puerto"]);
            Console.WriteLine("Iniciando Servidor en puerto{0}: ", puerto);
            ServerSocket servidor = new ServerSocket(puerto);
            if (servidor.Iniciar())
            {
                Console.WriteLine("Servidor Iniciando");

                while (true)
                {
                    Console.WriteLine("Esperando cliente... ");
                    Socket cliente = servidor.ObtenerCliente();
                    Console.WriteLine("Cliente conectado");

                    ClienteCom clienteCom = new ClienteCom(cliente);
                    clienteCom.Escribir("Ingrese numero medidor: ");
                    string numero = clienteCom.Leer();
                    clienteCom.Escribir("Ingrese Fecha: ");
                    string fecha = clienteCom.Leer();
                    clienteCom.Escribir("Ingrese valor consumo: ");
                    string valor = clienteCom.Leer();

                    Medidor medidor = new Medidor()
                    {
                        NroMedidor = Convert.ToInt32(numero),
                        Fecha = Convert.ToString(fecha),
                        ValorConsumo = Convert.ToDecimal(valor)
                    };
                    medidorDAL.AgregarMedidor(medidor);
                    clienteCom.Desconectar();
                }
            }
            else
            {
                Console.WriteLine("No es posible conectar al servidor");
            }
        }
        private static bool Menu()
        {
            bool continuar = true;
            Console.WriteLine("Bienvenido");
            Console.WriteLine("1. Ingresar");
            Console.WriteLine("2. Mostrar");
            Console.WriteLine("0. Salir");

            switch (Console.ReadLine().Trim())
            {
                case "1":
                    Ingresar();
                    break;
                case "2":
                    Mostrar();
                    break;
                case "0":
                    continuar = false;
                    break;
                default:
                    Console.WriteLine("Elija bien su opcion");
                    break;
            }
            return continuar;
        }

        private static void Ingresar()
        {
            Console.WriteLine("Ingrese los Datos: ");
            string datos = Console.ReadLine().Trim();

            string[] data = datos.Split('|','|','|');

            int num = Convert.ToInt32(data[0]);
            string fecha = Convert.ToString(data[1]);
            decimal valor = Convert.ToDecimal(data[2]);

            Medidor medidor = new Medidor()
            {
                NroMedidor = num,
                Fecha = fecha,
                ValorConsumo = valor
            };

            medidorDAL.AgregarMedidor(medidor);
        }

        private static void Mostrar()
        {
            List<Medidor> medidors = medidorDAL.ObtenerMedidor();
            foreach (Medidor medidor in medidors)
            {
                Console.WriteLine(medidor);
            }
        }
    }
}

