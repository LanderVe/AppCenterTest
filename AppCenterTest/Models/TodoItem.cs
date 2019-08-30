using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using Android.App;
using Android.Content;
using Android.OS;
using Android.Runtime;
using Android.Views;
using Android.Widget;

namespace AppCenterTest.Models
{
  class TodoItem
  {
    public Guid Id { get; set; } = Guid.NewGuid(); //generates random id
    public string Text { get; set; }
  }
}