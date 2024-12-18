using System.Diagnostics;
using System.IO;

namespace Startwizard.Class
{
    class pProcess
    {
        /// <summary>
        /// プロセスを実行
        /// </summary>
        /// <param name="filePath">ファイルパス</param>
        public async Task<bool> Run(string filePath)
        {
            var procOption = new ProcessStartInfo
            {
                CreateNoWindow = true,                // Windowは表示させない
                FileName = filePath,                 // ファイルパス
                RedirectStandardOutput = true,       // 標準出力(stdout)
                RedirectStandardError = true,        // 標準エラー(stderr)
                UseShellExecute = false              // 指定されているプログラムで開く
            };

            var proc = Process.Start(procOption); // 発火

            var stdoutTask = Task.Run(() =>
            {
                string stdout = string.Empty; // 標準出力

                try
                {
                    stdout = proc.StandardOutput.ReadToEnd(); // コマンドプロンプトの出力をポート

                    using (StreamWriter sw = new StreamWriter(@".\stdout.Log"))
                    {
                        sw.Write(stdout); // logに書き込み
                    }
                }
                catch (Exception ex)
                {
                    Debug.WriteLine(ex);
                }

                return true; // 終わったらtrueを返す。
            });

            var stderrTask = Task.Run(() =>
            {
                string stderr = string.Empty;

                try
                {
                    stderr = proc.StandardError.ReadToEnd();

                    using (StreamWriter sw = new StreamWriter(@".\stderr.Log"))
                    {
                        sw.Write(stderr); // logに書き込み
                    }
                }
                catch (Exception ex)
                {
                    Debug.WriteLine(ex);
                }

                return true; // 終わったらtrueを返す。
            });

            var start = Task.Run(() =>
            {
                while (!proc.HasExited) // プロセスが終了するまで
                {
                    try
                    {
                    }
                    catch (Exception ex)
                    {
                        Debug.WriteLine(ex);
                    }
                }

                return true; // ！！！！ここの判別は後々変える！！！！
            });

            return (await Task.WhenAll(stdoutTask, stderrTask, start))[1];
        }

        // ショートカットなどを開く場合
        public async Task<bool> RunProgram(string filePath)
        {
            string arguments = string.Empty;

            return await Task.Run(() =>
            {
                var procOption = new ProcessStartInfo
                {
                    FileName = filePath.Replace(",", ""),  // ファイルパス
                    Arguments = arguments, // 引数を指定
                    UseShellExecute = true // 指定されているプログラムで開く
                };

                var proc = Process.Start(procOption); // 発火

                return true;
            });
        }
    }
}
