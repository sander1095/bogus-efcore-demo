using System;
using System.Text.Json;
using static System.Console;

namespace BogusBasics;

internal static class ExtensionsForConsole
{
   public static void Dump(this User usr)
   {
      WriteLine($"  Id: {usr.Id}");
      WriteLine($"  IsActive: {usr.IsActive}");
      WriteLine($"  DateCreated: {usr.DateCreated}");
      WriteLine($"  FirstName: {usr.FirstName}");
      WriteLine($"  LastName: {usr.LastName}");
      WriteLine($"  Email: {usr.Email}");

   }
}
