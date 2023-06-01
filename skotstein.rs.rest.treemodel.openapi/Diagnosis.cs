// MIT License
//Copyright (c) 2023 Sebastian Kotstein
//
//Permission is hereby granted, free of charge, to any person obtaining a copy
//of this software and associated documentation files (the "Software"), to deal
//in the Software without restriction, including without limitation the rights
//to use, copy, modify, merge, publish, distribute, sublicense, and/or sell
//copies of the Software, and to permit persons to whom the Software is
//furnished to do so, subject to the following conditions:
//
//The above copyright notice and this permission notice shall be included in all
//copies or substantial portions of the Software.
//
//THE SOFTWARE IS PROVIDED "AS IS", WITHOUT WARRANTY OF ANY KIND, EXPRESS OR
//IMPLIED, INCLUDING BUT NOT LIMITED TO THE WARRANTIES OF MERCHANTABILITY,
//FITNESS FOR A PARTICULAR PURPOSE AND NONINFRINGEMENT. IN NO EVENT SHALL THE
//AUTHORS OR COPYRIGHT HOLDERS BE LIABLE FOR ANY CLAIM, DAMAGES OR OTHER
//LIABILITY, WHETHER IN AN ACTION OF CONTRACT, TORT OR OTHERWISE, ARISING FROM,
//OUT OF OR IN CONNECTION WITH THE SOFTWARE OR THE USE OR OTHER DEALINGS IN THE
//SOFTWARE.
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace skotstein.rs.rest.treemodel.openapi
{
    public class Diagnosis
    {
        private int _endpointCounter;
        private int _payloadCounter;
        private int _parameterCounter;
        private IList<LogItem> _log = new List<LogItem>();

        public int EndpointCounter { get => _endpointCounter; set => _endpointCounter = value; }
        public int PayloadCounter { get => _payloadCounter; set => _payloadCounter = value; }
        public int ParameterCounter { get => _parameterCounter; set => _parameterCounter = value; }
        public IList<LogItem> Log { get => _log; set => _log = value; }

        public void Info(string msg)
        {
            _log.Add(new LogItem(msg, LogLevel.info));
        }

        public void Warning(string msg)
        {
            _log.Add(new LogItem(msg, LogLevel.warning));
        }

        public void Error(string msg)
        {
            _log.Add(new LogItem(msg, LogLevel.error));
        }

        public void Verbose(string msg)
        {
            _log.Add(new LogItem(msg, LogLevel.verbose));
        }

        public int Count(LogLevel level)
        {
            int counter = 0;
            foreach (LogItem logItem in _log)
            {
                if (logItem.Level == level)
                {
                    counter++;
                }
            }
            return counter;
        }

        public IList<LogItem> getItemsOfLevel(LogLevel level)
        {
            IList<LogItem> logItems = new List<LogItem>();
            foreach (LogItem logItem in _log)
            {
                if (logItem.Level == level)
                {
                    logItems.Add(logItem);
                }
            }
            return logItems;
        }
    }

    public class LogItem
    {
        private string _message;
        private LogLevel _level;

        public LogItem(string message, LogLevel level)
        {
            _message = message;
            _level = level;
        }

        public string Message { get => _message; set => _message = value; }
        public LogLevel Level { get => _level; set => _level = value; }
    }

    public enum LogLevel
    {
        verbose, info, warning, error
    }
}
