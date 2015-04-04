﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using WebLocaliza.ClienteService;

namespace WebLocaliza.Paginas
{
    public partial class Cliente : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {

        }

        protected void btnguardar_Click(object sender, EventArgs e)
        {

            ClienteSVCClient proxy=new ClienteSVCClient();
            bool valida = proxy.validaDni(txtdni.Text);

            if (valida == true)
            {
                bool validadni = proxy.encuentraDni(txtdni.Text);

                if (validadni == false)
                {
                    proxy.CrearCliente(txtnombre.Text, txtap.Text, txtam.Text, cbogenero.SelectedValue, txtdni.Text,
                                       txtemail.Text, txtfono.Text, txtdireccion.Text, cbodistrito.SelectedItem.ToString());
                    Label5.Text = "Cliente Registrado";
                }
                else
                {
                    Label5.Text = proxy.mensajes(2);
                }
            }
            else
            {
                Label5.Text = proxy.mensajes(1);
            }

           
        }
    }
}