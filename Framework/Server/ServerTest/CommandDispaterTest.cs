﻿/***********************************************************************************************\
 * (C) KAL ATM Software GmbH, 2022
 * KAL ATM Software GmbH licenses this file to you under the MIT license.
 * See the LICENSE file in the project root for more information.
\***********************************************************************************************/

using System;
using System.Runtime.InteropServices;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System.Threading;
using System.Threading.Tasks;
using XFS4IoT;
using System.Collections.Generic;

namespace XFS4IoTServer.Test
{

    [TestClass]
    public class MessageDispatcherTest
    {

        [TestMethod]
        public async Task NewMessageDispatcherTest()
        {
            var dispatcher = new CommandDispatcher(new[] { XFSConstants.ServiceClass.Publisher }, new TestLogger());


            await dispatcher.Dispatch(new TestConnection(), new TestMessage1(), CancellationToken.None);
            await dispatcher.Dispatch(new TestConnection(), new TestMessage2(), CancellationToken.None);
            await dispatcher.Dispatch(new TestConnection(), new TestMessage3(), CancellationToken.None);
        }
    }

    public class TestServiceProvider : IServiceProvider
    {
        public string Name { get; } = String.Empty;
        public Uri Uri { get; } = new Uri(string.Empty);
        public Uri WSUri { get; } = new Uri(string.Empty);
        public IDevice Device { get => throw new NotImplementedException(); }
        public Task BroadcastEvent(object payload) => throw new NotImplementedException();
        public Task BroadcastEvent(IEnumerable<IConnection> connections, object payload) => throw new NotImplementedException();
        public Task<bool> AnyValidRequestID(IConnection Connection, List<int> RequestIds, CancellationToken token) => throw new NotImplementedException();
        public Task CancelCommandsAsync(IConnection Connection, List<int> RequestIds, CancellationToken Token) => throw new NotImplementedException();
        public Task Dispatch(IConnection Connection, MessageBase Command, CancellationToken Token) => throw new NotImplementedException();
        public Task DispatchError(IConnection Connection, MessageBase Command, Exception CommandException) => throw new NotImplementedException();
        public Task RunAsync(CancellationSource cancellationSource) => throw new NotImplementedException();
    }

    internal class TestLogger : ILogger
    {
        private static void WriteLine(string v) => System.Diagnostics.Debug.Write(v);

        public void Trace(string SubSystem, string Operation, string Message) => WriteLine($"SubSystem:{SubSystem},Operation:{Operation},Event:{Message}");

        public void Warning(string SubSystem, string Message) => Trace(SubSystem, "WARNING", Message);

        public void Log(string SubSystem, string Message) => Trace(SubSystem, "INFO", Message);

        public void TraceSensitive(string SubSystem, string Operation, string Message) => Trace(SubSystem, Operation, Message);

        public void WarningSensitive(string SubSystem, string Message) => Warning(SubSystem, Message);

        public void LogSensitive(string SubSystem, string Message) => Log(SubSystem, Message);
    }

    class TestConnection : IConnection
    {
        public Task SendMessageAsync(object result) => Task.CompletedTask; //Return CompletedTask when sending Acknowledge
    }

    [Command(Name = "Common.TestMessage1")]
    public class TestMessage1 : XFS4IoT.Commands.Command<XFS4IoT.Commands.MessagePayload>
    {
        public TestMessage1() : base(new Random().Next(), null)
        { }
    }
    [Command(Name = "Common.TestMessage2")]
    public class TestMessage2 : XFS4IoT.Commands.Command<XFS4IoT.Commands.MessagePayload>
    {
        public TestMessage2() : base(new Random().Next(), null)
        { }
    }
    [Command(Name = "Common.TestMessage3")]
    public class TestMessage3 : XFS4IoT.Commands.Command<XFS4IoT.Commands.MessagePayload>
    {
        public TestMessage3() : base(new Random().Next(), null)
        { }
    }

    [CommandHandler(XFSConstants.ServiceClass.Publisher, typeof(TestMessage1))]
    public class TestMessageHandler1 : ICommandHandler
    {
        public TestMessageHandler1(IConnection _, ICommandDispatcher _1, ILogger _2 ){}

        public async Task Handle(object command, CancellationToken cancel)
        {
            Assert.IsInstanceOfType(command, typeof(TestMessage1));
            await Task.CompletedTask;
        }

        public async Task HandleError(object command, Exception commandException)
        {
            Assert.IsInstanceOfType(command, typeof(TestMessage1));
            await Task.CompletedTask;
        }
    }
    [CommandHandler(XFSConstants.ServiceClass.Publisher, typeof(TestMessage2))]
    [CommandHandler(XFSConstants.ServiceClass.Publisher, typeof(TestMessage3))]
    //[CommandHandler(typeof(Int32))] // Non-CommandMessage types will FE on process startup. 
    public class TestMessageHandler2 : ICommandHandler
    {
        public TestMessageHandler2(IConnection _, ICommandDispatcher _1, ILogger _2) { }

        public async Task Handle(object command, CancellationToken cancel)
        {
            Assert.IsTrue(command is TestMessage2 || command is TestMessage3);

            await Task.CompletedTask;
        }

        public async Task HandleError(object command, Exception commandException)
        {
            Assert.IsTrue(command is TestMessage2 || command is TestMessage3);

            await Task.CompletedTask;
        }
    }
}
