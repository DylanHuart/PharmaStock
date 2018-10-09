using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Xml;
using System.Xml.Linq;
using Android.App;
using Android.Content;
using Android.OS;
using Android.Runtime;
using Android.Views;
using Android.Widget;

namespace PharmaTab
{
    class XML
    {
        private static XmlDocument xdoc = new XmlDocument();
        public static void EcritXml(string path, string Username, string Password)
        {

                xdoc.Load(path); //emplacement de l'objet

                var username = xdoc.SelectSingleNode("//root/User" + Username);

                if (Cryptage.ProtectPassword(Password) == username.InnerText) return;

                username.InnerText = Cryptage.ProtectPassword(Password);

                //MessageBox.Show("Vous avez changé les paramètres de connexion");
                //sauvegarde
                xdoc.Save(path);
            
        }


        public static bool LitXml(string path, string Username, string Password)
        {
            xdoc.Load(path); //emplacement de l'objet

            var username = xdoc.SelectSingleNode("//root/User" + Username);

            if (Password == Cryptage.UnprotectPassword(username.InnerText))
            {
                return true;
            }

            return false;
        }


        public static List<string> ListeUser(string path)
        {
            XDocument cpo = XDocument.Load(path);

            List<string> listenoeuds = new List<string>();

            foreach (var name in cpo.Root.DescendantNodes().OfType<XElement>()
        .Select(x => x.Name).Distinct())
            {
                listenoeuds.Add(name.ToString().Substring(4));
            }


            return listenoeuds;
        }

        public static void CreateUser(string path, string matricule, string mdp)
        {
            if (File.Exists(Android.OS.Environment.ExternalStorageDirectory + Java.IO.File.Separator + "PharmastockXML" + Java.IO.File.Separator + "Config.xml"))
            {
                xdoc.Load(path);
                XmlNode rootNode = xdoc.SelectSingleNode("//root");

                XmlNode userNode = xdoc.CreateElement("User" + matricule);
                userNode.InnerText = Cryptage.ProtectPassword(mdp);
                rootNode.AppendChild(userNode);

                xdoc.Save(path);
            }
        }
        public static void CreateXml(string path)
        {
            XmlNode docNode = xdoc.CreateXmlDeclaration("1.0", "UTF-8", null);
            xdoc.AppendChild(docNode);

            XmlNode rootNode = xdoc.CreateElement("root");
            xdoc.AppendChild(rootNode);

            XmlNode userNode = xdoc.CreateElement("Useradmin");
            userNode.InnerText = Cryptage.ProtectPassword("admin");
            rootNode.AppendChild(userNode);

            xdoc.Save(path);

        }

        public static void DeleteXml(string path, string Username)
        {
            xdoc.Load(path);
            XmlNode rootNode = xdoc.SelectSingleNode("//root");
            XmlNode userNode = xdoc.SelectSingleNode("//root/User" + Username);
            rootNode.RemoveChild(userNode);
            xdoc.Save(path);
        }
    }
}