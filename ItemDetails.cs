namespace BotwTrainer
{
   using System;
   using System.Collections.Generic;
   using System.IO;

   using YamlDotNet.Serialization;

   public class ItemDatum
   {
      public string Name { get; set; }
   }

   public class ItemTypes
   {
      public Dictionary<string, ItemDatum> this[string key] {
         get {
            try {
               return this.GetType().GetProperty(key).GetValue(this) as Dictionary<string, ItemDatum>;
            } catch (NullReferenceException) {
               return null;
            }
         }
      }

      public Dictionary<string, ItemDatum> Weapons { get; set; }
      public Dictionary<string, ItemDatum> Bows { get; set; }
      public Dictionary<string, ItemDatum> Shields { get; set; }
      public Dictionary<string, ItemDatum> Armor { get; set; }
      public Dictionary<string, ItemDatum> Arrows { get; set; }
      public Dictionary<string, ItemDatum> Materials { get; set; }
      public Dictionary<string, ItemDatum> Food { get; set; }
      public Dictionary<string, ItemDatum> KeyItems { get; set; }
   }

   public class LocationData
   {
      public string Name { get; set; }
      public float LocX { get; set; }
      public float LocY { get; set; }
      public float LocZ { get; set; }
   }

   public class ItemDetails
   {
      private const string dataFile = @"items.yaml";

      public ItemTypes Items { get; set; }
      public Dictionary<string, ItemDatum> Others { get; set; }
      public Dictionary<string, LocationData> Shrines { get; set; }
      public Dictionary<string, LocationData> Towers { get; set; }
      public Dictionary<string, LocationData> Ranches { get; set; }
      public Dictionary<string, LocationData> Misc { get; set; }

      public static ItemDetails LoadData()
      {
         try
         {
            using (StringReader data = new ResourceDataFile(dataFile).Contents()) {
               var ds = new DeserializerBuilder().Build();
               return ds.Deserialize<ItemDetails>(data);
            }
         }
         catch (Exception ex)
         {
            throw new Exception(@"Error loading item data", ex.InnerException == null ? ex : ex.InnerException);
         }
      }
   }
}

