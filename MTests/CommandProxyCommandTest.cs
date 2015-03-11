using System;
using System.Text;
using System.Collections.Generic;
using System.Linq;
using MContracts.Commands;
using MContracts.Controls;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace MTests
{
    class CommandsHolderStub
    {
        private int _count = 0;
        private readonly RelayCommand _command;
        public CommandsHolderStub(bool canExecute)
        {
            _command = new RelayCommand(x=>++_count, x=>canExecute);
        }

        public int Count { get { return _count; } }
        public RelayCommand Command { get { return _command; } }
    }

    [TestClass]
    public class CommandProxyCommandTest
    {
        [TestMethod]
        public void CommandProxyCommandExecuteTest()
        {
            CommandsHolderStub stub = new CommandsHolderStub(true);
            CommandProxyCommand cmd = new CommandProxyCommand("Command", stub);
            Assert.IsTrue(cmd.CanExecute(null));
            cmd.Execute(null);
            Assert.AreEqual(1, stub.Count);
        }

        [TestMethod]
        public void CommandProxyCommandCanExecuteTest()
        {
            CommandsHolderStub stub = new CommandsHolderStub(false);
            CommandProxyCommand cmd = new CommandProxyCommand("Command", stub);
            Assert.IsFalse(cmd.CanExecute(null));            
        }
    }
}
