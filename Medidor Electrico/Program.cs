using MedidorModel.DAL;
using MedidorModel.DTO;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MedidorElectrico
{
    class Program
    {
        private static IMedidorDAL medidorDAL = MedidorDALArchivo.GetInstancia();

        static void Main(string[] args)
        {
            while (Menu()) ;
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
            Console.WriteLine("Ingrese Número Medidor:");
            string nmr = Console.ReadLine().Trim();
            Console.WriteLine("Ingrese Fecha:");
            string fecha = Console.ReadLine().Trim();
            Console.WriteLine("Ingrese Valor de Consumo:");
            string valor = Console.ReadLine().Trim();

            Medidor medidor = new Medidor();
            int nmrs = Convert.ToInt32(nmr);
            decimal valors = Convert.ToDecimal(valor);
            medidor.NroMedidor = nmrs;
            medidor.Fecha = fecha;
            medidor.ValorConsumo = valors;
            medidorDAL.AgregarMedidor(medidor);
        }

        private static void Mostrar()
        {
            List<Medidor> medidors = null;
            lock (medidorDAL)
            {
                medidors = medidorDAL.ObtenerMedidor();
            }
            foreach (Medidor medidor in medidors)
            {
                Console.WriteLine(medidor);
            }
        }
    }
}

