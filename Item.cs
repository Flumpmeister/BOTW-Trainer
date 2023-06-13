namespace BotwTrainer
{
    using System;
    using System.Windows;

    public class Item
    {
        public string Id { get; set; }

        public string Name { get; set; }

        public uint NameStart { get; set; }

        public string NameStartHex
        {
            get
            {
                try
                {
                    return NameStart.ToString("x8").ToUpper();
                }
                catch (FormatException formatException)
                {
                    MessageBox.Show(formatException.Message);
                    return string.Empty;
                }
            }
        }

        public uint BaseAddress { get; set; }

        public string BaseAddressHex
        {
            get
            {
                try
                {
                    return BaseAddress.ToString("x8").ToUpper();
                }
                catch (FormatException formatException)
                {
                    MessageBox.Show(formatException.Message);
                    return string.Empty;
                }
            }
        }

        public uint Value { get; set; }

        public string ValueHex
        {
            get
            {
                try
                {
                    return Value.ToString("x8").ToUpper();
                }
                catch (FormatException formatException)
                {
                    MessageBox.Show(formatException.Message);
                    return string.Empty;
                }
            }
        }

        public string ValueAddressHex
        {
            get
            {
                try
                {
                    return (BaseAddress + 0x8).ToString("x8").ToUpper();
                }
                catch (FormatException formatException)
                {
                    MessageBox.Show(formatException.Message);
                    return string.Empty;
                }
            }
        }

        public bool Equipped { get; set; }

        public bool Current { get; set; }

        public string Modifier1Value { get; set; }

        public string Modifier1Address
        {
            get
            {
                var offset = BaseAddress + 0x5c;
                try
                {
                    return offset.ToString("x8").ToUpper();
                }
                catch (FormatException formatException)
                {
                    MessageBox.Show(formatException.Message);
                    return string.Empty;
                }
            }
        }

        public string Modifier2Value { get; set; }

        public string Modifier2Address
        {
            get
            {
                var offset = BaseAddress + 0x60;
                try
                {
                    return offset.ToString("x8").ToUpper();
                }
                catch (FormatException formatException)
                {
                    MessageBox.Show(formatException.Message);
                    return string.Empty;
                }
            }
        }

        public string Modifier3Value { get; set; }

        public string Modifier3Address
        {
            get
            {
                var offset = BaseAddress + 0x64;
                try
                {
                    return offset.ToString("x8").ToUpper();
                }
                catch (FormatException formatException)
                {
                    MessageBox.Show(formatException.Message);
                    return string.Empty;
                }
            }
        }

        public string Modifier4Value { get; set; }

        public string Modifier4Address
        {
            get
            {
                var offset = BaseAddress + 0x68;
                try
                {
                    return offset.ToString("x8").ToUpper();
                }
                catch (FormatException formatException)
                {
                    MessageBox.Show(formatException.Message);
                    return string.Empty;
                }
            }
        }

        public string Modifier5Value { get; set; }

        public string Modifier5Address
        {
            get
            {
                var offset = BaseAddress + 0x6c;
                try
                {
                    return offset.ToString("x8").ToUpper();
                }
                catch (FormatException formatException)
                {
                    MessageBox.Show(formatException.Message);
                    return string.Empty;
                }
            }
        }

        public int Page { get; set; }

        public string PageName
        {
            get
            {
                string name;
                switch (Page)
                {
                    case 0:
                        name = "Weapons";
                        break;
                    case 1:
                        name = "Bows";
                        break;
                    case 2:
                        name = "Arrows";
                        break;
                    case 3:
                        name = "Shields";
                        break;
                    case 4:
                        name = "Head";
                        break;
                    case 5:
                        name = "Torso";
                        break;
                    case 6:
                        name = "Legs";
                        break;
                    case 7:
                        name = "Materials";
                        break;
                    case 8:
                        name = "Food";
                        break;
                    case 9:
                        name = "Key Items";
                        break;
                    default:
                        name = "Unknown";
                        break;
                }

                return name;
            }
        }

        public int Unknown { get; set; }
    }
}