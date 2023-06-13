namespace BotwTrainer
{
   using System;
   using System.Collections.Generic;
   using System.IO;
   using System.Linq;
   using System.Net;
   using System.Text;

   using YamlDotNet.Serialization;
   using YamlDotNet.Serialization.NamingConventions;
   using BotwTrainer.Properties;

   public class CodeHandler
   {
      public uint Start { get; set; }
      public uint End { get; set; }
      public uint Enabled { get; set; }
   }

   public class BotwVersion : IComparable
   {
      protected System.Version _version { get; set; }

      private string version;
      public string Version {
         get { return version; }
         set {
            version = value;
            _version = new System.Version(value);
         }
      }

      public uint Start { get; set; }
      public uint End { get; set; }
      public uint Count { get; set; }

      public int CompareTo(Object obj)
      {
         if (obj is BotwVersion)
         {
            return _version.CompareTo((obj as BotwVersion)._version);
         }
         throw new ArgumentException(string.Format("Expected a Version argument but got a {0} object", obj.GetType().Name));
      }
   }

   public class Versions 
   {
      private Dictionary<string, BotwVersion> versions;

      public Versions(List<BotwVersion> versions)
      {
         this.versions = versions.OrderByDescending(v => v.Version).ToDictionary(v => v.Version, v => v);
      }

      public BotwVersion this[string key]
      {
         get { return this.versions[key]; }
      }

      public BotwVersion newest()
      {
         return this.versions.Values.First();
      }

      public List<string> Keys()
      {
         return this.versions.Keys.ToList();
      }
   }

   public class Offsets
   {
      private const string dataFile = @"offsets.yaml";

      public CodeHandler CodeHandler { get; set; }

      public Versions versions { get; private set; }
      private List<BotwVersion> _versions;
      public List<BotwVersion> Versions {
         get { return _versions; }
         set {
            _versions = value;
            versions = new BotwTrainer.Versions(value);
         }
      }
      
      public static Offsets LoadOffsets()
      {
         try
         {
            using (StringReader data = new ResourceDataFile(dataFile).Contents())
            {
               var ds = new DeserializerBuilder().Build();
               return ds.Deserialize<Offsets>(data);
            }
         }
         catch (Exception ex)
         {
            throw new Exception(@"Error loading offsets", ex.InnerException == null ? ex : ex.InnerException);
         }
      }
   }
}

