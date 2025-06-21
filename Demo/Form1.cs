#region Copyright 2025 Simon Vonhoff & Contributors

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
using System.Globalization;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Demo
{
    public partial class Form1 : Form
    {
        private RichTextBoxSinkOptions? _options;
        private bool _toolbarsVisible = true;

        public Form1()
        {
            InitializeComponent();
        }

        private void Initialize()
        {
            _options = new RichTextBoxSinkOptions(
                theme: ThemePresets.Literate,
                outputTemplate: "[{Timestamp:HH:mm:ss} {Level:u3}] {Message:l}{NewLine}{Exception}",
                formatProvider: new CultureInfo("en-US"));

            var sink = new RichTextBoxSink(richTextBox1, _options);
            Log.Logger = new LoggerConfiguration()
                .MinimumLevel.Verbose()
                .WriteTo.Sink(sink, LogEventLevel.Verbose)
                .CreateLogger();

            Log.Debug("Started logger.");
            btnDispose.Enabled = true;
            txtMaxLines.Text = _options.MaxLogLines.ToString();
        }

        private void Form1_Load(object sender, EventArgs e)
        {
            SelfLog.Enable(message =>
            {
                Trace.WriteLine($"INTERNAL ERROR: {message}");
            });
            Initialize();

            Log.Information("Application started. Environment: {Environment}, Version: {Version}",
                Environment.GetEnvironmentVariable("ASPNETCORE_ENVIRONMENT") ?? "Development",
                typeof(Form1).Assembly.GetName().Version);

            var apiRequest = new
            {
                Method = "POST",
                Endpoint = "/api/users",
                RequestId = Guid.NewGuid()
            };
            Log.Information("API Request: {@Request}", apiRequest);

            try
            {
                SimulateDatabaseOperation();
            }
            catch (Exception ex)
            {
                Log.Error(ex, "Database operation failed. Connection string: {ConnectionString}",
                    "Server=localhost;Trusted_Connection=True;");
            }
        }

        private static void CloseAndFlush()
        {
            Log.Debug("Dispose requested.");
            Log.CloseAndFlush();
        }

        private void btnClear_Click(object sender, EventArgs e)
        {
            richTextBox1.Clear();
        }

        private void btnDebug_Click(object sender, EventArgs e)
        {
            var query = new
            {
                Sql = "SELECT * FROM Users WHERE Status = @status",
                Parameters = new { status = "Active" },
                ExecutionTime = 150 // ms
            };
            Log.Debug("Database query executed: {@Query}", query);
        }

        private void btnError_Click(object sender, EventArgs e)
        {
            try
            {
                throw new InvalidOperationException("Failed to process payment",
                    new Exception("Gateway timeout"));
            }
            catch (Exception ex)
            {
                Log.Error(ex, "Payment processing failed for OrderId: {OrderId}", Guid.NewGuid());
            }
        }

        private void btnFatal_Click(object sender, EventArgs e)
        {
            try
            {
                throw new OutOfMemoryException("Application memory limit exceeded");
            }
            catch (Exception ex)
            {
                Log.Fatal(ex, "Critical system failure. Memory usage: {MemoryUsage}MB",
                    Process.GetCurrentProcess().WorkingSet64 / 1024 / 1024);
            }
        }

        private void btnInformation_Click(object sender, EventArgs e)
        {
            var userAction = new
            {
                UserId = "user123",
                Action = "Login",
                IP = "192.168.1.1",
                UserAgent = "Mozilla/5.0 (Windows NT 10.0; Win64; x64)",
                Timestamp = DateTime.UtcNow
            };
            Log.Information("User activity: {@UserAction}", userAction);
        }

        private void btnParallelFor_Click(object sender, EventArgs e)
        {
            for (var stepNumber = 1; stepNumber <= 100; stepNumber++)
            {
                Log.Verbose("Processing batch item Step {StepNumber:000} - Status: {Status}", stepNumber, "InProgress");
                Log.Debug("Batch processing metrics for Step {StepNumber:000} - Duration: {Duration}ms", stepNumber, 150);
                Log.Information("Completed processing Step {StepNumber:000} - Items processed: {Count}", stepNumber, 1000);
                Log.Warning("Performance warning for Step {StepNumber:000} - Response time: {ResponseTime}ms", stepNumber, 500);
                Log.Error("Failed to process Step {StepNumber:000} - Error code: {ErrorCode}", stepNumber, "E1001");
                Log.Fatal("Critical failure in Step {StepNumber:000} - System state: {State}", stepNumber, "Unrecoverable");
            }
        }

        private async void btnTaskRun_Click(object sender, EventArgs e)
        {
            var tasks = new List<Task>();

            for (var i = 1; i <= 100; i++)
            {
                var stepNumber = i;
                var task = Task.Run(() =>
                {
                    Log.Verbose("Background task Step {StepNumber:000} - Status: {Status}", stepNumber, "Started");
                    Log.Debug("Background task Step {StepNumber:000} - Thread ID: {ThreadId}", stepNumber, Environment.CurrentManagedThreadId);
                    Log.Information("Background task Step {StepNumber:000} - Progress: {Progress}%", stepNumber, 75);
                    Log.Warning("Background task Step {StepNumber:000} - Resource usage: {CpuUsage}%", stepNumber, 85);
                    Log.Error("Background task Step {StepNumber:000} - Failed with code: {ErrorCode}", stepNumber, "E2001");
                    Log.Fatal("Background task Step {StepNumber:000} - System impact: {Impact}", stepNumber, "Critical");
                });

                tasks.Add(task);
            }

            await Task.WhenAll(tasks);
        }

        private void btnVerbose_Click(object sender, EventArgs e)
        {
            Log.Verbose("Processing batch item {ItemId} of {TotalItems}", 42, 100);
        }

        private void btnWarning_Click(object sender, EventArgs e)
        {
            Log.Warning("High memory usage detected: {MemoryUsage}MB (Threshold: {Threshold}MB)",
                Process.GetCurrentProcess().WorkingSet64 / 1024 / 1024, 16);
        }

        private static void SimulateDatabaseOperation()
        {
            throw new Exception("Database connection timeout", new TimeoutException("Network connection lost"));
        }

        private void btnObject_Click(object sender, EventArgs e)
        {
            var systemMetrics = new
            {
                MemoryUsage = 1024.5,
                LastGcCollection = DateTime.UtcNow.AddMinutes(-5)
            };

            Log.Information("System metrics: {@Metrics:j}", systemMetrics);
        }

        private void btnScalar_Click(object sender, EventArgs e)
        {
            Log.Information("Cache hit ratio: {HitRatio:P2}", 0.856);
            Log.Information("Response received: {ResponseTime}", DateTime.Now);
            Log.Information("Valid batch size: {IsValid}", true);
            Log.Information("API version: {ApiVersion}", "2.1.0");
        }

        private void btnDictionary_Click(object sender, EventArgs e)
        {
            var config = new Dictionary<string, object>
            {
                ["ConnectionTimeout"] = 30,
                ["MaxRetries"] = 3,
                ["EnableCompression"] = true,
                ["AllowedOrigins"] = new[] { "https://api.example.com", "https://admin.example.com", "\\\\int\\server\\region\\share\\project\\client\\folder with space\\1_All ABC changes\\Local~output.csv" },
                ["FeatureFlags"] = new Dictionary<string, bool>
                {
                    ["NewUI"] = true,
                    ["BetaFeatures"] = false
                }
            };

            Log.Information("Configuration loaded: {@Config:l}", config);
        }

        private void btnStructure_Click(object sender, EventArgs e)
        {
            var auditLog = new
            {
                EventId = Guid.NewGuid(),
                Timestamp = DateTime.UtcNow,
                User = new
                {
                    Id = "user456",
                    Role = "Administrator",
                    Department = "IT"
                },
                Action = "ConfigurationUpdate",
                Changes = new[]
                {
                    new { Property = "MaxConnections", OldValue = 100, NewValue = 200 },
                    new { Property = "Timeout", OldValue = 30, NewValue = 60 }
                } as object[]
            };

            Log.Information("Audit log entry: {@AuditLog:j}", auditLog);
        }

        private void btnComplex_Click(object sender, EventArgs e)
        {
            var deploymentInfo = new
            {
                DeploymentId = Guid.NewGuid(),
                Environment = "Production",
                Version = "2.1.0",
                Timestamp = DateTime.UtcNow,
                Services = new object[]
                {
                    new
                    {
                        Name = "API",
                        Status = "Healthy",
                        Metrics = new
                        {
                            ResponseTime = 45,
                            ErrorRate = 0.01,
                            RequestsPerSecond = 150
                        }
                    },
                    new
                    {
                        Name = "Database",
                        Status = "Degraded",
                        Metrics = new
                        {
                            ConnectionCount = 85,
                            QueryTime = 120,
                            ReplicationLag = 5
                        }
                    }
                },
                Infrastructure = new
                {
                    Region = "us-east-1",
                    InstanceType = "t3.large",
                    Scaling = new
                    {
                        MinInstances = 2,
                        MaxInstances = 5,
                        CurrentInstances = 3
                    }
                }
            };

            Log.Information("Deployment status: {@DeploymentInfo:j}", deploymentInfo);
        }

        private void btnDispose_Click(object sender, EventArgs e)
        {
            CloseAndFlush();
            btnDispose.Enabled = false;
        }

        private void btnReset_Click(object sender, EventArgs e)
        {
            CloseAndFlush();
            Initialize();
        }

        private void btnAutoScroll_Click(object sender, EventArgs e)
        {
            if (_options == null)
            {
                return;
            }

            _options.AutoScroll = !_options.AutoScroll;
            btnAutoScroll.Text = _options.AutoScroll ? "Disable Auto Scroll" : "Enable Auto Scroll";
        }

        private void txtMaxLines_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode != Keys.Enter)
            {
                return;
            }

            if (!int.TryParse(txtMaxLines.Text, out var newLimit))
            {
                return;
            }

            newLimit = Math.Max(1, Math.Min(1000, newLimit));

            if (_options != null)
            {
                _options.MaxLogLines = newLimit;
                Log.Information("Log line limit set to {lineLimit}", newLimit);
            }

            txtMaxLines.Text = newLimit.ToString();

            e.SuppressKeyPress = true; // prevent ding sound
        }

        private void Form1_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.Control && e.KeyCode == Keys.T)
            {
                _toolbarsVisible = !_toolbarsVisible;
                toolStrip1.Visible = _toolbarsVisible;
                toolStrip2.Visible = _toolbarsVisible;
            }
        }
    }
}