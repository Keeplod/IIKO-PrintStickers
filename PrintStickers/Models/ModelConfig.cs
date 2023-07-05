namespace PrintStickers.Models
{
    internal class ModelConfig
    {
        public int StikerHeight { get; set; }
        public int StikerWidth { get; set; }
        public int SpaceUpToLogo { get; set; }
        public int LogoHeight { get; set; }
        public int LogoWidth { get; set; }
        public int SpaceLogoToProduct { get; set; }
        public int ProductDivHeight { get; set; }
        public int ProductFontSize { get; set; }
        public bool ProductFontStyleBolt { get; set; }
        public bool ProductFontStyleItalic { get; set; }
        public int ClientDivHeight { get; set; }
        public int ClientFontSize { get; set; }
        public bool ClientFontStyleBolt { get; set; }
        public bool ClientFontStyleItalic { get; set; }
        public string PrinterName { get; set; }
        public bool AutoPrint { get; set; }
        public string ProductCategories { get; set; }
    }
}
