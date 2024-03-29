﻿using System;
using System.Text;
using System.Collections.Generic;
using System.Linq;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using TestProject1.ServiceCliente;
using System.Net;
using System.IO;
using System.Web.Script.Serialization;
using TestProject1.Clases;
using TestProject1.UsuarioService;
using TestProject1.ContraseñaService;
using TestProject1.UbicacionService;

namespace TestProject1
{
    /// <summary>
    /// Summary description for UnitTest1
    /// </summary>
    [TestClass]
    public class UnitTest1
    {
        [TestMethod]
        public void TestCrear()
        {
            
            ClienteSVCClient proxy = new ClienteSVCClient();

            string nombre = "Alessandra";
            string paterno = "diaz";
            string materno = "puente";
            string genero = "F";
            string dni = "123458";
            string email = "Alessandra@gmail.com";
            string telefono = "12121212";
            string direccion = "av. aviacion 123";
            string distrito = "san borja";

            bool valida = proxy.validaDni(dni);
            if (valida == true)
            {
                Cliente result = proxy.CrearCliente(nombre, paterno, materno, genero, dni, email, telefono, direccion, distrito);

                string mensaje = "Cliente " + result.nombre + " creado";

                Assert.AreEqual("Cliente Alessandra creado", mensaje);
            }
            else
            {
                string mensaje = proxy.mensajes(1); ;

                Assert.AreEqual("La cantidad de digitos debe ser 8", mensaje);
            }
           
        }
        [TestMethod]
        public void TestValidar()
        {  
            ClienteSVCClient proxy = new ClienteSVCClient();

            bool valida = proxy.validaDni("1234568");

            Assert.IsFalse(valida);
        }
        [TestMethod]
        public void TestCrearEnvio()
        {
            string postdata = "{\"IdEnvio\":\"4\",\"IdCliente\":\"1\",\"Cantidad\":\"2\",\"Peso\":\"100\",\"DestinoInicio\":\"Lima\",\"DestinoFin\":\"Trujillo\",\"IdTransporte\":\"1\",\"Estado\":\"1\"}";
            byte[] data = Encoding.UTF8.GetBytes(postdata);
            HttpWebRequest req = (HttpWebRequest)WebRequest.Create("http://localhost:31030/EnviosSVC.svc/Envios");
            req.Method = "POST";
            req.ContentLength = data.Length;
            req.ContentType = "application/json";
            var reqStream = req.GetRequestStream();
            reqStream.Write(data, 0, data.Length);

            try
            {
                var res = (HttpWebResponse)req.GetResponse();
                StreamReader reader = new StreamReader(res.GetResponseStream());
                string clientejson = reader.ReadToEnd();
                JavaScriptSerializer js = new JavaScriptSerializer();
                Envios crear = js.Deserialize<Envios>(clientejson);
                Assert.AreEqual("2", crear.IdEnvio);
            }
            catch(WebException e)
            {
                 HttpStatusCode code = ((HttpWebResponse)e.Response).StatusCode;
                string message = ((HttpWebResponse)e.Response).StatusDescription;
                StreamReader reader = new StreamReader(e.Response.GetResponseStream());
                string error = reader.ReadToEnd();
                JavaScriptSerializer js = new JavaScriptSerializer();
                string mensaje = js.Deserialize<string>(error);
                Assert.AreEqual("El Peso excede el Peso Maximo del transporte, asigne otro tranpsort", mensaje);
            }
        }

        [TestMethod]
        public void TestCrearUsuario()
        {
            UsuarioServiceClient proxy = new UsuarioServiceClient();
           
            string user= proxy.crearUsuario("12345678","Gianpiere","Morales");

            Assert.AreEqual("gmorales",user);
        }

       [TestMethod]
        public void TestContraseñaExpirada()
        {
            UsuarioServiceClient proxy = new UsuarioServiceClient();

            string user = proxy.ContraseñaExpirada("gmorales");

           Assert.AreEqual("La contraseña ha expirado", user);
        }
        
            [TestMethod]
        public void TestValidaContraseñaActual()
        {
            ContraseñaClient proxy = new ContraseñaClient();

            string user = proxy.ValidaContraseñaActual("adiaz", "pruebaTest");

            Assert.AreEqual("La contraseña actual es incorrecta", user);
        }

         [TestMethod]
        public void TestCambiarContraseña()
        {
            ContraseñaClient proxy = new ContraseñaClient();

            string user = proxy.CambContraseña("gmorales", "pruebaTest", "pruebaTest");

            Assert.AreEqual("1", user);
        }

         [TestMethod]
         public void TestRegistrarUbicacion()
         {
             UbicacionServiceClient proxy = new UbicacionServiceClient();

             Ubicacion ubi = new Ubicacion();
             ubi.idEnvio = "EM126";
             ubi.latitud = -12.1017361;
             ubi.longitud= -77.0001988;
             string user = proxy.actualizaUbicacion(ubi);

             Assert.AreEqual("EM126",user);
         }

         [TestMethod]
         public void TestObtenerUbicacion()
         {
             UbicacionServiceClient proxy = new UbicacionServiceClient();

             Ubicacion ubi = new Ubicacion();

             ubi = proxy.listaUbicacion("em124");

             Assert.AreEqual(ubi.latitud, -12.1017361);
             Assert.AreEqual(ubi.longitud, -77.0001988);
         }
             
    
    }
    
}
