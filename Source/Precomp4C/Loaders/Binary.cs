using System.Diagnostics;

namespace Precomp4C.Loaders {
    static class Binary {
        public static DateTime GetLastModifyTime(string ProjectDir, CO_Binary[] ConfigObject) {
            DateTime LastModifyTime = DateTime.MinValue;
            foreach (var BinaryItem in ConfigObject) {
                string BinFilePath = Path.GetFullPath(ProjectDir + BinaryItem.File);
                if (!File.Exists(BinFilePath)) {
                    RTL.RaiseError(200, "Binary file (" + BinFilePath + ") not exists");
                }
                DateTime SourceModifyTime = File.GetLastWriteTime(BinFilePath);
                if (SourceModifyTime > LastModifyTime) {
                    LastModifyTime = SourceModifyTime;
                }
            }
            return LastModifyTime;
        }
        public static bool Process(string ProjectDir, FileStream fsHeader, FileStream fsSource, CO_Binary[] ConfigObject) {
            Stopwatch Elapsed = new();
            Elapsed.Start();
            Console.WriteLine("Binary Loader starts...");

            foreach (var BinaryItem in ConfigObject) {
                string BinFilePath = Path.GetFullPath(ProjectDir + BinaryItem.File);
                Console.WriteLine("Binary Loader - Compiling: " + BinFilePath);
                FileStream fsBinFile = null;
                FileInfo BinFile = null;
                try {
                    BinFile = new FileInfo(BinFilePath);
                    fsBinFile = BinFile.OpenRead();
                } catch {
                    RTL.RaiseError(301, "Open binary file (" + BinFilePath + ") failed");
                }
                if (BinFile.Length <= 0) {
                    RTL.RaiseError(302, "Binary file (" + BinFilePath + ") is empty!");
                }
                var Data = new byte[BinFile.Length];
                RTL.ReadStream(fsBinFile, Data, 0);
                var SymbolDef = string.Format("unsigned char {0}[{1:D}]", "BIN_" + BinaryItem.Export, BinFile.Length);
                int RemainSize = Convert.ToInt32(BinFile.Length);
                uint BytesPerLine = 25;
                RTL.WriteOutput(fsHeader, "extern " + SymbolDef + ";");
                RTL.WriteOutput(fsSource, SymbolDef + " = {");
                while (RemainSize > 0) {
                    var LineSize = RemainSize > BytesPerLine ? BytesPerLine : Convert.ToUInt32(RemainSize);
                    var Bytes = new string[LineSize];
                    for (var j = 0; j < LineSize; j++)
                        Bytes[j] = string.Format("0x{0:X2}", Data[BinFile.Length - RemainSize--]);
                    RTL.WriteOutput(fsSource, "\t" + string.Join(", ", Bytes) + (RemainSize == 0 ? null : ","));
                }
                RTL.WriteOutput(fsSource, "};\r\n");
            }

            Elapsed.Stop();
            Console.WriteLine("Binary Loader finished, elapsed {0:G}ms", Elapsed.Elapsed.TotalMilliseconds);
            return true;
        }
    }
}
