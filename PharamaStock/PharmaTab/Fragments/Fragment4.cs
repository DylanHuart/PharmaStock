using Android.App;
using Android.Media;
using Android.OS;
using Android.Support.Design.Widget;
using Android.Views;
using Android.Widget;
using System;
using System.Threading.Tasks;
using ZXing.Mobile;

namespace PharmaTab.Fragments
{
    public class Fragment4 : Android.Support.V4.App.Fragment
    {
        public override void OnCreate(Bundle savedInstanceState)
        {
            base.OnCreate(savedInstanceState);

            // Create your fragment here
        }

        public static Fragment4 NewInstance()
        {
            var frag4 = new Fragment4 { Arguments = new Bundle() };
            return frag4;
        }
        public override View OnCreateView(LayoutInflater inflater, ViewGroup container, Bundle savedInstanceState)
        {
            var ignored = base.OnCreateView(inflater, container, savedInstanceState);
            var view = inflater.Inflate(Resource.Layout.fragment4, null);

            //Chemin du fichier xml
            string path = Android.OS.Environment.ExternalStorageDirectory + Java.IO.File.Separator + "PharmatrackXML" + Java.IO.File.Separator+ "Config.xml";

            Button modif = view.FindViewById<Button>(Resource.Id.btnModif);
            Button supp = view.FindViewById<Button>(Resource.Id.btnSuppr);
            EditText pwd = view.FindViewById<EditText>(Resource.Id.idpwd);
            Spinner list = view.FindViewById<Spinner>(Resource.Id.spinner1);
            ImageButton scan = view.FindViewById<ImageButton>(Resource.Id.btnscan15);
            var tabs = Activity.FindViewById<TabLayout>(Resource.Id.tabs);

            scan.Click += async (s, e) =>
            {
                pwd.Text = await Scan();
            };
            //Contenu des balises user du fichier xml chargé dans l'adapter 
            var users = XML.ListeUser(path);
            ArrayAdapter<string> adapter = new ArrayAdapter<string>(Application.Context, Android.Resource.Layout.SimpleListItemActivated1, users);
            list.Adapter = adapter;

            tabs.TabSelected += (s, e) =>
            {
                var tab = e.Tab;
                var text = tab.Text;
                if (text == "Modifier")
                {
                    Refresh();
                }
            };
                //Rafraîchit la listview
                void Refresh()
            {
                users.Clear();
                adapter.Clear();
                users = XML.ListeUser(path);
                adapter.AddAll(users);
                list.Adapter = adapter;
            }

            //Bouton modifier
            modif.Click += (s, e) =>
            {
                try
                {
                    var positem = list.SelectedItemPosition;
                    var item = adapter.GetItem(positem).ToString(); 
                    XML.EcritXml(path,item,pwd.Text);
                    Toast.MakeText(Application.Context, "Mot de passe modifié avec succès", ToastLength.Long).Show();
                    Refresh();
                }
                catch (Exception ex)
                {
                    Toast.MakeText(Application.Context, ex.Message, ToastLength.Long).Show();
                }
            };

            //Bouton supprimer avec alerte
            supp.Click += (s, e) =>
            {
                try
                {
                    var positem = list.SelectedItemPosition;
                    var item = adapter.GetItem(positem).ToString();
                    AlertDialog.Builder alert = new AlertDialog.Builder(this.Activity);
                    alert.SetTitle("Suppression");
                    alert.SetMessage("Voulez-vous vraiment supprimer l'utilisateur " + item + "?");
                    alert.SetPositiveButton("Supprimer", (senderAlert, args) =>
                    {
                        XML.DeleteXml(path, item);
                        Toast.MakeText(Application.Context, "Utilisateur supprimé avec succès", ToastLength.Long).Show();
                        Refresh();
                    });

                    alert.SetNegativeButton("Annuler", (senderAlert, args) =>
                    {
                        Toast.MakeText(Application.Context, "Annulé !", ToastLength.Short).Show();
                    });

                    Dialog dialog = alert.Create();
                    dialog.Show();
                }
                catch (Exception ex)
                {
                    Toast.MakeText(Application.Context, ex.Message, ToastLength.Long).Show();

                }
            };






            return view;
        }

        async Task<string> Scan()
        {
            MobileBarcodeScanner scanner;
            MobileBarcodeScanner.Initialize(Activity.Application);

            var options = new MobileBarcodeScanningOptions
            {
                AutoRotate = false,
                UseFrontCameraIfAvailable = false,
                DelayBetweenContinuousScans = 1500,
            };

            scanner = new MobileBarcodeScanner()
            {
                TopText = "Mot de passe"
            };

            var result = await scanner.Scan(options);
            Android.Media.Stream str = Android.Media.Stream.Music;
            ToneGenerator tg = new ToneGenerator(str, 100);
            tg.StartTone(Tone.PropAck);

            if (result == null)
            {
                return "";
            }

            return result.Text;
        }
    }
}