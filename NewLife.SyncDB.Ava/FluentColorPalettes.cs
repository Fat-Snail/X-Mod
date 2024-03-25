using Avalonia.Media;
using Avalonia.Themes.Fluent;

namespace NewLife.SyncDB.Ava;

public enum ThemeColorType
{
    Lavender,
    Forest,
    Nighttime
}
public class FluentColorPalettes
{
    internal static Dictionary<string, ColorPaletteResources> DarkPalettes =
        new Dictionary<string, ColorPaletteResources>();

    internal static Dictionary<string, ColorPaletteResources> LightPalettes =
        new Dictionary<string, ColorPaletteResources>();

    static FluentColorPalettes()
    {
        //Lavender
        var cpr = CreateLightPalette(Accent:"#ff8961cc",BaseLow:"#ffeeceff",BaseMedium:"#ffa987bc",BaseMediumHigh:"#ff7b5890",BaseMediumLow:"#ff9270a6",ChromeAltLow:"#ff7b5890",ChromeBlackLow:"#ffeeceff",ChromeBlackMedium:"#ff7b5890",ChromeBlackMediumLow:"#ffa987bc",ChromeDisabledHigh:"#ffeeceff",ChromeDisabledLow:"#ffa987bc",ChromeGray:"#ff9270a6",ChromeHigh:"#ffeeceff",ChromeLow:"#fffeeaff",ChromeMedium:"#fffbe4ff",ChromeMediumLow:"#fffeeaff",ListLow:"#fffbe4ff",ListMedium:"#ffeeceff",RegionColor:"#fffef6ff");
        LightPalettes.Add("Lavender",cpr);
        cpr = CreateDarkPalette(Accent: "#ff8961cc", BaseLow: "#ff64576b",
            BaseMedium: "#ffb6aabc", BaseMediumHigh: "#ffcbbfd0", BaseMediumLow: "#ff8d8193", ChromeAltLow: "#ffcbbfd0",
            ChromeBlackLow: "#ffcbbfd0", ChromeBlackMedium: "Black",
            ChromeBlackMediumLow: "Black", ChromeDisabledHigh: "#ff64576b", ChromeDisabledLow: "#ffb6aabc",
            ChromeGray: "#ffa295a8", ChromeHigh: "#ffa295a8", ChromeLow: "#ff332041", ChromeMedium: "#ff3f2e4b",
            ChromeMediumLow: "#ff584960",  ListLow: "#ff3f2e4b", ListMedium: "#ff64576b",
            RegionColor: "#ff262738");
        DarkPalettes.Add("Lavender",cpr);
        
        //Forest
        cpr= CreateLightPalette(Accent:"#ff34854d",BaseLow:"#ffc2db65",BaseMedium:"#ff7d9728",BaseMediumHigh:"#ff4f6a00",BaseMediumLow:"#ff668114",ChromeAltLow:"#ff4f6a00",ChromeBlackLow:"#ffc2db65",ChromeBlackMedium:"#ff4f6a00",ChromeBlackMediumLow:"#ff7d9728",ChromeDisabledHigh:"#ffc2db65",ChromeDisabledLow:"#ff7d9728",ChromeGray:"#ff668114",ChromeHigh:"#ffc2db65",ChromeLow:"#ffe6f3bb",ChromeMedium:"#ffdfeeaa",ChromeMediumLow:"#ffe6f3bb",ListLow:"#ffdfeeaa",ListMedium:"#ffc2db65",RegionColor:"#fff7ffff");
        LightPalettes.Add("Forest",cpr);
        cpr = CreateDarkPalette(Accent:"#ff34854d",BaseLow:"#ff784834",BaseMedium:"#ffc5a294",BaseMediumHigh:"#ffd8b8ac",BaseMediumLow:"#ff9e7564",ChromeAltLow:"#ffd8b8ac",ChromeBlackLow:"#ffd8b8ac",ChromeBlackMedium:"Black",ChromeBlackMediumLow:"Black",ChromeDisabledHigh:"#ff784834",ChromeDisabledLow:"#ffc5a294",ChromeGray:"#ffb28b7c",ChromeHigh:"#ffb28b7c",ChromeLow:"#ff46150a",ChromeMedium:"#ff532215",ChromeMediumLow:"#ff6c3b2a",ListLow:"#ff532215",ListMedium:"#ff784834",RegionColor:"#ff353819");
        DarkPalettes.Add("Forest",cpr);
        
        //Nighttime
        cpr= CreateLightPalette(Accent:"#ffcc4d11",BaseLow:"#ff7cbee0",BaseMedium:"#ff3282a8",BaseMediumHigh:"#ff005a83",BaseMediumLow:"#ff196e96",ChromeAltLow:"#ff005a83",ChromeBlackLow:"#ff7cbee0",ChromeBlackMedium:"#ff005a83",ChromeBlackMediumLow:"#ff3282a8",ChromeDisabledHigh:"#ff7cbee0",ChromeDisabledLow:"#ff3282a8",ChromeGray:"#ff196e96",ChromeHigh:"#ff7cbee0",ChromeLow:"#ffc1e9fe",ChromeMedium:"#ffb3e0f8",ChromeMediumLow:"#ffc1e9fe",ListLow:"#ffb3e0f8",ListMedium:"#ff7cbee0",RegionColor:"#ffcfeaff");
        LightPalettes.Add("Nighttime",cpr);
        cpr = CreateDarkPalette(Accent:"#ffcc4d11",BaseLow:"#ff2f7bad",BaseMedium:"#ff8dbfdf",BaseMediumHigh:"#ffa5d0ec",BaseMediumLow:"#ff5e9dc6",ChromeAltLow:"#ffa5d0ec",ChromeBlackLow:"#ffa5d0ec",ChromeBlackMedium:"Black",ChromeBlackMediumLow:"Black",ChromeDisabledHigh:"#ff2f7bad",ChromeDisabledLow:"#ff8dbfdf",ChromeGray:"#ff76aed3",ChromeHigh:"#ff76aed3",ChromeLow:"#ff093b73",ChromeMedium:"#ff134b82",ChromeMediumLow:"#ff266b9f",ListLow:"#ff134b82",ListMedium:"#ff2f7bad",RegionColor:"#ff0d2644");
        DarkPalettes.Add("Nighttime",cpr);
        
    }

    static ColorPaletteResources CreateLightPalette(string Accent, string BaseLow, string BaseMedium,
        string BaseMediumHigh, string BaseMediumLow, string ChromeAltLow, string ChromeBlackLow,
        string ChromeBlackMedium, string ChromeBlackMediumLow,
        string ChromeDisabledHigh, string ChromeDisabledLow, string ChromeGray, string ChromeHigh, string ChromeLow,
        string ChromeMedium, string ChromeMediumLow,
        string ListLow, string ListMedium, string RegionColor)
    {
        var cpr = new ColorPaletteResources()
        {
            Accent = Color.Parse(Accent), BaseLow = Color.Parse(BaseLow), BaseMedium = Color.Parse(BaseMedium),
            BaseMediumHigh = Color.Parse(BaseMediumHigh), BaseMediumLow = Color.Parse(BaseMediumLow),
            ChromeAltLow = Color.Parse(ChromeAltLow),
            ChromeBlackLow = Color.Parse(ChromeBlackLow), ChromeBlackMedium = Color.Parse(ChromeBlackMedium),
            ChromeBlackMediumLow = Color.Parse(ChromeBlackMediumLow),
            ChromeDisabledHigh = Color.Parse(ChromeDisabledHigh), ChromeDisabledLow = Color.Parse(ChromeDisabledLow),
            ChromeGray = Color.Parse(ChromeGray),
            ChromeHigh = Color.Parse(ChromeHigh), ChromeLow = Color.Parse(ChromeLow),
            ChromeMedium = Color.Parse(ChromeMedium), ChromeMediumLow = Color.Parse(ChromeMediumLow),
            ListLow = Color.Parse(ListLow), ListMedium = Color.Parse(ListMedium), RegionColor = Color.Parse(RegionColor)
        };
        cpr.AltHigh = cpr.AltLow =
            cpr.AltMedium = cpr.AltMediumHigh = cpr.AltMediumLow = cpr.ChromeWhite = cpr.BaseHigh = Colors.White;
        cpr.BaseHigh = cpr.ChromeBlackHigh = Colors.Black;
        return cpr;
        //new ColorPaletteResources() { AltHigh = Colors.White, AltLow }; 
    }

    static ColorPaletteResources CreateDarkPalette(string Accent, string BaseLow,string BaseMedium,
        string BaseMediumHigh,string BaseMediumLow,string ChromeAltLow,string ChromeBlackLow,string ChromeBlackMedium,string ChromeBlackMediumLow,
        string ChromeDisabledHigh,string ChromeDisabledLow,string ChromeGray,string ChromeHigh,string ChromeLow,string ChromeMedium,string ChromeMediumLow,
        string ListLow,string ListMedium,string RegionColor)
    {
        var cpr = new ColorPaletteResources()
        {
            Accent = Color.Parse(Accent), BaseLow = Color.Parse(BaseLow),BaseMedium=Color.Parse(BaseMedium) ,
            BaseMediumHigh=Color.Parse(BaseMediumHigh),BaseMediumLow=Color.Parse(BaseMediumLow),ChromeAltLow=Color.Parse(ChromeAltLow),
            ChromeBlackLow=Color.Parse(ChromeBlackLow),ChromeBlackMedium=Color.Parse(ChromeBlackMedium),ChromeBlackMediumLow=Color.Parse(ChromeBlackMediumLow),
            ChromeDisabledHigh=Color.Parse(ChromeDisabledHigh),ChromeDisabledLow=Color.Parse(ChromeDisabledLow),ChromeGray=Color.Parse(ChromeGray),
            ChromeHigh=Color.Parse(ChromeHigh),ChromeLow=Color.Parse(ChromeLow),ChromeMedium=Color.Parse(ChromeMedium),ChromeMediumLow=Color.Parse(ChromeMediumLow),
            ListLow=Color.Parse(ListLow),ListMedium=Color.Parse(ListMedium),RegionColor=Color.Parse(RegionColor)
        };
        cpr.AltHigh = cpr.AltLow =
            cpr.AltMedium = cpr.AltMediumHigh = cpr.AltMediumLow = cpr.ChromeWhite=cpr.ChromeBlackHigh = Colors.Black;
        cpr.BaseHigh = Colors.White;
        return cpr;
        //new ColorPaletteResources() { AltHigh = Colors.White, AltLow }; 
    }
}