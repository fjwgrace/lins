using CommunityToolkit.Mvvm.Messaging.Messages;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Trader.Models;

namespace Trader.Message
{
    internal class PositionSettingMsg:ValueChangedMessage<PositionSetting>
    {
        public PositionSettingMsg(PositionSetting value) : base(value)
        {
        }
    }
}
