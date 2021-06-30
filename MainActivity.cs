using System;
using Android.App;
using Android.Gms.Tasks;
using Android.OS;
using Android.Support.V7.App;
using Android.Widget;
using Firebase;
using Firebase.Firestore;
using Java.Util;

namespace firestoreArray
{
    [Activity(Label = "@string/app_name", Theme = "@style/AppTheme", MainLauncher = true)]
    public class MainActivity : AppCompatActivity, IOnSuccessListener
    {
        Button saveButton, loadOrganizationButton;
        TextView resultTextView;

        protected override void OnCreate(Bundle savedInstanceState)
        {
            base.OnCreate(savedInstanceState);

            // Set our view from the "main" layout resource
            SetContentView(Resource.Layout.activity_main);
            saveButton = (Button)FindViewById(Resource.Id.saveButton);
            loadOrganizationButton = (Button)FindViewById(Resource.Id.LoadOrganizationButton);
            saveButton.Click += SaveButton_Click;
            loadOrganizationButton.Click += LoadOrganizationButton_Click;
            resultTextView = (TextView)FindViewById(Resource.Id.resultTextView);
        }

        private void LoadOrganizationButton_Click(object sender, EventArgs e)
        {
            FecthOrganization();
        }

        private void FecthOrganization()
        {
            GetFireStore().Collection("organizations").Get().AddOnSuccessListener(this);
        }

        private void SaveButton_Click(object sender, System.EventArgs e)
        {
            HashMap phoneMap = new HashMap();
            phoneMap.Put("mobile", "+16373645383");
            phoneMap.Put("work", "+236894948");
            phoneMap.Put("home", "+3525736523");

            HashMap map = new HashMap();
            map.Put("full_name", "Carlos Henrique");
            map.Put("email", "carlos_santos@trimble.com");
            map.Put("phone_number", phoneMap);

            FirebaseFirestore database;
            database = GetFireStore();

            DocumentReference docRef = database.Collection("Users").Document();
            docRef.Set(map);

            resultTextView.Text = "It was good my Brother, check my data in the firestore!";
        }

        public FirebaseFirestore GetFireStore()
        {
            var app = FirebaseApp.InitializeApp(this);
            FirebaseFirestore database;
            if (app == null)
            {
                var options = new FirebaseOptions.Builder()
                    .SetProjectId("eld-efs-sandbox-5576df8f")
                    .SetApplicationId("1:784039552683:android:c369a46e59d6673816f9ee")
                    .SetApiKey("AIzaSyCELmCmkp8etxQ-jtm7AR0rsXZvF_YdCVE")
                    .SetDatabaseUrl("https://eld-efs-sandbox-5576df8f.firebaseio.com")
                    .SetStorageBucket("eld-efs-sandbox-5576df8f.appspot.com")
                    .Build();

                app = FirebaseApp.InitializeApp(Application.Context, options);
                database = FirebaseFirestore.GetInstance(app);
            }
            else
            {
                database = FirebaseFirestore.GetInstance(app);
            }
            return database;
        }

        public void OnSuccess(Java.Lang.Object result)
        {
            var snapshot = (QuerySnapshot)result;

            if (!snapshot.IsEmpty)
            {
                var documents = snapshot.Documents;
                string resultString = string.Empty;
                foreach (var item in documents)
                {
                    // here you can retrieve all data from organizations document.
                    resultString += $"organizationKey: {item.Get("organizationKey")}, companyName: {item.Get("companyName")} "; 
                }

                resultTextView.Text = resultString;
            }
        }
    }
}