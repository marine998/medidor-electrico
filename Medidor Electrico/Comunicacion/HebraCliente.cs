﻿using MedidorModel.DAL;
using MedidorModel.DTO;
using ServidorSocketUtils;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Medidor_Electrico.Comunicacion
{
    public class HebraCliente
    {
        private ClienteCom clienteCom;
        private IMedidorDAL medidorDAL = MedidorDALArchivo.GetInstancia();

        public HebraCliente(ClienteCom clienteCom)
        {
            this.clienteCom = clienteCom;
        }
        public void Ejecutar()
        {
            try
            {
                clienteCom.Escribir("Ingrese numero medidor: ");
                string numero = clienteCom.Leer();
                clienteCom.Escribir("Ingrese Fecha: ");
                string fecha = clienteCom.Leer();
                clienteCom.Escribir("Ingrese valor consumo: ");
                string valor = clienteCom.Leer();

                Medidor medidor = new Medidor()
                {
                    NroMedidor = Convert.ToInt32(numero),
                    Fecha = DateTime.Parse(fecha),
                    ValorConsumo = Convert.ToDecimal(valor)
                };

                lock (medidorDAL)
                {
                    medidorDAL.AgregarMedidor(medidor);
                }
                clienteCom.Escribir("Datos Ingresados con exito");
                clienteCom.Desconectar();
            }
            catch
            {
                clienteCom.Escribir("Datos ingresado erroneos");
                clienteCom.Desconectar();
            }
        }
    }
}
