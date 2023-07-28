using System;
using System.Diagnostics;
using System.IO;

namespace Plum
{
    public class ProcessTool
    {
        public static void ExecuteCommand(string cmd, Action<string> infoAction, Action<string> errorAction)
        {
            var process = new Process();
            try
            {
                process.StartInfo.FileName = "cmd.exe";
                process.StartInfo.UseShellExecute = false;    //是否使用操作系统shell启动
                process.StartInfo.RedirectStandardInput = true;//接受来自调用程序的输入信息
                process.StartInfo.RedirectStandardOutput = true;//由调用程序获取输出信息
                process.StartInfo.RedirectStandardError = true;//重定向标准错误输出
                process.StartInfo.CreateNoWindow = true;//不显示程序窗口
                process.Start();//启动程序

                //向cmd窗口发送输入信息
                process.StandardInput.WriteLine(cmd + "&exit");

                process.StandardInput.AutoFlush = true;
                string output = process.StandardOutput.ReadToEnd();
                string error = process.StandardError.ReadToEnd();
                if (infoAction is not null && !output.IsNullOrWhiteSpace())
                {
                    infoAction(output);
                }
                if (errorAction is not null && !error.IsNullOrWhiteSpace())
                {
                    errorAction(error);
                }

                process.WaitForExit();//等待程序执行完退出进程
            }
            catch (Exception ex)
            {
                if (errorAction is not null)
                {
                    errorAction(ex.ToString());
                }
            }
            finally
            {
                process.Close();
            }
        }

        public static void ExecuteApplication(string fileName, Action<string> infoAction, Action<string> errorAction)
        {
            if (fileName.IsNullOrEmpty() || !File.Exists(fileName))
            {
                throw new FileNotFoundException($"文件{fileName}不存在");
            }

            var process = new Process();
            try
            {
                process.StartInfo.FileName = fileName;
                process.StartInfo.UseShellExecute = false;    //是否使用操作系统shell启动
                process.StartInfo.RedirectStandardInput = true;//接受来自调用程序的输入信息
                process.StartInfo.RedirectStandardOutput = true;//由调用程序获取输出信息
                process.StartInfo.RedirectStandardError = true;//重定向标准错误输出
                process.StartInfo.CreateNoWindow = true;//不显示程序窗口
                process.Start();//启动程序

                process.StandardInput.AutoFlush = true;
                string output = process.StandardOutput.ReadToEnd();
                string error = process.StandardError.ReadToEnd();
                if (infoAction is not null && !output.IsNullOrWhiteSpace())
                {
                    infoAction(output);
                }
                if (errorAction is not null && !error.IsNullOrWhiteSpace())
                {
                    errorAction(error);
                }

                process.WaitForExit();//等待程序执行完退出进程
            }
            catch (Exception ex)
            {
                if (errorAction is not null)
                {
                    errorAction(ex.ToString());
                }
            }
            finally
            {
                process.Close();
            }
        }
    }
}