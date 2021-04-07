using RAGE;
using System;
using System.Collections.Generic;
using System.Text;

namespace cs_packages.Interface
{
    class VuiStore 
    { 

        public static string Modal
        {
            get { return Modal; }
            set
            {
                Modal = value;
                VuiStore.Dispatch("VuiData/setModal", Modal);
            }
        }

        public static void Dispatch(string store, string data) { 
            Vui.index.ExecuteJs("Vui.$store.dispatch('" + store + "', '" + data + "');");
            Chat.Output("Vui.$store.dispatch('" + store + "', '" + data + "');");
        }
    }
}
