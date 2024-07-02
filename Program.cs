using System.Text.Json;

class Program
{
    async static Task Main()
    {
        string Natives = string.Empty;

        using (HttpClient client = new HttpClient())
        {
            try
            {
                Console.WriteLine("Downloading natives...\n");
                Natives = await client.GetStringAsync("https://raw.githubusercontent.com/alloc8or/gta5-nativedb-data/master/natives.json");
            }
            catch (HttpRequestException Error)
            {
                Console.WriteLine($"Failed to download natives.json: {Error.Message}");
                return;
            }
        }

        using (JsonDocument document = JsonDocument.Parse(Natives))
        {
            JsonElement Root = document.RootElement;
            
            if (Root.ValueKind == JsonValueKind.Object)
            {
                string LongestNativeName = "Error";
                int LongestNativeLength = 1000;
                foreach (JsonProperty Namespace in Root.EnumerateObject())
                {
                    foreach (JsonProperty Native in Namespace.Value.EnumerateObject())
                    {
                        foreach (JsonProperty Data in Native.Value.EnumerateObject())
                        {
                            if (Data.Name != "name") continue;
                            string Name = Data.Value.ToString();
                            if (Name.Length > LongestNativeName.Length)
                            {
                                LongestNativeLength = Name.Length;
                                LongestNativeName = Namespace.Name + "::" + Name.ToString();
                            }
                        }
                    }
                }

                Console.WriteLine(string.Format("The longest native, with a length of {0} is:", LongestNativeLength));
                Console.WriteLine(LongestNativeName + "\n");

                string ShortestNativeName = "Error";
                int ShortestNativeLength = 1000;
                foreach (JsonProperty Namespace in Root.EnumerateObject())
                {
                    foreach (JsonProperty Native in Namespace.Value.EnumerateObject())
                    {
                        foreach (JsonProperty Data in Native.Value.EnumerateObject())
                        {
                            if (Data.Name != "name") continue;
                            string Name = Data.Value.ToString();
                            if (Name.Length < ShortestNativeLength)
                            {
                                ShortestNativeLength = Name.Length;
                                ShortestNativeName = Namespace.Name + "::" + Name.ToString();
                            }
                        }
                    }
                }

                Console.WriteLine(string.Format("The shortest native, with a length of {0} is:", ShortestNativeLength));
                Console.WriteLine(ShortestNativeName);
            }
        }
    }
}
