using System;
using System.IO;
using System.Linq;
using System.Net;
using System.Reflection;
using System.Text;
using BotwTrainer.Properties;

namespace BotwTrainer
{
   public class WebResourceFetcher: WebClient
   {
      public string Method { get; set; }
      public string FileName { get; private set; }
      public Uri Uri { get; private set; }

      public WebResourceFetcher(string name)
      {
         FileName = name;
         Uri = new Uri(string.Format("{0}{1}", Settings.Default.GitUrl, FileName));

         Encoding = Encoding.UTF8;
         CachePolicy = new System.Net.Cache.RequestCachePolicy(System.Net.Cache.RequestCacheLevel.BypassCache);
         Headers.Add("Cache-Control", "no-cache");
      }

      protected override WebRequest GetWebRequest(Uri address)
      {
         WebRequest webRequest = base.GetWebRequest(address);

         if (!string.IsNullOrWhiteSpace(Method))
            webRequest.Method = Method;

         return webRequest;
      }

      public StringReader Contents()
      {
         return new StringReader(DownloadString(this.Uri));
      }

      public bool Exists
      {
         get {
            var oldMethod = Method;
            try {
               Method = "HEAD";
               using (HttpWebResponse response = GetWebRequest(this.Uri).GetResponse() as HttpWebResponse)
               {
                  return response.StatusCode == HttpStatusCode.OK;
               }
            } catch (WebException) {
               return false;
            } finally {
               Method = oldMethod;
            }
         }
      }
   }

   class ResourceDataFile
   {
      private Assembly Assembly { get { return Assembly.GetExecutingAssembly(); } }

      private string name;
      private string EmbeddedPath { get; set; }
      private string ExecutingPath { get; set; }

      public bool EmbeddedExists {
         get { return Assembly.GetManifestResourceNames().Contains(EmbeddedPath); }
      }

      public bool LocalExists {
         get { return File.Exists(ExecutingPath); }
      }

      public bool RemoteExists {
         get { return new WebResourceFetcher(name).Exists; }
      }

      public bool Exists
      {
         get { return (LocalExists || EmbeddedExists || RemoteExists); }
      }

      public ResourceDataFile(String name)
      {
         this.name = name;
         EmbeddedPath = string.Format("{0}.Resources.{1}", Assembly.GetName().Name, name);
         ExecutingPath = Path.Combine(Path.GetDirectoryName(new Uri(Assembly.CodeBase).LocalPath), name);

      }

      public StringReader ContentsFromEmbedded()
      {
         using (Stream stream = Assembly.GetManifestResourceStream(EmbeddedPath))
         {
            using (StreamReader reader = new StreamReader(stream))
            {
               return new StringReader(reader.ReadToEnd());
            }
         }
      }

      public StringReader ContentsFromWeb()
      {
         return new WebResourceFetcher(name).Contents();
      }

      public StringReader ContentsFromLocalPath()
      {
         return new StringReader(File.OpenText(ExecutingPath).ReadToEnd());
      }

      public StringReader Contents()
      {
         try
         {
            /*
             * Search for offset.yaml in .exe directory first, 
             * followed by embedded resource, and then online.
             */
            
            if (LocalExists) {
               return ContentsFromLocalPath();
            } else if (EmbeddedExists) {
               return ContentsFromEmbedded();
            } else {
               return ContentsFromWeb();
            }
         }
         catch (Exception ex)
         {
            throw new Exception(string.Format("Error loading {0}: {1}: {2}", name, ex.GetType().Name, ex.Message));
         }
      }


   }
}
