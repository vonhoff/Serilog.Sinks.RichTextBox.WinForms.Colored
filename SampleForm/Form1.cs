#region Copyright 2022 Simon Vonhoff & Contributors

//
// Licensed under the Apache License, Version 2.0 (the "License");
// you may not use this file except in compliance with the License.
// You may obtain a copy of the License at
//
//     http://www.apache.org/licenses/LICENSE-2.0
//
// Unless required by applicable law or agreed to in writing, software
// distributed under the License is distributed on an "AS IS" BASIS,
// WITHOUT WARRANTIES OR CONDITIONS OF ANY KIND, either express or implied.
// See the License for the specific language governing permissions and
// limitations under the License.
//

#endregion

using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Threading.Tasks;
using System.Windows.Forms;
using Serilog;
using Serilog.Debugging;

namespace SampleForm
{
    public partial class Form1 : Form
    {
        public Form1()
        {
            InitializeComponent();
        }

        private void Form1_Load(object sender, EventArgs e)
        {
            SelfLog.Enable(message => Trace.WriteLine($"INTERNAL ERROR: {message}"));
            Initialize();

            Log.Information("Hello {Name}", Environment.UserName);
            Log.Warning("No coins remain at position {@Position}", new { Lat = 25, Long = 134 });

            try
            {
                Fail();
            }
            catch (Exception ex)
            {
                Log.Error(ex, "Oops... Something went wrong");
            }
        }

        private void Initialize()
        {
            const string outputTemplate = "[{Timestamp:HH:mm:ss} {Level:u3}] {Message:lj} {Properties:j}{NewLine}{Exception}";

            Log.Logger = new LoggerConfiguration()
                .MinimumLevel.Verbose()
                .WriteTo.RichTextBox(richTextBox1, outputTemplate: outputTemplate)
                .Enrich.WithThreadId()
                .CreateLogger();

            Log.Debug("Started logger.");
            btnDispose.Enabled = true;
        }

        private static void CloseAndFlush()
        {
            Log.Debug("Dispose requested.");
            Log.CloseAndFlush();
        }

        private void BtnClear_Click(object sender, EventArgs e)
        {
            richTextBox1.Clear();
        }

        private void BtnDebug_Click(object sender, EventArgs e)
        {
            Log.Debug("Hello! Now => {Now}", DateTime.Now);
        }

        private void BtnError_Click(object sender, EventArgs e)
        {
            Log.Error("Hello! Now => {Now}", DateTime.Now);
        }

        private void BtnFatal_Click(object sender, EventArgs e)
        {
            Log.Fatal("Hello! Now => {Now}", DateTime.Now);
            richTextBox1.Dispose();
        }

        private void BtnInformation_Click(object sender, EventArgs e)
        {
            Log.Information("Hello! Now => {Now}", DateTime.Now);
        }

        private void BtnParallelFor_Click(object sender, EventArgs e)
        {
            Parallel.For(1, 101, stepNumber =>
            {
                var stepName = FormattableString.Invariant($"Step {stepNumber:000}");

                Log.Verbose("Hello from Parallel.For({StepName}) Verbose", stepName);
                Log.Debug("Hello from Parallel.For({StepName}) Debug", stepName);
                Log.Information("Hello from Parallel.For({StepName}) Information", stepName);
                Log.Warning("Hello from Parallel.For({StepName}) Warning", stepName);
                Log.Error("Hello from Parallel.For({StepName}) Error", stepName);
                Log.Fatal("Hello from Parallel.For({StepName}) Fatal", stepName);
            });
        }

        private async void BtnTaskRun_Click(object sender, EventArgs e)
        {
            var tasks = new List<Task>();

            for (var i = 1; i <= 100; i++)
            {
                var stepNumber = i;
                var task = Task.Run(() =>
                {
                    var stepName = FormattableString.Invariant($"Step {stepNumber:000}");

                    Log.Verbose("Hello from Task.Run({StepName}) Verbose", stepName);
                    Log.Debug("Hello from Task.Run({StepName}) Debug", stepName);
                    Log.Information("Hello from Task.Run({StepName}) Information", stepName);
                    Log.Warning("Hello from Task.Run({StepName}) Warning", stepName);
                    Log.Error("Hello from Task.Run({StepName}) Error", stepName);
                    Log.Fatal("Hello from Task.Run({StepName}) Fatal", stepName);
                });

                tasks.Add(task);
            }

            await Task.WhenAll(tasks);
        }

        private void BtnVerbose_Click(object sender, EventArgs e)
        {
            Log.Verbose("Hello! Now => {Now}", DateTime.Now);
        }

        private void BtnWarning_Click(object sender, EventArgs e)
        {
            Log.Warning("Hello! Now => {Now}", DateTime.Now);
        }

        private static void Fail()
        {
            throw new DivideByZeroException();
        }

        private void BtnObject_Click(object sender, EventArgs e)
        {
            var weatherForecast = new WeatherForecast
            {
                Date = DateTime.Parse("2019-08-01"),
                TemperatureCelsius = 25,
                Summary = "Hot"
            };

            Log.Information("{@forecast}", weatherForecast);
        }

        private void BtnDispose_Click(object sender, EventArgs e)
        {
            CloseAndFlush();
            btnDispose.Enabled = false;
        }

        private void BtnReset_Click(object sender, EventArgs e)
        {
            CloseAndFlush();
            Initialize();
        }
    }
}