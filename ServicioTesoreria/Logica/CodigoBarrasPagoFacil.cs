using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace ServicioTesoreria.Logica
{
    public class CodigoBarrasPagoFacil
    {

        private string empresaServicio;
        private decimal monto1erVto;
        private DateTime fecha1erVto;
        private string numeroCuenta;
        private decimal monto2doVto;
        private DateTime fecha2doVto;
        private int digitoVerificador;

        public CodigoBarrasPagoFacil(string empresaServicio, decimal monto1erVto, DateTime fecha1erVto, string numeroCuenta, decimal monto2doVto, DateTime fecha2doVto)
        {
            setEmpresaServicio(empresaServicio);
            setMonto1erVto(monto1erVto);
            setFecha1erVto(fecha1erVto);
            setNumeroCuenta(numeroCuenta);
            setMonto2doVto(monto2doVto);
            setFecha2doVto(fecha2doVto);
            calcularDigitoVerificador();
        }

        public int getDigitoVerificador()
        {
            return digitoVerificador;
        }

        public string getEmpresaServicio()
        {
            return empresaServicio;
        }

        private void setEmpresaServicio(string empresaServivicio)
        {
            if (empresaServivicio.Length > 8)
            {
                throw new Exception("Identificacion de empresa de servicio demasiado larga");
            }
            else if (!empresaServivicio.Substring(0, 4).Equals("0937"))
            {
                throw new Exception("La identificacion de empresa de servicio no comienza con 0937");
            }
            this.empresaServicio = empresaServivicio;
        }

        public DateTime getFecha1erVto()
        {
            return fecha1erVto;
        }

        private void setFecha1erVto(DateTime fecha1erVto)
        {
            this.fecha1erVto = fecha1erVto;
        }

        public DateTime getFecha2doVto()
        {
            return fecha2doVto;
        }

        private void setFecha2doVto(DateTime fecha2doVto)
        {
            if (fecha2doVto != null)
            {
                this.fecha2doVto = fecha2doVto;
            }
            else
            {
                this.fecha2doVto = this.fecha1erVto;
            }
        }

        public decimal getMonto1erVto()
        {
            return monto1erVto;
        }

        private void setMonto1erVto(decimal monto1erVto)
        {
            this.monto1erVto = monto1erVto;
        }

        public decimal getMonto2doVto()
        {
            return monto2doVto;
        }

        private void setMonto2doVto(decimal? monto2doVto = null)
        {
            if (monto2doVto != null && monto2doVto.HasValue)
            {
                this.monto2doVto = monto2doVto.Value;
            }
            else
            {
                this.monto2doVto = this.monto1erVto;
            }
        }

        public String getNumeroCuenta()
        {
            return numeroCuenta;
        }

        private void setNumeroCuenta(string numeroCuenta)
        {
            if (numeroCuenta.Length > 9)
            {
                throw new Exception("Identificacion de numero de cuenta demasiado larga");
            }
            this.numeroCuenta = numeroCuenta;
        }

        private void calcularDigitoVerificador()
        {
            this.digitoVerificador = 0;
            int suma = 0;
            String secuenciaVerificadora = "135793579357935793579357935793579357935";
            String cadenaAVerificar = this.getCodigoDeBarras();

            for (int i = 0; i <= 38; i++)
            {
                suma = suma + (int.Parse(cadenaAVerificar.Substring(i, 1)) * int.Parse(secuenciaVerificadora.Substring(i, 1)));
            }

            suma = suma / 2;

            this.digitoVerificador = suma % 10;

        }

        public string getCodigoDeBarras()
        {
            string barcode = "";

            barcode += formatCadenaPagoFacil(empresaServicio, 8);
            //Para aquellos cursos en los que el costo de una sola cuota supera los 5 dígitos (>10000)
            if (monto1erVto > 10000)
                barcode += formatImportePagoFacil(monto1erVto, 5);
            else
                barcode += formatImportePagoFacil(monto1erVto, 4);
            //
            barcode += formatFechaPagoFacil(fecha1erVto);
            barcode += formatCadenaPagoFacil(numeroCuenta, 9);

            //Para aquellos cursos en los que el costo de una sola cuota supera los 5 dígitos (>10000)
            if (monto2doVto > 10000)
                barcode += formatImportePagoFacil(monto2doVto, 5);
            else
                barcode += formatImportePagoFacil(monto2doVto, 4);
            //
            barcode += formatFechaPagoFacil(fecha2doVto);
            barcode += digitoVerificador.ToString();


            if (barcode.Length > 42)
            {
                throw new Exception("El largo de la cadena es incorrecto.");
            }

            return barcode;
        }

        private String formatImportePagoFacil(decimal? importe, int largoEntero)
        {
            if (importe != null)
            {
                string auxiliar = Decimal.Round(importe.Value, 2, MidpointRounding.AwayFromZero).ToString();//String.Format("%1$.2f", importe);
                while (auxiliar.Substring(0, auxiliar.IndexOf(",")).Length < largoEntero)
                {
                    auxiliar = "0" + auxiliar;
                }
                auxiliar = auxiliar.Substring(0, auxiliar.IndexOf(",")) + auxiliar.Substring(auxiliar.IndexOf(",") + 1, 2);
                return auxiliar;
            }
            else
            {
                String cadena = "";
                while (cadena.Length < largoEntero + 2)
                {
                    cadena = "0" + cadena;
                }
                return cadena;
            }
        }

        private String formatCadenaPagoFacil(String cadena, int largoCadena)
        {
            while (cadena.Length < largoCadena)
            {
                cadena = "0" + cadena;
            }
            return cadena;
        }

        private String formatFechaPagoFacil(DateTime fecha)
        {
            //int largoCadena = 5;
            String cadena = "";
            //if (fecha != null)
            //{
            //    cadena = String.Format("%1$ty%1$tj", fecha);
            //}
            //while (cadena.Length < largoCadena)
            //{
            //    cadena = "0" + cadena;
            //}
            cadena = fecha.ToString("yy") + formatCadenaPagoFacil(fecha.DayOfYear.ToString(), 3);
            return cadena;
        }
    }

}
