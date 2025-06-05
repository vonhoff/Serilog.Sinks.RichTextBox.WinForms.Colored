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

using Serilog;
using Serilog.Debugging;
using Serilog.Events;
using Serilog.Sinks.RichTextBoxForms;
using Serilog.Sinks.RichTextBoxForms.Themes;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Demo
{
    public partial class Form1 : Form
    {
        private RichTextBoxSinkOptions? _options;

        public Form1()
        {
            InitializeComponent();
        }

        private void Initialize()
        {
            _options = new RichTextBoxSinkOptions(ThemePresets.Dark);
            var sink = new RichTextBoxSink(richTextBox1, _options);
            Log.Logger = new LoggerConfiguration()
                .MinimumLevel.Verbose()
                .WriteTo.Sink(sink, LogEventLevel.Verbose)
                .CreateLogger();

            Log.Debug("Started logger.");
            btnDispose.Enabled = true;
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
        }

        private void BtnInformation_Click(object sender, EventArgs e)
        {
            Log.Information("Hello! Now => {Now}", DateTime.Now);
        }

        private void BtnParallelFor_Click(object sender, EventArgs e)
        {
            for (var stepNumber = 1; stepNumber <= 100; stepNumber++)
            {
                var stepName = $"Step {stepNumber:000}";

                Log.Verbose("Hello from For({StepName}) Verbose", stepName);
                Log.Debug("Hello from For({StepName}) Debug", stepName);
                Log.Information("Hello from For({StepName}) Information", stepName);
                Log.Warning("Hello from For({StepName}) Warning", stepName);
                Log.Error("Hello from For({StepName}) Error", stepName);
                Log.Fatal("Hello from For({StepName}) Fatal", stepName);
            }
        }

        private async void BtnTaskRun_Click(object sender, EventArgs e)
        {
            var tasks = new List<Task>();

            for (var i = 1; i <= 100; i++)
            {
                var stepNumber = i;
                var task = Task.Run(() =>
                {
                    var stepName = $"Step {stepNumber:000}";

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
                Date = DateTime.Now,
                TemperatureCelsius = 25,
                Summary = "Hot"
            };

            Log.Information("{@forecast:j}", weatherForecast);
        }

        private void BtnScalar_Click(object sender, EventArgs e)
        {
            Log.Information("String value: {String}", "test");
            Log.Information("Number value: {Number}", 42);
            Log.Information("Boolean value: {Boolean}", true);
            Log.Information("DateTime value: {DateTime}", DateTime.Now);
        }

        private void BtnDictionary_Click(object sender, EventArgs e)
        {
            var dict = new Dictionary<string, object>
            {
                ["key1"] = "value1",
                ["key2"] = 42,
                ["key3"] = true
            };

            Log.Information("Dictionary: {@Dict:l}", dict);
        }

        private void BtnStructure_Click(object sender, EventArgs e)
        {
            var structure = new
            {
                Name = "Test",
                Value = 42,
                IsActive = true,
                CreatedAt = DateTime.Now
            };

            Log.Information("Object: {@Object:j}", structure);
        }

        private void BtnComplex_Click(object sender, EventArgs e)
        {
            var complex = new
            {
                Name = "Test",
                Numbers = new[] { 1, 2, 3 },
                Settings = new
                {
                    enabled = true,
                    count = 5,
                    options = new[] { "opt1", "opt2", "opt3" }
                },
                Metadata = new Dictionary<string, object>
                {
                    ["version"] = "1.0",
                    ["tags"] = new[] { "tag1", "tag2" }
                }
            };

            Log.Information("Complex: {@Complex:j}", complex);
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

        private void BtnAutoScroll_Click(object sender, EventArgs e)
        {
            if (_options == null)
            {
                return;
            }

            _options.AutoScroll = !_options.AutoScroll;
            btnAutoScroll.Text = _options.AutoScroll ? "Disable Auto Scroll" : "Enable Auto Scroll";
        }

        private void BtnLogLimit_Click(object sender, EventArgs e)
        {
            if (_options == null)
            {
                return;
            }

            var limitEnabled = _options.MaxLogLines != int.MaxValue;
            _options.MaxLogLines = limitEnabled ? 0 : 35;
            btnLogLimit.Text = limitEnabled ? "Enable Line Limit" : "Disable Line Limit";
            Log.Information("Log line limit set to {lineLimit}", _options.MaxLogLines == int.MaxValue ? "Maximum" : _options.MaxLogLines.ToString());
        }
    }
}