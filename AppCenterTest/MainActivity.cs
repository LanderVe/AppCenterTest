using System;
using System.Linq;
using System.Collections.Generic;
using Android.App;
using Android.OS;
using Android.Runtime;
using Android.Support.Design.Widget;
using Android.Support.V7.App;
using Android.Views;
using Android.Widget;
using Microsoft.AppCenter;
using Microsoft.AppCenter.Analytics;
using Microsoft.AppCenter.Crashes;
using Microsoft.AppCenter.Auth;
using Microsoft.AppCenter.Push;
using Microsoft.AppCenter.Data;
using Xamarin.Essentials;
using AppCenterTest.Models;
using Microsoft.AppCenter.Distribute;

namespace AppCenterTest
{
  [Activity(Label = "@string/app_name", Theme = "@style/AppTheme.NoActionBar", MainLauncher = true)]
  public class MainActivity : AppCompatActivity
  {
    const string APP_SECRET = "00000000-0000-0000-0000-000000000000"; //also in manifest file
    UserInformation userInfo;

    protected override void OnCreate(Bundle savedInstanceState)
    {
      base.OnCreate(savedInstanceState);
      Xamarin.Essentials.Platform.Init(this, savedInstanceState);

      SetContentView(Resource.Layout.activity_main);

      Android.Support.V7.Widget.Toolbar toolbar = FindViewById<Android.Support.V7.Widget.Toolbar>(Resource.Id.toolbar);
      SetSupportActionBar(toolbar);

      FloatingActionButton fab = FindViewById<FloatingActionButton>(Resource.Id.fab);
      fab.Click += FabOnClick;

      Button crash = FindViewById<Button>(Resource.Id.crash);
      crash.Click += CrashOnClick;

      Button signin = FindViewById<Button>(Resource.Id.signin);
      signin.Click += SignInOnClick;

      Button signout = FindViewById<Button>(Resource.Id.signout);
      signout.Click += SignOutOnClick;

      Button data = FindViewById<Button>(Resource.Id.data);
      data.Click += DataOnClick;

      ReceivePush();

      AppCenter.Start(APP_SECRET, typeof(Analytics), typeof(Crashes), typeof(Auth), typeof(Data), typeof(Push), typeof(Distribute));
      AppCenter.LogLevel = LogLevel.Verbose;
    }

    private void ReceivePush()
    {
      if (!AppCenter.Configured)
      {
        Push.PushNotificationReceived += (sender, e) =>
        {
          // Add the notification message and title to the message
          var summary = $"{e.Title}\n{e.Message}";

          // If there is custom data associated with the notification
          if (e.CustomData != null)
          {
            foreach (var key in e.CustomData.Keys)
            {
              summary += $"\t{key}: {e.CustomData[key]}\n";
            }
          }

          View content = FindViewById(Resource.Id.content);
          Snackbar.Make(content, summary, Snackbar.LengthLong).Show();
        };
      }
    }

    private void FabOnClick(object sender, EventArgs eventArgs)
    {
      Analytics.TrackEvent("file opened", new Dictionary<string, string> {
        ["Category"] = "Music",
        ["FileName"] = "favorite.avi"
      });

      View view = (View)sender;
      Snackbar.Make(view, "Track Event Sent", Snackbar.LengthLong).Show();
    }

    private void CrashOnClick(object sender, EventArgs e)
    {
      try
      {
        var a = 0;
        var b = 3 / a;
      }
      catch (Exception ex)
      {
        Crashes.TrackError(ex, new Dictionary<string, string> {
          ["Category"] = "Arithmetic"
        });
      }

      Crashes.GenerateTestCrash();
    }

    private async void SignInOnClick(object sender, EventArgs e)
    {
      try
      {
        // Sign-in succeeded.
        userInfo = await Auth.SignInAsync();
      }
      catch (Exception ex)
      {
        // Do something with sign-in failure.
      }
    }

    private void SignOutOnClick(object sender, EventArgs e) => Auth.SignOut();

    public override bool OnCreateOptionsMenu(IMenu menu)
    {
      MenuInflater.Inflate(Resource.Menu.menu_main, menu);
      return true;
    }

    //sign in before using this method
    private async void DataOnClick(object sender, EventArgs e)
    {
      var todo = new TodoItem { Text = "Make better UI for todos" };
      await Data.CreateAsync(todo.Id.ToString(), todo, DefaultPartitions.UserDocuments, new WriteOptions(TimeSpan.FromDays(1)));
      var allTodoPages = await Data.ListAsync<TodoItem>(DefaultPartitions.UserDocuments);
      var allTodos = allTodoPages.CurrentPage.Items;

      View view = (View)sender;
      Snackbar.Make(view, $"Todos: {allTodos.Count}", Snackbar.LengthShort).Show();
    }

    public override bool OnOptionsItemSelected(IMenuItem item)
    {
      int id = item.ItemId;
      if (id == Resource.Id.action_settings)
      {
        return true;
      }

      return base.OnOptionsItemSelected(item);
    }

    public override void OnRequestPermissionsResult(int requestCode, string[] permissions, [GeneratedEnum] Android.Content.PM.Permission[] grantResults)
    {
      Platform.OnRequestPermissionsResult(requestCode, permissions, grantResults);
      base.OnRequestPermissionsResult(requestCode, permissions, grantResults);
    }
  }
}

