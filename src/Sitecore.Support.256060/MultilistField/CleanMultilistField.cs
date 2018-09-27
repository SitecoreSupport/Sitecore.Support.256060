using Sitecore.Configuration;
using Sitecore.Data.Items;
using Sitecore.Diagnostics;
using Sitecore.Events;
using Sitecore.Publishing;
using Sitecore.Data.Fields;
using System.Collections.Generic;
using System;

namespace Sitecore.Support.MultilistField
{
  /// <summary>
  /// Represents an item event handler.
  /// </summary>
  public class CleanDuplicate
  {
    /// <summary>
    /// Gets the link database.
    /// </summary>
    /// <value>The link database.</value>



    protected void OnItemSaved(object sender, EventArgs args)
    {
      if (args == null)
      {
        return;
      }
      
      Item item = Event.ExtractParameter(args, 0) as Item;
      Assert.IsNotNull(item, "No item in parameters");
      item.Editing.BeginEdit();
      foreach(Field field in item.Fields)
      {
        if(field.Type.Equals("Multilist"))
        {
          List<string> fieldVal = new List<string>(field.Value.Split('|'));
          string result = "";
          fieldVal.Sort();
          if (fieldVal.Count > 1)
          {
            int s = 0;
            int t = 1;
            int divideCount = 0;
            for (; t < fieldVal.Count; t++)
            {
              if (fieldVal[s].Equals(fieldVal[t]))
              {
                fieldVal[t] = "";
              }
              else
              {
                s = t;
                divideCount++;
              }
            }
            for (s = 0; s < fieldVal.Count; s++)
            {
              if (!fieldVal[s].Equals(String.Empty))
              {
                result = result + fieldVal[s];
                if (divideCount > 0)
                {
                  result += "|";
                  divideCount--;
                }
              }
            }



          }
          field.Value = result;
          
        }
        
      }
      item.Editing.EndEdit();
    }
  }
}