namespace Precomp4C {
    public class CO_I18N {
        public string Pattern { get; set; }
        public string Default { get; set; }
    }
    public class CO_SymbolExtract_Symbol {
        public string Name { get; set; }
        public string Export { get; set; }
    }
    public class CO_SymbolExtract {
        public string File { get; set; }
        public CO_SymbolExtract_Symbol[] Symbols { get; set; }
    }
    public class CO_Binary {
        public string File { get; set; }
        public string Export { get; set; }
    }
    public class CO_Loaders {
        public CO_I18N I18N { get; set; }
        public CO_SymbolExtract[] SymExtract { get; set; }
        public CO_Binary[] Binary { get; set; }
    }
    class CConfigObject {
        public string NTAssassinIntegrate { get; set; }
        public CO_Loaders Loaders { get; set; }
        public string Output { get; set; }
    }
}
