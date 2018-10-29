﻿using System;
using System.Windows.Forms;
using System.Threading;
using Datalogic.API;
using OpenNETCF.Threading;
using OpenNETCF.Windows.Forms;
using gamma_mob.Common;
using System.IO;
using System.Reflection;

namespace gamma_mob
{
    internal static class Program
    {
        private static NamedMutex _mutex;

        /// <summary>
        ///     Главная точка входа для приложения.
        /// </summary>
        [MTAThread]
        private static void Main()
        {
            bool isNew;
            _mutex = new NamedMutex(false, "gammamob", out isNew);
            if (!isNew) return;
            Decode.SetWedge(WedgeType.Barcode, false);
#if !DEBUG
            UpdateProgram.DropFlagUpdateLoading();
            int num = 0;
            // устанавливаем метод обратного вызова
            TimerCallback tm = new TimerCallback(UpdateProgram.LoadUpdate);
            // создаем таймер
            System.Threading.Timer timer = new System.Threading.Timer(tm, num, 2000, 300000);
#endif
            try
            {
                string[] files = Directory.GetFiles(Path.GetDirectoryName(Assembly.GetExecutingAssembly().GetName().CodeBase), "doc*.xml");
                foreach (string file in files)
                {
                    File.Delete(file);
                }
            }
            catch
            {
            }
            var loginForm = new LoginForm();
            if (loginForm.ShowDialog() == DialogResult.OK)
            {
                Application2.Run(new MainForm());
            }
            else BarcodeScanner.Scanner.Dispose();
        }
    }
}