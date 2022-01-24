using Moq.AutoMock;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SignalRChat.Tests.Base
{
    public abstract class UnitTestFixture
    {
        public AutoMocker Mocker { get; set; }

        public UnitTestFixture()
        {
            Mocker = new AutoMocker();
        }
    }
}
