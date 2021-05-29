using MedidorModel.DTO;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MedidorModel.DAL
{
    public class MedidorDALArchivo : IMedidorDAL
    {
        private MedidorDALArchivo()
        {

        }

        private static MedidorDALArchivo instancia;

        public static IMedidorDAL GetInstancia()
        {
            if (instancia == null)
            {
                instancia = new MedidorDALArchivo();
            }
            return instancia;

        }

        private static string url = Directory.GetCurrentDirectory();
        private static string archivo = url + "/lectura.txt";


        public void AgregarMedidor(Medidor medidor)
        {
            try
            {
                using (StreamWriter writer = new StreamWriter(archivo, true))
                {
                    writer.WriteLine(medidor.NroMedidor + " | " + medidor.Fecha + " | " + medidor.ValorConsumo);
                    writer.Flush();
                }
            }
            catch (Exception ex)
            {
                
            }
        }

        public List<Medidor> ObtenerMedidor()
        {
            List<Medidor> lista = new List<Medidor>();
            try
            {
                using (StreamReader reader = new StreamReader(archivo))
                {
                    string texto = "";
                    do
                    {
                        texto = reader.ReadLine();
                        if (texto != null)
                        {
                            string[] arr = texto.Trim().Split('|');
                            Medidor medidor = new Medidor();
                            string nmr = Convert.ToString(medidor.NroMedidor);
                            string valor = Convert.ToString(medidor.ValorConsumo);
                            nmr = arr[0];
                            medidor.Fecha = arr[1];
                            valor = arr[2];

                            lista.Add(medidor);
                        }
                    } while (texto != null);
                }
            }
            catch (Exception ex)
            {
                lista = null;
            }
            return lista;
        }
    }
}
